namespace Tekapo
{
    public interface IConfiguration
    {
        int MaxCollisionIncrement { get; }

        int MaxNameFormatItems { get; }

        int MaxSearchDirectoryItems { get; }
    }
}