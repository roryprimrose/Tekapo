using System;

namespace Neovolve.Windows.Forms.Controls
{
    /// <summary>
    /// The <see cref="WizardBannerPage"/> class is a control that inherits from the
    /// <see cref="WizardPage"/> control to provide a wizard page that has a banner at the top of the page.
    /// </summary>
    partial class WizardBannerPage
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
            this.BannerPanel = new System.Windows.Forms.Panel();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.BannerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Contents
            // 
            this.Contents.Location = new System.Drawing.Point(0, 73);
            this.Contents.Padding = new System.Windows.Forms.Padding(0, 71, 0, 0);
            this.Contents.Size = new System.Drawing.Size(495, 264);
            // 
            // BannerPanel
            // 
            this.BannerPanel.BackColor = System.Drawing.Color.White;
            this.BannerPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BannerPanel.Controls.Add(this.TitleLabel);
            this.BannerPanel.Controls.Add(this.DescriptionLabel);
            this.BannerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.BannerPanel.Location = new System.Drawing.Point(0, 0);
            this.BannerPanel.Name = "BannerPanel";
            this.BannerPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.BannerPanel.Size = new System.Drawing.Size(495, 73);
            this.BannerPanel.TabIndex = 0;
            this.BannerPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.BannerPanel_Paint);
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(18, 10);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(0, 13);
            this.TitleLabel.TabIndex = 1;
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.BackColor = System.Drawing.Color.Transparent;
            this.DescriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DescriptionLabel.Location = new System.Drawing.Point(32, 32);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(460, 38);
            this.DescriptionLabel.TabIndex = 2;
            // 
            // WizardBannerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.BannerPanel);
            this.Name = "WizardBannerPage";
            this.Controls.SetChildIndex(this.BannerPanel, 0);
            this.Controls.SetChildIndex(this.Contents, 0);
            this.BannerPanel.ResumeLayout(false);
            this.BannerPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BannerPanel;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label DescriptionLabel;
    }
}
