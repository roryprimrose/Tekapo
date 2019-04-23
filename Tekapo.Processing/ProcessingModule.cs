namespace Tekapo.Processing
{
    using Autofac;

    public class ProcessingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JpegMediaManager>().As<IMediaManager>();
            builder.RegisterType<PathManager>().As<IPathManager>();
        }
    }
}