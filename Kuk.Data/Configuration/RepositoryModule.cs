using System.Reflection;
using Autofac;
using Kuk.Data.Common;
using Module = Autofac.Module;

namespace Kuk.Data.Configuration
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<KukDbContext>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.IsPublic && x.IsClass && x.Name.EndsWith("Repository")).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
