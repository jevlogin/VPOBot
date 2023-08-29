using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WORLDGAMEDEVELOPMENT
{
    internal class AdminMessageHandler : MessageHandler
    {
        #region Fields

        private DatabaseService _databaseService;
        private Dictionary<long, string> _adminList;
        private Dictionary<long, UserVPO> _userList;

        #endregion


        #region ClassLifeCycles

        public AdminMessageHandler(TelegramBotClient botClient, DatabaseService databaseService,
                                    Dictionary<long, string> adminList, Dictionary<long, UserVPO> userList) : base(botClient)
        {
            _databaseService = databaseService;
            _adminList = adminList;
            _userList = userList;
        }



        #endregion


        #region MessageHandler

        public override async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync($"An error occurred during handling admin message: {exception}");

            if (exception is ApiRequestException apiException)
            {
                await Console.Out.WriteLineAsync($"API error occurred: {apiException.ErrorCode} - {apiException.Message}");
            }
            else
            {
                await Console.Out.WriteLineAsync("An unknown error occurred.");
            }
        }

        public override async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    if (update.Message is { } message)
                    {
                        switch (message.Type)
                        {
                            case MessageType.Unknown:
                                break;
                            case MessageType.Text:
                                if (message.Text is { } text)
                                {
                                    if (text.StartsWith('/'))
                                    {
                                        await HandleCommandAsync(message, cancellationToken);
                                    }
                                    else
                                    {
                                        await HandleTextAsync(message, cancellationToken);
                                    }
                                }
                                break;
                            case MessageType.Photo:
                                break;
                            case MessageType.Audio:
                                break;
                            case MessageType.Video:
                                break;
                            case MessageType.Voice:
                                break;
                            case MessageType.Document:
                                break;
                            case MessageType.Sticker:
                                break;
                            case MessageType.Location:
                                break;
                            case MessageType.Contact:
                                break;
                            case MessageType.Venue:
                                break;
                            case MessageType.Game:
                                break;
                            case MessageType.VideoNote:
                                break;
                            case MessageType.Invoice:
                                break;
                            case MessageType.SuccessfulPayment:
                                break;
                            case MessageType.WebsiteConnected:
                                break;
                            case MessageType.ChatMembersAdded:
                                break;
                            case MessageType.ChatMemberLeft:
                                break;
                            case MessageType.ChatTitleChanged:
                                break;
                            case MessageType.ChatPhotoChanged:
                                break;
                            case MessageType.MessagePinned:
                                break;
                            case MessageType.ChatPhotoDeleted:
                                break;
                            case MessageType.GroupCreated:
                                break;
                            case MessageType.SupergroupCreated:
                                break;
                            case MessageType.ChannelCreated:
                                break;
                            case MessageType.MigratedToSupergroup:
                                break;
                            case MessageType.MigratedFromGroup:
                                break;
                            case MessageType.Poll:
                                break;
                            case MessageType.Dice:
                                break;
                            case MessageType.MessageAutoDeleteTimerChanged:
                                break;
                            case MessageType.ProximityAlertTriggered:
                                break;
                            case MessageType.WebAppData:
                                break;
                            case MessageType.VideoChatScheduled:
                                break;
                            case MessageType.VideoChatStarted:
                                break;
                            case MessageType.VideoChatEnded:
                                break;
                            case MessageType.VideoChatParticipantsInvited:
                                break;
                            case MessageType.Animation:
                                break;
                            case MessageType.ForumTopicCreated:
                                break;
                            case MessageType.ForumTopicClosed:
                                break;
                            case MessageType.ForumTopicReopened:
                                break;
                            case MessageType.ForumTopicEdited:
                                break;
                            case MessageType.GeneralForumTopicHidden:
                                break;
                            case MessageType.GeneralForumTopicUnhidden:
                                break;
                            case MessageType.WriteAccessAllowed:
                                break;
                            case MessageType.UserShared:
                                break;
                            case MessageType.ChatShared:
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case UpdateType.CallbackQuery:

                    break;
            }
        }

        #endregion


        #region Methods

        private async Task HandleTextAsync(Message message, CancellationToken cancellationToken)
        {
            if (message.ReplyToMessage is not { } replyToMessage)
                return;

            if (replyToMessage.ForwardFrom is { } forwardFrom && forwardFrom.Id != _botClient.BotId)
            {
                var userId = forwardFrom.Id;
                var adminName = message.From.FirstName;

                switch (message.Type)
                {
                    case MessageType.Unknown:
                        break;
                    case MessageType.Text:
                        if (message.Text is { } text)
                        {
                            var msgText = $"<b>{adminName}</b>: <i>{text}</i>";

                            try
                            {
                                await _botClient.SendTextMessageAsync(userId, msgText,
                                                                        parseMode: ParseMode.Html, replyToMessageId: replyToMessage.MessageId,
                                                                        cancellationToken: cancellationToken);
                            }
                            catch (ApiRequestException ex)
                            {
                                await Console.Out.WriteLineAsync($"По не известной причине, блок метода - 'HandleTextAsync', не отработал.\n{ex.Message}");

                                await _botClient.SendTextMessageAsync(userId, msgText, parseMode: ParseMode.Html,
                                                                        cancellationToken: cancellationToken);
                                throw;
                            }
                        }
                        break;
                    case MessageType.Photo:
                        break;
                    case MessageType.Audio:
                        break;
                    case MessageType.Video:
                        break;
                    case MessageType.Voice:
                        break;
                    case MessageType.Document:
                        break;
                    case MessageType.Sticker:
                        break;
                    case MessageType.Location:
                        break;
                    case MessageType.Contact:
                        break;
                    case MessageType.Venue:
                        break;
                    case MessageType.Game:
                        break;
                    case MessageType.VideoNote:
                        break;
                    case MessageType.Invoice:
                        break;
                    case MessageType.SuccessfulPayment:
                        break;
                    case MessageType.WebsiteConnected:
                        break;
                    case MessageType.ChatMembersAdded:
                        break;
                    case MessageType.ChatMemberLeft:
                        break;
                    case MessageType.ChatTitleChanged:
                        break;
                    case MessageType.ChatPhotoChanged:
                        break;
                    case MessageType.MessagePinned:
                        break;
                    case MessageType.ChatPhotoDeleted:
                        break;
                    case MessageType.GroupCreated:
                        break;
                    case MessageType.SupergroupCreated:
                        break;
                    case MessageType.ChannelCreated:
                        break;
                    case MessageType.MigratedToSupergroup:
                        break;
                    case MessageType.MigratedFromGroup:
                        break;
                    case MessageType.Poll:
                        break;
                    case MessageType.Dice:
                        break;
                    case MessageType.MessageAutoDeleteTimerChanged:
                        break;
                    case MessageType.ProximityAlertTriggered:
                        break;
                    case MessageType.WebAppData:
                        break;
                    case MessageType.VideoChatScheduled:
                        break;
                    case MessageType.VideoChatStarted:
                        break;
                    case MessageType.VideoChatEnded:
                        break;
                    case MessageType.VideoChatParticipantsInvited:
                        break;
                    case MessageType.Animation:
                        break;
                    case MessageType.ForumTopicCreated:
                        break;
                    case MessageType.ForumTopicClosed:
                        break;
                    case MessageType.ForumTopicReopened:
                        break;
                    case MessageType.ForumTopicEdited:
                        break;
                    case MessageType.GeneralForumTopicHidden:
                        break;
                    case MessageType.GeneralForumTopicUnhidden:
                        break;
                    case MessageType.WriteAccessAllowed:
                        break;
                    case MessageType.UserShared:
                        break;
                    case MessageType.ChatShared:
                        break;
                    default:
                        break;
                }

                await _botClient.SendTextMessageAsync(message.Chat.Id, $"Пользователю {_userList[userId].FirstName}, был отправлен ответ.");
            }
            else
            {
                await _botClient.SendTextMessageAsync(message.Chat.Id,
                    $"Для отправки ответного сообщения, пожалуйста, ответьте на сообщение пользователя, на которое вы хотите ответить.", cancellationToken: cancellationToken);
            }
        }

        private async Task HandleCommandAsync(Message message, CancellationToken cancellationToken)
        {
            if (message.Text is not { } text) return;

            var command = text.Split(' ')[0].ToLower();
            var args = text.Split(' ').Skip(1).ToArray();

            switch (command)
            {
                case "/start":
                    await _botClient.SendTextMessageAsync(message.Chat.Id, "Привет Админ! Ты управляешь этим чатом.");
                    break;
                case "/help":
                    await _botClient.SendTextMessageAsync(
                        message.Chat.Id,
                         "Вы можете использовать следующие команды:\n" +
                        "/start - Начать общение\n" +
                        "/help - Помощь\n" +
                        "/status - Показать статус бота\n");
                    break;
                case "/status":
                    await _botClient.SendTextMessageAsync(message.Chat.Id, "Бот исправно работает и слушает все сообщения.");
                    break;
                case "/addadmin":
                    await HandleAddAdminCommandAsync(message, args, cancellationToken);
                    break;
                case "/admins":
                    await HandleAdminsCommandAsync(message, cancellationToken);
                    break;
                default:
                    await _botClient.SendTextMessageAsync(message.Chat.Id, "Простите, но я не понимаю данной команды.", cancellationToken: cancellationToken);
                    break;
            }
        }

        private async Task HandleAdminsCommandAsync(Message message, CancellationToken cancellationToken)
        {
            foreach (var admin in _adminList)
            {
                var text = $"ID = {admin.Key}. Имя пользователя: {admin.Value}";
                await _botClient.SendTextMessageAsync(message.Chat.Id, text, cancellationToken: cancellationToken);
            }
        }

        private async Task HandleAddAdminCommandAsync(Message message, string[] args, CancellationToken cancellationToken)
        {
            if (message.From?.Id is { } id)
            {
                if (IsAdmin(id))
                {
                    if (args.Length > 0)
                    {
                        long userId = long.Parse(args[0]);

                        if (_adminList.TryGetValue(userId, out var admin))
                        {
                            var msgInfo = $"Такой администратор уже есть";
                            await Console.Out.WriteLineAsync(msgInfo);
                            await _botClient.SendTextMessageAsync(message.Chat.Id, msgInfo, cancellationToken: cancellationToken);
                            return;
                        }

                        var newAdminName = args.Skip(1);
                        var userName = !string.IsNullOrEmpty(newAdminName.ToString()) ? newAdminName.ToString() : $"Admin_{userId}";

                        _adminList[userId] = userName!;

                        var userAdmin = new UserAdmin
                        {
                            UserId = userId,
                            UserName = userName!,
                        };

                        await _databaseService.AddAdminAsync(userAdmin);
                    }
                    else
                    {
                        await _botClient.SendTextMessageAsync(
                            message.Chat.Id,
                            "Пожалуйста введите корректные данные",
                            cancellationToken: cancellationToken);
                    }
                }
            }
        }

        public override bool CanHandle(long? userId, CancellationToken cancellationToken)
        {
            if (userId is { } id)
            {
                return IsAdmin(id);
            }
            return false;
        }


        private bool IsAdmin(long id)
        {
            return _adminList.ContainsKey(id);
        }

        #endregion

    }
}