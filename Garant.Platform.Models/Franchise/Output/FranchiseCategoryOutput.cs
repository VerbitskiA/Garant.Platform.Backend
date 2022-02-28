namespace Garant.Platform.Models.Franchise.Output
{
    /// <summary>
    /// Класс выходной модели списка категорий франшиз.
    /// </summary>
    public class FranchiseCategoryOutput
    {
        /// <summary>
        /// Путь к изображению.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Номер столбца.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Позиция.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Название категории.
        /// </summary>
        public string Name { get; set; }
    }
}