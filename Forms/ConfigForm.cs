using System;
using System.Windows.Forms;
using khi.Catalog;
using khi.Properties;

namespace khi.Forms
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            Icon = Resources.icon;
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            chkLoadRecent.Checked = Configuration.LoadSongUpdatesOnStartup;
            numRetries.Value = Configuration.Retries;
            chkOverwriteIfFileExists.Checked = Configuration.OverwriteIfFileExists;
            foreach (var fileType in Enum.GetValues(typeof(FileType))) comboPreferredFileType.Items.Add(fileType);

            comboPreferredFileType.SelectedIndex =
                comboPreferredFileType.FindStringExact(Configuration.PreferredFileType);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Configuration.LoadSongUpdatesOnStartup = chkLoadRecent.Checked;
            Configuration.Retries = numRetries.Value;
            Configuration.OverwriteIfFileExists = chkOverwriteIfFileExists.Checked;
            Configuration.PreferredFileType = comboPreferredFileType.Text;
            Configuration.Save();
            Close();
        }
    }
}