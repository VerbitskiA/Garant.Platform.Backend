using Autofac;
using AutoMapper;
using Garant.Platform.Configurator.Abstractions;
using Garant.Platform.Configurator.Services;
using Garant.Platform.Core.Attributes;

namespace Garant.Platform.Configurator.AutofacModules
{
    /// <summary>
    /// Класс регистрации сервисов автофака.
    /// </summary>
    [CommonModule]
    public sealed class ConfiguratorModule : Module
    {
        public static void InitModules(ContainerBuilder builder)
        {
            // Сервис конфигуратора.
            builder.RegisterType<ConfiguratorService>().Named<IConfiguratorService>("ConfiguratorService");
            builder.RegisterType<ConfiguratorService>().As<IConfiguratorService>();

            builder.RegisterType<ConfiguratorRepository>().Named<IConfiguratorRepository>("ConfiguratorRepository");
            builder.RegisterType<ConfiguratorRepository>().As<IConfiguratorRepository>();

            builder.RegisterType<IMapper>().InstancePerLifetimeScope();
        }
    }
}