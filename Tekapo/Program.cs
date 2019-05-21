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
        private static IContainer BuildContainer(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<TekapoModule>();
            builder.RegisterModule<ProcessingModule>();

            builder.Register(x => new ExecutionContext(args)).As<IExecutionContext>();

            return builder.Build();
        }

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(params string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
        var container = BuildContainer(args);

            var form = container.Resolve<MainForm>();

            Application.Run(form);
        }
    }
}