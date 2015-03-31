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
            this.StepTitle = new System.Windows.Forms.Label();
            this.Contents.SuspendLayout();
            this.SuspendLayout();
            // 
            // Contents
            // 
            this.Contents.Controls.Add(this.StepTitle);
            this.Contents.Controls.SetChildIndex(this.StepTitle, 0);
            // 
            // StepTitle
            // 
            this.StepTitle.AutoSize = true;
            this.StepTitle.Location = new System.Drawing.Point(21, 29);
            this.StepTitle.Name = "StepTitle";
            this.StepTitle.Size = new System.Drawing.Size(179, 13);
            this.StepTitle.TabIndex = 1;
            this.StepTitle.Text = "Step 1 of 3: Searching for directories";
            // 
            // FileSearchPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Description = "The search path is now being searched for files that match the supported formats " +
                "and any filters specified.";
            this.Name = "FileSearchPage";
            this.Title = "Searching for files";
            this.Contents.ResumeLayout(false);
            this.Contents.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label StepTitle;
    }
}
