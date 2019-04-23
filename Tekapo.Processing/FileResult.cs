namespace Tekapo.Processing
{
    using System;

    /// <summary>
    ///     The <see cref="Tekapo.Processing.FileResult" /> class is used to store information about a file that has been
    ///     processed.
    /// </summary>
    [Serializable]
    public class FileResult
    {
        /// <summary>
        ///     Gets or sets the error message.
        /// </summary>
        /// <value>
        ///     The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is successful.
        /// </summary>
        /// <value>
        ///     <c>true</c>if this instance is successful; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuccessful { get; set; }

        /// <summary>
        ///     Gets or sets the new path.
        /// </summary>
        /// <value>
        ///     The new path.
        /// </value>
        public string NewPath { get; set; }

        /// <summary>
        ///     Gets or sets the new picture taken date.
        /// </summary>
        /// <value>
        ///     The new picture taken date.
        /// </value>
        public string NewPictureTakenDate { get; set; }

        /// <summary>
        ///     Gets or sets the original path.
        /// </summary>
        /// <value>
        ///     The original path.
        /// </value>
        public string OriginalPath { get; set; }

        /// <summary>
        ///     Gets or sets the original picture taken date.
        /// </summary>
        /// <value>
        ///     The original picture taken date.
        /// </value>
        public string OriginalPictureTakenDate { get; set; }
    }
}