using Autofac;
using Garant.Platform.Core.Attributes;
using Garant.Platform.Integrations.Abstraction.YandexMaps;
using Garant.Platform.Integrations.Service.YandexMaps;

namespace Garant.Platform.Integrations.AutofacModules
{
    /// <summary>
    /// Класс регистрации сервисов автофака.
    /// </summary>
    [CommonModule]
    public class IntegrationModule : Module
    {
        public static void InitModules(ContainerBuilder builder)
        {
            // Сервис работы с картами яндекса.
            builder.RegisterType<YandexMapsService>().Named<IYandexMapsService>("YandexMapsService");
            builder.RegisterType<YandexMapsService>().As<IYandexMapsService>();
        }
    }
}
