namespace Garant.Platform.Models.Franchise.Input
{
    /// <summary>
    /// Класс входной модели франшизы.
    /// </summary>
    public class FranchiseInput
    {
        /// <summary>
        /// Id франшизы.
        /// </summary>
        public long FranchiseId { get; set; }

        /// <summary>
        /// Режим (View или Edit). 
        /// </summary>
        public string Mode { get; set; }
    }
}
