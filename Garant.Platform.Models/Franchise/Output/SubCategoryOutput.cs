namespace Garant.Platform.Models.Franchise.Output
{
    public class SubCategoryOutput
    {
        /// <summary>
        /// Guid код подкатегории.
        /// </summary>
        public string SubCategoryCode { get; set; }

        /// <summary>
        /// Название подкатегории.
        /// </summary>
        public string SubCategoryName { get; set; }
        
        /// <summary>
        /// Системное название сферы.
        /// </summary>
        public string FranchiseCategorySysName { get; set; }

        /// <summary>
        /// Системное название категории.
        /// </summary>
        public string FranchiseSubCategorySysName { get; set; }

        /// <summary>
        /// Код категории.
        /// </summary>
        public string FranchiseCategoryCode { get; set; }
    }
}
