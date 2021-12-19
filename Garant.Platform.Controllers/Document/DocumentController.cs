using System.Threading.Tasks;
using Garant.Platform.Abstractions.Document;
using Garant.Platform.Models.Document.Input;
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
        private readonly IDocumentRepository _documentRepository;

        public DocumentController(IDocumentService documentService, IDocumentRepository documentRepository)
        {
            _documentService = documentService;
            _documentRepository = documentRepository;
        }

        /// <summary>
        /// Метод прикрепит документ продавца к сделке.
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

        /// <summary>
        /// Метод отправит документ основного договора продавца на согласование покупателю.
        /// </summary>
        /// <param name="documentInput">Входная модель.</param>
        /// <returns>Флаг успеха.</returns>
        [HttpPost]
        [Route("send-vendor-document-deal")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> SendMainDealDocumentVendorAsync([FromBody] DocumentInput documentInput)
        {
            var result = await _documentService.SendDocumentVendorAsync(documentInput.DocumentItemId, documentInput.IsDealDocument, documentInput.DocumentType);

            return Ok(result);
        }

        /// <summary>
        /// Метод получит название документа, который отправлен на согласование покупателю.
        /// </summary>
        /// <returns>Название документа.</returns>
        [HttpPost]
        [Route("get-attachment-document-deal-name")]
        [ProducesResponseType(200, Type = typeof(DocumentOutput))]
        public async Task<IActionResult> GetAttachmentNameDocumentVendorDealAsync([FromBody] DocumentInput documentInput)
        {
            var result = await _documentRepository.GetAttachmentNameDocumentVendorDealAsync(documentInput.DocumentItemId);

            return Ok(result);
        }

        /// <summary>
        /// Метод проверит, подтвердил ли покупатель договор продавца.
        /// </summary>
        /// <returns>Флаг проверки.</returns>
        [HttpPost]
        [Route("check-approve-document-vendor")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> CheckApproveDocumentVendorAsync([FromBody] DocumentInput documentInput)
        {
            var result = await _documentRepository.CheckApproveDocumentVendorAsync(documentInput.DocumentItemId, GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод подтвердит договор продавца.
        /// </summary>
        /// <param name="documentInput">Входная модель.</param>
        /// <returns>Флаг подтверждения.</returns>
        [HttpPost]
        [Route("approve-document-vendor")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> ApproveDocumentVendorAsync([FromBody] DocumentInput documentInput)
        {
            var result = await _documentRepository.ApproveDocumentVendorAsync(documentInput.DocumentItemId);

            return Ok(result);
        }

        /// <summary>
        /// Метод отправит документ основного договора продавца на согласование покупателю.
        /// </summary>
        /// <param name="documentInput">Входная модель.</param>
        /// <returns>Флаг успеха.</returns>
        [HttpPost]
        [Route("send-customer-document-deal")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> SendMainDealDocumentCustomerAsync([FromBody] DocumentInput documentInput)
        {
            var result = await _documentService.SendMainDealDocumentCustomerAsync(documentInput.DocumentItemId, documentInput.IsDealDocument, documentInput.DocumentType);

            return Ok(result);
        }

        /// <summary>
        /// Метод прикрепит документ к сделке.
        /// </summary>
        /// <param name="files">Файлы сделки.</param>
        /// <param name="documentData">Входной модель.</param>
        /// <returns>Данные файлов.</returns>
        [HttpPost]
        [Route("attachment-customer-document-deal")]
        [ProducesResponseType(200, Type = typeof(DocumentOutput))]
        public async Task<IActionResult> AttachmentCustomerDocumentDealAsync([FromForm] IFormCollection files, [FromForm] string documentData)
        {
            var result = await _documentService.AttachmentCustomerDocumentDealAsync(files, documentData, GetUserName());

            return Ok(result);
        }
    }
}
