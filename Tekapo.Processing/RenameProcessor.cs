namespace Tekapo.Processing
{
    using System;
    using System.IO;

    public class RenameProcessor : IRenameProcessor
    {
        private readonly IMediaManager _mediaManager;
        private readonly IPathManager _pathManager;

        public RenameProcessor(IMediaManager mediaManager, IPathManager pathManager)
        {
            _mediaManager = mediaManager;
            _pathManager = pathManager;
        }

        public FileResult RenameFile(string filePath, RenameConfiguration config)
        {
            // Calculate the new path
            var result = new FileResult {OriginalPath = filePath};

            DateTime? currentTime;

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                currentTime = _mediaManager.ReadMediaCreatedDate(stream);
            }

            if (currentTime == null)
            {
                result.ErrorMessage =
                    "The date and time the media was created could not be determined, skipping this file.";

                return result;
            }

            var newPath = _pathManager.GetRenamedPath(config.RenameFormat,
                currentTime.Value,
                filePath,
                config.IncrementOnCollision,
                config.MaxCollisionIncrement);

            result.NewPath = newPath;

            // Check that the file paths are different
            if (filePath == newPath)
            {
                result.ErrorMessage =
                    "The calculated file path is the same as the original file path, skipping this file.";

                return result;
            }

            try
            {
                // Get the parent directory path
                var parentDirectory = Path.GetDirectoryName(newPath);

                // Check if the directory exists
                if (Directory.Exists(parentDirectory) == false)
                {
                    // Create the directory
                    Directory.CreateDirectory(parentDirectory);
                }

                // Check if collision checks are in place
                if (config.IncrementOnCollision)
                {
                    // Get the file named without an increment and the path with the first increment
                    var newPathWithoutIncrement = _pathManager.GetRenamedPath(config.RenameFormat,
                        currentTime.Value,
                        filePath,
                        false,
                        config.MaxCollisionIncrement);
                    var firstIncrementPath = _pathManager.CreateFilePathWithIncrement(newPathWithoutIncrement,
                        1,
                        config.MaxCollisionIncrement);

                    // Check if the first file needs to be renamed
                    if (File.Exists(newPathWithoutIncrement)
                        && File.Exists(firstIncrementPath) == false)
                    {
                        // Rename the original file as the first increment
                        File.Move(newPathWithoutIncrement, firstIncrementPath);
                    }
                }

                // Rename the file
                File.Move(filePath, newPath);

                result.IsSuccessful = true;
            }
            catch (IOException ex)
            {
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}