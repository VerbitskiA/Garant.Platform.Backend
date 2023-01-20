using Garant.Platform.Abstractions.DataBase;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.Models.Entities.User;
using Garant.Platform.Models.User.Output;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garant.Platform.Services.Service.User
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly PostgreDbContext _postgreDbContext;

        public RefreshTokenRepository()
        {
            var dbContext = AutoFac.Resolve<IDataBaseConfig>();
            _postgreDbContext = dbContext.GetDbContext();
        }

        public async Task<RefreshTokenOutput> CreateAsync(string refreshToken, string userId)
        {
            try
            {
                RefreshTokenEntity newRefreshToken = new RefreshTokenEntity
                {
                    Token = refreshToken,
                    UserId = userId
                };

                await _postgreDbContext.RefreshTokens.AddAsync(newRefreshToken);

                await _postgreDbContext.SaveChangesAsync();

                RefreshTokenOutput result = new()
                {
                    Token = refreshToken,
                    UserId = userId
                };

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        public async Task<bool> DeleteAllAsync(string userId)
        {
            try
            {
                List<RefreshTokenEntity> refreshTokens = await _postgreDbContext.RefreshTokens.Where(x => x.UserId == userId).ToListAsync();

                foreach (var item in refreshTokens)
                {
                    _postgreDbContext.RefreshTokens.Remove(item);
                }

                await _postgreDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid refreshTokenId)
        {
            try
            {
                var deletedToken = await _postgreDbContext.
                                             RefreshTokens.
                                             FirstOrDefaultAsync(b => b.RefreshTokenId.Equals(refreshTokenId));

                _postgreDbContext.RefreshTokens.Remove(deletedToken);

                await _postgreDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        public async Task<RefreshTokenOutput> GetByTokenAsync(string refreshToken)
        {
            try
            {
                var result = await _postgreDbContext.RefreshTokens
                    .Where(b => b.Token.Equals(refreshToken))
                    .Select(b => new RefreshTokenOutput
                    {
                        TokenId = b.RefreshTokenId,
                        Token = b.Token,
                        UserId = b.UserId
                    })
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }
    }
}
