namespace Garant.Platform.Models.Suggestion.Input
{
    /// <summary>
    /// Класс входной модели для предложений.
    /// </summary>
    public class SuggestionInput
    {
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
