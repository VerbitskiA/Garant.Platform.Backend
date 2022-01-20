namespace Garant.Platform.Models.Header.Output
{
    /// <summary>
    /// Класс выходной модели для получения полей хидера.
    /// </summary>
    public class HeaderOutput
    {
        /// <summary>
        /// Название поля хидера.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип хидера.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Позиция.
        /// </summary>
        public int Position { get; set; }
    }
}
