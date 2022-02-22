namespace Garant.Platform.Models.Franchise.Input
{
    /// <summary>
    /// Класс модели фильтров франшиз с пагинацией.
    /// </summary>
    public class FilterFranchisesWithPaginationInput
    {
        /// <summary>
        /// Тип сортировки: Asc - по возрастанию, Desc - по убыванию.
        /// </summary>
        public string TypeSortPrice { get; set; }

        /// <summary>
        /// Код вида.
        /// </summary>
        public string ViewBusinessesCode { get; set; }

        /// <summary>
        /// Код категории.
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// Минимальные ивестиции от.
        /// </summary>
        public double MinInvest { get; set; }

        /// <summary>
        /// Максимальные инвестии до.
        /// </summary>
        public double MaxInvest { get; set; }        

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
        public int PageNumber { get; set; }

        /// <summary>
        /// Количество объектов.
        /// </summary>
        public int CountRows { get; set; }

        /// <summary>
        /// Флаг Гаранта.
        /// </summary>
        public bool IsGarant { get; set; }
    }
}
