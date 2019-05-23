namespace Tekapo
{
    using System.Configuration;

    public class Configuration : IConfiguration
    {
        public Configuration()
        {
            MaxCollisionIncrement = ParseInt(nameof(MaxCollisionIncrement), 1000);
            MaxNameFormatItems = ParseInt(nameof(MaxNameFormatItems), 5);
            MaxSearchDirectoryItems = ParseInt(nameof(MaxSearchDirectoryItems), 5);
        }

        private int ParseInt(string key, int defaultValue = 0)
        {
            var value = ConfigurationManager.AppSettings[key];

            if (int.TryParse(value, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        public int MaxCollisionIncrement { get; }

        public int MaxNameFormatItems { get; }
        public int MaxSearchDirectoryItems { get; }
    }
}