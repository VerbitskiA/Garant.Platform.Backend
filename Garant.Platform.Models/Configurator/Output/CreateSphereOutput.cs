namespace Garant.Platform.Models.Configurator.Output
{
    /// <summary>
    /// Класс выходной модели создания сферы.
    /// </summary>
    public class CreateSphereOutput
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
        /// Код сферы (guid).
        /// </summary>
        public string SphereCode { get; set; }

        /// <summary>
        /// Системное название сферы.
        /// </summary>
        public string SphereSysName { get; set; }
    }
}