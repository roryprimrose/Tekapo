namespace Tekapo.Controls
{
    partial class CompletedPage
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
            this.Results = new System.Windows.Forms.Label();
            this.ViewLog = new System.Windows.Forms.Button();
            this.Contents.SuspendLayout();
            this.SuspendLayout();
            // 
            // Contents
            // 
            this.Contents.Controls.Add(this.ViewLog);
            this.Contents.Controls.Add(this.Results);
            // 
            // Results
            // 
            this.Results.Location = new System.Drawing.Point(18, 23);
            this.Results.Name = "Results";
            this.Results.Size = new System.Drawing.Size(462, 203);
            this.Results.TabIndex = 0;
            // 
            // ViewLog
            // 
            this.ViewLog.Location = new System.Drawing.Point(405, 229);
            this.ViewLog.Name = "ViewLog";
            this.ViewLog.Size = new System.Drawing.Size(75, 23);
            this.ViewLog.TabIndex = 1;
            this.ViewLog.Text = "View Log";
            this.ViewLog.UseVisualStyleBackColor = true;
            this.ViewLog.Click += new System.EventHandler(this.ViewLog_Click);
            // 
            // CompletedPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Description = "All files specified have now been processed.";
            this.Name = "CompletedPage";
            this.Title = "Processing completed";
            this.Opening += new System.EventHandler(this.CompletedPage_Opening);
            this.Contents.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Results;
        private System.Windows.Forms.Button ViewLog;
    }
}
