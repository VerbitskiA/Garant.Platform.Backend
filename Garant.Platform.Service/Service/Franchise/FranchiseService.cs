using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Franchise.Output;
using System.Linq;
using Garant.Platform.FTP.Abstraction;
using Garant.Platform.Models.Entities.Franchise;
using Garant.Platform.Models.Franchise.Input;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Service.Service.Franchise
{
    public sealed class FranchiseService : IFranchiseService
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IFtpService _ftpService;
        private readonly IUserService _userService;

        public FranchiseService(PostgreDbContext postgreDbContext, IFtpService ftpService, IUserService userService)
        {
            _postgreDbContext = postgreDbContext;
            _ftpService = ftpService;
            _userService = userService;
        }

        /// <summary>
        /// Метод получит список франшиз.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        public async Task<IEnumerable<FranchiseOutput>> GetFranchisesListAsync()
        {
            try
            {
                var items = await (from p in _postgreDbContext.Franchises
                                    select new FranchiseOutput
                                    {
                                        DateCreate = p.DateCreate,
                                        Price = string.Format("{0:0,0}", p.Price),
                                        CountDays = DateTime.Now.Day - p.DateCreate.Day,
                                        DayDeclination = "дня",
                                        Text = p.Text,
                                        TextDoPrice = p.TextDoPrice,
                                        Title = p.Title,
                                        Url = p.Url,
                                        IsGarant = p.IsGarant,
                                        ProfitPrice = p.ProfitPrice
                                    })
                    .ToListAsync();

                foreach (var item in items)
                {
                    item.FullText = item.Text + " " + item.CountDays + " " + item.DayDeclination;
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
        /// Метод получит список популярных франшиз для главной страницы.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        public async Task<IEnumerable<PopularFranchiseOutput>> GetMainPopularFranchises()
        {
            try
            {
                var result = await (from p in _postgreDbContext.PopularFranchises
                                    select new PopularFranchiseOutput
                                    {
                                        DateCreate = p.DateCreate,
                                        Price = string.Format("{0:0,0}", p.Price),
                                        CountDays = DateTime.Now.Day - p.DateCreate.Day,
                                        DayDeclination = "дня",
                                        Text = p.Text,
                                        TextDoPrice = p.TextDoPrice,
                                        Title = p.Title,
                                        Url = p.Url
                                    })
                    .Take(4)
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
        /// Метод получит 4 франшизы для выгрузки в блок с быстрым поиском.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        public async Task<IEnumerable<FranchiseOutput>> GetFranchiseQuickSearchAsync()
        {
            try
            {
                var items = await (from p in _postgreDbContext.Franchises
                                   select new FranchiseOutput
                                   {
                                       DateCreate = p.DateCreate,
                                       Price = string.Format("{0:0,0}", p.Price),
                                       CountDays = DateTime.Now.Day - p.DateCreate.Day,
                                       DayDeclination = "дня",
                                       Text = p.Text,
                                       TextDoPrice = p.TextDoPrice,
                                       Title = p.Title,
                                       Url = p.Url,
                                       IsGarant = p.IsGarant,
                                       ProfitPrice = p.ProfitPrice
                                   })
                    .Take(4)
                    .ToListAsync();

                foreach (var item in items)
                {
                    item.FullText = item.Text + " " + item.CountDays + " " + item.DayDeclination;
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
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий.</returns>
        public async Task<IEnumerable<CategoryOutput>> GetFranchisesCategoriesListAsync()
        {
            try
            {
                var result = await (from c in _postgreDbContext.Categories
                                    select new CategoryOutput
                                    {
                                        CategoryCode = c.CategoryCode,
                                        CategoryName = c.CategoryName
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
        public async Task<IEnumerable<FranchiseOutput>> FilterFranchisesAsync(string typeSort, string minPrice, string maxPrice, bool isGarant = false)
        {
            try
            {
                IEnumerable<FranchiseOutput> items = null;

                // Сортировать на возрастанию цены.
                if (typeSort.Equals("Asc"))
                {
                    var query = (from f in _postgreDbContext.Franchises
                                 orderby f.Price
                                 select new FranchiseOutput
                                 {
                                     DateCreate = f.DateCreate,
                                     Price = string.Format("{0:0,0}", f.Price),
                                     CountDays = DateTime.Now.Day - f.DateCreate.Day,
                                     DayDeclination = "дня",
                                     Text = f.Text,
                                     TextDoPrice = f.TextDoPrice,
                                     Title = f.Title,
                                     Url = f.Url,
                                     IsGarant = f.IsGarant,
                                     ProfitPrice = f.ProfitPrice
                                 })
                        .AsQueryable();

                    // Нужно ли дополнить запрос для сортировки по прибыли.
                    if (!string.IsNullOrEmpty(minPrice) && !string.IsNullOrEmpty(maxPrice))
                    {
                        query = query.Where(c => c.ProfitPrice <= Convert.ToDouble(maxPrice) 
                                                 && c.ProfitPrice >= Convert.ToDouble(minPrice));
                    }

                    items = await query.ToListAsync();
                }

                // Сортировать на убыванию цены.
                else if (typeSort.Equals("Desc"))
                {
                    var query = (from f in _postgreDbContext.Franchises
                                 orderby f.Price descending
                                 select new FranchiseOutput
                                 {
                                     DateCreate = f.DateCreate,
                                     Price = string.Format("{0:0,0}", f.Price),
                                     CountDays = DateTime.Now.Day - f.DateCreate.Day,
                                     DayDeclination = "дня",
                                     Text = f.Text,
                                     TextDoPrice = f.TextDoPrice,
                                     Title = f.Title,
                                     Url = f.Url,
                                     IsGarant = f.IsGarant,
                                     ProfitPrice = f.ProfitPrice
                                 })
                        .AsQueryable();

                    // Нужно ли дополнить запрос для сортировки по прибыли.
                    if (!string.IsNullOrEmpty(minPrice) && !string.IsNullOrEmpty(maxPrice))
                    {
                        query = query.Where(c => c.ProfitPrice <= Convert.ToDouble(maxPrice)
                                                 && c.ProfitPrice >= Convert.ToDouble(minPrice));
                    }

                    items = await query.ToListAsync();
                }

                foreach (var item in items)
                {
                    item.FullText = item.Text + " " + item.CountDays + " " + item.DayDeclination;
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
        public async Task<IEnumerable<FranchiseOutput>> GetNewFranchisesAsync()
        {
            try
            {
                var year = DateTime.Now.Year;

                var items = await (from f in _postgreDbContext.Franchises
                                    where f.DateCreate.Year == year
                                    select new FranchiseOutput
                                    {
                                        DateCreate = f.DateCreate,
                                        Price = string.Format("{0:0,0}", f.Price),
                                        CountDays = DateTime.Now.Day - f.DateCreate.Day,
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
                    item.FullText = item.Text + " " + item.CountDays + " " + item.DayDeclination;
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
        public async Task<IEnumerable<FranchiseOutput>> GetReviewsFranchisesAsync()
        {
            try
            {
                var items = await (from f in _postgreDbContext.Franchises
                                   select new FranchiseOutput
                                   {
                                       DateCreate = f.DateCreate,
                                       Price = string.Format("{0:0,0}", f.Price),
                                       CountDays = DateTime.Now.Day - f.DateCreate.Day,
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
                    item.FullText = item.Text + " " + item.CountDays + " " + item.DayDeclination;
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
        /// <param name="franchiseInput">Входная модель.</param>
        /// <returns>Данные франшизы.</returns>
        public async Task<CreateUpdateFranchiseOutput> CreateUpdateFranchiseAsync(CreateUpdateFranchiseInput franchiseInput)
        {
            try
            {
                // Отправит файлы дополнительных фото франшизы на сервер по FTP.
                if (franchiseInput.UrlsDetails.Count > 0)
                {
                    await _ftpService.UploadFilesFtpAsync(franchiseInput.UrlsDetails);
                }

                // Отправит файл основного фото на сервер по FTP.
                if (!string.IsNullOrEmpty(franchiseInput.FranchisePhoto.Files[0].FileName))
                {
                    await _ftpService.UploadFilesFtpAsync(franchiseInput.FranchisePhoto);
                }

                // Отправит фин.модель на сервер по FTP.
                if (!string.IsNullOrEmpty(franchiseInput.FinModelFile.Files[0].FileName))
                {
                    await _ftpService.UploadFilesFtpAsync(franchiseInput.FinModelFile);
                }

                // Отправит лого на сервер по FTP.
                if (!string.IsNullOrEmpty(franchiseInput.FileLogo.Files[0].FileName))
                {
                    await _ftpService.UploadFilesFtpAsync(franchiseInput.FileLogo);
                }

                // Отправит файл презентации на сервер по FTP.
                if (!string.IsNullOrEmpty(franchiseInput.PresentFile.Files[0].FileName))
                {
                    await _ftpService.UploadFilesFtpAsync(franchiseInput.PresentFile);
                }

                // Отправит файл обучения на сервер по FTP.
                if (!string.IsNullOrEmpty(franchiseInput.TrainingPhoto.Files[0].FileName))
                {
                    await _ftpService.UploadFilesFtpAsync(franchiseInput.TrainingPhoto);
                }

                // Найдет франшизу с таким названием.
                var findFranchise = await FindFranchiseByTitle(franchiseInput.Title);

                var urlsDetails = new List<string>();

                // Запишет пути к доп.изображениям франшизы.
                foreach (var item in franchiseInput.UrlsDetails.Files)
                {
                    urlsDetails.Add("../../../assets/images/" + item.FileName);
                }

                var lastFranchiseId = await _postgreDbContext.Franchises.MaxAsync(c => c.FranchiseId);
                lastFranchiseId++;

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
                        UrlsDetails = urlsDetails.ToArray(),
                        UrlLogo = "../../../assets/images/" + franchiseInput.FileLogo.Files[0].FileName,
                        NameFinIndicators = franchiseInput.FinIndicators,
                        NameFinModelFile = franchiseInput.FinModelFile.Files[0].FileName,
                        NameFranchisePhoto = franchiseInput.FranchisePhoto.Files[0].FileName,
                        NamePresentFile = franchiseInput.PresentFile.Files[0].FileName,
                        TrainingPhotoName = franchiseInput.TrainingPhoto.Files[0].FileName,
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
                    findFranchise.UrlsDetails = urlsDetails.ToArray();
                    findFranchise.UrlLogo = "../../../assets/images/" + franchiseInput.FileLogo.Files[0].FileName;
                    findFranchise.NameFinIndicators = franchiseInput.FinIndicators;
                    findFranchise.NameFinModelFile = franchiseInput.FinModelFile.Files[0].FileName;
                    findFranchise.NameFranchisePhoto = franchiseInput.FranchisePhoto.Files[0].FileName;
                    findFranchise.NamePresentFile = franchiseInput.PresentFile.Files[0].FileName;
                    findFranchise.TrainingPhotoName = franchiseInput.TrainingPhoto.Files[0].FileName;
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

                    _postgreDbContext.Update(findFranchise);
                }

                await _postgreDbContext.SaveChangesAsync();

                var result = new CreateUpdateFranchiseOutput
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
                    UrlsDetails = urlsDetails.ToArray(),
                    FileLogoUrl = "../../../assets/images/" + franchiseInput.FileLogo.Files[0].FileName,
                    NameFinIndicators = franchiseInput.FinIndicators,
                    FinModelFileUrl = "/docs" + franchiseInput.FinModelFile.Files[0].FileName,
                    FranchisePhotoUrl = "../../../assets/images/" + franchiseInput.FranchisePhoto.Files[0].FileName,
                    PresentFileUrl = "/docs" + franchiseInput.PresentFile.Files[0].FileName,
                    TrainingPhotoUrl = "../../../assets/images/" + franchiseInput.TrainingPhoto.Files[0].FileName,
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
        private async Task<FranchiseEntity> FindFranchiseByTitle(string title)
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
                        Reviews = f.Reviews
                    })
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return null;
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
        /// Метод получит франшизу для просмотра или изменения.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <param name="mode">Режим (Edit или View).</param>
        /// <returns>Данные франшизы.</returns>
        public async Task<FranchiseEntity> GetFranchiseAsync(long franchiseId, string mode)
        {
            try
            {
                var result = await (from f in _postgreDbContext.Franchises
                                    where f.FranchiseId == franchiseId
                                    select new FranchiseEntity
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
                                        Reviews = f.Reviews
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
        /// <param name="form">Файлы.</param>
        /// <returns>Список названий файлов.</returns>
        public async Task<IEnumerable<string>> AddTempFilesBeforeCreateFranchiseAsync(IFormCollection form, string user)
        {
            try
            {
                var results = new List<string>();

                // Отправит файлы на FTP-сервер.
                if (form.Files.Count > 0)
                {
                    await _ftpService.UploadFilesFtpAsync(form);
                }

                // Найдет такого пользователя.
                var findUser = await _userService.FindUserByCodeAsync(user);

                if (findUser)
                {
                    // Запишет во временную таблицу какие названия файлов, которые добавили но еще не сохранили.
                    foreach (var item in form.Files)
                    {
                        await _postgreDbContext.TempFranchises.AddAsync(new TempFranchiseEntity
                        {
                            FileName = item.FileName
                        });

                        results.Add(item.FileName);
                    }
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
    }
}
