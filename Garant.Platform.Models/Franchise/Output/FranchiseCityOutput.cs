using System;

namespace Garant.Platform.Models.Franchise.Output
{
    /// <summary>
    /// Класс выходной модели городов франшиз.
    /// </summary>
    public class FranchiseCityOutput
    {
        /// <summary>
        /// Guid код города.
        /// </summary>
        public Guid CityCode { get; set; }

        /// <summary>
        /// Название города.
        /// </summary>
        public string CityName { get; set; }
    }
}
