namespace Garant.Platform.Configurator.Models.Output
{
    /// <summary>
    /// Класс выходной модели списка действий для блога.
    /// </summary>
    public class BlogActionOutput
    {
        public int BlogActionId { get; set; }

        /// <summary>
        /// Название действия.
        /// </summary>
        public string BlogActionName { get; set; }

        /// <summary>
        /// Системное название действия.
        /// </summary>
        public string BlogActionSysName { get; set; }
    }
}