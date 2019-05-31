namespace Tekapo
{
    using System;
    using Autofac;
    using EnsureThat;
    using Tekapo.Processing;

    public class Startup : IStartable
    {
        private readonly IExecutionContext _executionContext;
        private readonly ISettings _settings;

        public Startup(IExecutionContext executionContext, ISettings settings)
        {
            Ensure.Any.IsNotNull(executionContext, nameof(executionContext));
            Ensure.Any.IsNotNull(settings, nameof(settings));

            _executionContext = executionContext;
            _settings = settings;
        }

        public void Start()
        {
            // Determine whether there is a directory path in the commandline arguments
            var commandLinePath = _executionContext.SearchDirectory;

            // Check if there is a single search path
            if (string.IsNullOrWhiteSpace(commandLinePath) == false)
            {
                _settings.SearchPath = commandLinePath;

                return;
            }

            if (string.IsNullOrWhiteSpace(_settings.SearchPath))
            {
                // Set the search path to the personal directory
                _settings.SearchPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }
        }
    }
}