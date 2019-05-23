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
            label1.Location = new System.Drawing.Point(28, 27);
            label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(178, 25);
            label1.TabIndex = 0;
            label1.Text = "File name format:";
            // 
            // Example
            // 
            this.Example.Location = new System.Drawing.Point(36, 146);
            this.Example.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Example.Name = "Example";
            this.Example.Size = new System.Drawing.Size(920, 338);
            this.Example.TabIndex = 2;
            // 
            // ErrorDisplay
            // 
            this.ErrorDisplay.ContainerControl = this;
            // 
            // IncrementOnCollision
            // 
            this.IncrementOnCollision.AutoSize = true;
            this.IncrementOnCollision.Checked = global::Tekapo.Properties.Settings.Default.IncrementOnCollision;
            this.IncrementOnCollision.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IncrementOnCollision.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Tekapo.Properties.Settings.Default, "IncrementOnCollision", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.IncrementOnCollision.Location = new System.Drawing.Point(42, 108);
            this.IncrementOnCollision.Margin = new System.Windows.Forms.Padding(6);
            this.IncrementOnCollision.Name = "IncrementOnCollision";
            this.IncrementOnCollision.Size = new System.Drawing.Size(413, 29);
            this.IncrementOnCollision.TabIndex = 3;
            this.IncrementOnCollision.Text = "Add increment to file name on collision";
            this.IncrementOnCollision.UseVisualStyleBackColor = true;
            this.IncrementOnCollision.CheckedChanged += new System.EventHandler(this.IncrementOnCollision_CheckedChanged);
            // 
            // InsertFormat
            // 
            this.InsertFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InsertFormat.Location = new System.Drawing.Point(930, 58);
            this.InsertFormat.Margin = new System.Windows.Forms.Padding(6);
            this.InsertFormat.Name = "InsertFormat";
            this.InsertFormat.Size = new System.Drawing.Size(40, 40);
            this.InsertFormat.TabIndex = 4;
            this.InsertFormat.Text = ">";
            this.InsertFormat.UseVisualStyleBackColor = true;
            this.InsertFormat.Click += new System.EventHandler(this.InsertFormat_Click);
            // 
            // NameFormat
            // 
            this.NameFormat.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Tekapo.Properties.Settings.Default, "NameFormat", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.NameFormat.FormattingEnabled = true;
            this.NameFormat.Location = new System.Drawing.Point(34, 58);
            this.NameFormat.Margin = new System.Windows.Forms.Padding(6);
            this.NameFormat.Name = "NameFormat";
            this.NameFormat.Size = new System.Drawing.Size(880, 33);
            this.NameFormat.TabIndex = 5;
            this.NameFormat.Text = global::Tekapo.Properties.Settings.Default.NameFormat;
            this.NameFormat.TextChanged += new System.EventHandler(this.NameFormat_TextChanged);
            // 
            // NameFormatPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.Description = "Please specify a file rename format. This is done by specifying literal character" +
    "s and format masks.";
            this.Margin = new System.Windows.Forms.Padding(12);
            this.Name = "NameFormatPage";
            this.PageSettings.BackButtonSettings.Text = "Back";
            this.PageSettings.CancelButtonSettings.Text = "Cancel";
            this.PageSettings.CustomButtonSettings.Text = "Custom";
            this.PageSettings.CustomButtonSettings.Visible = false;
            this.PageSettings.HelpButtonSettings.Text = "Help";
            this.PageSettings.HelpButtonSettings.Visible = false;
            this.PageSettings.NextButtonSettings.Text = "Back";
            this.Title = "Specify file rename format";
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
