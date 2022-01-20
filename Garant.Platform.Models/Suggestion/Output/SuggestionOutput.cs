namespace Garant.Platform.Models.Suggestion.Output
{
    /// <summary>
    /// Класс выходной модели для предложений.
    /// </summary>
    public class SuggestionOutput
    {
        /// <summary>
        /// Текст описания.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Текст кнопки не интересно.
        /// </summary>
        public string Button1Text { get; set; }

        /// <summary>
        /// Текст кнопки подробнее.
        /// </summary>
        public string Button2Text { get; set; }

        /// <summary>
        /// Флаг скрытия блока.
        /// </summary>
        public bool IsDisplay { get; set; }

        /// <summary>
        /// Id пользователя, которому не интересно предложение.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Флаг нужно ли получить одно предложение с этим флагом.
        /// </summary>
        public bool IsSingle { get; set; }

        /// <summary>
        /// Флаг нужно ли получить все предложения с этим флагом.
        /// </summary>
        public bool IsAll { get; set; }
    }
}
