using Autofac;

namespace Garant.Platform.Service.AutofacModules
{
    /// <summary>
    /// Класс регистрации сервисов автофака.
    /// </summary>
    public sealed class ServicesModule
    {
        public static void InitModules(ContainerBuilder builder)
        {
            //builder.RegisterType<TaskService>().Named<ITaskService>("TaskService");

            //// Сервис стартовой страницы.
            //builder.RegisterType<MainPageService>().As<IMainPageService>();
        }
    }
}
