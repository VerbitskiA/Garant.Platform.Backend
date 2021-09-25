using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Entities.User;
using Garant.Platform.Models.User.Output;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Garant.Platform.Service.Service.User
{
    /// <summary>
    /// Сервис пользователя.
    /// </summary>
    public sealed class UserService : IUserService
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly PostgreDbContext _postgreDbContext;

        public UserService(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, PostgreDbContext postgreDbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод авторизует пользователя стандартным способом.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Данные авторизованного пользователя.</returns>
        public async Task<ClaimOutput> LoginAsync(string email, string password)
        {
            try
            {
                var auth = await _signInManager.PasswordSignInAsync(email, password, false, false);

                if (auth == null || !auth.Succeeded)
                {
                    throw new ErrorUserAuthorize(email);
                }

                var claim = GetIdentityClaim(email);

                // Генерит токен юзеру.
                var token = GenerateToken(claim).Result;

                var result = new ClaimOutput
                {
                    Email = email,
                    Token = token
                };

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод выдает токен пользователю, если он прошел авторизацию.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <returns>Токен пользователя.</returns>
        private ClaimsIdentity GetIdentityClaim(string email)
        {
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email)
                //new Claim(JwtRegisteredClaimNames.UniqueName, username)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }

        /// <summary>
        /// Метод генерит токен юзеру.
        /// </summary>
        /// <param name="claimsIdentity">Объект полномочий.</param>
        /// <returns>Строку токена.</returns>
        private Task<string> GenerateToken(ClaimsIdentity claimsIdentity)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: claimsIdentity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Task.FromResult(encodedJwt);
        }

        /// <summary>
        /// Метод проверит правильность кода подтверждения.
        /// </summary>
        /// <param name="code">Код подтверждения.</param>
        /// <returns>Статус проверки.</returns>
        public async Task<bool> CheckAcceptCodeAsync(string code)
        {
            try
            {
                var user = await (from u in _postgreDbContext.Users
                                  where u.Code.Equals(code)
                                  select u)
                    .FirstOrDefaultAsync();

                return user != null;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод создаст нового пользователя.
        /// </summary>
        /// <param name="name">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="city">Город.</param>
        /// <param name="email">Email.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="role">Роль.</param>
        /// <returns>Данные созданного пользователя.</returns>
        public async Task<UserOutput> CreateAsync(string name, string lastName, string city, string email, string password, string role)
        {
            try
            {
                var date = DateTime.UtcNow;

                // Создаст пользователя.
                await _userManager.CreateAsync(new UserEntity
                {
                    UserName = email,
                    FirstName = name,
                    LastName = lastName,
                    Email = email,
                    City = city,
                    DateRegister = date,
                    Code = string.Empty,
                    UserPassword = password,
                    LockoutEnabled = false,
                    UserRole = role
                }, password);

                var reult = new UserOutput
                {
                    Email = email,
                    City = city,
                    FirstName = name,
                    LastName = lastName,
                    DateRegister = date
                };

                return reult;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }
    }
}
