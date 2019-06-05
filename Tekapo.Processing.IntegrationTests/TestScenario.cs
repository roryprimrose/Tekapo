namespace Tekapo.Processing.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class TestScenario : IDisposable
    {
        public TestScenario(string basePath, params string[] paths)
        {
            // Create the paths
            ScenarioDirectory = Path.Combine(basePath, Guid.NewGuid().ToString("N"));

            Directory.CreateDirectory(ScenarioDirectory);

            foreach (var path in paths)
            {
                var filePath = Path.Combine(ScenarioDirectory, path);

                var directory = Path.GetDirectoryName(filePath);

                if (Directory.Exists(directory) == false)
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(filePath, Guid.NewGuid().ToString());

                Files.Add(filePath);
            }
        }

        public void Dispose()
        {
            foreach (var file in Files)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }

            Directory.Delete(ScenarioDirectory, true);
            Files.Clear();
        }

        public List<string> Files { get; } = new List<string>();

        public string ScenarioDirectory { get; }
    }
}