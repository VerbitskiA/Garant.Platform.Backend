using Garant.Platform.Models.User.Output;
using System;
using System.Threading.Tasks;

namespace Garant.Platform.Abstractions.User
{
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Метод создаст в таблице новую запись.
        /// </summary>
        /// <param name="refreshToken">Значение рефреш токена.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Созданная запись.</returns>
        Task<RefreshTokenOutput> CreateAsync(string refreshToken, string userId);

        /// <summary>
        /// Метод найдет в таблице рефреш токенов запись по значению токена.
        /// </summary>
        /// <param name="refreshToken">Значение рефреш токена.</param>
        /// <returns>Запись о рефреш токене.</returns>
        Task<RefreshTokenOutput> GetByTokenAsync(string refreshToken);

        /// <summary>
        /// Метод удалит рефреш токен.
        /// </summary>
        /// <param name="refreshTokenId">Идентификатор токена.</param>
        /// <returns>Статус операции.</returns>
        Task<bool> DeleteAsync(Guid refreshTokenId);

        /// <summary>
        /// Метод удалит все токены принадлежащие пользователю.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Статус операции.</returns>
        Task<bool> DeleteAllAsync(string userId);
    }
}
