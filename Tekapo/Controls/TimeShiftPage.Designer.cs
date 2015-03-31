using System;

namespace Tekapo.Controls
{
    /// <summary>
    /// The <see cref="TimeShiftPage"/> class is used to display a wizard page to allow the user to specify a timeshift
    /// measured in years, months, days, hours, minutes and/or seconds.
    /// </summary>
    partial class TimeShiftPage
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            this.txtHours = new System.Windows.Forms.NumericUpDown();
            this.txtMinutes = new System.Windows.Forms.NumericUpDown();
            this.txtSeconds = new System.Windows.Forms.NumericUpDown();
            this.txtYears = new System.Windows.Forms.NumericUpDown();
            this.txtMonths = new System.Windows.Forms.NumericUpDown();
            this.txtDays = new System.Windows.Forms.NumericUpDown();
            this.ErrorDisplay = new System.Windows.Forms.ErrorProvider(this.components);
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            this.Contents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSeconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYears)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMonths)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // Contents
            // 
            this.Contents.Controls.Add(this.txtDays);
            this.Contents.Controls.Add(label6);
            this.Contents.Controls.Add(this.txtMonths);
            this.Contents.Controls.Add(label5);
            this.Contents.Controls.Add(this.txtYears);
            this.Contents.Controls.Add(label4);
            this.Contents.Controls.Add(this.txtSeconds);
            this.Contents.Controls.Add(label3);
            this.Contents.Controls.Add(this.txtMinutes);
            this.Contents.Controls.Add(label2);
            this.Contents.Controls.Add(this.txtHours);
            this.Contents.Controls.Add(label1);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(18, 19);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(35, 13);
            label1.TabIndex = 0;
            label1.Text = "Hours";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(18, 78);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(44, 13);
            label2.TabIndex = 2;
            label2.Text = "Minutes";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(18, 141);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(49, 13);
            label3.TabIndex = 4;
            label3.Text = "Seconds";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(238, 19);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(34, 13);
            label4.TabIndex = 6;
            label4.Text = "Years";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(238, 78);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(42, 13);
            label5.TabIndex = 8;
            label5.Text = "Months";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(238, 141);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(31, 13);
            label6.TabIndex = 10;
            label6.Text = "Days";
            // 
            // txtHours
            // 
            this.txtHours.Location = new System.Drawing.Point(21, 35);
            this.txtHours.Maximum = new decimal(new Int32[] {
            1000,
            0,
            0,
            0});
            this.txtHours.Minimum = new decimal(new Int32[] {
            9999,
            0,
            0,
            -2147483648});
            this.txtHours.Name = "txtHours";
            this.txtHours.Size = new System.Drawing.Size(120, 20);
            this.txtHours.TabIndex = 1;
            // 
            // txtMinutes
            // 
            this.txtMinutes.Location = new System.Drawing.Point(21, 94);
            this.txtMinutes.Maximum = new decimal(new Int32[] {
            1000,
            0,
            0,
            0});
            this.txtMinutes.Minimum = new decimal(new Int32[] {
            9999,
            0,
            0,
            -2147483648});
            this.txtMinutes.Name = "txtMinutes";
            this.txtMinutes.Size = new System.Drawing.Size(120, 20);
            this.txtMinutes.TabIndex = 3;
            // 
            // txtSeconds
            // 
            this.txtSeconds.Location = new System.Drawing.Point(21, 157);
            this.txtSeconds.Maximum = new decimal(new Int32[] {
            1000,
            0,
            0,
            0});
            this.txtSeconds.Minimum = new decimal(new Int32[] {
            9999,
            0,
            0,
            -2147483648});
            this.txtSeconds.Name = "txtSeconds";
            this.txtSeconds.Size = new System.Drawing.Size(120, 20);
            this.txtSeconds.TabIndex = 5;
            // 
            // txtYears
            // 
            this.txtYears.Location = new System.Drawing.Point(241, 35);
            this.txtYears.Maximum = new decimal(new Int32[] {
            1000,
            0,
            0,
            0});
            this.txtYears.Minimum = new decimal(new Int32[] {
            9999,
            0,
            0,
            -2147483648});
            this.txtYears.Name = "txtYears";
            this.txtYears.Size = new System.Drawing.Size(120, 20);
            this.txtYears.TabIndex = 7;
            // 
            // txtMonths
            // 
            this.txtMonths.Location = new System.Drawing.Point(241, 94);
            this.txtMonths.Maximum = new decimal(new Int32[] {
            1000,
            0,
            0,
            0});
            this.txtMonths.Minimum = new decimal(new Int32[] {
            9999,
            0,
            0,
            -2147483648});
            this.txtMonths.Name = "txtMonths";
            this.txtMonths.Size = new System.Drawing.Size(120, 20);
            this.txtMonths.TabIndex = 9;
            // 
            // txtDays
            // 
            this.txtDays.Location = new System.Drawing.Point(241, 157);
            this.txtDays.Maximum = new decimal(new Int32[] {
            1000,
            0,
            0,
            0});
            this.txtDays.Minimum = new decimal(new Int32[] {
            9999,
            0,
            0,
            -2147483648});
            this.txtDays.Name = "txtDays";
            this.txtDays.Size = new System.Drawing.Size(120, 20);
            this.txtDays.TabIndex = 11;
            // 
            // ErrorDisplay
            // 
            this.ErrorDisplay.ContainerControl = this;
            // 
            // TimeShiftPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Description = "The values entered will be used to shift all the files processed by the specified" +
                " amounts. At least one field must be populated. Negative values are supported.";
            this.Name = "TimeShiftPage";
            this.Title = "Specify time shift values";
            this.Closing += new System.EventHandler(this.TimeShiftPage_Closing);
            this.Opening += new System.EventHandler(this.TimeShiftPage_Opening);
            this.Contents.ResumeLayout(false);
            this.Contents.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSeconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYears)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMonths)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown txtDays;
        private System.Windows.Forms.NumericUpDown txtMonths;
        private System.Windows.Forms.NumericUpDown txtYears;
        private System.Windows.Forms.NumericUpDown txtSeconds;
        private System.Windows.Forms.NumericUpDown txtMinutes;
        private System.Windows.Forms.NumericUpDown txtHours;
        private System.Windows.Forms.ErrorProvider ErrorDisplay;

    }
}
