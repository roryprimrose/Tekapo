namespace Tekapo.Processing
{
    using System;
    using EnsureThat;

    public class PathProgressEventArgs : EventArgs
    {
        public PathProgressEventArgs(string path, int currentItem, int totalItems)
        {
            Ensure.String.IsNotNullOrWhiteSpace(path);

            Path = path;
            CurrentItem = currentItem;
            TotalItems = totalItems;
        }

        public static PathProgressEventArgs For(string path, int currentItem, int totalItems)
        {
            return new PathProgressEventArgs(path, currentItem, totalItems);
        }

        public int CurrentItem { get; set; }

        public string Path { get; set; }

        public int TotalItems { get; set; }
    }
}