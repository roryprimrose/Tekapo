namespace Tekapo.Processing
{
    using System;

    [Serializable]
    public class FileResult
    {
        public string ErrorMessage { get; set; }

        public bool IsSuccessful { get; set; }

        public string NewMediaCreatedDate { get; set; }

        public string NewPath { get; set; }

        public string OriginalMediaCreatedDate { get; set; }

        public string OriginalPath { get; set; }
    }
}