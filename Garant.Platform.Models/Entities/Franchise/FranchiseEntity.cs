using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Garant.Platform.Models.Entities.User;

namespace Garant.Platform.Models.Entities.Franchise
{
    /// <summary>
    /// Класс сопоставляется с таблицей Franchises.Franchises.
    /// </summary>
    [Table("Franchises", Schema = "Franchises")]
    public class FranchiseEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long FranchiseId { get; set; }

        /// <summary>
        /// Путь к изображению.
        /// </summary>
        [Column("Url", TypeName = "text")]
        public string Url { get; set; }

        /// <summary>
        /// Массив с изображениями, которые связаны с изображением франшизы.
        /// </summary>
        [Column("UrlsDetails", TypeName = "text[]")]
        public string[] UrlsDetails { get; set; }

        /// <summary>
        /// Логотип франшизы.
        /// </summary>
        [Column("UrlLogo", TypeName = "text")]
        public string UrlLogo { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        [Column("Title", TypeName = "varchar(200)")]
        public string Title { get; set; }

        /// <summary>
        /// Текст описания.
        /// </summary>
        [Column("Text", TypeName = "varchar(400)")]
        public string Text { get; set; }

        /// <summary>
        /// Цена.
        /// </summary>
        [Column("Price", TypeName = "numeric")]
        public double Price { get; set; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        [Column("DateCreate", TypeName = "timestamp")]
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Текст до цены.
        /// </summary>
        [Column("TextDoPrice", TypeName = "varchar(100)")]
        public string TextDoPrice { get; set; }

        /// <summary>
        /// Категория франшизы.
        /// </summary>
        [Column("Category", TypeName = "varchar(100)")]
        public string Category { get; set; }

        /// <summary>
        /// Подкатегория.
        /// </summary>
        [Column("SubCategory", TypeName = "varchar(100)")]
        public string SubCategory { get; set; }

        /// <summary>
        /// Вид бизнеса.
        /// </summary>
        [Column("ViewBusiness", TypeName = "varchar(200)")]
        public string ViewBusiness { get; set; }

        /// <summary>
        /// Покупка через гарант.
        /// </summary>
        [Column("IsGarant", TypeName = "bool")]
        public bool IsGarant { get; set; }

        [Column("City", TypeName = "varchar(200)")]
        public string City { get; set; }

        /// <summary>
        /// Желаемая прибыль в мес.
        /// </summary>
        [Column("ProfitPrice", TypeName = "numeric")]
        public double ProfitPrice { get; set; }

        /// <summary>
        /// Статус или должность.
        /// </summary>
        [Column("Status", TypeName = "varchar(200)")]
        public string Status { get; set; }

        /// <summary>
        /// Сумма общих инвестиций (включая паушальный взнос).
        /// </summary>
        [Column("GeneralInvest", TypeName = "numeric")]
        public double GeneralInvest { get; set; }

        /// <summary>
        /// Паушальный взнос (зависит от выбранного пакета).
        /// </summary>
        [Column("LumpSumPayment", TypeName = "numeric")]
        public double LumpSumPayment { get; set; }

        /// <summary>
        /// Роялти (от валовой выручки).
        /// </summary>
        [Column("Royalty", TypeName = "numeric")]
        public double Royalty { get; set; }

        /// <summary>
        /// Окупаемость (средняя и планируемая). Кол-во мес.
        /// </summary>
        [Column("Payback", TypeName = "int")]
        public int Payback { get; set; }

        /// <summary>
        /// Месячная прибыль (планируемая чистая прибыль).
        /// </summary>
        [Column("ProfitMonth", TypeName = "numeric")]
        public double ProfitMonth { get; set; }

        /// <summary>
        /// Срок запуска (средний срок открытия бизнеса).
        /// </summary>
        [Column("LaunchDate", TypeName = "numeric")]
        public int LaunchDate { get; set; }

        /// <summary>
        /// Описание деятельности.
        /// </summary>
        [Column("ActivityDetail", TypeName = "text")]
        public string ActivityDetail { get; set; }

        /// <summary>
        /// Входит в инвестиции (json).
        /// </summary>
        [Column("InvestInclude", TypeName = "json")]
        public string InvestInclude { get; set; }

        /// <summary>
        /// Год основания.
        /// </summary>
        [Column("BaseDate", TypeName = "int")]
        public int BaseDate { get; set; }

        /// <summary>
        /// Год запуска.
        /// </summary>
        [Column("YearStart", TypeName = "int")]
        public int YearStart { get; set; }

        /// <summary>
        /// Кол-во точек.
        /// </summary>
        [Column("DotCount", TypeName = "int")]
        public int DotCount { get; set; }

        /// <summary>
        /// Кол-во собственных предприятий.
        /// </summary>
        [Column("BusinessCount", TypeName = "int")]
        public int BusinessCount { get; set; }

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
        /// Название файла презентации.
        /// </summary>
        [Column("NamePresentFile", TypeName = "varchar(400)")]
        public string NamePresentFile { get; set; }

        /// <summary>
        /// Название фото франшизы.
        /// </summary>
        [Column("NameFranchisePhoto", TypeName = "varchar(400)")]
        public string NameFranchisePhoto { get; set; }

        /// <summary>
        /// Описание расчета.
        /// </summary>
        [Column("PaymentDetail", TypeName = "varchar(400)")]
        public string PaymentDetail { get; set; }

        /// <summary>
        /// Название финансовых показателей (json).
        /// </summary>
        [Column("NameFinIndicators", TypeName = "varchar(200)")]
        public string NameFinIndicators { get; set; }

        /// <summary>
        /// Список фин.показателей (json).
        /// </summary>
        [Column("FinIndicators", TypeName = "json")]
        public string FinIndicators { get; set; }

        /// <summary>
        /// Описание обучения.
        /// </summary>
        [Column("TrainingDetails", TypeName = "varchar(400)")]
        public string TrainingDetails { get; set; }

        /// <summary>
        /// Название фото обучения.
        /// </summary>
        [Column("TrainingPhotoName", TypeName = "text")]
        public string TrainingPhotoName { get; set; }

        /// <summary>
        /// Пакеты франшизы (json).
        /// </summary>
        [Column("FranchisePacks", TypeName = "json")]
        public string FranchisePacks { get; set; }

        /// <summary>
        /// Ссылка на видео о франшизе.
        /// </summary>
        [Column("UrlVideo", TypeName = "text")]
        public string UrlVideo { get; set; }

        /// <summary>
        /// Отзывы о франшизе (json).
        /// </summary>
        [Column("Reviews", TypeName = "json")]
        public string Reviews { get; set; }

        /// <summary>
        /// FK на Id пользователя создавшего франшизу.
        /// </summary>
        [Column("UserId", TypeName = "text")]
        [ForeignKey("Id")]
        public string UserId { get; set; }

        /// <summary>
        /// Подтверждена ли карточка.
        /// </summary>
        public bool IsAccepted { get; set; }
        
        /// <summary>
        /// Отклонена ли карточка.
        /// </summary>
        public bool IsRejected { get; set; }

        /// <summary>
        /// Комментарий причины отклонения.
        /// </summary>
        public string CommentRejection { get; set; }

        /// <summary>
        /// Архивирована ли франшиза.
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Дата архивации.
        /// </summary>
        public DateTime ArchivedDate { get; set; }

        public UserEntity User { get; set; }        
    }
}
