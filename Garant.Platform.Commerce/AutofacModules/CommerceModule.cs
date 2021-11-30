using Autofac;
using Garant.Platform.Commerce.Abstraction.Garant.Customer;
using Garant.Platform.Commerce.Abstraction.Tinkoff;
using Garant.Platform.Commerce.Abstraction.ЮKassa;
using Garant.Platform.Commerce.Service.Garant.Customer;
using Garant.Platform.Commerce.Service.Tinkoff;
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

            // Сервис платежной системы Тинькофф.
            builder.RegisterType<TinkoffService>().Named<ITinkoffService>("TinkoffService");
            builder.RegisterType<TinkoffService>().As<ITinkoffService>();
            builder.RegisterType<TinkoffRepository>().Named<ITinkoffRepository>("TinkoffRepository");
            builder.RegisterType<TinkoffRepository>().As<ITinkoffRepository>();

            // Сервис Гаранта со стороны покупателя.
            builder.RegisterType<CustomerService>().Named<BaseGarantService<CustomerService>>("CustomerService");
            builder.RegisterType<CustomerService>().As<BaseGarantService<CustomerService>>();
            builder.RegisterType<GarantRepository>().Named<BaseGarantRepository<GarantRepository>>("GarantRepository");
            builder.RegisterType<GarantRepository>().As<BaseGarantRepository<GarantRepository>>();
        }
    }
}
