namespace Tekapo
{
    /// <summary>
    ///     The <see cref="SearchFilterType" /> enumeration defines the types of filters that can be used when
    ///     searching for files.
    /// </summary>
    public enum SearchFilterType
    {
        /// <summary>
        ///     Defines that no filter will be applied.
        /// </summary>
        None = 0,

        /// <summary>
        ///     Defines that a wildcard filter will be used.
        /// </summary>
        Wildcard,

        /// <summary>
        ///     Defines that a regular expression filter will be used.
        /// </summary>
        RegularExpression
    }
}