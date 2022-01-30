namespace Garant.Platform.Models.Configurator.Input
{
    /// <summary>
    /// Класс входной модели получения пунктов меню конфигуратора.
    /// </summary>
    public class ConfiguratorMenuInput
    {
        /// <summary>
        /// Id элемента меню.
        /// </summary>
        public int MenuItemId { get; set; }

        /// <summary>
        /// Системное название пункта меню.
        /// </summary>
        public string MenuItemSysName { get; set; }
        
        /// <summary>
        /// Название действия при нажатии на пункт меню.
        /// </summary>
        public string ActionName { get; set; }
    }
}