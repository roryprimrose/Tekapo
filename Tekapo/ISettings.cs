namespace Tekapo
{
    using System.ComponentModel;

    public interface ISettings
    {
        bool IncrementOnCollision { get; set; }

        string NameFormat { get; set; }

        BindingList<string> NameFormatList { get; }

        bool RecursiveSearch { get; set; }

        string RegularExpressionFilter { get; set; }

        BindingList<string> SearchDirectoryList { get; }

        SearchFilterType SearchFilterType { get; set; }

        string SearchPath { get; set; }

        string WildcardFilter { get; set; }
    }
}