namespace Garant.Platform.Models.Pagination.Input
{
    /// <summary>
    /// Класс входной модели пагинации.
    /// </summary>
    public class PaginationInput
    {
        /// <summary>
        /// Номер страницы. По дефолту 1.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Кол-во строк для отображения.
        /// </summary>
        public int CountRows { get; set; } = 10;
    }
}
