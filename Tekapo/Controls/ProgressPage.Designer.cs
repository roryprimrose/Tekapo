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
            this.SearchProgress.Location = new System.Drawing.Point(41, 25);
            this.SearchProgress.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.SearchProgress.Name = "SearchProgress";
            this.SearchProgress.Size = new System.Drawing.Size(902, 44);
            this.SearchProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.SearchProgress.TabIndex = 0;
            // 
            // ProgressStatus
            // 
            this.ProgressStatus.Location = new System.Drawing.Point(42, 92);
            this.ProgressStatus.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.ProgressStatus.Name = "ProgressStatus";
            this.ProgressStatus.Size = new System.Drawing.Size(902, 381);
            this.ProgressStatus.TabIndex = 2;
            // 
            // ProgressPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.Name = "ProgressPage";
            this.PageSettings.BackButtonSettings.Text = "Back";
            this.PageSettings.CancelButtonSettings.Text = "Cancel";
            this.PageSettings.CustomButtonSettings.Text = "Custom";
            this.PageSettings.CustomButtonSettings.Visible = false;
            this.PageSettings.HelpButtonSettings.Text = "Help";
            this.PageSettings.HelpButtonSettings.Visible = false;
            this.PageSettings.NextButtonSettings.Text = "Next";
            this.Contents.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ProgressStatus;
        private System.Windows.Forms.ProgressBar SearchProgress;

    }
}
