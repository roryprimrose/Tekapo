namespace Tekapo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autofac;
    using Neovolve.Windows.Forms.Controls;
    using Tekapo.Processing;

    public class TekapoModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Populate all the wizard page controls
            var pageTypes = (from x in ThisAssembly.GetTypes()
                where x.IsAssignableTo<WizardPage>() && x.IsAbstract == false && x.IsInterface == false
                select x).ToList();

            pageTypes.ForEach(x => builder.RegisterType(x).Named<WizardPage>(x.FullName));
            builder.Register(c => BuildWizardPageRegistration(c, pageTypes));
            builder.RegisterType<Configuration>().As<IConfiguration>().SingleInstance();
            builder.RegisterType<Settings>().As<ISettings>().SingleInstance();
            builder.RegisterType<MainForm>().AsSelf();

            base.Load(builder);
        }

        private static IList<WizardPage> BuildWizardPageRegistration(
            IComponentContext componentContext,
            IEnumerable<Type> resolverTypes)
        {
            var resolvers = new List<WizardPage>();

            foreach (var resolverType in resolverTypes)
            {
                resolvers.Add(componentContext.ResolveNamed<WizardPage>(resolverType.FullName));
            }

            return resolvers;
        }
    }
}