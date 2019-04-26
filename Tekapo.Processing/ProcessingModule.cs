namespace Tekapo.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Autofac;

    public class ProcessingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterMediaManagers(builder);

            builder.RegisterType<PathManager>().As<IPathManager>();
            builder.RegisterType<RenameProcessor>().As<IRenameProcessor>();
        }
        
        private static IEnumerable<IMediaManager> BuildMediaManagerRegistration(
            IComponentContext componentContext,
            IEnumerable<Type> resolverTypes)
        {
            var resolvers = new Collection<IMediaManager>();

            foreach (var resolverType in resolverTypes)
            {
                resolvers.Add(componentContext.ResolveNamed<IMediaManager>(resolverType.FullName));
            }

            return resolvers;
        }

        private void RegisterMediaManagers(ContainerBuilder builder)
        {
            var resolverTypes = (from x in ThisAssembly.GetTypes()
                where x.IsAssignableTo<IMediaManager>()
                      && x != typeof(CompositeMediaManager)
                      && x.IsAbstract == false
                      && x.IsInterface == false
                select x).ToList();

            resolverTypes.ForEach(x => builder.RegisterType(x).Named<IMediaManager>(x.FullName));

            builder.Register(c => BuildMediaManagerRegistration(c, resolverTypes));
            builder.RegisterType<CompositeMediaManager>().As<IMediaManager>();
        }

    }
}