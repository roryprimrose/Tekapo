namespace Tekapo.Controls
{
    partial class SelectFilesPage
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
            this.Files = new System.Windows.Forms.ListBox();
            this.RemoveAll = new System.Windows.Forms.Button();
            this.RemoveSelected = new System.Windows.Forms.Button();
            this.AddFiles = new System.Windows.Forms.Button();
            this.ErrorDisplay = new System.Windows.Forms.ErrorProvider(this.components);
            this.AddFolder = new System.Windows.Forms.Button();
            this.Contents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // Contents
            // 
            this.Contents.Controls.Add(this.AddFolder);
            this.Contents.Controls.Add(this.AddFiles);
            this.Contents.Controls.Add(this.RemoveSelected);
            this.Contents.Controls.Add(this.RemoveAll);
            this.Contents.Controls.Add(this.Files);
            // 
            // Files
            // 
            this.Files.AllowDrop = true;
            this.Files.FormattingEnabled = true;
            this.Files.HorizontalScrollbar = true;
            this.Files.IntegralHeight = false;
            this.Files.ItemHeight = 25;
            this.Files.Location = new System.Drawing.Point(24, 33);
            this.Files.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Files.Name = "Files";
            this.Files.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.Files.Size = new System.Drawing.Size(712, 439);
            this.Files.Sorted = true;
            this.Files.TabIndex = 0;
            this.Files.DragDrop += new System.Windows.Forms.DragEventHandler(this.Files_DragDrop);
            this.Files.DragEnter += new System.Windows.Forms.DragEventHandler(this.Files_DragEnter);
            this.Files.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Files_KeyUp);
            // 
            // RemoveAll
            // 
            this.RemoveAll.Location = new System.Drawing.Point(752, 431);
            this.RemoveAll.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.RemoveAll.Name = "RemoveAll";
            this.RemoveAll.Size = new System.Drawing.Size(214, 44);
            this.RemoveAll.TabIndex = 1;
            this.RemoveAll.Text = "Remove All";
            this.RemoveAll.UseVisualStyleBackColor = true;
            this.RemoveAll.Click += new System.EventHandler(this.RemoveAll_Click);
            // 
            // RemoveSelected
            // 
            this.RemoveSelected.Location = new System.Drawing.Point(752, 375);
            this.RemoveSelected.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.RemoveSelected.Name = "RemoveSelected";
            this.RemoveSelected.Size = new System.Drawing.Size(214, 44);
            this.RemoveSelected.TabIndex = 2;
            this.RemoveSelected.Text = "Remove Selected";
            this.RemoveSelected.UseVisualStyleBackColor = true;
            this.RemoveSelected.Click += new System.EventHandler(this.RemoveSelected_Click);
            // 
            // AddFiles
            // 
            this.AddFiles.Location = new System.Drawing.Point(748, 263);
            this.AddFiles.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.AddFiles.Name = "AddFiles";
            this.AddFiles.Size = new System.Drawing.Size(214, 44);
            this.AddFiles.TabIndex = 3;
            this.AddFiles.Text = "Add Files";
            this.AddFiles.UseVisualStyleBackColor = true;
            this.AddFiles.Click += new System.EventHandler(this.AddFiles_Click);
            // 
            // ErrorDisplay
            // 
            this.ErrorDisplay.ContainerControl = this;
            // 
            // AddFolder
            // 
            this.AddFolder.Location = new System.Drawing.Point(752, 319);
            this.AddFolder.Margin = new System.Windows.Forms.Padding(6);
            this.AddFolder.Name = "AddFolder";
            this.AddFolder.Size = new System.Drawing.Size(214, 44);
            this.AddFolder.TabIndex = 4;
            this.AddFolder.Text = "Add Folder";
            this.AddFolder.UseVisualStyleBackColor = true;
            this.AddFolder.Click += new System.EventHandler(this.AddFolder_Click);
            // 
            // SelectFilesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.Description = "This page will be loaded with files found from a files search. Please add and/or " +
    "remove files as required before selecting Next.";
            this.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.Name = "SelectFilesPage";
            this.PageSettings.BackButtonSettings.Text = "Back";
            this.PageSettings.CancelButtonSettings.Text = "Cancel";
            this.PageSettings.CustomButtonSettings.Text = "Custom";
            this.PageSettings.CustomButtonSettings.Visible = false;
            this.PageSettings.HelpButtonSettings.Text = "Help";
            this.PageSettings.HelpButtonSettings.Visible = false;
            this.PageSettings.NextButtonSettings.Text = "Back";
            this.Title = "Determine files to process.";
            this.Opening += new System.EventHandler(this.SelectFiles_Opening);
            this.Contents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox Files;
        private System.Windows.Forms.Button RemoveAll;
        private System.Windows.Forms.Button RemoveSelected;
        private System.Windows.Forms.Button AddFiles;
        private System.Windows.Forms.ErrorProvider ErrorDisplay;
        private System.Windows.Forms.Button AddFolder;
    }
}
