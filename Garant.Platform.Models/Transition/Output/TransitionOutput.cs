namespace Garant.Platform.Models.Transition.Output
{
    /// <summary>
    /// Класс выходной модели перехода.
    /// </summary>
    public class TransitionOutput
    {
        /// <summary>
        /// Id пользователя, который совершил переход.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Тип перехода.
        /// </summary>
        public string TransitionType { get; set; }

        /// <summary>
        /// Id франшизы или готового бизнеса.
        /// </summary>
        public long ReferenceId { get; set; }

        public string OtherId { get; set; }

        public string TypeItem { get; set; }
    }
}
