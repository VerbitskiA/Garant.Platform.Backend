using AutoMapper;
using Garant.Platform.Models.Blog.Output;
using Garant.Platform.Models.Configurator.Output;
using Garant.Platform.Models.Entities.Blog;
using Garant.Platform.Models.Entities.News;
using Garant.Platform.Models.Entities.User;
using Garant.Platform.Models.User.Output;

namespace Garant.Platform.Core.Mapper
{
    /// <summary>
    /// Класс конфигурации маппера.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmployeeEntity, CreateEmployeeOutput>();
            CreateMap<EmployeeEntity, ConfiguratorLoginOutput>();
            CreateMap<BlogEntity, BlogOutput>();
            CreateMap<ArticleEntity, ArticleOutput>();
            CreateMap<NewsEntity, NewsOutput>();
        }
    }
}