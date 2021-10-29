namespace Garant.Platform.Models.User.Output
{
    /// <summary>
    /// Класс выходной модели 
    /// </summary>
    public class ClaimOutput
    {
        public string User { get; set; }

        public string Token { get; set; }

        public bool IsSuccess { get; set; }
    }
}
