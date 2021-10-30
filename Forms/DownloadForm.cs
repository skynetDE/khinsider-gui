using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;
using ByteSizeLib;
using khi.Catalog;
using khi.Properties;
using Serilog;

namespace khi.Forms
{
    public partial class DownloadForm : Form
    {
        private readonly IEnumerable<Album> _selected;
        private readonly string _targetFolder;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public DownloadForm(IEnumerable<Album> selected, string targetFolder)
        {
            InitializeComponent();
            Icon = Resources.icon;
            _selected = selected;
            _targetFolder = targetFolder;
        }

        public async void StartDownload()
        {
            var browsingContext = KhiBrowsingContext.Build();

            var httpClient = new HttpClient();

            var downloader = new Downloader(_targetFolder, _selected, browsingContext, httpClient)
            {
                PreferredFileType = (FileType) Enum.Parse(typeof(FileType), Configuration.PreferredFileType),
                OnFileExists = path => Configuration.OverwriteIfFileExists,
                OnException = exception =>
                {
                    //Log.Logger.Error(exception);
                },

                MaxRetries = Configuration.Retries
            };
            timer1.Tick += (sender, args) => OnProgress(downloader.GetProgress());
            timer1.Enabled = true;

            var cancellationToken = _cancellationTokenSource.Token;
            await downloader.StartDownload(cancellationToken)
                .ContinueWith(t =>
                {
                    timer1.Enabled = false;
                    browsingContext.Dispose();
                    httpClient.Dispose();
                    timer1.Dispose();
                    CloseForm();
                })
                .ConfigureAwait(false);
        }

        private void CloseForm()
        {
            if (InvokeRequired)
            {
                void Action()
                {
                    CloseForm();
                }

                Invoke((Action) Action);
            }
            else
            {
                Close();
            }
        }

        private void OnProgress(DownloadProgress progress)
        {
            if (Disposing || IsDisposed) return;

            if (InvokeRequired)
            {
                void Action()
                {
                    OnProgress(progress);
                }

                Invoke((Action) Action);
            }
            else
            {
                SetTotalProgress(progress.Total);
                SetAlbumProgress(progress.Album);
                SetFileProgress(progress.File, progress.Album);

                var albumDownload = progress.AlbumDownload;

                if (albumDownload?.Covers.Count > 0)
                {
                    var cover = albumDownload.Covers.First();
                    if (cover.Thumb != imgThumb.ImageLocation)
                    {
                        imgThumb.ImageLocation = cover.Thumb;
                    }
                }
            }
        }

        private void SetTotalProgress(DownloadProgress.Stats stats)
        {
            if (progressTotal.Maximum != stats.Max) progressTotal.Maximum = stats.Max;
            progressTotal.SetProgressNoAnimation((int) stats.Current);
            lblTotal.Text = $"{stats.Label} ({stats.Current} of {stats.Max})";
        }

        private void SetFileProgress(DownloadProgress.FileStats stats, DownloadProgress.Stats albumStats)
        {
            progressFile.SetProgressNoAnimation((int) stats.Current);
            lblCurrentFile.Text = $"{stats.Label} ({albumStats.Current} of {albumStats.Max})";
            lblFileSize.Text =
                $"File Size: {ByteSize.FromBytes(stats.TotalBytesToReceive).ToString("MiB")}";
        }

        private void SetAlbumProgress(DownloadProgress.Stats stats)
        {
            if (progressAlbum.Maximum != stats.Max) progressAlbum.Maximum = stats.Max;
            progressAlbum.SetProgressNoAnimation((int) stats.Current);

            var totalCurrent = stats.Max == 0 ? 0 : Math.Ceiling(100 * stats.Current / stats.Max);
            if (totalCurrent > 100) totalCurrent = 100;

            lblCurrentAlbum.Text = $"{stats.Label} ({totalCurrent}%)";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }
    }
}