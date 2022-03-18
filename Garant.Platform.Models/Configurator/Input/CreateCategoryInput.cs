namespace Garant.Platform.Models.Configurator.Input
{
    /// <summary>
    /// Класс входной модели создания категории.
    /// </summary>
    public class CreateCategoryInput
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
    }
}