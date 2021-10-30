
namespace khi.Forms
{
    partial class ConfigForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.chkOverwriteIfFileExists = new System.Windows.Forms.CheckBox();
            this.numRetries = new System.Windows.Forms.NumericUpDown();
            this.lblRetries = new System.Windows.Forms.Label();
            this.chkLoadRecent = new System.Windows.Forms.CheckBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboPreferredFileType = new System.Windows.Forms.ComboBox();
            this.grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRetries)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.chkOverwriteIfFileExists);
            this.grpSettings.Controls.Add(this.numRetries);
            this.grpSettings.Controls.Add(this.lblRetries);
            this.grpSettings.Controls.Add(this.chkLoadRecent);
            this.grpSettings.Location = new System.Drawing.Point(11, 12);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(309, 100);
            this.grpSettings.TabIndex = 0;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Settings";
            // 
            // chkOverwriteIfFileExists
            // 
            this.chkOverwriteIfFileExists.AutoSize = true;
            this.chkOverwriteIfFileExists.Location = new System.Drawing.Point(6, 70);
            this.chkOverwriteIfFileExists.Name = "chkOverwriteIfFileExists";
            this.chkOverwriteIfFileExists.Size = new System.Drawing.Size(127, 17);
            this.chkOverwriteIfFileExists.TabIndex = 8;
            this.chkOverwriteIfFileExists.Text = "Overwrite File if exists";
            this.chkOverwriteIfFileExists.UseVisualStyleBackColor = true;
            // 
            // numRetries
            // 
            this.numRetries.Location = new System.Drawing.Point(98, 44);
            this.numRetries.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numRetries.Name = "numRetries";
            this.numRetries.Size = new System.Drawing.Size(63, 20);
            this.numRetries.TabIndex = 7;
            this.numRetries.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblRetries
            // 
            this.lblRetries.AutoSize = true;
            this.lblRetries.Location = new System.Drawing.Point(6, 46);
            this.lblRetries.Name = "lblRetries";
            this.lblRetries.Size = new System.Drawing.Size(86, 13);
            this.lblRetries.TabIndex = 6;
            this.lblRetries.Text = "Retries per Song";
            // 
            // chkLoadRecent
            // 
            this.chkLoadRecent.AutoSize = true;
            this.chkLoadRecent.Location = new System.Drawing.Point(7, 20);
            this.chkLoadRecent.Name = "chkLoadRecent";
            this.chkLoadRecent.Size = new System.Drawing.Size(240, 17);
            this.chkLoadRecent.TabIndex = 0;
            this.chkLoadRecent.Text = "Load list of last recently added albums at start";
            this.chkLoadRecent.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(133, 177);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboPreferredFileType);
            this.groupBox1.Location = new System.Drawing.Point(11, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(309, 53);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Formats";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Preferred";
            // 
            // comboPreferredFileType
            // 
            this.comboPreferredFileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPreferredFileType.FormattingEnabled = true;
            this.comboPreferredFileType.Location = new System.Drawing.Point(67, 19);
            this.comboPreferredFileType.Name = "comboPreferredFileType";
            this.comboPreferredFileType.Size = new System.Drawing.Size(121, 21);
            this.comboPreferredFileType.TabIndex = 0;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 212);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuration";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRetries)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSettings;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox chkLoadRecent;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.NumericUpDown numRetries;
        private System.Windows.Forms.Label lblRetries;
        private System.Windows.Forms.CheckBox chkOverwriteIfFileExists;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboPreferredFileType;
    }
}