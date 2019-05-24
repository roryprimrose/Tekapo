namespace Tekapo.Processing
{
    using System;
    using EnsureThat;

    public class PathEventArgs : EventArgs
    {
        public PathEventArgs(string path)
        {
            Ensure.String.IsNotNullOrWhiteSpace(path);

            Path = path;
        }

        public static PathEventArgs For(string path)
        {
            return new PathEventArgs(path);
        }

        public string Path { get; }
    }
}