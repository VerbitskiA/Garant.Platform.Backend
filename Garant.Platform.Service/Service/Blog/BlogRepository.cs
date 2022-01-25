using Garant.Platform.Abstractions.Blog;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Blog.Input;
using Garant.Platform.Models.Blog.Output;
using Garant.Platform.Models.Entities.Blog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garant.Platform.Services.Service.Blog
{
    /// <summary>
    /// Репозиторий блогов.
    /// </summary>
    public class BlogRepository : IBlogRepository
    {
        private readonly PostgreDbContext _postgreDbContext;

        public BlogRepository(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }
        
        /// <summary>
        /// Метод создаст новый блог.
        /// </summary>
        /// <param name="blogInput">Входная модель блога.</param>
        /// <returns>Созданный блог.</returns>
        public Task<BlogOutput> CreateBlog(CreateBlogInput blogInput)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Метод получит блог по названию.
        /// </summary>
        /// <param name="title">Название блога.</param>
        /// <returns>Данные блога.</returns>
        public async Task<BlogOutput> GetBlogAsync(string title)
        {
            try
            {
                var result = await _postgreDbContext.Blogs
                    .Where(b => b.Title.Equals(title))
                    .Select(b => new BlogOutput
                    {
                        Title = b.Title,
                        Url = b.Url,
                        IsPaid = b.IsPaid
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

        /// <summary>
        /// Метод вернёт список блогов.
        /// </summary>
        /// <returns>Список блогов.</returns>
        public async Task<IEnumerable<BlogOutput>> GetBlogsListAsync()
        {
            try
            {
                var result = await (from b in _postgreDbContext.Blogs
                                    select new BlogOutput
                                    {
                                        Title = b.Title,
                                        Url = b.Url,
                                        IsPaid = b.IsPaid
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
        /// Метод вернёт список тем блогов.
        /// </summary>        
        /// <returns>Список тем блогов.</returns>
        public async Task<IEnumerable<BlogThemesOutput>> GetBlogThemesAsync()
        {
            try
            {
                var result = await (from b in _postgreDbContext.BlogThemes
                                    select new BlogThemesOutput
                                    {
                                        Title = b.Title
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
        /// Метод обновит существующий блог
        /// </summary>
        /// <param name="blogInput">Входная модель блога.</param>
        /// <returns>Обновлённый блог.</returns>
        public Task<BlogOutput> UpdateBlog(UpdateBlogInput blogInput)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Метод обновит существующий или создаст новый блог.
        /// </summary>
        /// <param name="blogInput">Входная модель.</param>
        /// <returns>Данные блога.</returns>
        //public async Task<CreateBlogInput> UpdateCreateBlog(CreateBlogInput blogInput)
        //{
        //    //TODO^ втф
        //    try
        //    {
        //        CreateBlogInput result = null;

        //        if (blogInput != null)
        //        {

        //            // Найдет блог с таким названием.
        //            var findBlog = await GetBlogAsync(blogInput.Title);

        //            // Создаст новый блог.
        //            if (blogInput.IsNew && findBlog == null)
        //            {
        //                await _postgreDbContext.Blogs.AddAsync(new BlogEntity
        //                {
        //                    Title = blogInput.Title,
        //                    Url = blogInput.Url,
        //                    IsPaid = false
        //                });
        //            }

        //            // Обновит блог.
        //            else if (!blogInput.IsNew && blogInput != null)
        //            {
        //                findBlog.Title = blogInput.Title;
        //                findBlog.Url = blogInput.Url;


        //                _postgreDbContext.Update(findBlog);
        //            }

        //            await _postgreDbContext.SaveChangesAsync();

        //            result = new CreateUpdateBusinessOutput
        //            {
        //                ActivityDetail = businessInput.ActivityDetail,
        //                ActivityPhotoName = "../../../assets/images/" + files.Where(c => c.Name.Equals("filesTextBusiness")).ToArray()[0].FileName,
        //                Address = businessInput.Address,
        //                Assets = businessInput.Assets,
        //                AssetsPhotoName = "../../../assets/images/" + files.Where(c => c.Name.Equals("filesAssets")).ToArray()[0].FileName,
        //                BusinessAge = businessInput.BusinessAge,
        //                BusinessId = lastBusinessId,
        //                BusinessName = businessInput.BusinessName,
        //                EmployeeCountYear = businessInput.EmployeeCountYear,
        //                Form = businessInput.Form,
        //                Status = businessInput.Status,
        //                Price = businessInput.Price,
        //                UrlsBusiness = urls,
        //                TurnPrice = businessInput.TurnPrice,
        //                ProfitPrice = businessInput.ProfitPrice,
        //                Payback = businessInput.Payback,
        //                Profitability = businessInput.Profitability,
        //                InvestPrice = businessInput.InvestPrice,
        //                Text = businessInput.Text,
        //                Share = businessInput.Share,
        //                Site = businessInput.Site,
        //                Peculiarity = businessInput.Peculiarity,
        //                NameFinModelFile = "../../../assets/images/" + files.Where(c => c.Name.Equals("finModelFile")).ToArray()[0].FileName,
        //                ReasonsSale = businessInput.ReasonsSale,
        //                ReasonsSalePhotoName = "../../../assets/images/" + files.Where(c => c.Name.Equals("filesReasonsSale")).ToArray()[0].FileName,
        //                UrlVideo = businessInput.UrlVideo,
        //                IsGarant = businessInput.IsGarant,
        //                DateCreate = DateTime.Now,
        //                TextDoPrice = "Стоимость:",
        //                BusinessCity = businessInput.BusinessCity
        //            };
        //        }

        //        return result;
        //    }

        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
        //        await logger.LogCritical();
        //        throw;
        //    }
        //}
    }
}
