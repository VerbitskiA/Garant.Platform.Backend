using System;

namespace Garant.Platform.Models.Business.Output
{
    /// <summary>
    /// Класс выходной модели создания или изменения карточки бизнеса.
    /// </summary>
    public class CreateUpdateBusinessOutput
    {
        /// <summary>
        /// PK.
        /// </summary>
        public long BusinessId { get; set; }

        /// <summary>
        /// Название бизнеса.
        /// </summary>
        public string BusinessName { get; set; }

        /// <summary>
        /// Массив с именами изображений готового бизнеса.
        /// </summary>
        public string UrlsBusiness { get; set; }

        /// <summary>
        /// Статус или должность.
        /// </summary>
        public string Status { get; set; }

        public double Price { get; set; }

        /// <summary>
        /// Сумма оборота.
        /// </summary>
        public double TurnPrice { get; set; }

        /// <summary>
        /// Желаемая прибыль в мес.
        /// </summary>
        public double ProfitPrice { get; set; }

        /// <summary>
        /// Окупаемость (средняя и планируемая). Кол-во мес.
        /// </summary>
        public int Payback { get; set; }

        /// <summary>
        /// Рентабельность.
        /// </summary>
        public double Profitability { get; set; }

        /// <summary>
        /// Возраст бизнеса.
        /// </summary>
        public int BusinessAge { get; set; }

        /// <summary>
        /// Входит в стоимость (json).
        /// </summary>
        public string InvestPrice { get; set; }

        /// <summary>
        /// Описание готового бизнеса.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Кол-во сотрудников в год.
        /// </summary>
        public int EmployeeCountYear { get; set; }

        /// <summary>
        /// Форма.
        /// </summary>
        public string Form { get; set; }

        /// <summary>
        /// Доля с продажи.
        /// </summary>
        public double Share { get; set; }

        /// <summary>
        /// Ссылка на сайт.
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Описание деятельности бизнеса.
        /// </summary>
        public string ActivityDetail { get; set; }

        /// <summary>
        /// Название фото деятельнотси бизнеса.
        /// </summary>
        public string ActivityPhotoName { get; set; }

        /// <summary>
        /// Особенность франшизы.
        /// </summary>
        public string Peculiarity { get; set; }

        /// <summary>
        /// Название файла финансовой модели.
        /// </summary>
        public string NameFinModelFile { get; set; }

        /// <summary>
        /// Активы.
        /// </summary>
        public string Assets { get; set; }

        /// <summary>
        /// Название изображения активов.
        /// </summary>
        public string AssetsPhotoName { get; set; }

        /// <summary>
        /// Причины продажи.
        /// </summary>
        public string ReasonsSale { get; set; }

        /// <summary>
        /// Название фото причин продажи.
        /// </summary>
        public string ReasonsSalePhotoName { get; set; }

        /// <summary>
        /// Адрес бизнеса.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Ссылка на видео о бизнесе.
        /// </summary>
        public string UrlVideo { get; set; }

        /// <summary>
        /// Флаг продажи через гарант
        /// </summary>
        public bool IsGarant { get; set; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Текст до цены.
        /// </summary>
        public string TextDoPrice { get; set; }
    }
}
