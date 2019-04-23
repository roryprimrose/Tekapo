using System;

namespace Neovolve.Windows.Forms.Test
{
    partial class InvokeKeyPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(Boolean disposing)
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
            this.TestButton = new System.Windows.Forms.Button();
            this.Contents.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContentsPanel
            // 
            this.Contents.Controls.Add(this.TestButton);
            // 
            // TestButton
            // 
            this.TestButton.Location = new System.Drawing.Point(74, 75);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(249, 23);
            this.TestButton.TabIndex = 0;
            this.TestButton.Text = "Invoke custom navigation";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // InvokeKeyPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.DefaultFocus = this.TestButton;
            this.Name = "InvokeKeyPage";
            this.PageSettings.CancelButtonSettings.Text = "Cancel";
            this.PageSettings.CustomButtonSettings.Text = "Custom";
            this.PageSettings.CustomButtonSettings.Visible = false;
            this.PageSettings.HelpButtonSettings.Text = "Help";
            this.PageSettings.HelpButtonSettings.Visible = false;
            this.PageSettings.NextButtonSettings.Text = "Next";
            this.PageSettings.BackButtonSettings.Text = "Back";
            this.Contents.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button TestButton;
    }
}
