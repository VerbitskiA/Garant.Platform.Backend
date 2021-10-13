using System.Collections;

namespace Garant.Platform.Models.Pagination.Output
{
    /// <summary>
    /// Класс с информацией о пагинации.
    /// </summary>
    public class IndexOutput
    {
        public IEnumerable Results { get; set; }

        public PaginationOutput PageData { get; set; }

        /// <summary>
        /// Всего строк.
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// Нужно ли загрузить оставшиеся записи.
        /// </summary>
        public bool IsLoadAll { get; set; }

        /// <summary>
        /// Флаг видимости пагинации.
        /// </summary>
        public bool IsVisiblePagination { get; set; }

        /// <summary>
        /// Кол-во записе всего в таблице.
        /// </summary>
        public int CountAll { get; set; }
    }
}
