namespace Tekapo
{
    using System.Configuration;

    public class Config : IConfig
    {
        public Config()
        {
            MaxCollisionIncrement = ParseInt(nameof(MaxCollisionIncrement), 1000);
            MaxNameFormatItems = ParseInt(nameof(MaxNameFormatItems), 5);
            MaxSearchDirectoryItems = ParseInt(nameof(MaxSearchDirectoryItems), 5);
        }

        private static int ParseInt(string key, int defaultValue = 0)
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