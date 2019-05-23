namespace Tekapo
{
    using System;
    using System.ComponentModel;
    using Newtonsoft.Json;
    using Tekapo.Properties;

    public class Settings : ISettings
    {
        private BindingList<string> ReadList(string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<BindingList<string>>(value) ?? new BindingList<string>();
            }
            catch (JsonReaderException)
            {
                return new BindingList<string>();
            }
        }

        public bool IncrementOnCollision
        {
            get => Properties.Settings.Default.IncrementOnCollision;
            set => Properties.Settings.Default.IncrementOnCollision = value;
        }

        public string NameFormat { get => Properties.Settings.Default.NameFormat; set => Properties.Settings.Default.NameFormat = value; }

        public BindingList<string> NameFormatList => ReadList(Properties.Settings.Default.NameFormatMRU);

        public bool RecursiveSearch
        {
            get => Properties.Settings.Default.SearchSubDirectories;
            set => Properties.Settings.Default.SearchSubDirectories = value;
        }

        public string RegularExpressionFilter
        {
            get => Properties.Settings.Default.RegularExpressionSearchFilter;
            set => Properties.Settings.Default.RegularExpressionSearchFilter = value;
        }

        public BindingList<string> SearchDirectoryList => ReadList(Properties.Settings.Default.SearchDirectoryMRU);

        public SearchFilterType SearchFilterType
        {
            get
            {
                var value = Properties.Settings.Default.SearchFilterType;

                if (Enum.TryParse(value, out SearchFilterType result))
                {
                    return result;
                }

                return SearchFilterType.None;
            }
            set => Properties.Settings.Default.SearchFilterType = value.ToString();
        }

        public string SearchPath
        {
            get => Properties.Settings.Default.LastSearchDirectory;
            set => Properties.Settings.Default.LastSearchDirectory = value;
        }

        public string WildcardFilter
        {
            get => Properties.Settings.Default.WildcardSearchFilter;
            set => Properties.Settings.Default.WildcardSearchFilter = value;
        }
    }
}