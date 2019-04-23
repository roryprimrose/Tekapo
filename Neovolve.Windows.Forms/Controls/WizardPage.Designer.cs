using System;
using System.Windows.Forms;

namespace Neovolve.Windows.Forms.Controls
{
    partial class WizardPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Contents = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // Contents
            // 
            this.Contents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Contents.Location = new System.Drawing.Point(0, 0);
            this.Contents.Name = "Contents";
            this.Contents.Size = new System.Drawing.Size(495, 337);
            this.Contents.TabIndex = 0;
            // 
            // WizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Contents);
            this.Name = "WizardPage";
            this.Size = new System.Drawing.Size(495, 337);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Defines the panel that contains the contents of the page.
        /// </summary>
        protected Panel Contents;
    }
}
