using System;
namespace Tekapo.Controls
{
    partial class SelectPathPage
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
            this.Browse = new System.Windows.Forms.Button();
            this.Recurse = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.UseRegularExpression = new System.Windows.Forms.RadioButton();
            this.Wildcard = new System.Windows.Forms.TextBox();
            this.UseWildcard = new System.Windows.Forms.RadioButton();
            this.UseNoFilter = new System.Windows.Forms.RadioButton();
            this.errProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.SearchPath = new System.Windows.Forms.ComboBox();
            this.Expression = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            this.Contents.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // Contents
            // 
            this.Contents.Controls.Add(this.SearchPath);
            this.Contents.Controls.Add(this.groupBox1);
            this.Contents.Controls.Add(this.Recurse);
            this.Contents.Controls.Add(this.Browse);
            this.Contents.Controls.Add(label1);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(16, 12);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(79, 13);
            label1.TabIndex = 3;
            label1.Text = "Path to search:";
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(405, 26);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(75, 23);
            this.Browse.TabIndex = 1;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // Recurse
            // 
            this.Recurse.AutoSize = true;
            this.Recurse.Checked = true;
            this.Recurse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Recurse.Location = new System.Drawing.Point(19, 54);
            this.Recurse.Name = "Recurse";
            this.Recurse.Size = new System.Drawing.Size(111, 17);
            this.Recurse.TabIndex = 2;
            this.Recurse.Text = "Search subfolders";
            this.Recurse.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Expression);
            this.groupBox1.Controls.Add(this.UseRegularExpression);
            this.groupBox1.Controls.Add(this.Wildcard);
            this.groupBox1.Controls.Add(this.UseWildcard);
            this.groupBox1.Controls.Add(this.UseNoFilter);
            this.groupBox1.Location = new System.Drawing.Point(19, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(461, 172);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtering";
            // 
            // UseRegularExpression
            // 
            this.UseRegularExpression.AutoSize = true;
            this.UseRegularExpression.Location = new System.Drawing.Point(14, 93);
            this.UseRegularExpression.Name = "UseRegularExpression";
            this.UseRegularExpression.Size = new System.Drawing.Size(138, 17);
            this.UseRegularExpression.TabIndex = 6;
            this.UseRegularExpression.Text = "Regular Expression filter";
            this.UseRegularExpression.UseVisualStyleBackColor = true;
            this.UseRegularExpression.CheckedChanged += new System.EventHandler(this.UseRegularExpression_CheckedChanged);
            // 
            // Wildcard
            // 
            this.Wildcard.Enabled = false;
            this.Wildcard.Location = new System.Drawing.Point(33, 67);
            this.Wildcard.Name = "Wildcard";
            this.Wildcard.Size = new System.Drawing.Size(338, 20);
            this.Wildcard.TabIndex = 5;
            // 
            // UseWildcard
            // 
            this.UseWildcard.AutoSize = true;
            this.UseWildcard.Location = new System.Drawing.Point(14, 44);
            this.UseWildcard.Name = "UseWildcard";
            this.UseWildcard.Size = new System.Drawing.Size(89, 17);
            this.UseWildcard.TabIndex = 4;
            this.UseWildcard.Text = "Wildcard filter";
            this.UseWildcard.UseVisualStyleBackColor = true;
            this.UseWildcard.CheckedChanged += new System.EventHandler(this.UseWildcard_CheckedChanged);
            // 
            // UseNoFilter
            // 
            this.UseNoFilter.AutoSize = true;
            this.UseNoFilter.Checked = true;
            this.UseNoFilter.Location = new System.Drawing.Point(14, 19);
            this.UseNoFilter.Name = "UseNoFilter";
            this.UseNoFilter.Size = new System.Drawing.Size(61, 17);
            this.UseNoFilter.TabIndex = 3;
            this.UseNoFilter.TabStop = true;
            this.UseNoFilter.Text = "No filter";
            this.UseNoFilter.UseVisualStyleBackColor = true;
            // 
            // errProvider
            // 
            this.errProvider.ContainerControl = this;
            // 
            // SearchPath
            // 
            this.SearchPath.FormattingEnabled = true;
            this.SearchPath.Location = new System.Drawing.Point(19, 28);
            this.SearchPath.Name = "SearchPath";
            this.SearchPath.Size = new System.Drawing.Size(371, 21);
            this.SearchPath.TabIndex = 6;
            // 
            // Expression
            // 
            this.Expression.Enabled = false;
            this.Expression.Location = new System.Drawing.Point(33, 116);
            this.Expression.Name = "Expression";
            this.Expression.Size = new System.Drawing.Size(338, 20);
            this.Expression.TabIndex = 7;
            // 
            // SelectPathPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Description = "Select the path to search and any filters to apply to the results. Select Skip to" +
                " specify files manually without searching for files.";
            this.Name = "SelectPathPage";
            this.PageSettings.CancelButtonSettings.Text = "Cancel";
            this.PageSettings.CustomButtonSettings.Text = "Custom";
            this.PageSettings.CustomButtonSettings.Visible = false;
            this.PageSettings.HelpButtonSettings.Text = "Help";
            this.PageSettings.HelpButtonSettings.Visible = false;
            this.PageSettings.NextButtonSettings.Text = "Next";
            this.PageSettings.BackButtonSettings.Text = "Back";
            this.Title = "Select Path";
            this.Closing += new System.EventHandler(this.SelectPathPage_Closing);
            this.Opening += new System.EventHandler(this.SelectPathPage_Opening);
            this.Contents.ResumeLayout(false);
            this.Contents.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.CheckBox Recurse;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton UseRegularExpression;
        private System.Windows.Forms.TextBox Wildcard;
        private System.Windows.Forms.RadioButton UseWildcard;
        private System.Windows.Forms.RadioButton UseNoFilter;
        private System.Windows.Forms.ErrorProvider errProvider;
        private System.Windows.Forms.ComboBox SearchPath;
        private System.Windows.Forms.TextBox Expression;
    }
}
