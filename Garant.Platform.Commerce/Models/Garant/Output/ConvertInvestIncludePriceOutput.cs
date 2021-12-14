using System.Collections.Generic;

namespace Garant.Platform.Commerce.Models.Garant.Output
{
    public class ConvertInvestIncludePriceOutput
    {
        public string Name { get; set; }

        public List<string> Items { get; set; }

        public double Price { get; set; }
    }
}
