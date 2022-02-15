namespace Garant.Platform.Models.Configurator.Output
{
    /// <summary>
    /// Класс выходной модели получения пунктов меню конфигуратора.
    /// </summary>
    public class ConfiguratorMenuOutput
    {
        /// <summary>
        /// Id элемента меню.
        /// </summary>
        public int MenuItemId { get; set; }
        
        /// <summary>
        /// Название пункта меню.
        /// </summary>
        public string MenuItemName { get; set; }

        /// <summary>
        /// Название действия при нажатии на пункт меню.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Системное название пункта меню.
        /// </summary>
        public string MenuItemSysName { get; set; }
        
        /// <summary>
        /// Номер позиции в списке.
        /// </summary>
        public int Position { get; set; }
    }
}