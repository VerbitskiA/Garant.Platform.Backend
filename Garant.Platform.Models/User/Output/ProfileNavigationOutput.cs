namespace Garant.Platform.Models.User.Output
{
    /// <summary>
    /// Класс выходной модели для меню ЛК.
    /// </summary>
    public class ProfileNavigationOutput
    {
        /// <summary>
        /// Название пункта навигации.
        /// </summary>
        public string NavigationText { get; set; }

        /// <summary>
        /// Ссылка на страницу.
        /// </summary>
        public string NavigationLink { get; set; }

        /// <summary>
        /// Флаг видимости пункта.
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// Позиция в списке.
        /// </summary>
        public int Position { get; set; }
    }
}
