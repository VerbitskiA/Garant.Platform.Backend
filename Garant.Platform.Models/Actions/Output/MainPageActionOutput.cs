namespace Garant.Platform.Models.Actions.Output
{
    /// <summary>
    /// Класс выходной модели для событий на главной страницы.
    /// </summary>
    public class MainPageActionOutput
    {
        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Подзаголовок.
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// Текст описания.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Текст кнопки.
        /// </summary>
        public string ButtonText { get; set; }

        /// <summary>
        /// Флаг необходимости размещения в топе страницы.
        /// </summary>
        public bool IsTop { get; set; }
    }
}
