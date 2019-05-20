namespace Tekapo
{
    using System;
    using System.Windows.Forms;
    using Autofac;
    using Tekapo.Processing;

    /// <summary>
    ///     The <see cref="Program" />
    ///     class is used to provide the entry point of the program.
    /// </summary>
    internal static class Program
    {
        private static readonly IContainer _container = BuildContainer();

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<TekapoModule>();
            builder.RegisterModule<ProcessingModule>();

            return builder.Build();
        }

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = _container.Resolve<MainForm>();

            Application.Run(form);
        }
    }
}