using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Franchise.Other
{
    /// <summary>
    /// Класс описывает подкатегорию франшизы.
    /// </summary>
    [NotMapped]
    public class SubCategory
    {
        /// <summary>
        /// Значение.
        /// </summary>
        public string Value { get; set; }
    }
}
