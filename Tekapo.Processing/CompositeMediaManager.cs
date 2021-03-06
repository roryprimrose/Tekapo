﻿namespace Tekapo.Processing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class CompositeMediaManager : IMediaManager
    {
        private readonly List<IMediaManager> _managers;

        public CompositeMediaManager(IEnumerable<IMediaManager> managers)
        {
            _managers = managers.ToList();
        }

        public bool CanProcess(Stream stream)
        {
            var manager = GetSupportingManager(stream);

            if (manager == null)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<string> GetSupportedFileTypes(MediaOperationType operationType)
        {
            return _managers.SelectMany(x => x.GetSupportedFileTypes(operationType)).Distinct();
        }

        public DateTime? ReadMediaCreatedDate(Stream stream)
        {
            var manager = GetSupportingManager(stream);

            if (manager == null)
            {
                // This is not a supported file
                // Possibly a file content format issue as the file extension looks like it should be supported
                return null;
            }

            return manager.ReadMediaCreatedDate(stream);
        }

        public Stream SetMediaCreatedDate(Stream stream, DateTime createdAt)
        {
            var manager = GetSupportingManager(stream);

            if (manager == null)
            {
                throw new InvalidOperationException("The file specified is not supported.");
            }

            return manager.SetMediaCreatedDate(stream, createdAt);
        }

        private IMediaManager GetSupportingManager(Stream stream)
        {
            return _managers.FirstOrDefault(x => x.CanProcess(stream));
        }
    }
}