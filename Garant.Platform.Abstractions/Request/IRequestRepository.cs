using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Request.Output;

namespace Garant.Platform.Abstractions.Request
{
    /// <summary>
    /// Абстракция репозитория для работы с заявками в БД.
    /// </summary>
    public interface IRequestRepository
    {
        /// <summary>                                                
        /// Метод получит список сделок пользователя.                
        /// </summary>                                               
        /// <param name="account">Аккаунт.</param>                   
        /// <returns>Список сделок.</returns>                        
        Task<IEnumerable<RequestDealOutput>> GetDealsAsync(string account); 
        
        /// <summary>
        /// етод проверит подтверждена ли заявка продавцом.
        /// </summary>
        /// <param name="requestId">Id аявки.</param>
        /// <param name="type">Тип заявки.</param>
        /// <returns>Статус проверки.</returns>
        Task<bool> CheckConfirmRequestAsync(long requestId, string type);
    }
}