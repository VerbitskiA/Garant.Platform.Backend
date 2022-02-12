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
    }
}