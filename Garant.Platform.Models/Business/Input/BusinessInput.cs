namespace Garant.Platform.Models.Business.Input
{
    /// <summary>
    /// Класс входной модели бизнеса.
    /// </summary>
    public class BusinessInput
    {
        /// <summary>
        /// Id бизнеса.
        /// </summary>
        public long BusinessId { get; set; }

        /// <summary>
        /// Режим (View или Edit). 
        /// </summary>
        public string Mode { get; set; }
    }
}
