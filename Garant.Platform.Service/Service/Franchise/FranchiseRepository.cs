using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Base.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Entities.Franchise;
using Garant.Platform.Models.Franchise.Input;
using Garant.Platform.Models.Franchise.Output;
using Garant.Platform.Models.Request.Output;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Services.Service.Franchise
{
    /// <summary>
    /// Репозиторий франшиз для работы с БД.
    /// </summary>
    public sealed class FranchiseRepository : IFranchiseRepository
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IUserRepository _userRepository;
        private readonly ICommonService _commonService;

        public FranchiseRepository(PostgreDbContext postgreDbContext, IUserRepository userRepository, ICommonService commonService)
        {
            _postgreDbContext = postgreDbContext;
            _userRepository = userRepository;
            _commonService = commonService;
        }

        public async Task<List<FranchiseOutput>> GetFranchisesAsync()
        {
            try
            {
                var items = await (from p in _postgreDbContext.Franchises
                                   select new FranchiseOutput
                                   {
                                       DateCreate = p.DateCreate,
                                       Price = string.Format("{0:0,0}", p.Price),
                                       CountDays = DateTime.Now.Subtract(p.DateCreate).Days,
                                       DayDeclination = "дня",
                                       Text = p.Text,
                                       TextDoPrice = p.TextDoPrice,
                                       Title = p.Title,
                                       Url = p.Url,
                                       IsGarant = p.IsGarant,
                                       ProfitPrice = p.ProfitPrice,
                                       TotalInvest = string.Format("{0:0,0}", p.GeneralInvest),
                                       FranchiseId = p.FranchiseId
                                   })
                    .ToListAsync();

                foreach (var item in items)
                {
                    item.DayDeclination = await _commonService.GetCorrectDayDeclinationAsync(item.CountDays);
                }

                return items;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        public async Task<IEnumerable<PopularFranchiseOutput>> GetMainPopularFranchisesList()
        {
            try
            {
                var result = await (from p in _postgreDbContext.Franchises
                                    select new PopularFranchiseOutput
                                    {
                                        DateCreate = p.DateCreate,
                                        Price = string.Format("{0:0,0}", p.Price),
                                        CountDays = DateTime.Now.Subtract(p.DateCreate).Days,
                                        DayDeclination = "дня",
                                        Text = p.Text,
                                        TextDoPrice = p.TextDoPrice,
                                        Title = p.Title,
                                        Url = p.Url,
                                        TotalInvest = string.Format("{0:0,0}", p.GeneralInvest),
                                        FranchiseId = p.FranchiseId
                                    })
                    .Take(4)
                    .ToListAsync();

                foreach (var item in result)
                {
                    item.DayDeclination = await _commonService.GetCorrectDayDeclinationAsync(item.CountDays);
                }

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
        /// Метод получит 4 франшизы для выгрузки в блок с быстрым поиском.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        public async Task<List<FranchiseOutput>> GetFranchiseQuickSearchAsync()
        {
            try
            {
                var items = await (from p in _postgreDbContext.Franchises
                                   select new FranchiseOutput
                                   {
                                       DateCreate = p.DateCreate,
                                       Price = string.Format("{0:0,0}", p.Price),
                                       CountDays = DateTime.Now.Subtract(p.DateCreate).Days,
                                       DayDeclination = "дня",
                                       Text = p.Text,
                                       TextDoPrice = p.TextDoPrice,
                                       Title = p.Title,
                                       Url = p.Url,
                                       IsGarant = p.IsGarant,
                                       ProfitPrice = p.ProfitPrice,
                                       TotalInvest = string.Format("{0:0,0}", p.GeneralInvest),
                                       FranchiseId = p.FranchiseId
                                   })
                    .Take(4)
                    .ToListAsync();

                foreach (var item in items)
                {
                    item.DayDeclination = await _commonService.GetCorrectDayDeclinationAsync(item.CountDays);
                }

                return items;
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
        /// Метод получит список городов франшиз.
        /// </summary>
        /// <returns>Список городов.</returns>
        public async Task<IEnumerable<FranchiseCityOutput>> GetFranchisesCitiesListAsync()
        {
            try
            {
                var result = await (from c in _postgreDbContext.FranchiseCities
                                    select new FranchiseCityOutput
                                    {
                                        CityCode = c.CityCode,
                                        CityName = c.CityName
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
        /// Метод получит список категорий франшиз.
        /// </summary>
        /// <returns>Список категорий.</returns>
        public async Task<IEnumerable<CategoryOutput>> GetFranchisesCategoriesListAsync()
        {
            try
            {
                var result = await (from c in _postgreDbContext.FranchiseCategories
                                    select new CategoryOutput
                                    {
                                        CategoryCode = c.FranchiseCategoryCode,
                                        CategoryName = c.FranchiseCategoryName
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
        /// Метод получит список видов бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        public async Task<IEnumerable<ViewBusinessOutput>> GetFranchisesViewBusinessListAsync()
        {
            try
            {
                var result = await (from c in _postgreDbContext.ViewBusiness
                                    select new ViewBusinessOutput
                                    {
                                        ViewCode = c.ViewCode,
                                        ViewName = c.ViewName
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
        /// Метод фильтрации франшиз по разным атрибутам.
        /// </summary>
        /// <param name="typeSort">Тип фильтрации цены (по возрастанию или убыванию).</param>
        /// <param name="isGarant">Покупка через гарант.</param>
        /// <param name="minPrice">Прибыль от.</param>
        /// <param name="maxPrice">Прибыль до.</param>
        /// <returns>Список франшиз после фильтрации.</returns>
        public async Task<List<FranchiseOutput>> FilterFranchisesAsync(string typeSort, double minPrice, double maxPrice, string viewCode, string categoryCode, double minPriceInvest, double maxPriceInvest, bool isGarant = false)
        {
            try
            {
                List<FranchiseOutput> items = null;
                IQueryable<FranchiseOutput> query = null;

                // Сортировать на возрастанию цены.
                if (typeSort.Equals("Asc"))
                {
                    query = (from f in _postgreDbContext.Franchises
                             where f.ViewBusiness.Equals(viewCode)
                                   && f.Category.Equals(categoryCode)
                                   && (f.Price <= maxPrice && f.Price >= minPrice)
                                   && (f.GeneralInvest >= minPriceInvest && f.GeneralInvest <= maxPriceInvest)
                                   && f.IsGarant == isGarant
                             orderby f.FranchiseId
                             select new FranchiseOutput
                             {
                                 DateCreate = f.DateCreate,
                                 Price = string.Format("{0:0,0}", f.Price),
                                 CountDays = DateTime.Now.Subtract(f.DateCreate).Days,
                                 DayDeclination = "дня",
                                 Text = f.Text,
                                 TextDoPrice = f.TextDoPrice,
                                 Title = f.Title,
                                 Url = f.Url,
                                 IsGarant = f.IsGarant,
                                 ProfitPrice = f.ProfitPrice,
                                 TotalInvest = string.Format("{0:0,0}", f.GeneralInvest),
                                 FranchiseId = f.FranchiseId
                             })
                        .AsQueryable();

                    Console.WriteLine();
                }

                // Сортировать на убыванию цены.
                else if (typeSort.Equals("Desc"))
                {
                    query = (from f in _postgreDbContext.Franchises
                             where f.ViewBusiness.Equals(viewCode)
                                   && f.Category.Equals(categoryCode)
                                   && (f.Price <= maxPrice && f.Price >= minPrice)
                                   && (f.GeneralInvest >= minPriceInvest && f.GeneralInvest <= maxPriceInvest)
                                   && f.IsGarant == isGarant
                             orderby f.FranchiseId descending
                             select new FranchiseOutput
                             {
                                 DateCreate = f.DateCreate,
                                 Price = string.Format("{0:0,0}", f.Price),
                                 CountDays = DateTime.Now.Subtract(f.DateCreate).Days,
                                 DayDeclination = "дня",
                                 Text = f.Text,
                                 TextDoPrice = f.TextDoPrice,
                                 Title = f.Title,
                                 Url = f.Url,
                                 IsGarant = f.IsGarant,
                                 ProfitPrice = f.ProfitPrice
                             })
                        .AsQueryable();
                }

                if (query != null)
                {
                    // Нужно ли дополнить запрос для сортировки по прибыли.
                    if (minPrice > 0 && maxPrice > 0)
                    {
                        query = query.Where(c => c.ProfitPrice <= Convert.ToDouble(maxPrice)
                                                 && c.ProfitPrice >= Convert.ToDouble(minPrice));
                    }

                    items = await query.ToListAsync();

                    foreach (var item in items)
                    {
                        item.DayDeclination = await _commonService.GetCorrectDayDeclinationAsync(item.CountDays);
                    }
                }

                return items;
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
        /// Метод получит новые франшизы, которые были созданы в текущем месяце.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        public async Task<List<FranchiseOutput>> GetNewFranchisesAsync()
        {
            try
            {
                var month = DateTime.Now.Month;

                var items = await (from f in _postgreDbContext.Franchises
                                   where f.DateCreate.Month == month
                                   select new FranchiseOutput
                                   {
                                       DateCreate = f.DateCreate,
                                       Price = string.Format("{0:0,0}", f.Price),
                                       CountDays = DateTime.Now.Subtract(f.DateCreate).Days,
                                       DayDeclination = "дня",
                                       Text = f.Text,
                                       TextDoPrice = f.TextDoPrice,
                                       Title = f.Title,
                                       Url = f.Url,
                                       IsGarant = f.IsGarant,
                                       ProfitPrice = f.ProfitPrice,
                                       TotalInvest = string.Format("{0:0,0}", f.GeneralInvest),
                                       FranchiseId = f.FranchiseId
                                   })
                    .Take(10)
                    .ToListAsync();

                foreach (var item in items)
                {
                    item.DayDeclination = await _commonService.GetCorrectDayDeclinationAsync(item.CountDays);
                }

                return items;
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
        /// Метод получит список отзывов о франшизах.
        /// </summary>
        /// <returns>Список отзывов.</returns>
        public async Task<List<FranchiseOutput>> GetReviewsFranchisesAsync()
        {
            try
            {
                var items = await (from f in _postgreDbContext.Franchises
                                   select new FranchiseOutput
                                   {
                                       DateCreate = f.DateCreate,
                                       Price = string.Format("{0:0,0}", f.Price),
                                       CountDays = DateTime.Now.Subtract(f.DateCreate).Days,
                                       DayDeclination = "дня",
                                       Text = f.Text,
                                       TextDoPrice = f.TextDoPrice,
                                       Title = f.Title,
                                       Url = f.Url,
                                       IsGarant = f.IsGarant,
                                       ProfitPrice = f.ProfitPrice
                                   })
                    .Take(10)
                    .ToListAsync();

                foreach (var item in items)
                {
                    item.DayDeclination = await _commonService.GetCorrectDayDeclinationAsync(item.CountDays);
                }

                return items;
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
        /// Метод создаст новую  или обновит существующую франшизу.
        /// </summary>
        /// <param name="files">Входные файлы.</param>
        /// <param name="franchiseInput">Входная модель.</param>
        /// <param name="lastFranchiseId">Id последней франшизы.</param>
        /// <param name="urlsDetails">Пути к доп.изображениям.</param>
        /// <returns>Данные франшизы.</returns>
        public async Task<CreateUpdateFranchiseOutput> CreateUpdateFranchiseAsync(
            CreateUpdateFranchiseInput franchiseInput, long lastFranchiseId, string[] urlsDetails,
            IFormFileCollection files, string account)
        {
            try
            {
                CreateUpdateFranchiseOutput result = null;

                if (franchiseInput != null)
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

                    // Найдет франшизу по Id.
                    var findFranchise = await FindFranchiseByIdAsync(franchiseInput.FranchiseId);
                    var urls = await _commonService.JoinArrayWithDelimeterAsync(franchiseInput.UrlsFranchise) ?? string.Empty;

                    // Создаст новую франшизу.
                    if (franchiseInput.IsNew && findFranchise == null)
                    {
                        await _postgreDbContext.Franchises.AddAsync(new FranchiseEntity
                        {
                            FranchiseId = lastFranchiseId,
                            ActivityDetail = franchiseInput.ActivityDetail,
                            BaseDate = DateTime.Now.Year,
                            BusinessCount = franchiseInput.BusinessCount,
                            Category = franchiseInput.Category,
                            SubCategory = franchiseInput.SubCategory,
                            DateCreate = DateTime.Now,
                            DotCount = franchiseInput.DotCount,
                            FinIndicators = franchiseInput.FinIndicators,
                            FranchisePacks = franchiseInput.FranchisePacks,
                            //UrlsDetails = urlsDetails.ToArray(),
                            UrlLogo = "../../../assets/images/" + files.Where(c => c.Name.Equals("filesLogo")).ToArray()[0].FileName,
                            NameFinIndicators = franchiseInput.FinIndicators,
                            NameFinModelFile = files.Where(c => c.Name.Equals("finModelFile")).ToArray()[0].FileName,
                            NameFranchisePhoto = files.Where(c => c.Name.Equals("franchiseFile")).ToArray()[0].FileName,
                            NamePresentFile = files.Where(c => c.Name.Equals("presentFile")).ToArray()[0].FileName,
                            TrainingPhotoName = files.Where(c => c.Name.Equals("trainingPhoto")).ToArray()[0].FileName,
                            Title = franchiseInput.Title,
                            Text = franchiseInput.Text,
                            Price = franchiseInput.Price,
                            ViewBusiness = franchiseInput.ViewBusiness,
                            IsGarant = franchiseInput.IsGarant,
                            ProfitMonth = franchiseInput.ProfitMonth,
                            ProfitPrice = franchiseInput.ProfitPrice,
                            Status = franchiseInput.Status,
                            YearStart = franchiseInput.YearStart,
                            GeneralInvest = franchiseInput.GeneralInvest,
                            LumpSumPayment = franchiseInput.LumpSumPayment,
                            Royalty = franchiseInput.Royalty,
                            Payback = franchiseInput.Payback,
                            LaunchDate = franchiseInput.LaunchDate,
                            InvestInclude = franchiseInput.InvestInclude,
                            Peculiarity = franchiseInput.Peculiarity,
                            PaymentDetail = franchiseInput.PaymentDetail,
                            TrainingDetails = franchiseInput.TrainingDetails,
                            UrlVideo = franchiseInput.UrlVideo,
                            Reviews = franchiseInput.Reviews,
                            TextDoPrice = "Сумма сделки:",
                            UserId = userId,
                            Url = urls
                        });
                    }

                    // Обновит франшизу.
                    else if (!franchiseInput.IsNew && findFranchise != null)
                    {
                        findFranchise.ActivityDetail = franchiseInput.ActivityDetail;
                        findFranchise.BaseDate = DateTime.Now.Year;
                        findFranchise.BusinessCount = franchiseInput.BusinessCount;
                        findFranchise.Category = franchiseInput.Category;
                        findFranchise.SubCategory = franchiseInput.SubCategory;
                        findFranchise.DateCreate = DateTime.Now;
                        findFranchise.DotCount = franchiseInput.DotCount;
                        findFranchise.FinIndicators = franchiseInput.FinIndicators;
                        findFranchise.FranchisePacks = franchiseInput.FranchisePacks;
                        //findFranchise.UrlsDetails = urlsDetails.ToArray();
                        findFranchise.UrlLogo = "../../../assets/images/" + files.Where(c => c.Name.Equals("filesLogo")).ToArray()[0].FileName;
                        findFranchise.NameFinIndicators = franchiseInput.FinIndicators;
                        findFranchise.NameFinModelFile =
                            files.Where(c => c.Name.Equals("finModelFile")).ToArray()[0].FileName;
                        findFranchise.NamePresentFile =
                            findFranchise.NameFranchisePhoto =
                                files.Where(c => c.Name.Equals("franchiseFile")).ToArray()[0].FileName;
                        findFranchise.NamePresentFile =
                            files.Where(c => c.Name.Equals("presentFile")).ToArray()[0].FileName;
                        findFranchise.TrainingPhotoName =
                            files.Where(c => c.Name.Equals("trainingPhoto")).ToArray()[0].FileName;
                        findFranchise.Title = franchiseInput.Title;
                        findFranchise.Text = franchiseInput.Text;
                        findFranchise.Price = franchiseInput.Price;
                        findFranchise.ViewBusiness = franchiseInput.ViewBusiness;
                        findFranchise.IsGarant = franchiseInput.IsGarant;
                        findFranchise.ProfitMonth = franchiseInput.ProfitMonth;
                        findFranchise.ProfitPrice = franchiseInput.ProfitPrice;
                        findFranchise.Status = franchiseInput.Status;
                        findFranchise.YearStart = franchiseInput.YearStart;
                        findFranchise.GeneralInvest = franchiseInput.GeneralInvest;
                        findFranchise.LumpSumPayment = franchiseInput.LumpSumPayment;
                        findFranchise.Royalty = franchiseInput.Royalty;
                        findFranchise.Payback = franchiseInput.Payback;
                        findFranchise.LaunchDate = franchiseInput.LaunchDate;
                        findFranchise.InvestInclude = franchiseInput.InvestInclude;
                        findFranchise.Peculiarity = franchiseInput.Peculiarity;
                        findFranchise.PaymentDetail = franchiseInput.PaymentDetail;
                        findFranchise.TrainingDetails = franchiseInput.TrainingDetails;
                        findFranchise.UrlVideo = franchiseInput.UrlVideo;
                        findFranchise.Reviews = franchiseInput.Reviews;
                        findFranchise.TextDoPrice = "Сумма сделки:";
                        findFranchise.Url = urls;

                        _postgreDbContext.Update(findFranchise);
                    }

                    await _postgreDbContext.SaveChangesAsync();

                    result = new CreateUpdateFranchiseOutput
                    {
                        ActivityDetail = franchiseInput.ActivityDetail,
                        BaseDate = DateTime.Now.Year,
                        BusinessCount = franchiseInput.BusinessCount,
                        Category = franchiseInput.Category,
                        SubCategory = franchiseInput.SubCategory,
                        DateCreate = DateTime.Now,
                        DotCount = franchiseInput.DotCount,
                        FinIndicators = franchiseInput.FinIndicators,
                        FranchisePacks = franchiseInput.FranchisePacks,
                        //UrlsDetails = urlsDetails.ToArray(),
                        FileLogoUrl = "../../../assets/images/" + files.Where(c => c.Name.Equals("filesLogo")).ToArray()[0].FileName,
                        NameFinIndicators = franchiseInput.FinIndicators,
                        FinModelFileUrl = "/docs" + files.Where(c => c.Name.Equals("finModelFile")).ToArray()[0].FileName,
                        FranchisePhotoUrl = "../../../assets/images/" + files.Where(c => c.Name.Equals("franchiseFile")).ToArray()[0].FileName,
                        PresentFileUrl = "/docs" + files.Where(c => c.Name.Equals("presentFile")).ToArray()[0].FileName,
                        TrainingPhotoUrl = "../../../assets/images/" + files.Where(c => c.Name.Equals("trainingPhoto")).ToArray()[0].FileName,
                        Title = franchiseInput.Title,
                        Text = franchiseInput.Text,
                        Price = franchiseInput.Price,
                        ViewBusiness = franchiseInput.ViewBusiness,
                        IsGarant = franchiseInput.IsGarant,
                        ProfitMonth = franchiseInput.ProfitMonth,
                        ProfitPrice = franchiseInput.ProfitPrice,
                        Status = franchiseInput.Status,
                        YearStart = franchiseInput.YearStart,
                        GeneralInvest = franchiseInput.GeneralInvest,
                        LumpSumPayment = franchiseInput.LumpSumPayment,
                        Royalty = franchiseInput.Royalty,
                        Payback = franchiseInput.Payback,
                        LaunchDate = franchiseInput.LaunchDate,
                        InvestInclude = franchiseInput.InvestInclude,
                        Peculiarity = franchiseInput.Peculiarity,
                        PaymentDetail = franchiseInput.PaymentDetail,
                        TrainingDetails = franchiseInput.TrainingDetails,
                        UrlVideo = franchiseInput.UrlVideo,
                        Reviews = franchiseInput.Reviews
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
        /// Метод найдет франшизу по названию.
        /// </summary>
        /// <param name="title">Название франшизы.</param>
        /// <returns>Данные франшизы.</returns>
        public async Task<FranchiseEntity> FindFranchiseByTitleAsync(string title)
        {
            try
            {
                var result = await _postgreDbContext.Franchises
                    .Where(f => f.Title.Equals(title))
                    .Select(f => new FranchiseEntity
                    {
                        FranchiseId = f.FranchiseId,
                        ActivityDetail = f.ActivityDetail,
                        BaseDate = f.BaseDate,
                        BusinessCount = f.BusinessCount,
                        Category = f.Category,
                        SubCategory = f.SubCategory,
                        DateCreate = f.DateCreate,
                        DotCount = f.DotCount,
                        FinIndicators = f.FinIndicators,
                        FranchisePacks = f.FranchisePacks,
                        UrlsDetails = f.UrlsDetails,
                        UrlLogo = f.UrlLogo,
                        NameFinIndicators = f.FinIndicators,
                        NameFinModelFile = f.NameFinModelFile,
                        NameFranchisePhoto = f.NameFranchisePhoto,
                        NamePresentFile = f.NamePresentFile,
                        TrainingPhotoName = f.TrainingPhotoName,
                        Title = f.Title,
                        Text = f.Text,
                        Price = f.Price,
                        ViewBusiness = f.ViewBusiness,
                        IsGarant = f.IsGarant,
                        ProfitMonth = f.ProfitMonth,
                        ProfitPrice = f.ProfitPrice,
                        Status = f.Status,
                        YearStart = f.YearStart,
                        GeneralInvest = f.GeneralInvest,
                        LumpSumPayment = f.LumpSumPayment,
                        Royalty = f.Royalty,
                        Payback = f.Payback,
                        LaunchDate = f.LaunchDate,
                        InvestInclude = f.InvestInclude,
                        Peculiarity = f.Peculiarity,
                        PaymentDetail = f.PaymentDetail,
                        TrainingDetails = f.TrainingDetails,
                        UrlVideo = f.UrlVideo,
                        Reviews = f.Reviews,
                        Url = f.Url
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
        /// Метод найдет франшизу по Id.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <returns>Данные франшизы.</returns>
        public async Task<FranchiseEntity> FindFranchiseByIdAsync(long franchiseId)
        {
            try
            {
                var result = await _postgreDbContext.Franchises
                    .Where(f => f.FranchiseId == franchiseId)
                    .Select(f => new FranchiseEntity
                    {
                        FranchiseId = f.FranchiseId,
                        ActivityDetail = f.ActivityDetail,
                        BaseDate = f.BaseDate,
                        BusinessCount = f.BusinessCount,
                        Category = f.Category,
                        SubCategory = f.SubCategory,
                        DateCreate = f.DateCreate,
                        DotCount = f.DotCount,
                        FinIndicators = f.FinIndicators,
                        FranchisePacks = f.FranchisePacks,
                        UrlsDetails = f.UrlsDetails,
                        UrlLogo = f.UrlLogo,
                        NameFinIndicators = f.FinIndicators,
                        NameFinModelFile = f.NameFinModelFile,
                        NameFranchisePhoto = f.NameFranchisePhoto,
                        NamePresentFile = f.NamePresentFile,
                        TrainingPhotoName = f.TrainingPhotoName,
                        Title = f.Title,
                        Text = f.Text,
                        Price = f.Price,
                        ViewBusiness = f.ViewBusiness,
                        IsGarant = f.IsGarant,
                        ProfitMonth = f.ProfitMonth,
                        ProfitPrice = f.ProfitPrice,
                        Status = f.Status,
                        YearStart = f.YearStart,
                        GeneralInvest = f.GeneralInvest,
                        LumpSumPayment = f.LumpSumPayment,
                        Royalty = f.Royalty,
                        Payback = f.Payback,
                        LaunchDate = f.LaunchDate,
                        InvestInclude = f.InvestInclude,
                        Peculiarity = f.Peculiarity,
                        PaymentDetail = f.PaymentDetail,
                        TrainingDetails = f.TrainingDetails,
                        UrlVideo = f.UrlVideo,
                        Reviews = f.Reviews,
                        Url = f.Url
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
        /// Метод найдет франшизу по Id пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Данные франшизы.</returns>
        public async Task<FranchiseEntity> FindFranchiseByUserIdAsync(string userId)
        {
            try
            {
                var result = await _postgreDbContext.Franchises
                   .Where(f => f.UserId.Equals(userId))
                   .Select(f => new FranchiseEntity
                   {
                       FranchiseId = f.FranchiseId,
                       ActivityDetail = f.ActivityDetail,
                       BaseDate = f.BaseDate,
                       BusinessCount = f.BusinessCount,
                       Category = f.Category,
                       SubCategory = f.SubCategory,
                       DateCreate = f.DateCreate,
                       DotCount = f.DotCount,
                       FinIndicators = f.FinIndicators,
                       FranchisePacks = f.FranchisePacks,
                       UrlsDetails = f.UrlsDetails,
                       UrlLogo = f.UrlLogo,
                       NameFinIndicators = f.FinIndicators,
                       NameFinModelFile = f.NameFinModelFile,
                       NameFranchisePhoto = f.NameFranchisePhoto,
                       NamePresentFile = f.NamePresentFile,
                       TrainingPhotoName = f.TrainingPhotoName,
                       Title = f.Title,
                       Text = f.Text,
                       Price = f.Price,
                       ViewBusiness = f.ViewBusiness,
                       IsGarant = f.IsGarant,
                       ProfitMonth = f.ProfitMonth,
                       ProfitPrice = f.ProfitPrice,
                       Status = f.Status,
                       YearStart = f.YearStart,
                       GeneralInvest = f.GeneralInvest,
                       LumpSumPayment = f.LumpSumPayment,
                       Royalty = f.Royalty,
                       Payback = f.Payback,
                       LaunchDate = f.LaunchDate,
                       InvestInclude = f.InvestInclude,
                       Peculiarity = f.Peculiarity,
                       PaymentDetail = f.PaymentDetail,
                       TrainingDetails = f.TrainingDetails,
                       UrlVideo = f.UrlVideo,
                       Reviews = f.Reviews,
                       Url = f.Url
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
        /// Метод получит франшизу для просмотра или изменения.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <param name="mode">Режим (Edit или View).</param>
        /// <returns>Данные франшизы.</returns>
        public async Task<FranchiseOutput> GetFranchiseAsync(long franchiseId, string mode)
        {
            try
            {
                // Найдет кто создал франшизу.
                var userId = await _postgreDbContext.Franchises
                    .Where(f => f.FranchiseId == franchiseId)
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

                var result = await (from f in _postgreDbContext.Franchises
                                    where f.FranchiseId == franchiseId
                                    select new FranchiseOutput
                                    {
                                        FranchiseId = f.FranchiseId,
                                        ActivityDetail = f.ActivityDetail,
                                        BaseDate = f.BaseDate,
                                        BusinessCount = f.BusinessCount,
                                        Category = f.Category,
                                        SubCategory = f.SubCategory,
                                        DateCreate = f.DateCreate,
                                        DotCount = f.DotCount,
                                        FinIndicators = f.FinIndicators,
                                        FranchisePacks = f.FranchisePacks,
                                        UrlsDetails = f.UrlsDetails,
                                        UrlLogo = f.UrlLogo,
                                        NameFinIndicators = f.FinIndicators,
                                        NameFinModelFile = f.NameFinModelFile,
                                        NameFranchisePhoto = f.NameFranchisePhoto,
                                        NamePresentFile = f.NamePresentFile,
                                        TrainingPhotoName = f.TrainingPhotoName,
                                        Title = f.Title,
                                        Text = f.Text,
                                        Price = string.Format("{0:0,0}", f.Price),
                                        ViewBusiness = f.ViewBusiness,
                                        IsGarant = f.IsGarant,
                                        ProfitMonth = f.ProfitMonth,
                                        ProfitPrice = f.ProfitPrice,
                                        Status = f.Status,
                                        YearStart = f.YearStart,
                                        GeneralInvest = f.GeneralInvest,
                                        LumpSumPayment = f.LumpSumPayment,
                                        Royalty = f.Royalty,
                                        Payback = f.Payback,
                                        LaunchDate = f.LaunchDate,
                                        InvestInclude = f.InvestInclude,
                                        Peculiarity = f.Peculiarity,
                                        PaymentDetail = f.PaymentDetail,
                                        TrainingDetails = f.TrainingDetails,
                                        UrlVideo = f.UrlVideo,
                                        Reviews = f.Reviews,
                                        Mode = mode,
                                        TotalInvest = string.Format("{0:0,0}", f.GeneralInvest),
                                        Url = f.Url,
                                        FullName = fio.FullName,
                                        UserId = f.UserId
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
        /// Метод отправит файл в папку и временно запишет в БД.
        /// </summary>
        /// <param name="files">Файлы.</param>
        /// <returns>Список названий файлов.</returns>
        public async Task<IEnumerable<string>> AddTempFilesBeforeCreateFranchiseAsync(IFormFileCollection files, string account)
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
                        await _postgreDbContext.TempFranchises.AddAsync(new TempFranchiseEntity
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
        /// Метод получит список категорий франшиз.
        /// </summary>
        /// <returns>Список категорий.</returns>
        public async Task<IEnumerable<CategoryOutput>> GetCategoryListAsync()
        {
            try
            {
                var result = await _postgreDbContext.FranchiseCategories
                    .Select(fc => new CategoryOutput
                    {
                        CategoryCode = fc.FranchiseCategoryCode,
                        CategoryName = fc.FranchiseCategoryName
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
        /// Метод получит список подкатегорий франшиз.
        /// </summary>
        /// <returns>Список подкатегорий.</returns>
        public async Task<IEnumerable<SubCategoryOutput>> GetSubCategoryListAsync()
        {
            try
            {
                var result = await _postgreDbContext.FranchiseSubCategories
                    .Select(fc => new SubCategoryOutput
                    {
                        SubCategoryCode = fc.FranchiseSubCategoryCode,
                        SubCategoryName = fc.FranchiseSubCategoryName
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
        /// Метод получит заголовок франшизы по Id пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Заголовок франшизы.</returns>
        public async Task<string> GetFranchiseTitleAsync(string userId)
        {
            try
            {
                var title = await _postgreDbContext.Franchises
                    .Where(f => f.UserId.Equals(userId))
                    .Select(f => f.Title)
                    .FirstOrDefaultAsync();

                return title;
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
        /// Метод найдет среди франшиз по запросу.
        /// </summary>
        /// <param name="searchText">Текст поиска.</param>
        /// <returns>Список с результатами.</returns>
        public async Task<IEnumerable<FranchiseOutput>> SearchByFranchisesAsync(string searchText)
        {
            try
            {
                var result = await _postgreDbContext.Franchises
                    .Where(f => f.Title.Contains(searchText) || f.ActivityDetail.Contains(searchText))
                    .Select(f => new FranchiseOutput
                    {
                        DateCreate = f.DateCreate,
                        Price = string.Format("{0:0,0}", f.Price),
                        CountDays = DateTime.Now.Subtract(f.DateCreate).Days,
                        DayDeclination = "дня",
                        Text = f.Text,
                        TextDoPrice = f.TextDoPrice,
                        Title = f.Title,
                        Url = f.Url,
                        IsGarant = f.IsGarant,
                        ProfitPrice = f.ProfitPrice,
                        TotalInvest = string.Format("{0:0,0}", f.GeneralInvest),
                        FranchiseId = f.FranchiseId
                    })
                    .ToListAsync();

                foreach (var item in result)
                {
                    item.DayDeclination = await _commonService.GetCorrectDayDeclinationAsync(item.CountDays);
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
        /// Метод создаст заявку франшизы.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="phone">Телефон.</param>
        /// <param name="city">Город.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <param name="franchiseId">Id франшизы, по которой оставлена заявка.</param>
        /// <returns>Данные заявки.</returns>
        public async Task<RequestFranchiseOutput> CreateRequestFranchiseAsync(string userName, string phone, string city, string account, long franchiseId)
        {
            try
            {
                var userId = await _userRepository.FindUserIdUniverseAsync(account);
                var now = DateTime.Now;

                var addRequestData = new RequestFranchiseEntity
                {
                    UserId = userId,
                    City = city,
                    DateCreate = now,
                    Phone = phone,
                    UserName = userName,
                    FranchiseId = franchiseId
                };

                await _postgreDbContext.RequestsFranchises.AddAsync(addRequestData);
                await _postgreDbContext.SaveChangesAsync();

                var result = new RequestFranchiseOutput
                {
                    UserId = userId,
                    City = city,
                    DateCreate = now,
                    Phone = phone,
                    UserName = userName,
                    FranchiseId = franchiseId
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
    }
}