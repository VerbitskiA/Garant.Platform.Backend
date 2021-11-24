using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Messaging.Abstraction.Chat;
using Garant.Platform.Messaging.Models.Chat.Input;
using Garant.Platform.Messaging.Models.Chat.Output;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatController : BaseController
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        /// <summary>
        /// TODO: Пока не используется.
        /// Метод отправит сообщение в RabbitMQ.
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //[Route("send-queue")]
        //public async Task<IActionResult> SendMessagesRabbitMqAsync([FromBody] SendRabbitMqMessageInput sendQueueMessageInput)
        //{
        //    await _rabbitMqService.SendMessagesRabbitMqAsync(sendQueueMessageInput.Message);

        //    return Ok();
        //}

        /// <summary>
        /// TODO: Пока не используется.
        /// Метод получит сообщения из RabbitMQ.
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //[Route("receive-queue")]
        //public async Task<IActionResult> ReceiveMessagesRabbitMqAsync()
        //{
        //    var result = await _rabbitMqService.ReceiveMessagesRabbitMqAsync();

        //    return Ok(result);
        //}

        /// <summary>
        /// Метод отправит сообщение.
        /// </summary>
        /// <param name="chatInput">Входная модель.</param>
        /// <returns>Список сообщений.</returns>
        [HttpPost]
        [Route("send-message")]
        [ProducesResponseType(200, Type = typeof(GetResultMessageOutput))]
        public async Task<IActionResult> SendAsync([FromBody] ChatInput chatInput)
        {
            var messages = await _chatService.SendMessageAsync(chatInput.Message, GetUserName(), chatInput.DialogId);

            return Ok(messages);
        }

        /// <summary>
        /// Метод получит диалог, либо создаcт новый.
        /// </summary>
        /// <param name="dialogInput">Входная модель.</param>
        /// <returns>Список сообщений.</returns>
        [HttpPost]
        [Route("get-dialog")]
        [ProducesResponseType(200, Type = typeof(GetResultMessageOutput))]
        public async Task<IActionResult> GetDialogAsync([FromBody] DialogInput dialogInput)
        {
            var messages = await _chatService.GetDialogAsync(dialogInput.DialogId, GetUserName(), dialogInput.OwnerId, dialogInput.TypeItem);
            
            return Ok(messages);
        }

        /// <summary>
        /// Метод получит список диалогов с текущим пользователем.
        /// </summary>
        /// <returns>Список диалогов.</returns>
        [HttpPost]
        [Route("dialogs")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DialogOutput>))]
        public async Task<IActionResult> GetDialogsAsync()
        {
            var result = await _chatService.GetDialogsAsync(GetUserName());

            return Ok(result);
        }
    }
}
