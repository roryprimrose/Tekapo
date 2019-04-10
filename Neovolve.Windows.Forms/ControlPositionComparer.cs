namespace Neovolve.Windows.Forms
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Forms;

    /// <summary>
    ///     The <see cref="ControlPositionComparer" />
    ///     class is used to compare positions of <see cref="Control" /> instances.
    /// </summary>
    internal class ControlPositionComparer : IComparer<Control>, IComparer
    {
        /// <summary>
        ///     Defines the control vertical alignment values.
        /// </summary>
        private enum ControlVerticalAlignment : byte
        {
            /// <summary>
            ///     No vertical alignment is used.
            /// </summary>
            None = 1,

            /// <summary>
            ///     Controls are aligned by their top position.
            /// </summary>
            Top = 2,

            /// <summary>
            ///     Controls are aligned by their middle position.
            /// </summary>
            Middle = 3,

            /// <summary>
            ///     Controls are aligned by their bottom position.
            /// </summary>
            Bottom = 4
        }

        /// <summary>
        ///     Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">
        ///     The first object to compare.
        /// </param>
        /// <param name="y">
        ///     The second object to compare.
        /// </param>
        /// <returns>
        ///     Value Condition Less than zerox is less than y.Zerox equals y.Greater than zerox is greater than y.
        /// </returns>
        public int Compare(Control x, Control y)
        {
            // Ensure that there are two controls to compare
            if (x == null
                || y == null)
            {
                // There is not a valid comparison
                return 0;
            }

            // Attempt to determine whether the controls are vertically aligned against each other
            var virticalAlignment = DetermineControlVerticalAlignment(x, y);

            // Compare the positions of the controls
            if (virticalAlignment != ControlVerticalAlignment.None)
            {
                return x.Left.CompareTo(y.Left);
            }

            if (x.Top < y.Top)
            {
                return -1;
            }

            if (x.Top > y.Top)
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        ///     Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">
        ///     The first object to compare.
        /// </param>
        /// <param name="y">
        ///     The second object to compare.
        /// </param>
        /// <returns>
        ///     Value Condition Less than zero x is less than y. Zero x equals y. Greater than zero x is greater than y.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        ///     Neither x nor y implements the <see cref="T:System.IComparable"></see> interface.-or- x and y are of different
        ///     types and neither one can handle comparisons with the other.
        /// </exception>
        public int Compare(object x, object y)
        {
            // Check if the objects are controls
            var first = x as Control;
            var second = y as Control;

            if (first != null
                && second != null)
            {
                return Compare(first, second);
            }

            return 0;
        }

        /// <summary>
        ///     Determines the control vertical alignment.
        /// </summary>
        /// <param name="firstControl">
        ///     The first control.
        /// </param>
        /// <param name="secondControl">
        ///     The second control.
        /// </param>
        /// <returns>
        ///     A <see cref="ControlVerticalAlignment" /> value.
        /// </returns>
        private static ControlVerticalAlignment DetermineControlVerticalAlignment(
            Control firstControl,
            Control secondControl)
        {
            if (firstControl.Top == secondControl.Top)
            {
                return ControlVerticalAlignment.Top;
            }

            if (firstControl.Bottom == secondControl.Bottom)
            {
                return ControlVerticalAlignment.Bottom;
            }

            // Check if the controls are aligned to their middle
            // This check should also allow for the alignments to be calculated as one pixel out
            var middleX = firstControl.Top + firstControl.Height / 2;
            var middleY = secondControl.Top + secondControl.Height / 2;

            if (middleX == middleY
                || Convert.ToInt32(Math.Abs(middleX - middleY)) == 1)
            {
                return ControlVerticalAlignment.Middle;
            }

            return ControlVerticalAlignment.None;
        }
    }
}