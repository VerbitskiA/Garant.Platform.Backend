namespace Garant.Platform.Models.User.Output
{
    /// <summary>
    /// Класс выходной модели 
    /// </summary>
    public class ClaimOutput
    {
        /// <summary>
        /// Логин или email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Токен пользователя.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Токен пользователя.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Флаг успеха авторизации.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Флаг заполнения данных о себе.
        /// </summary>
        //public bool IsWriteProfileData { get; set; }
    }
}
