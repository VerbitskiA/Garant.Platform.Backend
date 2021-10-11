using System;

namespace Garant.Platform.Models.Franchise.Output
{
    /// <summary>
    /// Класс выходной модели видов бизнеса франшиз.
    /// </summary>
    public class ViewBusinessOutput
    {
        /// <summary>
        /// Guid код вида бизнеса.
        /// </summary>
        public Guid ViewCode { get; set; }

        /// <summary>
        /// Вид бизнеса.
        /// </summary>
        public string ViewName { get; set; }
    }
}
