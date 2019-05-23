namespace Tekapo.Controls
{
    partial class FileSearchPage
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
            this.ProgressStatus = new System.Windows.Forms.Label();
            this.Contents.SuspendLayout();
            this.SuspendLayout();
            // 
            // Contents
            // 
            this.Contents.Controls.Add(this.ProgressStatus);
            // 
            // ProgressStatus
            // 
            this.ProgressStatus.Location = new System.Drawing.Point(44, 42);
            this.ProgressStatus.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.ProgressStatus.Name = "ProgressStatus";
            this.ProgressStatus.Size = new System.Drawing.Size(902, 419);
            this.ProgressStatus.TabIndex = 3;
            // 
            // FileSearchPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.Description = "Searching for files that match the supported formats and any filters specified.";
            this.Name = "FileSearchPage";
            this.PageSettings.BackButtonSettings.Text = "Back";
            this.PageSettings.CancelButtonSettings.Text = "Cancel";
            this.PageSettings.CustomButtonSettings.Text = "Custom";
            this.PageSettings.CustomButtonSettings.Visible = false;
            this.PageSettings.HelpButtonSettings.Text = "Help";
            this.PageSettings.HelpButtonSettings.Visible = false;
            this.PageSettings.NextButtonSettings.Text = "Next";
            this.Title = "Searching for files";
            this.Contents.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ProgressStatus;
    }
}
