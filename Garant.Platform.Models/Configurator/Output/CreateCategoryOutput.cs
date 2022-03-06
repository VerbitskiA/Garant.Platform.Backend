namespace Garant.Platform.Models.Configurator.Output
{
    /// <summary>
    /// Класс выходной модели создания категории.
    /// </summary>
    public class CreateCategoryOutput
    {
        /// <summary>
        /// Код сферы (guid).
        /// </summary>
        public string SphereCode { get; set; }

        /// <summary>
        /// Название категории.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Тип категории.
        /// </summary>
        public string CategoryType { get; set; }
        
        /// <summary>
        /// Системное название.
        /// </summary>
        public string SysName { get; set; }

        /// <summary>
        /// Код категории (guid).
        /// </summary>
        public string CategoryCode { get; set; }
    }
}