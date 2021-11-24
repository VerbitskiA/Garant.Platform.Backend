using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Messaging.Abstraction.Chat;
using Garant.Platform.Messaging.Enums;
using Garant.Platform.Messaging.Exceptions;
using Garant.Platform.Messaging.Models.Chat.Output;

namespace Garant.Platform.Messaging.Service.Chat
{
    /// <summary>
    /// Сервис чата.
    /// </summary>
    public sealed class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IUserRepository _userRepository;
        private readonly IFranchiseRepository _franchiseRepository;
        private readonly IBusinessRepository _businessRepository;

        public ChatService(IChatRepository chatRepository, PostgreDbContext postgreDbContext, IUserRepository userRepository, IFranchiseRepository franchiseRepository, IBusinessRepository businessRepository)
        {
            _chatRepository = chatRepository;
            _postgreDbContext = postgreDbContext;
            _userRepository = userRepository;
            _franchiseRepository = franchiseRepository;
            _businessRepository = businessRepository;
        }

        /// <summary>
        /// Метод отправит сообщение.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <param name="dialogId">Id диалога.</param>
        /// <returns>Список сообщений.</returns>
        public async Task<GetResultMessageOutput> SendMessageAsync(string message, string account, long dialogId)
        {
            try
            {
                // Если сообщения не передано, то ничего не делать.
                if (string.IsNullOrEmpty(message))
                {
                    return null;
                }

                var userId = await _userRepository.FindUserIdUniverseAsync(account);

                // Проверит существование диалога.
                var checkDialog = await _chatRepository.CheckDialogAsync(dialogId);

                if (!checkDialog)
                {
                    throw new NotFoundDialogIdException(dialogId);
                }

                // Запишет сообщение в БД.
                await _chatRepository.SaveMessageAsync(message, dialogId, DateTime.Now, userId, true);

                // Получит список сообщений диалога.
                var messages = await _chatRepository.GetMessagesAsync(dialogId);

                var result = new GetResultMessageOutput();

                foreach (var item in messages)
                {
                    // Проставит флаг принадлежности сообщения.
                    item.IsMyMessage = item.UserId.Equals(userId);

                    // Затирает Id пользователя, чтобы фронт не видел.
                    item.UserId = null;

                    result.Messages.Add(item);
                }

                result.DialogState = DialogStateEnum.Open.ToString();

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
        /// Метод получит диалог или создаст новый.
        /// </summary>
        /// <param name="dialogId">Id диалога.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <param name="ownerId">Id владельца/представителя. Пользователя, который владеет бизнесом или франшизей.</param>
        /// <param name="typeItem">Тип предмета.</param>
        /// <returns>Список сообщений.</returns>
        public async Task<GetResultMessageOutput> GetDialogAsync(long? dialogId, string account, string ownerId, string typeItem)
        {
            try
            {
                var messagesList = new GetResultMessageOutput();

                // Найдет Id текущего пользователя.
                var userId = await _userRepository.FindUserIdUniverseAsync(account);

                // Если dialogId не передан, попробует найти диалог с таким пользователем иначе.
                //if (dialogId == null)
                //{

                //}

                // Ищет Id диалога с текущим пользователем и с владельцем/представителем, на чат с которым нажали. Затем сравнит их DialogId, если он совпадает, значит текущий пользователь общается с владельцем/представителем.
                if (!string.IsNullOrEmpty(ownerId))
                {
                    // Выберет id диалога текущего пользователя.
                    var currentDialogId = await _chatRepository.FindDialogIdAsync(userId);

                    // Выберет id диалога владельца/представителя.
                    var ownerDialogId = await _chatRepository.FindDialogIdAsync(ownerId);

                    // Сравнит DialogId текущего пользователя с владельцем/представителем. Если они равны, значит текущий пользователь общается с владельцем/представителем и возьмет DialogId этого чата.
                    if (currentDialogId != ownerDialogId || currentDialogId <= 0 && ownerDialogId <= 0)
                    {
                        // Создаст новый диалог.
                        await _chatRepository.CreateDialogAsync(string.Empty, DateTime.Now);

                        var lastDialogId = await _chatRepository.GetLastDialogIdAsync();

                        // Добавит участников нового диалога.
                        await _chatRepository.AddDialogMembersAsync(userId, ownerId, lastDialogId);

                        messagesList.DialogState = DialogStateEnum.Open.ToString();

                        return messagesList;
                    }

                    dialogId = ownerDialogId;
                }

                // Проверит существование диалога.
                var checkDialog = await _chatRepository.CheckDialogAsync(Convert.ToInt64(dialogId));

                if (!checkDialog)
                {
                    throw new NotFoundDialogIdException(dialogId);
                }

                // Получит список сообщений.
                var messages = await _chatRepository.GetMessagesAsync(Convert.ToInt64(dialogId));

                // Если у диалога нет сообщений, значит вернуть пустой диалог, который будет открыт.
                if (!messages.Any())
                {
                    messagesList.DialogState = DialogStateEnum.Empty.ToString();

                    return messagesList;
                }

                foreach (var message in messages)
                {
                    // Проставит флаг принадлежности сообщения.
                    message.IsMyMessage = message.UserId.Equals(userId);

                    // Затирает Id пользователя, чтобы фронт его не видел.
                    message.UserId = null;

                    messagesList.Messages.Add(message);
                }

                messagesList.DialogState = DialogStateEnum.Open.ToString();

                // Найдет Id участников диалога по DialogId.
                var membersIds = await _chatRepository.GetDialogMembersAsync(Convert.ToInt64(dialogId));

                if (membersIds == null)
                {
                    throw new NotDialogMembersException(dialogId);
                }

                // Найдет Id владельца/представления.
                var id = membersIds.FirstOrDefault(m => !m.Equals(userId));
                var user = await _userRepository.GetUserProfileInfoByIdAsync(id);

                // Запишет имя и фамилию пользователя, диалог с которым открыт.
                if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
                {
                    messagesList.FirstName = user.FirstName;
                    messagesList.LastName = user.LastName;
                    messagesList.FullName = messagesList.FirstName + " " + messagesList.LastName;
                }

                // Запишет заголовок предмета обсуждения (т.е заголовок франшизы или бизнеса).
                // Найдет заголовок франшизы.
                if (typeItem.Equals("Franchise"))
                {
                    messagesList.ChatItemName = await _franchiseRepository.GetFranchiseTitleAsync(ownerId);
                }

                // Найдет заголовок бизнеса.
                else if (typeItem.Equals("Business"))
                {
                    messagesList.ChatItemName = await _businessRepository.GetBusinessTitleAsync(ownerId);
                }

                // Запишет дату начала диалога.
                var startDate = await _chatRepository.GetDialogStartDate(Convert.ToInt64(dialogId));
                messagesList.DateStartDialog = startDate;

                return messagesList;
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
        /// Метод получит список диалогов пользователя.
        /// </summary>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <returns>Список диалогов.</returns>
        public async Task<IEnumerable<DialogOutput>> GetDialogsAsync(string account)
        {
            try
            {
                var result = new List<DialogOutput>();

                // Найдет Id текущего пользователя, для которого нужно получить список диалогов.
                var userId = await _userRepository.FindUserIdUniverseAsync(account);

                // Получит список диалогов.
                var dialogs = await _chatRepository.GetDialogsAsync(userId);

                // Если диалоги не найдены, то вернет пустой массив.
                if (!dialogs.Any())
                {
                    return Array.Empty<DialogOutput>();
                }

                foreach (var dialog in dialogs)
                {
                    // Подтянет последнее сообщение диалога для отображения в свернутом виде взяв первые 40 символов и далее ставит ...
                    var lastMessage = await _chatRepository.GetLastMessageAsync(dialog.DialogId);
                    dialog.LastMessage = lastMessage.Length > 40
                        ? string.Concat(lastMessage.Substring(0, 40), "...")
                        : lastMessage;

                    // Найдет Id участников диалога по DialogId.
                    var membersIds = await _chatRepository.GetDialogMembersAsync(Convert.ToInt64(dialog.DialogId));

                    if (membersIds == null)
                    {
                        throw new NotDialogMembersException(dialog.DialogId);
                    }

                    // Запишет имя и фамилию участника диалога, с которым идет общение.
                    var otherUserId = membersIds.FirstOrDefault(m => !m.Equals(userId));
                    var otherData = await _userRepository.GetUserProfileInfoByIdAsync(otherUserId);
                    dialog.FullName = otherData.FirstName + " " + otherData.LastName;

                    // Если дата диалога совпадает с сегодняшней, то заполнит часы и минуты, иначе оставит их null.
                    if (DateTime.Now.ToString("d").Equals(Convert.ToDateTime(dialog.Created).ToString("d")))
                    {
                        // Запишет только часы и минуты.
                        dialog.CalcTime = Convert.ToDateTime(dialog.Created).ToString("t");
                    }

                    // Если дата диалога не совпадает с сегодняшней.
                    else
                    {
                        // Запишет только дату.
                        dialog.CalcShortDate = Convert.ToDateTime(dialog.Created).ToString("d");
                    }

                    // Форматирует дату убрав секунды.
                    dialog.Created = Convert.ToDateTime(dialog.Created).ToString("g");
                    result.Add(dialog);
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
    }
}
