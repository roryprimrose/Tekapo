namespace Tekapo
{
    public interface IConfig
    {
        int MaxCollisionIncrement { get; }

        int MaxNameFormatItems { get; }

        int MaxSearchDirectoryItems { get; }
    }
}