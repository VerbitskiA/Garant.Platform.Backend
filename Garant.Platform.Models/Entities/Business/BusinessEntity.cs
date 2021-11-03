using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Business
{
    /// <summary>
    /// Класс сопоставляется с таблицей готового бизнеса Business.Businesses.
    /// </summary>
    [Table("Businesses", Schema = "Business")]
    public class BusinessEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long BusinessId { get; set; }

        /// <summary>
        /// Название бизнеса.
        /// </summary>
        [Column("BusinessName", TypeName = "varchar(300)")]
        public string BusinessName { get; set; }

        /// <summary>
        /// Массив с именами изображений готового бизнеса.
        /// </summary>
        [Column("UrlsBusiness", TypeName = "text[]")]
        public string[] UrlsBusiness { get; set; }

        /// <summary>
        /// Статус или должность.
        /// </summary>
        [Column("Status", TypeName = "varchar(200)")]
        public string Status { get; set; }

        [Column("Price", TypeName = "numeric")]
        public double Price { get; set; }

        /// <summary>
        /// Сумма оборота.
        /// </summary>
        [Column("TurnPrice", TypeName = "numeric")]
        public double TurnPrice { get; set; }

        /// <summary>
        /// Желаемая прибыль в мес.
        /// </summary>
        [Column("ProfitPrice", TypeName = "numeric")]
        public double ProfitPrice { get; set; }

        /// <summary>
        /// Окупаемость (средняя и планируемая). Кол-во мес.
        /// </summary>
        [Column("Payback", TypeName = "int")]
        public int Payback { get; set; }

        /// <summary>
        /// Рентабельность.
        /// </summary>
        [Column("Profitability", TypeName = "numeric")]
        public double Profitability { get; set; }

        /// <summary>
        /// Возраст бизнеса.
        /// </summary>
        [Column("BusinessAge", TypeName = "int")]
        public int BusinessAge { get; set; }

        /// <summary>
        /// Входит в стоимость (json).
        /// </summary>
        [Column("InvestPrice", TypeName = "json")]
        public string InvestPrice { get; set; }

        /// <summary>
        /// Описание готового бизнеса.
        /// </summary>
        [Column("Text", TypeName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Кол-во сотрудников в год.
        /// </summary>
        [Column("EmployeeCountYear", TypeName = "int")]
        public int EmployeeCountYear { get; set; }

        /// <summary>
        /// Форма.
        /// </summary>
        [Column("Form", TypeName = "varchar(300)")]
        public string Form { get; set; }

        /// <summary>
        /// Доля с продажи.
        /// </summary>
        [Column("Share", TypeName = "numeric")]
        public double Share { get; set; }

        /// <summary>
        /// Ссылка на сайт.
        /// </summary>
        [Column("Site", TypeName = "text")]
        public string Site { get; set; }

        /// <summary>
        /// Описание деятельности бизнеса.
        /// </summary>
        [Column("ActivityDetail", TypeName = "text")]
        public string ActivityDetail { get; set; }

        /// <summary>
        /// Название фото деятельнотси бизнеса.
        /// </summary>
        [Column("ActivityPhotoName", TypeName = "varchar(300)")]
        public string ActivityPhotoName { get; set; }

        /// <summary>
        /// Особенность франшизы.
        /// </summary>
        [Column("Peculiarity", TypeName = "varchar(200)")]
        public string Peculiarity { get; set; }

        /// <summary>
        /// Название файла финансовой модели.
        /// </summary>
        [Column("NameFinModelFile", TypeName = "varchar(400)")]
        public string NameFinModelFile { get; set; }

        /// <summary>
        /// Активы.
        /// </summary>
        [Column("Assets", TypeName = "text")]
        public string Assets { get; set; }

        /// <summary>
        /// Название изображения активов.
        /// </summary>
        [Column("AssetsPhotoName", TypeName = "varchar(300)")]
        public string AssetsPhotoName { get; set; }

        /// <summary>
        /// Причины продажи.
        /// </summary>
        [Column("ReasonsSale", TypeName = "varchar(300)")]
        public string ReasonsSale { get; set; }

        /// <summary>
        /// Название фото причин продажи.
        /// </summary>
        [Column("ReasonsSalePhotoName", TypeName = "varchar(300)")]
        public string ReasonsSalePhotoName { get; set; }

        /// <summary>
        /// Адрес бизнеса.
        /// </summary>
        [Column("Address", TypeName = "varchar(300)")]
        public string Address { get; set; }

        /// <summary>
        /// Ссылка на видео о бизнесе.
        /// </summary>
        [Column("UrlVideo", TypeName = "text")]
        public string UrlVideo { get; set; }

        /// <summary>
        /// Флаг продажи через гарант
        /// </summary>
        [Column("IsGarant", TypeName = "bool")]
        public bool IsGarant { get; set; }
    }
}
