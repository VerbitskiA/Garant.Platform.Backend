using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Business
{
    [Table("BusinessCities", Schema = "Business")]
    public class BusinessCitiesEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public int BusinessCityId { get; set; }

        /// <summary>
        /// Guid код города.
        /// </summary>
        [Column("BusinessCityCode", TypeName = "varchar(100)")]
        public string BusinessCityCode { get; set; }

        /// <summary>
        /// Название города.
        /// </summary>
        [Column("BusinessCityName", TypeName = "varchar(200)")]
        public string BusinessCityName { get; set; }
    }
}
