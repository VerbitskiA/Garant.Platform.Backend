using Autofac;
using Garant.Platform.Commerce.Abstraction;
using Garant.Platform.Commerce.Service;
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
            // Сервис платежных систем.
            builder.RegisterType<CommerceService>().Named<ICommerceService>("CommerceService");
            builder.RegisterType<CommerceService>().As<ICommerceService>();
        }
    }
}
