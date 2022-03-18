using System.Collections.Generic;
using Garant.Platform.Models.Franchise.Output;

namespace Garant.Platform.Models.Category.Output
{
    /// <summary>
    /// Выходная модель результирующих списков категорий бизнеса и франшиз.
    /// </summary>
    public class GetResultCategoryOutput
    {
        public List<BusinessCategoryOutput> ResultCol1 { get; set; } = new List<BusinessCategoryOutput>();

        public List<BusinessCategoryOutput> ResultCol2 { get; set; } = new List<BusinessCategoryOutput>();
        
        public List<FranchiseCategoryOutput> ResultCol3 { get; set; } = new List<FranchiseCategoryOutput>();
        
        public List<FranchiseCategoryOutput> ResultCol4 { get; set; } = new List<FranchiseCategoryOutput>();
    }
}
