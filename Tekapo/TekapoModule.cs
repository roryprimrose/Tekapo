namespace Tekapo
{
    using System.Linq;
    using Autofac;
    using Neovolve.Windows.Forms.Controls;

    public class TekapoModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Populate all the wizard page controls
            var pageTypes = from x in ThisAssembly.GetTypes()
                where x.IsAssignableTo<WizardPage>()
                select x;

            foreach (var pageType in pageTypes)
            {
                builder.RegisterType(pageType).AsSelf();
            }

            base.Load(builder);
        }
    }
}