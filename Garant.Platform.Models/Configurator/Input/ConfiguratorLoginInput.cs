namespace Garant.Platform.Models.Configurator.Input
{
    /// <summary>
    /// Класс входной модели авторизации сотрудника конфигуратора.
    /// </summary>
    public class ConfiguratorLoginInput
    {
        /// <summary>
        /// Телефон или Email.
        /// </summary>
        public string InputData { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }
    }
}