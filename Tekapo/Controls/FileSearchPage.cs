namespace Tekapo.Controls
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using EnsureThat;
    using Tekapo.Processing;
    using Tekapo.Properties;

    public partial class FileSearchPage : ProcessingPage
    {
        private readonly IExecutionContext _executionContext;
        private readonly IFileSearcher _fileSearcher;
        private readonly ISettings _settings;

        [SuppressMessage("Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "The reference is held by the form and cannot be disposed here.")]
        public FileSearchPage(IFileSearcher fileSearcher, IExecutionContext executionContext, ISettings settings)
        {
            Ensure.Any.IsNotNull(fileSearcher, nameof(fileSearcher));
            Ensure.Any.IsNotNull(executionContext, nameof(executionContext));
            Ensure.Any.IsNotNull(settings, nameof(settings));

            _fileSearcher = fileSearcher;
            _executionContext = executionContext;
            _settings = settings;

            components = new Container();

            InitializeComponent();

            _fileSearcher.EvaluatingPath += FileSearcherOnEvaluatingPath;

            var disposeTrigger = new DisposeTrigger(disposing =>
            {
                if (disposing)
                {
                    _fileSearcher.EvaluatingPath -= FileSearcherOnEvaluatingPath;
                }
            });

            components.Add(disposeTrigger);
        }

        protected override void ProcessTask()
        {
            SetProgressStatus(Resources.ProcessCommandLineArguments);

            var searchPaths = _executionContext.SearchPaths.ToList();

            if (searchPaths.Count == 0)
            {
                // There are no command line arguments so we will use the search path identified in the previous wizard page
                searchPaths.Add(_settings.SearchPath);
            }

            var taskType = (TaskType) State[Tekapo.State.TaskKey];
            var operationType = taskType.AsMediaOperationType();

            var filesFound = _fileSearcher.FindSupportedFiles(searchPaths, operationType).ToList();

            var fileList = new BindingList<string>(filesFound);

            // Store the list of files found
            State[Tekapo.State.FileListKey] = fileList;
        }

        /// <summary>
        ///     Sets the search status.
        /// </summary>
        /// <param name="value">
        ///     The value.
        /// </param>
        protected void SetProgressStatus(string value)
        {
            // Check if a thread switch is required
            if (ProgressStatus.InvokeRequired)
            {
                object[] args = {value};

                ProgressStatus.Invoke(new StringThreadSwitch(SetProgressStatus), args);

                return;
            }

            Debug.WriteLine(value);

            // Assign the value
            ProgressStatus.Text = value;
        }

        private void FileSearcherOnEvaluatingPath(object sender, PathEventArgs e)
        {
            SetProgressStatus(e.Path);
        }
    }
}