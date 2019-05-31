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
            this.PathSearchLabel = new System.Windows.Forms.Label();
            this.Browse = new System.Windows.Forms.Button();
            this.Recurse = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Expression = new System.Windows.Forms.TextBox();
            this.UseRegularExpression = new System.Windows.Forms.RadioButton();
            this.Wildcard = new System.Windows.Forms.TextBox();
            this.UseWildcard = new System.Windows.Forms.RadioButton();
            this.UseNoFilter = new System.Windows.Forms.RadioButton();
            this.errProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.SearchPath = new System.Windows.Forms.ComboBox();
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
            this.Contents.Controls.Add(this.PathSearchLabel);
            // 
            // PathSearchLabel
            // 
            this.PathSearchLabel.AutoSize = true;
            this.PathSearchLabel.Location = new System.Drawing.Point(32, 23);
            this.PathSearchLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.PathSearchLabel.Name = "PathSearchLabel";
            this.PathSearchLabel.Size = new System.Drawing.Size(157, 25);
            this.PathSearchLabel.TabIndex = 3;
            this.PathSearchLabel.Text = "Path to search:";
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(810, 50);
            this.Browse.Margin = new System.Windows.Forms.Padding(6);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(150, 44);
            this.Browse.TabIndex = 1;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // Recurse
            // 
            this.Recurse.AutoSize = true;
            this.Recurse.Checked = global::Tekapo.Properties.Settings.Default.SearchSubDirectories;
            this.Recurse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Recurse.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Tekapo.Properties.Settings.Default, "SearchSubDirectories", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Recurse.Location = new System.Drawing.Point(38, 104);
            this.Recurse.Margin = new System.Windows.Forms.Padding(6);
            this.Recurse.Name = "Recurse";
            this.Recurse.Size = new System.Drawing.Size(218, 29);
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
            this.groupBox1.Location = new System.Drawing.Point(38, 154);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(922, 331);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtering";
            // 
            // Expression
            // 
            this.Expression.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Tekapo.Properties.Settings.Default, "RegularExpressionSearchFilter", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Expression.Enabled = false;
            this.Expression.Location = new System.Drawing.Point(66, 223);
            this.Expression.Margin = new System.Windows.Forms.Padding(6);
            this.Expression.Name = "Expression";
            this.Expression.Size = new System.Drawing.Size(672, 31);
            this.Expression.TabIndex = 7;
            this.Expression.Text = global::Tekapo.Properties.Settings.Default.RegularExpressionSearchFilter;
            // 
            // UseRegularExpression
            // 
            this.UseRegularExpression.AutoSize = true;
            this.UseRegularExpression.Location = new System.Drawing.Point(28, 179);
            this.UseRegularExpression.Margin = new System.Windows.Forms.Padding(6);
            this.UseRegularExpression.Name = "UseRegularExpression";
            this.UseRegularExpression.Size = new System.Drawing.Size(278, 29);
            this.UseRegularExpression.TabIndex = 6;
            this.UseRegularExpression.Text = "Regular Expression filter";
            this.UseRegularExpression.UseVisualStyleBackColor = true;
            this.UseRegularExpression.CheckedChanged += new System.EventHandler(this.UseRegularExpression_CheckedChanged);
            // 
            // Wildcard
            // 
            this.Wildcard.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Tekapo.Properties.Settings.Default, "WildcardSearchFilter", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Wildcard.Enabled = false;
            this.Wildcard.Location = new System.Drawing.Point(66, 129);
            this.Wildcard.Margin = new System.Windows.Forms.Padding(6);
            this.Wildcard.Name = "Wildcard";
            this.Wildcard.Size = new System.Drawing.Size(672, 31);
            this.Wildcard.TabIndex = 5;
            this.Wildcard.Text = global::Tekapo.Properties.Settings.Default.WildcardSearchFilter;
            // 
            // UseWildcard
            // 
            this.UseWildcard.AutoSize = true;
            this.UseWildcard.Location = new System.Drawing.Point(28, 85);
            this.UseWildcard.Margin = new System.Windows.Forms.Padding(6);
            this.UseWildcard.Name = "UseWildcard";
            this.UseWildcard.Size = new System.Drawing.Size(174, 29);
            this.UseWildcard.TabIndex = 4;
            this.UseWildcard.Text = "Wildcard filter";
            this.UseWildcard.UseVisualStyleBackColor = true;
            this.UseWildcard.CheckedChanged += new System.EventHandler(this.UseWildcard_CheckedChanged);
            // 
            // UseNoFilter
            // 
            this.UseNoFilter.AutoSize = true;
            this.UseNoFilter.Checked = true;
            this.UseNoFilter.Location = new System.Drawing.Point(28, 37);
            this.UseNoFilter.Margin = new System.Windows.Forms.Padding(6);
            this.UseNoFilter.Name = "UseNoFilter";
            this.UseNoFilter.Size = new System.Drawing.Size(117, 29);
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
            this.SearchPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Tekapo.Properties.Settings.Default, "LastSearchDirectory", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.SearchPath.FormattingEnabled = true;
            this.SearchPath.Location = new System.Drawing.Point(38, 54);
            this.SearchPath.Margin = new System.Windows.Forms.Padding(6);
            this.SearchPath.Name = "SearchPath";
            this.SearchPath.Size = new System.Drawing.Size(738, 33);
            this.SearchPath.TabIndex = 6;
            this.SearchPath.Text = global::Tekapo.Properties.Settings.Default.LastSearchDirectory;
            // 
            // SelectPathPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.Description = "Select the path to search and any filters to apply to the results. Select Skip to" +
    " specify files manually without searching for files.";
            this.Margin = new System.Windows.Forms.Padding(12);
            this.Name = "SelectPathPage";
            this.PageSettings.BackButtonSettings.Text = "Back";
            this.PageSettings.CancelButtonSettings.Text = "Cancel";
            this.PageSettings.CustomButtonSettings.Text = "Custom";
            this.PageSettings.CustomButtonSettings.Visible = false;
            this.PageSettings.HelpButtonSettings.Text = "Help";
            this.PageSettings.HelpButtonSettings.Visible = false;
            this.PageSettings.NextButtonSettings.Text = "Next";
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
        private System.Windows.Forms.Label PathSearchLabel;
    }
}
