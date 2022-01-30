using AutoMapper;
using Garant.Platform.Models.Configurator.Output;
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
        }
    }
}