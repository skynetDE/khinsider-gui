using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using khi.Catalog;
using Ookii.Dialogs.WinForms;

namespace khi.Forms
{
    public partial class MainForm : Form
    {
        private readonly ISet<Album> _albums = new HashSet<Album>(Album.EqualityComparer);
        private readonly ISet<Album> _selected = new HashSet<Album>(Album.EqualityComparer);

        public MainForm()
        {
            Icon = Properties.Resources.icon;
            InitializeComponent();

            RefreshLastUpdateInMenu();

            listAlbums.ListFilter = new ListFilter(delegate(IEnumerable objects)
            {
                var words = txtSearch.Text.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                return objects.Cast<Album>()
                    .Where(album => words.Length == 0 || words.All(word =>
                        album.Name.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();
            });
        }

        private async Task Initialize(string albumPath)
        {
            var loadedAlbums = LoadAlbums(albumPath);
            _albums.UnionWith(loadedAlbums);

            if (Configuration.LoadSongUpdatesOnStartup) await LoadUpdates(albumPath, false).ConfigureAwait(false);

            ReloadAlbumView();
        }

        private static IEnumerable<Album> LoadAlbums(string albumPath)
        {
            return new CatalogFileLoader(albumPath, CatalogFileWriter.Delimiter)
                .Load();
        }

        private void RefreshLastUpdateInMenu()
        {
            if (InvokeRequired)
            {
                void Action()
                {
                    RefreshLastUpdateInMenu();
                }

                Invoke((Action) Action);
            }
            else
            {
                var lastUpdateAlbumList = Configuration.LastUpdateAlbumList;
                if (lastUpdateAlbumList != null)
                    loadUpdatesToolStripMenuItem.Text =
                        $"Load Updates (Last Load: {DateTime.Parse(lastUpdateAlbumList)})";
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ConfigForm().ShowDialog(this);
        }

        private void refreshCatalogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var browsingContext = KhiBrowsingContext.Build();

            var cancellationTokenSource = new CancellationTokenSource();

            using var progressDialog = new ProgressDialog()
            {
                ShowTimeRemaining = true,
                CancellationText = "Cancelling...",
                MinimizeBox = false,
                ProgressBarStyle = Ookii.Dialogs.WinForms.ProgressBarStyle.ProgressBar
            };

            progressDialog.DoWork += (o, args) =>
            {
                var cancellationToken = args.Argument is CancellationToken token ? token : default;

                var task = new CatalogWebLoader(browsingContext)
                    .Load(
                        (total, current, text, letter) =>
                        {
                            progressDialog.ReportProgress(100 * current / total,
                                $"Scraping Catalog (Current: {letter})...", text);
                            if (progressDialog.CancellationPending)
                            {
                                cancellationTokenSource.Cancel();
                            }
                        },
                        cancellationToken);

                try
                {
                    task.Wait(cancellationToken);
                }
                catch (OperationCanceledException)
                {
                }

                args.Result = task.Result;
            };

            progressDialog.RunWorkerCompleted += (o, args) =>
            {
                browsingContext.Dispose();

                cancellationTokenSource.Dispose();

                Debug.WriteLine("Compl");

                if (args.Result is List<Album> result)
                {
                    Debug.WriteLine("Yes");
                    _albums.Clear();
                    _selected.Clear();
                    RefreshSelected();

                    _albums.UnionWith(result);
                    ReloadAlbumView();

                    SaveAlbums(GetAlbumPath());
                    MessageBox.Show($"Found {_albums.Count} albums");
                }
            };

            progressDialog.Show(cancellationTokenSource.Token);
        }

        private void RefreshSelected()
        {
            listSelected.Objects = _selected;
            listSelected.DeselectAll();
        }

        private void SaveAlbums(string albumPath)
        {
            new CatalogFileWriter(albumPath).Write(_albums);
        }

        private void loadUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.ShowWaitDialog(this, () => LoadUpdates(GetAlbumPath()).Wait(), "Loading Updates...");
        }

        private async Task LoadUpdates(string albumPath, bool reloadView = true)
        {
            var browsingContext = KhiBrowsingContext.Build();

            var result = await new CatalogWebLoader(browsingContext)
                .LoadUpdates()
                .ContinueWith(task =>
                {
                    browsingContext.Dispose();
                    return task.Result;
                })
                .ConfigureAwait(false);

            var albumsCountBeforeAdding = _albums.Count;

            _albums.UnionWith(result);

            if (reloadView) ReloadAlbumView();

            var added = _albums.Count - albumsCountBeforeAdding;

            if (added > 0)
            {
                SaveAlbums(albumPath);
                MessageBox.Show($"Found {added} new albums");
            }

            Configuration.LastUpdateAlbumList = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Configuration.Save();
            RefreshLastUpdateInMenu();
        }

        private void ReloadAlbumView()
        {
            if (InvokeRequired)
            {
                void Action()
                {
                    ReloadAlbumView();
                }

                Invoke((Action) Action);
            }
            else
            {
                listAlbums.Objects = _albums.ToList();
                listAlbums.Sort();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            listAlbums.UpdateColumnFiltering();
        }

        private void listAlbums_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ClickCount > 1) Process.Start(((Album) e.Model).GetFullUrl());
        }

        private void listSelected_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete) return;

            foreach (var selectedObject in listSelected.SelectedObjects) _selected.Remove((Album) selectedObject);
            RefreshSelected();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (_selected.Count == 0)
                MessageBox.Show("You have to add at least one album");
            else
            {
                using var dialog = new VistaFolderBrowserDialog
                {
                    SelectedPath = Configuration.LastSelectedFolder, 
                    RootFolder = Environment.SpecialFolder.Desktop,
                    ShowNewFolderButton = true
                };


                if (dialog.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(dialog.SelectedPath)) return;

                Configuration.LastSelectedFolder = dialog.SelectedPath;
                Configuration.Save();

                var downloader = new DownloadForm(_selected, dialog.SelectedPath);
                downloader.Show();
                downloader.StartDownload();
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Utils.ShowWaitDialog(this, () => Initialize(GetAlbumPath()).Wait(), "Loading Albums...");
        }

        private static string GetAlbumPath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "albums.txt");
        }

        private void listSelected_ModelCanDrop(object sender, ModelDropEventArgs e)
        {
            e.Handled = true;
            e.Effect = DragDropEffects.None;
            if (!e.SourceModels.Contains(e.TargetModel)) e.Effect = DragDropEffects.Move;
        }

        private void listSelected_ModelDropped(object sender, ModelDropEventArgs e)
        {
            foreach (Album eSourceModel in e.SourceModels) _selected.Add(eSourceModel);

            RefreshSelected();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _selected.Clear();
            RefreshSelected();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = $"{Properties.Resources.appName} - v{Application.ProductVersion}";
        }

        private void githubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Resources.githubUrl);
        }
    }
}