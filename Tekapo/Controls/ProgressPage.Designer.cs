namespace Tekapo.Controls
{
    partial class ProgressPage
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
            this.SearchProgress = new System.Windows.Forms.ProgressBar();
            this.ProgressStatus = new System.Windows.Forms.Label();
            this.Contents.SuspendLayout();
            this.SuspendLayout();
            // 
            // Contents
            // 
            this.Contents.Controls.Add(this.ProgressStatus);
            this.Contents.Controls.Add(this.SearchProgress);
            // 
            // SearchProgress
            // 
            this.SearchProgress.Location = new System.Drawing.Point(21, 45);
            this.SearchProgress.Name = "SearchProgress";
            this.SearchProgress.Size = new System.Drawing.Size(451, 23);
            this.SearchProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.SearchProgress.TabIndex = 0;
            // 
            // ProgressStatus
            // 
            this.ProgressStatus.Location = new System.Drawing.Point(21, 80);
            this.ProgressStatus.Name = "ProgressStatus";
            this.ProgressStatus.Size = new System.Drawing.Size(451, 166);
            this.ProgressStatus.TabIndex = 2;
            // 
            // FileSearchPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "FileSearchPage";
            this.Closing += new System.EventHandler(this.ProgressPage_Closing);
            this.Opening += new System.EventHandler(this.ProgressPage_Opening);
            this.Opened += new System.EventHandler(this.ProgressPage_Opened);
            this.Contents.ResumeLayout(false);
            this.Contents.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ProgressStatus;
        private System.Windows.Forms.ProgressBar SearchProgress;

    }
}
