using System.Collections.Generic;

namespace Garant.Platform.Models.Category.Output
{
    public class GetResultBusinessCategoryOutput
    {
        public List<BusinessCategoryOutput> ResultCol1 { get; set; } = new List<BusinessCategoryOutput>();

        public List<BusinessCategoryOutput> ResultCol2 { get; set; } = new List<BusinessCategoryOutput>();

        public List<BusinessCategoryOutput> ResultCol3 { get; set; } = new List<BusinessCategoryOutput>();

        public List<BusinessCategoryOutput> ResultCol4 { get; set; } = new List<BusinessCategoryOutput>();
    }
}
