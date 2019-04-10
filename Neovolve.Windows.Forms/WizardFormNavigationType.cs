namespace Neovolve.Windows.Forms
{
    /// <summary>
    ///     Defines the types of wizard page navigations available.
    /// </summary>
    public enum WizardFormNavigationType
    {
        /// <summary>
        ///     Defines that a previous navigation should be invoked.
        /// </summary>
        Previous,

        /// <summary>
        ///     Defines that a next navigation should be invoked.
        /// </summary>
        Next,

        /// <summary>
        ///     Defines that a cancel navigation should be invoked.
        /// </summary>
        Cancel,

        /// <summary>
        ///     Defines that a help navigation should be invoked.
        /// </summary>
        Help,

        /// <summary>
        ///     Defines that a custom navigation should be invoked.
        /// </summary>
        Custom,

        /// <summary>
        ///     Defines that a navigation to a specific navigation key should be invoked.
        /// </summary>
        NavigationKey,

        /// <summary>
        ///     Defines that no navigation action should be taken.
        /// </summary>
        Ignore
    }
}