namespace Garant.Platform.Models.Business.Output
{
    public class GetBusinessCategoryOutput
    {
        /// <summary>
        /// Guid код подкатегории.
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// Название подкатегории.
        /// </summary>
        public string CategoryName { get; set; }
    }
}
