using System.Collections.Generic;

namespace Garant.Platform.Models.Commerce.Output
{
    /// <summary>
    /// Класс выходной модели сделки.
    /// </summary>
    public class DealOutput
    {
        /// <summary>
        /// Id сделки.
        /// </summary>
        public long DealId { get; set; }

        /// <summary>
        /// Id предмета сделки (франшизы или бизнеса).
        /// </summary>
        public long DealItemId { get; set; }

        /// <summary>
        /// Id пользователя (не владельца предмета сделки).
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Список этапов сделки.
        /// </summary>
        public List<DealIterationOutput> DealIterations { get; set; }
    }
}
