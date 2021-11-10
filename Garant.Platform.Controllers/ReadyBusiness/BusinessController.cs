using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Models.Business.Input;
using Garant.Platform.Models.Business.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.ReadyBusiness
{
    /// <summary>
    /// Контроллер готового бизнеса.
    /// </summary>
    [ApiController]
    [Route("business")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BusinessController : BaseController
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        /// <summary>
        /// Метод создаст или обновит карточку бизнеса.
        /// </summary>
        /// <param name="businessFilesInput">Файлы карточки.</param>
        /// <param name="businessDataInput">Входная модель.</param>
        /// <returns>Данные карточки бизнеса.</returns>
        [HttpPost]
        [Route("create-update-business")]
        [ProducesResponseType(200, Type = typeof(CreateUpdateBusinessOutput))]
        public async Task<IActionResult> CreateUpdateBusinessAsync([FromForm] IFormCollection businessFilesInput, [FromForm] string businessDataInput)
        {
            var result = await _businessService.CreateUpdateBusinessAsync(businessFilesInput, businessDataInput, GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод отправит файл в папку и запишет в БД.
        /// </summary>
        /// <param name="files">Файлы.</param>
        [HttpPost]
        [Route("temp-file")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<string>))]
        public async Task<IActionResult> AddTempFilesBeforeCreateBusinessAsync([FromForm] IFormCollection files)
        {
            var result = await _businessService.AddTempFilesBeforeCreateBusinessAsync(files, GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод получит бизнес для просмотра или изменения.
        /// </summary>
        /// <param name="businessInput">Входная модель.</param>
        /// <returns></returns>
        [HttpPost, Route("get-business")]
        [ProducesResponseType(200, Type = typeof(BusinessOutput))]
        public async Task<IActionResult> GetFranchiseAsync([FromBody] BusinessInput businessInput)
        {
            var result = await _businessService.GetBusinessAsync(businessInput.BusinessId, businessInput.Mode);

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий.</returns>
        [HttpPost]
        [Route("category-list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetBusinessCategoryOutput>))]
        public async Task<IActionResult> GetBusinessCategoriesAsync()
        {
            var result = await _businessService.GetBusinessCategoriesAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список подкатегорий бизнеса.
        /// </summary>
        /// <returns>Список подкатегорий.</returns>
        [HttpPost]
        [Route("subcategory-list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BusinessSubCategoryOutput>))]
        public async Task<IActionResult> GetBusinessSubCategoryListAsync()
        {
            var result = await _businessService.GetSubBusinessCategoryListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список городов.
        /// </summary>
        /// <returns>Список городов.</returns>
        [HttpPost]
        [Route("cities-list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BusinessCitiesOutput>))]
        public async Task<IActionResult> GetCitiesListAsync()
        {
            var result = await _businessService.GetCitiesListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список популярного бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        [HttpPost]
        [Route("popular-business")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PopularBusinessOutput>))]
        public async Task<IActionResult> GetPopularBusinessAsync()
        {
            var result = await _businessService.GetPopularBusinessAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        [HttpPost]
        [Route("catalog-business")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PopularBusinessOutput>))]
        public async Task<IActionResult> GetBusinessListAsync()
        {
            var result = await _businessService.GetBusinessListAsync();

            return Ok(result);
        }
    }
}
