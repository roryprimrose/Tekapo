using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace Neovolve.Windows.Forms.Test
{
    /// <summary>
    /// Invoke key page.
    /// </summary>
    public partial class InvokeKeyPage : Controls.WizardBannerPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvokeKeyPage"/> class.
        /// </summary>
        public InvokeKeyPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Output tab by control.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <param name="item">
        /// The control.
        /// </param>
        private static void OutputTabByControl(IList items, Control item)
        {
            items.Add(item);

            foreach (Control child in item.Controls)
            {
                OutputTabByControl(items, child);
            }
        }

        /// <summary>
        /// Output tab ordering.
        /// </summary>
        private void OutputTabOrdering()
        {
            ArrayList items = new ArrayList();

            OutputTabByControl(items, ParentForm);

            items.Sort(new IndexComparer());

            foreach (Object item in items)
            {
                Control example = item as Control;

                if (example != null)
                {
                    Debug.WriteLine(
                        example.GetType().Name + " - " + example.Name + " - " + example.CanFocus + " - "
                        + example.TabIndex);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the TestButton control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void TestButton_Click(Object sender, EventArgs e)
        {
            OutputTabOrdering();

            InvokeNavigation("Page 1");
        }

        /// <summary>
        /// Index comparer.
        /// </summary>
        private class IndexComparer : IComparer
        {
            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">
            /// The first object to compare.
            /// </param>
            /// <param name="y">
            /// The second object to compare.
            /// </param>
            /// <returns>
            /// Value
            /// Condition
            /// Less than zero
            /// <paramref name="x"/>is less than <paramref name="y"/>.
            /// Zero
            /// <paramref name="x"/>equals <paramref name="y"/>.
            /// Greater than zero
            /// <paramref name="x"/>is greater than <paramref name="y"/>.
            /// </returns>
            /// <exception cref="T:System.ArgumentException">
            /// Neither <paramref name="x"/> nor <paramref name="y"/> implements the <see cref="T:System.IComparable"/> interface.
            /// -or-
            /// <paramref name="x"/>and <paramref name="y"/> are of different types and neither one can handle comparisons with the other.
            /// </exception>
            public Int32 Compare(Object x, Object y)
            {
                Control controlX = x as Control;
                Control controlY = y as Control;

                if (controlX == null
                    || controlY == null)
                {
                    return 0;
                }

                if (controlX.TabIndex
                    == controlY.TabIndex)
                {
                    return 0;
                }

                if (controlX.TabIndex
                    < controlY.TabIndex)
                {
                    return -1;
                }

                return 1;
            }
        }
    }
}