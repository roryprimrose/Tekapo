namespace Tekapo
{
    using System.Collections.Generic;

    public interface ISettingsWriter
    {
        void Save();

        void WriteNameFormatList(IEnumerable<string> values);

        void WriteSearchDirectoryList(IEnumerable<string> values);
    }
}