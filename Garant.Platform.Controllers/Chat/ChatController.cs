using System.Threading.Tasks;
using Garant.Platform.Messaging.Abstraction.RabbitMq;
using Garant.Platform.Messaging.Model.Input;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Chat
{
    /// <summary>
    /// Контроллер работы с чатом и сообщениями.
    /// </summary>
    [ApiController]
    [Route("chat")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatController : BaseController
    {
        private readonly IRabbitMqService _rabbitMqService;

        public ChatController(IRabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        /// <summary>
        /// Метод отправит сообщение в RabbitMQ.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("send-queue")]
        public async Task<IActionResult> SendMessagesRabbitMqAsync([FromBody] SendRabbitMqMessageInput sendQueueMessageInput)
        {
            await _rabbitMqService.SendMessagesRabbitMqAsync(sendQueueMessageInput.Message);

            return Ok();
        }

        /// <summary>
        /// Метод получит сообщения из RabbitMQ.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("receive-queue")]
        public async Task<IActionResult> ReceiveMessagesRabbitMqAsync()
        {
            var result = await _rabbitMqService.ReceiveMessagesRabbitMqAsync();

            return Ok(result);
        }
    }
}
