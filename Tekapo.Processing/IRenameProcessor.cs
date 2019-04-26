namespace Tekapo.Processing
{
    public interface IRenameProcessor
    {
        FileResult RenameFile(string filePath, RenameConfiguration config);
    }
}