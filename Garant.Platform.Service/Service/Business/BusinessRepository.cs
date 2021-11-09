using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Business.Input;
using Garant.Platform.Models.Business.Output;
using Garant.Platform.Models.Entities.Business;
using Garant.Platform.Models.Franchise.Output;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Services.Service.Business
{
    /// <summary>
    /// Репозиторий готового бизнеса.
    /// </summary>
    public sealed class BusinessRepository : IBusinessRepository
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IUserRepository _userRepository;

        public BusinessRepository(PostgreDbContext postgreDbContext, IUserRepository userRepository)
        {
            _postgreDbContext = postgreDbContext;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Метод создаст новую или обновит существующий бизнес.
        /// </summary>
        /// <param name="files">Входные файлы.</param>
        /// <param name="businessInput">Входная модель.</param>
        /// <param name="lastBusinessId">Id последней франшизы.</param>
        /// <param name="urlsBusiness">Пути к доп.изображениям.</param>
        /// <param name="account">Логин.</param>
        /// <returns>Данные франшизы.</returns>
        public async Task<CreateUpdateBusinessOutput> CreateUpdateBusinessAsync(CreateUpdateBusinessInput businessInput, long lastBusinessId, string[] urlsBusiness, IFormFileCollection files, string account)
        {
            try
            {
                CreateUpdateBusinessOutput result = null;

                if (businessInput != null)
                {
                    var userId = string.Empty;

                    var findUser = await _userRepository.FindUserByEmailOrPhoneNumberAsync(account);

                    // Если такого пользователя не найдено, значит поищет по коду.
                    if (findUser == null)
                    {
                        var findUserIdByCode = await _userRepository.FindUserByCodeAsync(account);

                        if (!string.IsNullOrEmpty(findUserIdByCode))
                        {
                            userId = findUserIdByCode;
                        }
                    }

                    else
                    {
                        userId = findUser.UserId;
                    }
                    
                    // Найдет бизнес с таким названием.
                    var findBusiness = await GetBusinessAsync(businessInput.BusinessName);

                    // Создаст новый бизнес.
                    if (businessInput.IsNew && findBusiness == null)
                    {
                        await _postgreDbContext.Businesses.AddAsync(new BusinessEntity
                        {
                            BusinessId = lastBusinessId,
                            ActivityDetail = businessInput.ActivityDetail,
                            ActivityPhotoName = files.Where(c => c.Name.Equals("filesTextBusiness")).ToArray()[0].FileName,
                            Address = businessInput.Address,
                            Assets = businessInput.Assets,
                            AssetsPhotoName = files.Where(c => c.Name.Equals("filesAssets")).ToArray()[0].FileName,
                            BusinessAge = businessInput.BusinessAge,
                            BusinessName = businessInput.BusinessName,
                            EmployeeCountYear = businessInput.EmployeeCountYear,
                            Form = businessInput.Form,
                            Status = businessInput.Status,
                            Price = businessInput.Price,
                            UrlsBusiness = urlsBusiness.ToArray(),
                            TurnPrice = businessInput.TurnPrice,
                            ProfitPrice = businessInput.ProfitPrice,
                            Payback = businessInput.Payback,
                            Profitability = businessInput.Profitability,
                            InvestPrice = businessInput.InvestPrice,
                            Text = businessInput.Text,
                            Share = businessInput.Share,
                            Site = businessInput.Site,
                            Peculiarity = businessInput.Peculiarity,
                            NameFinModelFile = files.Where(c => c.Name.Equals("finModelFile")).ToArray()[0].FileName,
                            ReasonsSale = businessInput.ReasonsSale,
                            ReasonsSalePhotoName = files.Where(c => c.Name.Equals("filesReasonsSale")).ToArray()[0].FileName,
                            UrlVideo = businessInput.UrlVideo,
                            IsGarant = businessInput.IsGarant,
                            UserId = userId,
                            DateCreate = DateTime.Now,
                            TextDoPrice = "Стоимость:",
                            Category = businessInput.Category,
                            SubCategory = businessInput.SubCategory
                        });
                    }

                    // Обновит бизнес.
                    else if (!businessInput.IsNew && findBusiness != null)
                    {
                        findBusiness.ActivityDetail = businessInput.ActivityDetail;
                        findBusiness.ActivityPhotoName =
                            files.Where(c => c.Name.Equals("filesTextBusiness")).ToArray()[0].FileName;
                        findBusiness.Address = businessInput.Address;
                        findBusiness.Assets = businessInput.Assets;
                        findBusiness.AssetsPhotoName =
                            files.Where(c => c.Name.Equals("filesAssets")).ToArray()[0].FileName;
                        findBusiness.BusinessAge = businessInput.BusinessAge;
                        findBusiness.BusinessName = businessInput.BusinessName;
                        findBusiness.EmployeeCountYear = businessInput.EmployeeCountYear;
                        findBusiness.Form = businessInput.Form;
                        findBusiness.Status = businessInput.Status;
                        findBusiness.Price = businessInput.Price;
                        findBusiness.UrlsBusiness = urlsBusiness.ToArray();
                        findBusiness.TurnPrice = businessInput.TurnPrice;
                        findBusiness.ProfitPrice = businessInput.ProfitPrice;
                        findBusiness.Payback = businessInput.Payback;
                        findBusiness.Profitability = businessInput.Profitability;
                        findBusiness.InvestPrice = businessInput.InvestPrice;
                        findBusiness.Text = businessInput.Text;
                        findBusiness.Share = businessInput.Share;
                        findBusiness.Site = businessInput.Site;
                        findBusiness.Peculiarity = businessInput.Peculiarity;
                        findBusiness.NameFinModelFile =
                            files.Where(c => c.Name.Equals("finModelFile")).ToArray()[0].FileName;
                        findBusiness.ReasonsSale = businessInput.ReasonsSale;
                        findBusiness.ReasonsSalePhotoName =
                            files.Where(c => c.Name.Equals("filesReasonsSale")).ToArray()[0].FileName;
                        findBusiness.UrlVideo = businessInput.UrlVideo;
                        findBusiness.IsGarant = businessInput.IsGarant;
                        findBusiness.DateCreate = DateTime.Now;
                        findBusiness.TextDoPrice = "Стоимость:";
                        findBusiness.Category = businessInput.Category;
                        findBusiness.SubCategory = businessInput.SubCategory;

                        _postgreDbContext.Update(findBusiness);
                    }

                    await _postgreDbContext.SaveChangesAsync();

                    result = new CreateUpdateBusinessOutput
                    {
                        ActivityDetail = businessInput.ActivityDetail,
                        ActivityPhotoName = "../../../assets/images/" + files.Where(c => c.Name.Equals("filesTextBusiness")).ToArray()[0].FileName,
                        Address = businessInput.Address,
                        Assets = businessInput.Assets,
                        AssetsPhotoName = "../../../assets/images/" + files.Where(c => c.Name.Equals("filesAssets")).ToArray()[0].FileName,
                        BusinessAge = businessInput.BusinessAge,
                        BusinessId = lastBusinessId,
                        BusinessName = businessInput.BusinessName,
                        EmployeeCountYear = businessInput.EmployeeCountYear,
                        Form = businessInput.Form,
                        Status = businessInput.Status,
                        Price = businessInput.Price,
                        UrlsBusiness = urlsBusiness.ToArray(),
                        TurnPrice = businessInput.TurnPrice,
                        ProfitPrice = businessInput.ProfitPrice,
                        Payback = businessInput.Payback,
                        Profitability = businessInput.Profitability,
                        InvestPrice = businessInput.InvestPrice,
                        Text = businessInput.Text,
                        Share = businessInput.Share,
                        Site = businessInput.Site,
                        Peculiarity = businessInput.Peculiarity,
                        NameFinModelFile = "../../../assets/images/" + files.Where(c => c.Name.Equals("finModelFile")).ToArray()[0].FileName,
                        ReasonsSale = businessInput.ReasonsSale,
                        ReasonsSalePhotoName = "../../../assets/images/" + files.Where(c => c.Name.Equals("filesReasonsSale")).ToArray()[0].FileName,
                        UrlVideo = businessInput.UrlVideo,
                        IsGarant = businessInput.IsGarant,
                        DateCreate = DateTime.Now,
                        TextDoPrice = "Стоимость:"
                    };
                }

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
        /// Метод найдет карточку готового бизнеса.
        /// </summary>
        /// <param name="title">Название карточки готового бизнеса.</param>
        /// <returns>Данные карточки готового бизнеса.</returns>
        private async Task<BusinessEntity> GetBusinessAsync(string title)
        {
            var result = await _postgreDbContext.Businesses
                .Where(b => b.BusinessName.Equals(title))
                .Select(b => new BusinessEntity
                {
                    ActivityDetail = b.ActivityDetail,
                    ActivityPhotoName = b.ActivityPhotoName,
                    Address = b.Address,
                    Assets = b.Assets,
                    AssetsPhotoName = b.AssetsPhotoName,
                    BusinessAge = b.BusinessAge,
                    BusinessId = b.BusinessId,
                    BusinessName = b.BusinessName,
                    EmployeeCountYear = b.EmployeeCountYear,
                    Form = b.Form,
                    Status = b.Status,
                    Price = b.Price,
                    UrlsBusiness = b.UrlsBusiness,
                    TurnPrice = b.TurnPrice,
                    ProfitPrice = b.ProfitPrice,
                    Payback = b.Payback,
                    Profitability = b.Profitability,
                    InvestPrice = b.InvestPrice,
                    Text = b.Text,
                    Share = b.Share,
                    Site = b.Site,
                    Peculiarity = b.Peculiarity,
                    NameFinModelFile = b.NameFinModelFile,
                    ReasonsSale = b.ReasonsSale,
                    ReasonsSalePhotoName = b.ReasonsSalePhotoName,
                    UrlVideo = b.UrlVideo,
                    IsGarant = b.IsGarant,
                    UserId = b.UserId,
                    DateCreate = b.DateCreate,
                    Category = b.Category,
                    SubCategory = b.SubCategory
                })
                .FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Метод отправит файл в папку и временно запишет в БД.
        /// </summary>
        /// <param name="files">Файлы.</param>
        /// <returns>Список названий файлов.</returns>
        public async Task<IEnumerable<string>> AddTempFilesBeforeCreateBusinessAsync(IFormFileCollection files, string account)
        {
            try
            {
                var results = new List<string>();
                var userId = string.Empty;

                // Найдет такого пользователя.
                var findUser = await _userRepository.FindUserByEmailOrPhoneNumberAsync(account);

                // Если такого пользователя не найдено, значит поищет по коду.
                if (findUser == null)
                {
                    var findUserIdByCode = await _userRepository.FindUserByCodeAsync(account);

                    if (!string.IsNullOrEmpty(findUserIdByCode))
                    {
                        userId = findUserIdByCode;
                    }
                }

                else
                {
                    userId = findUser.UserId;
                }

                if (!string.IsNullOrEmpty(userId))
                {
                    // Запишет во временную таблицу какие названия файлов, которые добавили но еще не сохранили.
                    foreach (var item in files)
                    {
                        await _postgreDbContext.TempBusinesses.AddAsync(new TempBusinessEntity
                        {
                            FileName = item.FileName,
                            Id = userId
                        });

                        results.Add("../../../assets/images/" + item.FileName);
                    }

                    await _postgreDbContext.SaveChangesAsync();
                }

                return results;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод получит франшизу для просмотра или изменения.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="mode">Режим (Edit или View).</param>
        /// <returns>Данные бизнеса.</returns>
        public async Task<BusinessOutput> GetBusinessAsync(long businessId, string mode)
        {
            try
            {
                // Найдет кто создал бизнес.
                var userId = await _postgreDbContext.Businesses
                    .Where(f => f.BusinessId == businessId)
                    .Select(f => f.UserId)
                    .FirstOrDefaultAsync();

                // Найдет фио пользователя, создавшего франшизу.
                var fio = await _postgreDbContext.Users
                    .Where(u => u.Id.Equals(userId))
                    .Select(u => new FranchiseOutput
                    {
                        FullName = (u.LastName ?? string.Empty) + " " + (u.FirstName ?? string.Empty) + " " + (u.Patronymic ?? string.Empty)
                    })
                    .FirstOrDefaultAsync();

                var result = await (from b in _postgreDbContext.Businesses
                                    where b.BusinessId == businessId
                                    select new BusinessOutput
                                    {
                                        ActivityDetail = b.ActivityDetail,
                                        ActivityPhotoName = b.ActivityPhotoName,
                                        Address = b.Address,
                                        Assets = b.Assets,
                                        AssetsPhotoName = b.AssetsPhotoName,
                                        BusinessAge = b.BusinessAge,
                                        BusinessId = b.BusinessId,
                                        BusinessName = b.BusinessName,
                                        EmployeeCountYear = b.EmployeeCountYear,
                                        Form = b.Form,
                                        Status = b.Status,
                                        Price = string.Format("{0:0,0}", b.Price),
                                        UrlsBusiness = b.UrlsBusiness,
                                        TurnPrice = b.TurnPrice,
                                        ProfitPrice = b.ProfitPrice,
                                        Payback = b.Payback,
                                        Profitability = b.Profitability,
                                        InvestPrice = b.InvestPrice,
                                        Text = b.Text,
                                        Share = b.Share,
                                        Site = b.Site,
                                        Peculiarity = b.Peculiarity,
                                        NameFinModelFile = b.NameFinModelFile,
                                        ReasonsSale = b.ReasonsSale,
                                        ReasonsSalePhotoName = b.ReasonsSalePhotoName,
                                        UrlVideo = b.UrlVideo,
                                        IsGarant = b.IsGarant,
                                        UserId = b.UserId,
                                        DateCreate = b.DateCreate,
                                        Category = b.Category,
                                        SubCategory = b.SubCategory
                                    })
                    .FirstOrDefaultAsync();

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
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий.</returns>
        public async Task<IEnumerable<GetBusinessCategoryOutput>> GetBusinessCategoriesAsync()
        {
            try
            {
                var result = await _postgreDbContext.BusinessCategories
                    .Select(fc => new GetBusinessCategoryOutput
                    {
                        CategoryCode = fc.BusinessCode,
                        CategoryName = fc.BusinessName
                    })
                    .ToListAsync();

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

        /// <summary>
        /// Метод получит список подкатегорий бизнеса.
        /// </summary>
        /// <returns>Список подкатегорий.</returns>
        public async Task<IEnumerable<BusinessSubCategoryOutput>> GetSubBusinessCategoryListAsync()
        {
            try
            {
                var result = await _postgreDbContext.BusinessSubCategories
                    .Select(fc => new BusinessSubCategoryOutput
                    {
                        SubCategoryCode = fc.BusinessSubCategoryCode,
                        SubCategoryName = fc.BusinessSubCategoryName
                    })
                    .ToListAsync();

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

        /// <summary>
        /// Метод получит список городов.
        /// </summary>
        /// <returns>Список городов.</returns>
        public async Task<IEnumerable<BusinessCitiesOutput>> GetCitiesListAsync()
        {
            try
            {
                var result = await _postgreDbContext.BusinessCities
                    .Select(fc => new BusinessCitiesOutput
                    {
                        BusinessCityCode = fc.BusinessCityCode,
                        BusinessCityName = fc.BusinessCityName
                    })
                    .ToListAsync();

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
