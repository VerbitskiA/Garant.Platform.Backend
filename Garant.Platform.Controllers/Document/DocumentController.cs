using System.Threading.Tasks;
using Garant.Platform.Abstractions.Document;
using Garant.Platform.Models.Document.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Document
{
    /// <summary>
    /// Контроллер работы со всеми документами.
    /// </summary>
    [ApiController, Route("document")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DocumentController : BaseController
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        /// <summary>
        /// Метод прикрепит документ к сделке.
        /// </summary>
        /// <param name="files">Файлы сделки.</param>
        /// <param name="documentData">Входной модель.</param>
        /// <returns>Данные файлов.</returns>
        [HttpPost]
        [Route("attachment-vendor-document-deal")]
        [ProducesResponseType(200, Type = typeof(DocumentOutput))]
        public async Task<IActionResult> AttachmentVendorDocumentDealAsync([FromForm] IFormCollection files, [FromForm] string documentData)
        {
            var result = await _documentService.AttachmentVendorDocumentDealAsync(files, documentData, GetUserName());

            return Ok(result);
        }

        //public async Task<IActionResult> SendDocumentVendorAsync()
        //{
        //    return Ok();
        //}
    }
}
