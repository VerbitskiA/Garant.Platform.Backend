using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Franchise
{
    /// <summary>
    /// Класс сопоставляется с таблицей городов франшиз Franchises.FranchiseCities.
    /// </summary>
    [Table("FranchiseCities", Schema = "Franchises")]
    public class FranchiseCityEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public int CityId { get; set; }

        /// <summary>
        /// Guid код города.
        /// </summary>
        [Column("CityCode", TypeName = "varchar(100)")]
        public Guid CityCode { get; set; }

        /// <summary>
        /// Название города.
        /// </summary>
        [Column("CityName", TypeName = "varchar(200)")]
        public string CityName { get; set; }
    }
}
