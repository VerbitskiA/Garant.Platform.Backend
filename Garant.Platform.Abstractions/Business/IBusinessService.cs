using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Business.Output;
using Microsoft.AspNetCore.Http;

namespace Garant.Platform.Abstractions.Business
{
    /// <summary>
    /// Абстракция сервиса готового бизнеса.
    /// </summary>
    public interface IBusinessService
    {
        /// <summary>
        /// Метод создаст или обновит карточку бизнеса.
        /// </summary>
        /// <param name="businessFilesInput">Файлы карточки.</param>
        /// <param name="businessDataInput">Входная модель.</param>
        /// <param name="account">Логин.</param>
        /// <returns>Данные карточки бизнеса.</returns>
        Task<CreateUpdateBusinessOutput> CreateUpdateBusinessAsync(IFormCollection businessFilesInput, string businessDataInput, string account);

        /// <summary>
        /// Метод отправит файл в папку и временно запишет в БД.
        /// </summary>
        /// <param name="form">Файлы.</param>
        /// <returns>Список названий файлов.</returns>
        Task<IEnumerable<string>> AddTempFilesBeforeCreateBusinessAsync(IFormCollection form, string account);
    }
}
