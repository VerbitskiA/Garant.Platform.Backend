using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Business.Input;
using Garant.Platform.Models.Business.Output;
using Microsoft.AspNetCore.Http;

namespace Garant.Platform.Abstractions.Business
{
    /// <summary>
    /// Абстракция репозитория готового бизнеса.
    /// </summary>
    public interface IBusinessRepository
    {
        /// <summary>
        /// Метод создаст или обновит карточку бизнеса.
        /// </summary>
        /// <param name="businessFilesInput">Файлы карточки.</param>
        /// <param name="businessDataInput">Входная модель.</param>
        /// <param name="account">Логин.</param>
        /// <returns>Данные карточки бизнеса.</returns>
        Task<CreateUpdateBusinessOutput> CreateUpdateBusinessAsync(CreateUpdateBusinessInput businessInput, long lastBusinessId, string[] urlsBusiness, IFormFileCollection files, string account);

        /// <summary>
        /// Метод отправит файл в папку и временно запишет в БД.
        /// </summary>
        /// <param name="form">Файлы.</param>
        /// <returns>Список названий файлов.</returns>
        Task<IEnumerable<string>> AddTempFilesBeforeCreateBusinessAsync(IFormFileCollection form, string account);

        /// <summary>
        /// Метод получит франшизу для просмотра или изменения.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="mode">Режим (Edit или View).</param>
        /// <returns>Данные бизнеса.</returns>
        Task<BusinessOutput> GetBusinessAsync(long businessId, string mode);
    }
}
