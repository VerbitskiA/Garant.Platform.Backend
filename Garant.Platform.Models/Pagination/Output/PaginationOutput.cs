using System;

namespace Garant.Platform.Models.Pagination.Output
{
    /// <summary>
    /// Класс пагинации.
    /// </summary>
    public class PaginationOutput
    {
        private int PageNumber { get; set; }
        private int TotalPages { get; set; }

        public PaginationOutput(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public bool HasPreviousPage => (PageNumber > 1);

        public bool HasNextPage => (PageNumber < TotalPages);
    }
}
