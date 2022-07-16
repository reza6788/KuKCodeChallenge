using System.Reflection;
using Autofac;
using Kuk.Data.Configuration;
using Module = Autofac.Module;

namespace Kuk.Services.Configuration
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterModule(new RepositoryModule());

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.IsPublic && x.IsClass && x.Name.EndsWith("Service")).AsImplementedInterfaces()
                .InstancePerLifetimeScope();

        }
    }
}
