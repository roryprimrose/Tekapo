namespace Tekapo.Processing
{
    using System;

    public interface IPathManager
    {
        string CreateFilePathWithIncrement(string path, int increment, int maxCollisionIncrement);

        string GetRenamedPath(
            string renamingFormat,
            DateTime mediaCreatedDate,
            string originalFilePath,
            bool incrementOnCollision,
            int maxCollisionIncrement);

        bool IsFormatValid(string renamingFormat);
    }
}