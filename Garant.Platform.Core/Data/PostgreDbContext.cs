using Garant.Platform.Models.Entities.Actions;
using Garant.Platform.Models.Entities.Add;
using Garant.Platform.Models.Entities.Blog;
using Garant.Platform.Models.Entities.Business;
using Garant.Platform.Models.Entities.Chat;
using Garant.Platform.Models.Entities.Commerce;
using Garant.Platform.Models.Entities.Configurator;
using Garant.Platform.Models.Entities.Control;
using Garant.Platform.Models.Entities.Document;
using Garant.Platform.Models.Entities.Footer;
using Garant.Platform.Models.Entities.Franchise;
using Garant.Platform.Models.Entities.Header;
using Garant.Platform.Models.Entities.LastBuy;
using Garant.Platform.Models.Entities.Logger;
using Garant.Platform.Models.Entities.News;
using Garant.Platform.Models.Entities.Suggestion;
using Garant.Platform.Models.Entities.Transition;
using Garant.Platform.Models.Entities.User;
using Garant.Platform.Models.Notification;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Core.Data
{
    public class PostgreDbContext : DbContext
    {
        private readonly DbContextOptions<PostgreDbContext> _options;

        public PostgreDbContext(DbContextOptions<PostgreDbContext> options) : base(options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsViewsEntity>()
                        .HasKey(c => new { c.NewsId, c.UserId });
        }

        public DbSet<BaseUserEntity> BaseUsers { get; set; }

        /// <summary>
        /// Таблица пользователей.
        /// </summary>
        public DbSet<UserEntity> Users { get; set; }

        /// <summary>
        /// Таблица логов Logs.Logs.
        /// </summary>
        public DbSet<LoggerEntity> Logs { get; set; }

        /// <summary>
        /// Таблица хидера.
        /// </summary>
        public DbSet<HeaderEntity> Headers { get; set; }

        /// <summary>
        /// Таблица футера.
        /// </summary>
        public DbSet<FooterEntity> Footers { get; set; }

        /// <summary>
        /// Таблица информации пользователя Info.UsersInformation.
        /// </summary>
        public DbSet<UserInformationEntity> UsersInformation { get; set; }

        /// <summary>
        /// Таблица новостей Info.News.
        /// </summary>
        public DbSet<NewsEntity> News { get; set; }

        /// <summary>
        /// Класс сопоставляется с таблицей Info.NewsViews.
        /// </summary>
        public DbSet<NewsViewsEntity> NewsViews { get; set; }

        /// <summary>
        /// Таблица категорий Business.BusinessCategories.
        /// </summary>
        public DbSet<BusinessCategoryEntity> BusinessCategories { get; set; }

        /// <summary>
        /// Таблица последних покупок Commerce.LastBuy.
        /// </summary>
        public DbSet<LastBuyEntity> LastBuys { get; set; }

        /// <summary>
        /// Таблица событий на главной странице dbo.MainPageActions.
        /// </summary>
        public DbSet<MainPageActionEntity> MainPageActions { get; set; }

        /// <summary>
        /// Таблица предложений Info.Suggestions.
        /// </summary>
        public DbSet<SuggestionEntity> Suggestions { get; set; }

        /// <summary>
        /// Таблица популярных франшиз Franchises.PopularFranchises.
        /// </summary>
        public DbSet<PopularFranchiseEntity> PopularFranchises { get; set; }

        /// <summary>
        /// Таблица популярных франшиз Franchises.Franchises.
        /// </summary>
        public DbSet<FranchiseEntity> Franchises { get; set; }

        /// <summary>
        /// Таблица объявлений Info.Ads.
        /// </summary>
        public DbSet<AdEntity> Ads { get; set; }

        /// <summary>
        /// Таблица блогов Info.Blogs.
        /// </summary>
        public DbSet<BlogEntity> Blogs { get; set; }

        /// <summary>
        /// Таблица статей info.Articles
        /// </summary>
        public DbSet<ArticleEntity> Articles { get; set; }

        /// <summary>
        /// Таблица блогов Info.BlogThemes.
        /// </summary>
        public DbSet<BlogThemeEntity> BlogThemes { get; set; }

        /// <summary>
        /// Таблица Franchises.FranchiseCities.
        /// </summary>
        public DbSet<FranchiseCityEntity> FranchiseCities { get; set; }

        /// <summary>
        /// Таблица Franchises.ViewBusiness.
        /// </summary>
        public DbSet<ViewBusinessEntity> ViewBusiness { get; set; }

        /// <summary>
        /// Таблица Franchises.FranchiseCategories.
        /// </summary>
        public DbSet<FranchiseCategoryEntity> FranchiseCategories { get; set; }

        /// <summary>
        /// Таблица dbo.Breadcrumbs.
        /// </summary>
        public DbSet<BreadcrumbEntity> Breadcrumbs { get; set; }

        /// <summary>
        /// Таблица Franchises.TempFranchises.
        /// </summary>
        public DbSet<TempFranchiseEntity> TempFranchises { get; set; }

        /// <summary>
        /// Таблица dbo.Transitions.
        /// </summary>
        public DbSet<TransitionEntity> Transitions { get; set; }

        /// <summary>
        /// Таблица Business.Businesses.
        /// </summary>
        public DbSet<BusinessEntity> Businesses { get; set; }

        /// <summary>
        /// Таблица Business.TempBusinesses.
        /// </summary>
        public DbSet<TempBusinessEntity> TempBusinesses { get; set; }

        /// <summary>
        /// Таблица Franchises.FranchiseSubCategories.
        /// </summary>
        public DbSet<FranchiseSubCategoryEntity> FranchiseSubCategories { get; set; }

        /// <summary>
        /// Таблица Business.BusinessSubCategories
        /// </summary>
        public DbSet<BusinessSubCategoryEntity> BusinessSubCategories { get; set; }

        /// <summary>
        /// Таблица Business.BusinessCities
        /// </summary>
        public DbSet<BusinessCitiesEntity> BusinessCities { get; set; }

        /// <summary>
        /// Таблица dbo.ProfileNavigations.
        /// </summary>
        public DbSet<ProfileNavigationEntity> ProfileNavigations { get; set; }

        /// <summary>
        /// Таблица Communications.MainInfoDialogs.
        /// </summary>
        public DbSet<MainInfoDialogEntity> MainInfoDialogs { get; set; }

        /// <summary>
        /// Таблица Communications.DialogMessages.
        /// </summary>
        public DbSet<DialogMessageEntity> DialogMessages { get; set; }

        /// <summary>
        /// Таблица Communications.DialogMembers
        /// </summary>
        public DbSet<DialogMemberEntity> DialogMembers { get; set; }

        /// <summary>
        /// Таблица Franchises.RequestsFranchises.
        /// </summary>
        public DbSet<RequestFranchiseEntity> RequestsFranchises { get; set; }

        /// <summary>
        /// Таблица Business.RequestsBusinesses.
        /// </summary>
        public DbSet<RequestBusinessEntity> RequestsBusinesses { get; set; }

        /// <summary>
        /// Таблица Commerce.Orders.
        /// </summary>
        public DbSet<OrderEntity> Orders { get; set; }

        /// <summary>
        /// Таблица Logs.Transactions.
        /// </summary>
        public DbSet<TransactionEntity> Transactions { get; set; }

        /// <summary>
        /// Таблица ссылок возвратов dbo.ReturnUrls.
        /// </summary>
        public DbSet<ReturnUrl> ReturnUrls { get; set; }

        /// <summary>
        /// Таблица Commerce.Deals.
        /// </summary>
        public DbSet<DealEntity> Deals { get; set; }

        /// <summary>
        /// Таблица Commerce.DealIterations
        /// </summary>
        public DbSet<DealIterationEntity> DealIterations { get; set; }

        /// <summary>
        /// Таблица Documents.Documents.
        /// </summary>
        public DbSet<DocumentEntity> Documents { get; set; }

        /// <summary>
        /// Таблица dbo.Controls.
        /// </summary>
        public DbSet<ControlEntity> Controls { get; set; }

        /// <summary>
        /// Таблица Commerce.Payments.
        /// </summary>
        public DbSet<PaymentEntity> Payments { get; set; }

        /// <summary>
        /// Таблица dbo.Employees.
        /// </summary>
        public DbSet<EmployeeEntity> Employees { get; set; }

        /// <summary>
        /// Таблица  Configurator.ConfiguratorMenuItems.
        /// </summary>
        public DbSet<ConfiguratorMenuEntity> ConfiguratorMenuItems { get; set; }

        /// <summary>
        /// Таблица Configurator.BlogActions.
        /// </summary>
        public DbSet<BlogActionEntity> BlogActions { get; set; }

        /// <summary>
        /// Таблица Info.ArticleThemes.
        /// </summary>
        public DbSet<ArticleThemeEntity> ArticleThemes { get; set; }

        /// <summary>
        /// Таблица Communications.Notification. 
        /// </summary>
        public DbSet<NotificationEntity> Notifications { get; set; }

    }
}
