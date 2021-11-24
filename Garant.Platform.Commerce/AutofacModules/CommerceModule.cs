using Autofac;
using Garant.Platform.Commerce.Abstraction.ЮKassa;
using Garant.Platform.Commerce.Service.ЮKassa;
using Garant.Platform.Core.Attributes;

namespace Garant.Platform.Commerce.AutofacModules
{
    /// <summary>
    /// Класс регистрации сервисов автофака.
    /// </summary>
    [CommonModule]
    public sealed class CommerceModule : Module
    {
        public static void InitModules(ContainerBuilder builder)
        {
            // Сервис платежной системы ЮKassa.
            builder.RegisterType<ЮKassaService>().Named<IЮKassaService>("ЮKassaService");
            builder.RegisterType<ЮKassaService>().As<IЮKassaService>();
        }
    }
}
