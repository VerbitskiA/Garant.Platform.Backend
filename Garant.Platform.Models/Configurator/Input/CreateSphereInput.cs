namespace Garant.Platform.Models.Configurator.Input
{
    /// <summary>
    /// Класс входной модели создания сферы.
    /// </summary>
    public class CreateSphereInput
    {
        /// <summary>
        /// Название новой сферы.
        /// </summary>
        public string SphereName { get; set; }

        /// <summary>
        /// Тип сферы (к чему относится сфера к бизнесам или франшизам).
        /// </summary>
        public string SphereType { get; set; }

        /// <summary>
        /// Системное название.
        /// </summary>
        public string SysName { get; set; }
    }
}