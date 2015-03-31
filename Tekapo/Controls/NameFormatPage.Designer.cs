namespace Tekapo.Controls
{
    partial class NameFormatPage
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
            System.Windows.Forms.Label label1;
            this.Example = new System.Windows.Forms.Label();
            this.ErrorDisplay = new System.Windows.Forms.ErrorProvider(this.components);
            this.IncrementOnCollision = new System.Windows.Forms.CheckBox();
            this.InsertFormat = new System.Windows.Forms.Button();
            this.NameFormat = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            this.Contents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // Contents
            // 
            this.Contents.Controls.Add(this.NameFormat);
            this.Contents.Controls.Add(this.InsertFormat);
            this.Contents.Controls.Add(this.IncrementOnCollision);
            this.Contents.Controls.Add(this.Example);
            this.Contents.Controls.Add(label1);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(14, 14);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(87, 13);
            label1.TabIndex = 0;
            label1.Text = "File name format:";
            // 
            // Example
            // 
            this.Example.Location = new System.Drawing.Point(18, 76);
            this.Example.Name = "Example";
            this.Example.Size = new System.Drawing.Size(460, 176);
            this.Example.TabIndex = 2;
            // 
            // ErrorDisplay
            // 
            this.ErrorDisplay.ContainerControl = this;
            // 
            // IncrementOnCollision
            // 
            this.IncrementOnCollision.AutoSize = true;
            this.IncrementOnCollision.Checked = true;
            this.IncrementOnCollision.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IncrementOnCollision.Location = new System.Drawing.Point(21, 56);
            this.IncrementOnCollision.Name = "IncrementOnCollision";
            this.IncrementOnCollision.Size = new System.Drawing.Size(206, 17);
            this.IncrementOnCollision.TabIndex = 3;
            this.IncrementOnCollision.Text = "Add increment to file name on collision";
            this.IncrementOnCollision.UseVisualStyleBackColor = true;
            this.IncrementOnCollision.CheckedChanged += new System.EventHandler(this.IncrementOnCollision_CheckedChanged);
            // 
            // InsertFormat
            // 
            this.InsertFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InsertFormat.Location = new System.Drawing.Point(465, 30);
            this.InsertFormat.Name = "InsertFormat";
            this.InsertFormat.Size = new System.Drawing.Size(20, 21);
            this.InsertFormat.TabIndex = 4;
            this.InsertFormat.Text = ">";
            this.InsertFormat.UseVisualStyleBackColor = true;
            this.InsertFormat.Click += new System.EventHandler(this.InsertFormat_Click);
            // 
            // NameFormat
            // 
            this.NameFormat.FormattingEnabled = true;
            this.NameFormat.Location = new System.Drawing.Point(17, 30);
            this.NameFormat.Name = "NameFormat";
            this.NameFormat.Size = new System.Drawing.Size(442, 21);
            this.NameFormat.TabIndex = 5;
            this.NameFormat.TextChanged += new System.EventHandler(this.NameFormat_TextChanged);
            // 
            // NameFormatPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Description = "Please specify a file rename format. This is done by specifying literal character" +
                "s and format masks.";
            this.Name = "NameFormatPage";
            this.PageSettings.CancelButtonSettings.Text = "Cancel";
            this.PageSettings.CustomButtonSettings.Text = "Custom";
            this.PageSettings.CustomButtonSettings.Visible = false;
            this.PageSettings.HelpButtonSettings.Text = "Help";
            this.PageSettings.HelpButtonSettings.Visible = false;
            this.PageSettings.NextButtonSettings.Text = "Next";
            this.PageSettings.NextButtonSettings.Text = "Back";
            this.Title = "Specify file rename format";
            this.Closing += new System.EventHandler(this.NameFormatPage_Closing);
            this.Opening += new System.EventHandler(this.NameFormatPage_Opening);
            this.Load += new System.EventHandler(this.NameFormatPage_Load);
            this.Contents.ResumeLayout(false);
            this.Contents.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Example;
        private System.Windows.Forms.ErrorProvider ErrorDisplay;
        private System.Windows.Forms.CheckBox IncrementOnCollision;
        private System.Windows.Forms.Button InsertFormat;
        private System.Windows.Forms.ComboBox NameFormat;

    }
}
