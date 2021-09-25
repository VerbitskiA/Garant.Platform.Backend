namespace Garant.Platform.Models.Footer.Output
{
    /// <summary>
    /// Класс выходной модели футера со списком его полей.
    /// </summary>
    public class FooterOutput
    {
        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Название поля футера.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Позиция.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Флаг поля размещения заявления.
        /// </summary>
        public bool IsPlace { get; set; }

        /// <summary>
        /// Флаг одиночного заголовка без других полей.
        /// </summary>
        public bool IsSignleTitle { get; set; }

        /// <summary>
        /// Номер столбца.
        /// </summary>
        public int Column { get; set; }
    }
}
