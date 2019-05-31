namespace Tekapo
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class SettingsWriter : ISettingsWriter
    {
        public void Save()
        {
            Properties.Settings.Default.Save();
        }

        public void WriteNameFormatList(IEnumerable<string> values)
        {
            Properties.Settings.Default.NameFormatMRU = SerializeValues(values);
        }

        public void WriteSearchDirectoryList(IEnumerable<string> values)
        {
            Properties.Settings.Default.SearchDirectoryMRU = JsonConvert.SerializeObject(values);
        }

        private static string SerializeValues(IEnumerable<string> values)
        {
            if (values == null)
            {
                return "[]";
            }

            return JsonConvert.SerializeObject(values);
        }
    }
}