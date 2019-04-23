using System;

namespace Neovolve.Windows.Forms.Controls
{
    partial class WizardSplashPage
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
            this.SplashPicture = new System.Windows.Forms.PictureBox();
            this.SplashPanel = new System.Windows.Forms.Panel();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.Contents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplashPicture)).BeginInit();
            this.SplashPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Contents
            // 
            this.Contents.Controls.Add(this.SplashPanel);
            this.Contents.Controls.Add(this.SplashPicture);
            // 
            // SplashPicture
            // 
            this.SplashPicture.Dock = System.Windows.Forms.DockStyle.Left;
            this.SplashPicture.Location = new System.Drawing.Point(0, 0);
            this.SplashPicture.Name = "SplashPicture";
            this.SplashPicture.Size = new System.Drawing.Size(100, 337);
            this.SplashPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.SplashPicture.TabIndex = 2;
            // 
            // SplashPanel
            // 
            this.SplashPanel.Controls.Add(this.DescriptionLabel);
            this.SplashPanel.Controls.Add(this.TitleLabel);
            this.SplashPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplashPanel.Location = new System.Drawing.Point(100, 0);
            this.SplashPanel.Name = "SplashPanel";
            this.SplashPanel.Size = new System.Drawing.Size(395, 337);
            this.SplashPanel.TabIndex = 3;
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.Location = new System.Drawing.Point(41, 95);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(339, 226);
            this.DescriptionLabel.TabIndex = 3;
            this.DescriptionLabel.Text = "[Description]";
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(41, 52);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(40, 13);
            this.TitleLabel.TabIndex = 2;
            this.TitleLabel.Text = "[Title]";
            // 
            // WizardSplashPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.Color.White;
            this.Name = "WizardSplashPage";
            this.Contents.ResumeLayout(false);
            this.Contents.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplashPicture)).EndInit();
            this.SplashPanel.ResumeLayout(false);
            this.SplashPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox SplashPicture;
        private System.Windows.Forms.Panel SplashPanel;
        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.Label TitleLabel;
    }
}
