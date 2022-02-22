namespace Garant.Platform.Models.Franchise.Input
{
    /// <summary>
    /// Класс модели фильтров бизнеса с пагинацией.
    /// </summary>
    public class FilterBusinessesWithPaginationInput
    {
        /// <summary>
        /// Тип сортировки: Asc - по возрастанию, Desc - по убыванию.
        /// </summary>
        public string TypeSortPrice { get; set; }
        
        /// <summary>
        /// Минимальная цена.
        /// </summary>
        public double MinPrice { get; set; }

        /// <summary>
        /// Максимальная цена.
        /// </summary>
        public double MaxPrice { get; set; }

        /// <summary>
        /// Город.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Код категории.
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// Минимальная прибыль в месяц.
        /// </summary>
        public double MinProfit { get; set; }

        /// <summary>
        /// Максимальная прибыль в месяц.
        /// </summary>
        public double MaxProfit { get; set; }

        /// <summary>
        /// Номер страницы
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Количество объектов.
        /// </summary>
        public int CountRows { get; set; } = 10;

        /// <summary>
        /// Флаг Гаранта.
        /// </summary>
        public bool IsGarant { get; set; }
    }
}
