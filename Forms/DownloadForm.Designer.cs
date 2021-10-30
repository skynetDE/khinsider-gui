
namespace khi.Forms
{
    partial class DownloadForm
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
            this.components = new System.ComponentModel.Container();
            this.lblTotal = new System.Windows.Forms.Label();
            this.progressTotal = new System.Windows.Forms.ProgressBar();
            this.lblCurrentAlbum = new System.Windows.Forms.Label();
            this.progressAlbum = new System.Windows.Forms.ProgressBar();
            this.lblCurrentFile = new System.Windows.Forms.Label();
            this.progressFile = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblFileSize = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.imgThumb = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgThumb)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(14, 202);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(31, 13);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "Total";
            // 
            // progressTotal
            // 
            this.progressTotal.Location = new System.Drawing.Point(14, 218);
            this.progressTotal.Name = "progressTotal";
            this.progressTotal.Size = new System.Drawing.Size(481, 30);
            this.progressTotal.TabIndex = 3;
            this.progressTotal.UseWaitCursor = true;
            // 
            // lblCurrentAlbum
            // 
            this.lblCurrentAlbum.AutoSize = true;
            this.lblCurrentAlbum.Location = new System.Drawing.Point(14, 96);
            this.lblCurrentAlbum.Name = "lblCurrentAlbum";
            this.lblCurrentAlbum.Size = new System.Drawing.Size(73, 13);
            this.lblCurrentAlbum.TabIndex = 6;
            this.lblCurrentAlbum.Text = "Current Album";
            // 
            // progressAlbum
            // 
            this.progressAlbum.Location = new System.Drawing.Point(14, 112);
            this.progressAlbum.Name = "progressAlbum";
            this.progressAlbum.Size = new System.Drawing.Size(375, 30);
            this.progressAlbum.TabIndex = 5;
            this.progressAlbum.UseWaitCursor = true;
            // 
            // lblCurrentFile
            // 
            this.lblCurrentFile.AutoSize = true;
            this.lblCurrentFile.Location = new System.Drawing.Point(14, 9);
            this.lblCurrentFile.Name = "lblCurrentFile";
            this.lblCurrentFile.Size = new System.Drawing.Size(60, 13);
            this.lblCurrentFile.TabIndex = 8;
            this.lblCurrentFile.Text = "Current File";
            // 
            // progressFile
            // 
            this.progressFile.Location = new System.Drawing.Point(14, 25);
            this.progressFile.Name = "progressFile";
            this.progressFile.Size = new System.Drawing.Size(481, 30);
            this.progressFile.TabIndex = 7;
            this.progressFile.UseWaitCursor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(219, 256);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblFileSize
            // 
            this.lblFileSize.AutoSize = true;
            this.lblFileSize.Location = new System.Drawing.Point(14, 58);
            this.lblFileSize.Name = "lblFileSize";
            this.lblFileSize.Size = new System.Drawing.Size(46, 13);
            this.lblFileSize.TabIndex = 11;
            this.lblFileSize.Text = "File Size";
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            // 
            // imgThumb
            // 
            this.imgThumb.ImageLocation = "";
            this.imgThumb.Location = new System.Drawing.Point(395, 112);
            this.imgThumb.Name = "imgThumb";
            this.imgThumb.Size = new System.Drawing.Size(100, 100);
            this.imgThumb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgThumb.TabIndex = 12;
            this.imgThumb.TabStop = false;
            // 
            // DownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 286);
            this.Controls.Add(this.imgThumb);
            this.Controls.Add(this.lblFileSize);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblCurrentFile);
            this.Controls.Add(this.progressFile);
            this.Controls.Add(this.lblCurrentAlbum);
            this.Controls.Add(this.progressAlbum);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.progressTotal);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DownloadForm";
            this.Text = "Download";
            ((System.ComponentModel.ISupportInitialize)(this.imgThumb)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.ProgressBar progressTotal;
        private System.Windows.Forms.Label lblCurrentAlbum;
        private System.Windows.Forms.ProgressBar progressAlbum;
        private System.Windows.Forms.Label lblCurrentFile;
        private System.Windows.Forms.ProgressBar progressFile;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblFileSize;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox imgThumb;
    }
}