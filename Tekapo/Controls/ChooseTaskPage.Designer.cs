namespace Tekapo.Controls
{
    partial class ChooseTaskPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseTaskPage));
            this.Shift = new System.Windows.Forms.RadioButton();
            this.Rename = new System.Windows.Forms.RadioButton();
            this.Contents.SuspendLayout();
            this.SuspendLayout();
            // 
            // Contents
            // 
            this.Contents.Controls.Add(this.Shift);
            this.Contents.Controls.Add(this.Rename);
            // 
            // Shift
            // 
            this.Shift.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.Shift.Location = new System.Drawing.Point(12, 113);
            this.Shift.Name = "Shift";
            this.Shift.Size = new System.Drawing.Size(464, 90);
            this.Shift.TabIndex = 1;
            this.Shift.TabStop = true;
            this.Shift.Text = resources.GetString("Shift.Text");
            this.Shift.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.Shift.UseVisualStyleBackColor = true;
            // 
            // Rename
            // 
            this.Rename.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.Rename.Checked = true;
            this.Rename.Location = new System.Drawing.Point(12, 23);
            this.Rename.Name = "Rename";
            this.Rename.Size = new System.Drawing.Size(470, 91);
            this.Rename.TabIndex = 0;
            this.Rename.TabStop = true;
            this.Rename.Text = resources.GetString("Rename.Text");
            this.Rename.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.Rename.UseVisualStyleBackColor = true;
            // 
            // ChooseTaskPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Description = "Select the task that you want to do. You may either rename photos, or shift their" +
                " picture taken date.";
            this.Name = "ChooseTaskPage";
            this.Title = "Choose Task";
            this.Closing += new System.EventHandler(this.ChooseTaskPage_Closing);
            this.Opening += new System.EventHandler(this.ChooseTaskPage_Opening);
            this.Contents.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton Shift;
        private System.Windows.Forms.RadioButton Rename;
    }
}
