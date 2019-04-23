namespace Tekapo.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CompositeMediaManager : IMediaManager
    {
        private readonly List<IMediaManager> _managers;

        public CompositeMediaManager(IEnumerable<IMediaManager> managers)
        {
            _managers = managers.ToList();
        }

        public IEnumerable<string> GetSupportedFileTypes()
        {
            return _managers.SelectMany(x => x.GetSupportedFileTypes()).Distinct();
        }

        public bool IsSupported(string filePath)
        {
            return _managers.Any(x => x.IsSupported(filePath));
        }

        public DateTime ReadMediaCreatedDate(string filePath)
        {
            var manager = GetSupportingManager(filePath);

            if (manager == null)
            {
                throw new InvalidOperationException("The file specified is not supported.");
            }

            return manager.ReadMediaCreatedDate(filePath);
        }

        public void SetMediaCreatedDate(string filePath, DateTime createdAt)
        {
            var manager = GetSupportingManager(filePath);

            if (manager == null)
            {
                throw new InvalidOperationException("The file specified is not supported.");
            }

            manager.SetMediaCreatedDate(filePath, createdAt);
        }

        private IMediaManager GetSupportingManager(string filePath)
        {
            return _managers.FirstOrDefault(x => x.IsSupported(filePath));
        }
    }
}