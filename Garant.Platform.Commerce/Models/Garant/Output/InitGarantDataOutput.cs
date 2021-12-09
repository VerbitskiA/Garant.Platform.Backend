namespace Garant.Platform.Commerce.Models.Garant.Output
{
    /// <summary>
    /// Класс выходной модели на ините Гаранта.
    /// </summary>
    public class InitGarantDataOutput
    {
        /// <summary>
        /// Заголовок левого блока.
        /// </summary>
        public string BlockLeftTitle { get; set; }

        /// <summary>
        /// Заголовок правого блока.
        /// </summary>
        public string BlockRightTitle { get; set; }

        /// <summary>
        /// Имя фамилия пользователя.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Роль (если зашел владелец предмета сделки, то продавец. Если не владелец, то покупатель).
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Имя фамилия другого пользователя.
        /// </summary>
        public string OtherUserFullName { get; set; }

        /// <summary>
        /// Роль другого пользователя.
        /// </summary>
        public string OtherUserRole { get; set; }

        /// <summary>
        /// Название заголовка суммы в левом блоке.
        /// </summary>
        public string BlockLeftSumTitle { get; set; }

        /// <summary>
        /// Название заголовка суммы в правом блоке.
        /// </summary>
        public string BlockRightSumTitle { get; set; }

        /// <summary>
        /// Заголовок статуса в правом блоке.
        /// </summary>
        public string BlockRightStatusTitle { get; set; }

        /// <summary>
        /// Текст статуса в правом блоке.
        /// </summary>
        public string BlockRightStatusText { get; set; }

        /// <summary>
        /// Общая сумма.
        /// </summary>
        public string TotalAmount { get; set; }

        /// <summary>
        /// Заголовок над предметом сделки.
        /// </summary>
        public string MainItemTitle { get; set; }

        /// <summary>
        /// Заголовок предмета сделки (франшизы или бизнеса).
        /// </summary>
        public string ItemTitle { get; set; }

        /// <summary>
        /// Изображение предмета сделки.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Заголовок блока с документами.
        /// </summary>
        public string DocumentBlockTitle { get; set; }

        /// <summary>
        /// Названия документов, если были прекреплены.
        /// </summary>
        public string DocumentsNames { get; set; }

        /// <summary>
        /// Заголовок черного блока.
        /// </summary>
        public string BlackBlockTitle { get; set; }

        /// <summary>
        /// Текст черного блока.
        /// </summary>
        public string BlackBlockText { get; set; }

        /// <summary>
        /// Текст синей кнопки черного блока.
        /// </summary>
        public string BlackBlueButtonText { get; set; }

        /// <summary>
        /// Текст черной кнопки черного блока.
        /// </summary>
        public string BlackButtonText { get; set; }

        /// <summary>
        /// Текст кнопки для следующего этапа.
        /// </summary>
        public string ContinueButtonText { get; set; }

        /// <summary>
        /// Флаг завершения этапов.
        /// </summary>
        public bool IsLast { get; set; }

        /// <summary>
        /// Название кнопки события.
        /// </summary>
        public string ButtonActionText { get; set; }

        /// <summary>
        /// Цена.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Текст кнопки отмены.
        /// </summary>
        public string ButtonCancel { get; set; }

        /// <summary>
        /// Название блоа с шаблонами документов.
        /// </summary>
        public string BlockDocumentsTemplatesName { get; set; }

        /// <summary>
        /// Текст пояснения блока с шаблонами документов.
        /// </summary>
        public string BlockDocumentsTemplatesDetail { get; set; }

        /// <summary>
        /// Список названий документов шаблонов документов.
        /// </summary>
        public string BlockDocumentsTemplatesFileNames { get; set; }

        /// <summary>
        /// Название блока документов сделки.
        /// </summary>
        public string BlockDocumentDealName { get; set; }

        /// <summary>
        /// Список названий документов сделки разделенных запятой.
        /// </summary>
        public string BlockDocumentDealFileNames { get; set; }

        /// <summary>
        /// Id предмета сделки.
        /// </summary>
        public long ItemDealId { get; set; }

        /// <summary>
        /// Тип предмета сделки (франшиза или бизнес).
        /// </summary>
        public string ItemDealType { get; set; }
    }
}
