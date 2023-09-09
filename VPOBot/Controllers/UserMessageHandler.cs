using Newtonsoft.Json.Linq;
using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WORLDGAMEDEVELOPMENT
{
    internal class UserMessageHandler : MessageHandler, IDisposable
    {
        #region Fields

        private static Random _random = new Random();
        private readonly DatabaseService _databaseService;
        private readonly Dictionary<long, string> _adminList;
        private readonly Dictionary<long, UserVPO> _userList;
        private Dictionary<long, ProgressUsers> _progressUsersList = new Dictionary<long, ProgressUsers>();

        #endregion


        #region ClassLifeCycles

        public UserMessageHandler(TelegramBotClient botClient, DatabaseService databaseService, Dictionary<long, string> adminList, Dictionary<long, UserVPO> userList) : base(botClient)
        {
            _databaseService = databaseService;
            _adminList = adminList;
            _userList = userList;

            if (_userList.Count > 0)
            {
                ConnectedToDatabaseAndSynchronizeProgress().Wait();

                foreach (var progress in _progressUsersList.Values)
                {
                    progress.ProgressUpdated += ProgressUsersUpdated;
                    if (!progress.IsTheNextStepSheduledInTime || !progress.IsTheNextDaysUpdateIsCompleted)
                    {
                        progress.UpdateState = UpdateState.UpdateDate;
                    }
                }
            }
        }

        ~UserMessageHandler()
        {
            Dispose();
        }

        public void Dispose()
        {
            foreach (var progresUser in _progressUsersList.Values)
            {
                progresUser.ProgressUpdated -= ProgressUsersUpdated;
            }
        }

        #endregion


        #region MessageHandler

        public async override Task HandlePollingErrorAsync(Exception exception, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync($"An error occurred during handling user message: {exception}");

            if (exception is ApiRequestException apiException)
            {
                await Console.Out.WriteLineAsync($"API error occurred: {apiException.ErrorCode} - {apiException.Message}");
            }
            else
            {
                await Console.Out.WriteLineAsync("An unknown error occurred.");
            }
            await Task.CompletedTask;
        }

        public async override Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            switch (update.Type)
            {
                case UpdateType.Unknown:
                    break;
                case UpdateType.Message:

                    if (update.Message is not { } message)
                        return;

                    switch (message.Type)
                    {
                        case MessageType.Unknown:
                            await Console.Out.WriteLineAsync($"Бог знает что пишет этот пользователь - {message.Chat.Id}\n{message}");
                            break;
                        case MessageType.Text:
                            if (message.Text is not { } text)
                                return;
                            if (text.StartsWith('/'))
                            {
                                await HandleCommandMessageAsync(message, cancellationToken);
                            }
                            else
                            {
                                await HandleTextMessageAsync(message, cancellationToken);
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
                            if (message.WebAppData is not { } webAppData)
                                return;

                            await _botClient.SendTextMessageAsync(message.Chat.Id, $"Данные получены! ❤ 👌 ✔", replyMarkup: new ReplyKeyboardRemove());
                            await Pause(500);

                            await ParseWebAppData(message, webAppData, cancellationToken);

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
                            await Console.Out.WriteLineAsync($"Бог знает что пишет этот пользователь - {message.Chat.Id}\n");
                            break;
                    }


                    break;
                case UpdateType.InlineQuery:
                    break;
                case UpdateType.ChosenInlineResult:
                    break;
                case UpdateType.CallbackQuery:
                    if (update.CallbackQuery is { } callbackQuery)
                    {
                        await HandleCallBackQueryAsync(callbackQuery, cancellationToken);
                    }
                    break;
                case UpdateType.EditedMessage:
                    break;
                case UpdateType.ChannelPost:
                    break;
                case UpdateType.EditedChannelPost:
                    break;
                case UpdateType.ShippingQuery:
                    break;
                case UpdateType.PreCheckoutQuery:
                    break;
                case UpdateType.Poll:
                    break;
                case UpdateType.PollAnswer:
                    break;
                case UpdateType.MyChatMember:
                    break;
                case UpdateType.ChatMember:
                    break;
                case UpdateType.ChatJoinRequest:
                    break;
                default:

                    break;
            }
        }

        #endregion


        #region Methods

        public override bool CanHandle(long userId, CancellationToken cancellationToken)
        {
            if (userId is { } id)
            {
                if (!IsAdmin(id))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsAdmin(long userId)
        {
            return _adminList.ContainsKey(userId);
        }

        private async void ProgressUsersUpdated(ProgressUsers progress)
        {
            if (_userList[progress.UserId] is { } user)
            {
                await Console.Out.WriteLineAsync($"Получены обновления прогресса пользователя - {user.FirstName} : {user.UserId}");
                await Console.Out.WriteLineAsync($"День: {progress.CurrentDay} - Шаг обновления: {progress.CurrentStep}");
            }
            else
            {
                await Console.Out.WriteLineAsync($"Ошибка синхронизации пользователей.");
                return;
            }

            await UpdateProgressUsersLocal(progress);
            var updateProgress = await _databaseService.UpdateUserProgressAsync(progress);

            if (updateProgress && progress.UpdateState != UpdateState.UpdateDate)
            {
                switch (progress.CurrentDay)
                {
                    case 1:
                        await Console.Out.WriteLineAsync("День 1");
                        await UpdateAsyncDay1(progress);

                        break;
                    case 2:
                        await Console.Out.WriteLineAsync("День 2");
                        await UpdateAsyncDay2(progress);

                        break;
                    default:
                        await Console.Out.WriteLineAsync($"Ошибка обновления прогресса пользователя.");
                        break;
                }
            }
        }

        private async Task UpdateAsyncDay2(ProgressUsers progress)
        {
            switch (progress.CurrentStep)
            {
                case 1:
                    await Console.Out.WriteLineAsync("Обновление 2 день, шаг 1");
                    await _botClient.SendTextMessageAsync(progress.UserId, GetStringFormatDialogUser(DialogData.GOOD_MORNING, progress.UserId));
                    await _botClient.SendTextMessageAsync(progress.UserId, DialogData.DIALOG_DAY_2_STEP_1);

                    await CreateMenuInlineKeyboardContinue(progress.UserId);
                    break;
                case 2:
                    await Console.Out.WriteLineAsync("Обновление 2 день, шаг 2");
                    //TODO - реализовать конфиг пользователя
                    await _botClient.SendTextMessageAsync(progress.UserId, $"При вызове меню, у вас появится возможность изменить настройки пользователя, или посмотреть их.");
                    await Pause(1500, 2000);
                    await CreateMenuSettingsBotAsync(progress.UserId, CancellationToken.None);
                    if (progress.IsTheNextStepSheduledInTime)
                    {
                        try
                        {
                            if (_progressUsersList.TryGetValue(progress.UserId, out var userProgres))
                            {
                                SetNextTimeStepAddMinutes(userProgres, 3);
                                await Pause(1000, 2000);
                            }
                        }
                        catch (Exception ex)
                        {
                            await Console.Out.WriteLineAsync($"Произошла ошибка изменения времени следующего шага и дня.\nПодробнее - {ex.Message}");
                            throw;
                        }
                    }
                    break;
                case 3:
                    await _botClient.SendTextMessageAsync(progress.UserId, $"{_userList[progress.UserId].FirstName}, надеюсь ты уже успел освоить новый пункт меню и нам пора двигаться дальше!\n" +
                        $"Ты готов?");
                    await Pause(2000);
                    await CreateMenuInlineKeyboardContinue(progress.UserId);

                    break;
                default:
                    await _botClient.SendTextMessageAsync(progress.UserId, $"{_userList[progress.UserId].FirstName}, Дальше общение, пока только в живом формате. Твой Персональный консультант уже в курсе, что хочешь с ним связаться.");
                    await Pause(2000, 2500);
                    await _botClient.SendTextMessageAsync(progress.UserId, $"{_userList[progress.UserId].FirstName}, Напоминаю, ты также можешь задавать свои вопросы, прямо в чат бота.");
                    await Pause(1000, 2000);
                    
                    foreach (var admin in _adminList.Keys)
                    {
                        await _botClient.SendTextMessageAsync(admin, $"Пользователь {_userList[progress.UserId].FirstName}, требует внимания и живого общения.");
                    }

                    break;
            }
        }

        private async Task UpdateAsyncDay1(ProgressUsers progress)
        {
            switch (progress.CurrentStep)
            {
                case 0:
                    await DialogZeroStepDayOne(progress.UserId);
                    await Pause(1500);
                    await CreateMenuInlineKeyboardContinue(progress.UserId);
                    break;
                case 1:
                    await _botClient.SendTextMessageAsync(progress.UserId,
                                DialogData.USER_VPO_OFFLINE_DAY_1_STEP_1, replyMarkup: new ReplyKeyboardRemove());
                    await Pause(2000);
                    //TODO - Тут добавить кейсы, результаты...
                    await SendingTheResultsOfTheProgramParticipants(progress.UserId);
                    await Pause();
                    await CreateMenuInlineKeyboardContinue(progress.UserId);
                    break;
                case 2:
                    await _botClient.SendTextMessageAsync(progress.UserId, DialogData.GEETING_TO_KNOW_HERBY, parseMode: ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
                    var fotoResultsUrl = "https://raw.githubusercontent.com/jevlogin/VPO/main/images/Herby.jpg";
                    await SendPhotoAsync(progress.UserId, fotoResultsUrl);
                    //TODO - (Показываем визуализированный образ Герби, он машет рукой)
                    await Pause(3000);
                    await _botClient.SendTextMessageAsync(progress.UserId, DialogData.GEETING_TO_KNOW_HERBY_TOGETHER, parseMode: ParseMode.Html);
                    await Pause(2000);
                    await CreateMenuInlineKeyboardContinue(progress.UserId);
                    break;
                case 3:
                    await _botClient.SendTextMessageAsync(progress.UserId, DialogData.PREPARING_FOR_THE_TRIP, parseMode: ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
                    await Pause(2000);
                    await _botClient.SendTextMessageAsync(progress.UserId, DialogData.PREPARING_FOR_THE_TRIP_LOST_STEP, parseMode: ParseMode.Html);
                    await Pause(3000);
                    await CreateMenuInlineKeyboardContinue(progress.UserId);
                    break;
                case 4:
                    var msgJourneBegins = GetStringFormatDialogUser(DialogData.THE_JOURNEY_BEGINS_USERFIELDS_0, progress.UserId);

                    await _botClient.SendTextMessageAsync(progress.UserId, msgJourneBegins, parseMode: ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
                    await Pause(2000, 5000);
                    await _botClient.SendTextMessageAsync(progress.UserId, DialogData.THE_JOURNEY_BEGINS_PHIRST_STEP, parseMode: ParseMode.Html);
                    await Pause(3000);
                    await _botClient.SendTextMessageAsync(progress.UserId, DialogData.THE_JOURNEY_BEGINS_CONCLISION, parseMode: ParseMode.Html);
                    await Pause(2000);
                    await CreateMenuInlineKeyboardContinue(progress.UserId);

                    break;
                case 5:
                    var msgCongratulations = DialogData.USER_CONGRATILATORY_RESPONSES_ANSWER[_random.Next(0, DialogData.USER_CONGRATILATORY_RESPONSES_ANSWER.Length)];

                    await _botClient.SendTextMessageAsync(progress.UserId, msgCongratulations, parseMode: ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
                    await Pause(500, 2000);

                    await _botClient.SendTextMessageAsync(progress.UserId, DialogData.INTRODUCTORY_INFORMATION_ABOUT_THE_TRIP, parseMode: ParseMode.Html);
                    await Pause(700, 3000);

                    await ActivateMethodSendBookStart(progress.UserId, CancellationToken.None);
                    await Pause(1000, 2000);

                    await _botClient.SendTextMessageAsync(progress.UserId, DialogData.INTRODUCTORY_INFORMATION_ABOUT_THE_TRIP_2, parseMode: ParseMode.Html);
                    await Pause(1200, 3000);

                    await ActivateMethodUserSendInstructionVPO(progress.UserId, CancellationToken.None);
                    await Pause(500, 1000);

                    await ActivateMethodUserOfHowToFillOutAFoodDiary(progress.UserId, CancellationToken.None);
                    await Pause(1000, 2000);

                    if (progress.IsTheNextStepSheduledInTime)
                    {
                        try
                        {
                            if (_progressUsersList.TryGetValue(progress.UserId, out var userProgres))
                            {
                                SetNextTimeStepAddMinutes(userProgres, 3);
                                await Pause(1000, 2000);
                                SetNextDayHourInProgress(userProgres, 5);
                            }
                        }
                        catch (Exception ex)
                        {
                            await Console.Out.WriteLineAsync($"Произошла ошибка изменения времени следующего шага и дня.\nПодробнее - {ex.Message}");
                            throw;
                        }
                    }
                    await _botClient.SendTextMessageAsync(progress.UserId, DialogData.BOT_ANSWER_GOODBUY, parseMode: ParseMode.Html);
                    break;
                case 6:
                    await _botClient.SendTextMessageAsync(progress.UserId, DialogData.REMINDER_OF_DAY_1, parseMode: ParseMode.Html);
                    break;
                default:
                    await Console.Out.WriteLineAsync($"Пользователь с id - {progress.UserId} вытворяет фокусы");
                    break;
            }
        }

        private void SetNextTimeStepAddMinutes(ProgressUsers userProgres, int minutes)
        {
            userProgres.DateTimeOfTheNextStep = DateTime.UtcNow.ToLocalTime().AddMinutes(minutes);
        }

        private void SetNextDayHourInProgress(ProgressUsers userProgres, int hour)
        {
            userProgres.DateNextDayVPO = DateTime.Today.AddDays(1) + new TimeSpan(hour, 0, 0);
        }

        private string GetStringFormatDialogUser(string data, long userId)
        {
            return string.Format(data, _userList[userId].FirstName);
        }

        private async Task SendingTheResultsOfTheProgramParticipants(long userId)
        {
            await _botClient.SendTextMessageAsync(userId, DialogData.RESULTS_OF_OUR_PARTICIPANTS, replyMarkup: new ReplyKeyboardRemove());
            await Pause(1500);

            var msgInstructionHowto = await LoadedInstruction(userId);

            var fotoResultsUrl = "https://raw.githubusercontent.com/jevlogin/VPO/main/results/result_komolova.png";
            var resultsPhotoSend = await _botClient.SendPhotoAsync(userId, InputFile.FromUri(fotoResultsUrl));

            if (resultsPhotoSend != null)
            {
                await _botClient.DeleteMessageAsync(userId, msgInstructionHowto.MessageId);
            }
        }

        private async Task CreateMenuInlineKeyboardContinue(long userId)
        {
            var answer = DialogData.USER_CONTINUER_RESPONSE_BUTTON[_random.Next(0, DialogData.USER_CONTINUER_RESPONSE_BUTTON.Length)];
            var button = InlineKeyboardButton.WithCallbackData(answer, callbackData: "/user_continue");
            var replyMarkup = new InlineKeyboardMarkup(button);
            var positivePhrase = DialogData.USER_MOTIVATIONAL_PHRASES[_random.Next(0, DialogData.USER_MOTIVATIONAL_PHRASES.Length)];
            await _botClient.SendTextMessageAsync(userId, positivePhrase, parseMode: ParseMode.Html, replyMarkup: replyMarkup);
        }

        private async Task DialogZeroStepDayOne(long userId)
        {
            await _botClient.SendTextMessageAsync(userId, DialogData.HERBY_WELCOME_TO_VPO, parseMode: ParseMode.Html);
            //TODO - [Фото, кружочки команды]
            await Pause(1200);
            await _botClient.SendTextMessageAsync(userId, DialogData.HERBY_PROGRAM_PRESENTATION_RIDDLES, parseMode: ParseMode.Html);
        }

        private async Task Pause(int valueMax = -1, int valueMin = -2)
        {
            int minV = 500;
            int maxV = 5000;
            int result = minV;

            if (valueMin == -2 && valueMax == -1)
            {
                result = _random.Next(minV, maxV);
            }
            else if (valueMin == -2)
            {
                result = _random.Next(minV, valueMax);
            }
            else if (valueMin > valueMax)
            {
                var temp = valueMin;
                valueMin = valueMax;
                valueMax = temp;
                result = _random.Next(valueMin, valueMax);
            }

            await Task.Delay(result);
        }

        private async Task UpdateProgressUsersLocal(ProgressUsers progressCurrent)
        {
            if (_progressUsersList.TryGetValue(progressCurrent.UserId, out var progress))
            {
                progress = progressCurrent;
                await Console.Out.WriteLineAsync($"Обновление локального прогресса пользователя - {DialogData.SUCCESS}");
            }
            else
            {
                await Console.Out.WriteLineAsync($"Обновление локального прогресса пользователя - {DialogData.FAILED}");
            }
        }

        private async Task ConnectedToDatabaseAndSynchronizeProgress()
        {
            _progressUsersList = await _databaseService.LoadProgressUsersAsync();
            await Console.Out.WriteLineAsync($"прогресс пользователей, был УСПЕШНО загружен.");
        }

        private async Task ParseWebAppData(Message message, WebAppData webAppData, CancellationToken cancellationToken)
        {
            var parseArray = JArray.Parse(webAppData.Data);

            JObject messageDataInfoType = (JObject)parseArray[0];

            MessageDataInfoType messageInfo;

            try
            {
                messageInfo = messageDataInfoType.ToObject<MessageDataInfoType>();
            }
            catch (Exception exception)
            {
                await Console.Out.WriteLineAsync($"Возникло исключение:\n\n{exception}");
                return;
            }

            if (messageInfo is { } msgInfo && msgInfo.CallBackMethod is { } callBackType)
            {
                switch (callBackType)
                {
                    case CallBackMethod.BotConfig:
                        await Console.Out.WriteLineAsync($"Мы получили настройки пользователя.");
                        JObject botSettingsObject = (JObject)parseArray[1];
                        UserBotSettings userBotSettings;
                        try
                        {
                            userBotSettings = botSettingsObject.ToObject<UserBotSettings>();
                        }
                        catch (Exception ex)
                        {
                            await Console.Out.WriteLineAsync($"Возникло исключение:\n\n{ex}");
                            return;
                        }
                        userBotSettings!.UserId = message.From.Id;

                        if (!_userList.Keys.Contains(userBotSettings!.UserId))
                        {
                            await _botClient.SendTextMessageAsync(message.From.Id, $"Данная функция доступна, только авторизованным пользователям.\n\n");
                            await Pause(1000, 2000);
                            await _botClient.SendTextMessageAsync(message.From.Id, $"Вот так могли бы выглядеть Ваши настройки:\n\n");
                            await Pause(1000, 2000);
                            await WriteUserBotSettingsAsync(message, userBotSettings);
                            await _botClient.SendTextMessageAsync(message.From.Id, $"Предлагаю Вам пройти быструю регистрацию, чтобы пользоваться сервисом в полном объеме.\n");
                            await CreateMenuKeyboardAuthUser(message.From.Id, cancellationToken);
                        }
                        else
                        {
                            await _databaseService.AddOrUpdateBotSettingsAsync(userBotSettings);
                            await Pause(1000, 2000);
                            await _botClient.SendTextMessageAsync(message.From.Id, $"Вот Ваша запись:\n\n");
                            await Pause(1000, 2000);
                            await WriteUserBotSettingsAsync(message, userBotSettings);
                            await _botClient.SendTextMessageAsync(message.From.Id, $"Вы можете быстро посмотреть свои настройки в соответствующем пункте меню.\n");
                        }

                        break;
                    case CallBackMethod.FoodDiaryFilling:

                        JObject foodDiaryForm = (JObject)parseArray[1];
                        var foodDiary = foodDiaryForm.ToObject<FoodDiaryEntry>();

                        if (foodDiary != null)
                        {
                            foodDiary.UserId = message.From.Id;

                            Console.WriteLine(foodDiary.ToString());

                            if (!_userList.Keys.Contains(foodDiary.UserId))
                            {
                                await _botClient.SendTextMessageAsync(message.From.Id, $"Данная функция доступна, только авторизованным пользователям.\n\n");
                                await Pause(1000, 2000);
                                await _botClient.SendTextMessageAsync(message.From.Id, $"Вот так могла бы выглядеть Ваша запись:\n\n");
                                await Pause(1000, 2000);
                                await _botClient.SendTextMessageAsync(message.From.Id, foodDiary.ToString());
                                await Pause(4000, 6000);
                                await _botClient.SendTextMessageAsync(message.From.Id, $"Предлагаю Вам пройти быструю регистрацию, чтобы пользоваться сервисом в полном объеме.\n");
                                await CreateMenuKeyboardAuthUser(message.From.Id, cancellationToken);
                            }
                            else
                            {
                                await _databaseService.AddOrUpdateFoodDiaryAsync(foodDiary);
                                await Pause(1000, 2000);
                                await _botClient.SendTextMessageAsync(message.From.Id, $"Вот Ваша запись:\n\n");
                                await Pause(1000, 2000);
                                await _botClient.SendTextMessageAsync(message.From.Id, foodDiary.ToString());
                                await Pause(1000, 2000);
                                await _botClient.SendTextMessageAsync(message.From.Id, $"Вы можете посмотреть свой дневник в соответствующем пункте меню.\n");
                            }
                        }


                        break;
                    case CallBackMethod.UserIntroduction:
                        JObject vpoForm = (JObject)parseArray[1];
                        var user = vpoForm.ToObject<UserVPO>();

                        if (user != null)
                        {
                            user.UserId = message.From.Id;

                            if (!_userList.Keys.Contains(user.UserId))
                            {
                                await AddedNewUserToLocalUserList(user);

                                var greeting = DialogData.GREETING_TEMPLATES_STRING_FORMAT[_random.Next(0, DialogData.GREETING_TEMPLATES_STRING_FORMAT.Length)];
                                string greetingMessage = GetStringFormatDialogUser(greeting, user.UserId);
                                var msgToUserIntro = await _botClient.SendTextMessageAsync(message.Chat.Id, greetingMessage, parseMode: ParseMode.Html);
                                await Pause(1000);

                                if (msgToUserIntro != null)
                                {
                                    await _databaseService.AddUserAsync(user);

                                    await AddedNewUserProgressInLocalListAndSubscribeUpdate(day: 1, step: 0, user: user);
                                    await _databaseService.UpdateUserProgressAsync(_progressUsersList[user.UserId]);

                                }
                                await Pause(700, 1500);

                                _progressUsersList[user.UserId].UpdateState = UpdateState.FullUpdate;
                            }
                            else
                            {
                                await Console.Out.WriteLineAsync("Ошибка. Пользователь должен был быть подгружен при старте программы.");
                            }
                        }

                        break;

                    case CallBackMethod.UserRequestForConsultation:

                        Console.WriteLine($"Получены данные: \"Заявка на консультацию\"");
                        JObject requestForConsultation = (JObject)parseArray[1];
                        var userRequest = requestForConsultation.ToObject<UserRequestConsultationOnline>();

                        if (userRequest != null)
                        {
                            userRequest.UserId = message.From!.Id;

                            var tempUser = new UserVPO()
                            {
                                UserId = userRequest.UserId,
                                FirstName = userRequest.FirstName,
                                LastName = userRequest.LastName,
                                Phone = userRequest.Phone
                            };

                            if (!_userList.Keys.Contains(tempUser.UserId))
                            {
                                await AddedNewUserToLocalUserList(tempUser);
                                var msgToUserIntro = await _botClient.SendTextMessageAsync(tempUser.UserId, $"Отлично! Рад знакомству, {tempUser.FirstName}");

                                if (msgToUserIntro != null)
                                {
                                    await _databaseService.AddUserAsync(tempUser);

                                    await AddedNewUserProgressInLocalListAndSubscribeUpdate(day: 1, step: 0, user: tempUser);
                                    await _databaseService.UpdateUserProgressAsync(_progressUsersList[tempUser.UserId]);

                                }
                                await SendingAMessageToTheAdministratorAboutAnAppointmentForAnOnlineConsultation(userRequest, cancellationToken);
                            }
                            else
                            {
                                await AddedNewUserToLocalUserList(tempUser);

                                await _botClient.SendTextMessageAsync(tempUser.UserId,
                                    $"{tempUser.FirstName}, я тебя помню ✌😊 и записал тебя на консультацию.");
                                await SendingAMessageToTheAdministratorAboutAnAppointmentForAnOnlineConsultation(userRequest, cancellationToken);
                            }
                        }
                        break;
                    default:
                        await Console.Out.WriteLineAsync($"Ошибка. callBackType в message.WebAppData не верный");
                        break;
                }
            }
        }
        
        private async Task WriteUserBotSettingsAsync(Message message, EmptyBotSettings userBotSettings)
        {
            await _botClient.SendTextMessageAsync(message.From.Id, userBotSettings.ToString(), parseMode: ParseMode.Html);
            await Pause(1500, 3000);
        }

        private async Task SendingAMessageToTheAdministratorAboutAnAppointmentForAnOnlineConsultation(UserRequestConsultationOnline userFormRequest, CancellationToken cancellationToken)
        {
            await _botClient.SendTextMessageAsync(_adminList.FirstOrDefault().Key,
               $"Пользователь:\n\n" +
               $"Имя - {userFormRequest.FirstName}\n" +
               $"Фамилия - {userFormRequest.LastName}\n" +
               (string.IsNullOrEmpty(userFormRequest.MiddleName) ?
                   "" : $"Отчество - {userFormRequest.MiddleName}\n") +
               $"Телефон - {userFormRequest.Phone}\n" +
               (string.IsNullOrEmpty(userFormRequest.Email) ?
                   "" : $"Email - {userFormRequest.Email}\n\n") +
               $"\nЗаписался на <b>ОНЛАЙН КОНСУЛЬТАЦИЮ!</b>",
               parseMode: ParseMode.Html, cancellationToken: cancellationToken);
        }

        private async Task AddedNewUserProgressInLocalListAndSubscribeUpdate(int day, int step, UserVPO user)
        {
            await Console.Out.WriteLineAsync($"Создаю прогресс нового пользователя {user.FirstName} - День {day}, Шаг {step}");

            var currentTime = DateTime.UtcNow.ToLocalTime();

            var currentProgressOfflineUser = new ProgressUsers(user.UserId, day, step, currentTime, currentTime,
                                                CheckingTheNextStepInTime(currentTime), UpdateState.None);

            await Console.Out.WriteLineAsync($"Добавляем прогресс пользователя в локальную базу данных.");
            _progressUsersList[user.UserId] = currentProgressOfflineUser;
            _progressUsersList[user.UserId].ProgressUpdated += ProgressUsersUpdated;
        }

        private bool CheckingTheNextStepInTime(DateTime nextDateEventStep)
        {
            if (DateTime.UtcNow.ToLocalTime() < nextDateEventStep)
                return false;
            return true;
        }

        private async Task AddedNewUserToLocalUserList(UserVPO user)
        {
            if (!_userList.ContainsKey(user.UserId))
            {
                _userList[user.UserId] = user;
                await Console.Out.WriteLineAsync($"Пользователь {user.FirstName}, был добавлен в локальный список пользователей.");
            }
            else
            {
                _userList[user.UserId] = user;
                await Console.Out.WriteLineAsync($"Пользователь с id {user.UserId} уже существует в локальном списке пользователей. Данные обновлены.");
            }
        }

        private async Task HandleTextMessageAsync(Message message, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync($"{message.Text}");

            foreach (var adminId in _adminList.Keys)
            {
                await _botClient.ForwardMessageAsync(
                        adminId,
                        message.Chat.Id,
                        message.MessageId,
                        cancellationToken: cancellationToken);
            }

            await _botClient.SendTextMessageAsync(message.Chat.Id, DialogData.YOUR_MESSAGE_HAS_BEEN_RECEIVED, cancellationToken: cancellationToken);
        }

        private async Task HandleCommandMessageAsync(Message message, CancellationToken cancellationToken)
        {
            if (message.Text is { } text)
            {
                await Console.Out.WriteLineAsync($"{text}");
                var commands = text.ToLower().Split(' ');
                var command1 = commands[0];

                switch (command1)
                {
                    case "/start":
                        if (_userList.TryGetValue(message.Chat.Id, out var user))
                        {
                            _progressUsersList[user.UserId].UpdateState = UpdateState.FullUpdate;

                            await Pause(5000);
                            await _botClient.SendTextMessageAsync(message.Chat.Id, DialogData.START_HELP_MESSAGE);
                        }
                        else
                        {
                            await _botClient.SendTextMessageAsync(message.Chat.Id,
                                DialogData.HERBY_IINTRODUCE_YOURSELF, parseMode: ParseMode.Html);
                            await Pause(1500);
                            await _botClient.SendTextMessageAsync(message.Chat.Id,
                                DialogData.WHAT_IS_YOUR_NAME, parseMode: ParseMode.Html);
                            await Pause(1000);
                            await CreateMenuKeyboardAuthUser(message.Chat.Id, cancellationToken);
                            await Pause(2000, 3000);

                            var predstavitsya_1 = "https://raw.githubusercontent.com/jevlogin/VPO/main/images/Intro1.jpg";
                            var predstavitsya_2 = "https://raw.githubusercontent.com/jevlogin/VPO/main/images/Intro2.jpg";

                            await _botClient.SendTextMessageAsync(message.Chat.Id, "Кнопка представиться ниже!!! Вот наглядно где она");
                            await SendPhotoAsync(message.Chat.Id, predstavitsya_1);
                            await Pause(1000, 2000);
                            await _botClient.SendTextMessageAsync(message.Chat.Id, "Еслии ты ее не видишь, жми на 4 квадратика!!! Вот наглядно куда нажать надо.");
                            await SendPhotoAsync(message.Chat.Id, predstavitsya_2);

                        }
                        break;
                    case "/menu":
                        await CreateMenuKeyboard(message.Chat.Id, cancellationToken);
                        break;
                    case "/help":
                        await _botClient.SendTextMessageAsync(
                            message.Chat.Id,
                            DialogData.HELP_MENU_BUTTON, parseMode: ParseMode.Html,
                            cancellationToken: cancellationToken, replyMarkup: new ReplyKeyboardRemove());
                        break;
                    default:
                        if (commands.Length > 1) { break; }
                        await _botClient.SendTextMessageAsync(message.Chat.Id,
                            "Извините, я не узнаю эту команду.",
                            cancellationToken: cancellationToken);
                        break;
                }

                if (commands.Length > 1)
                {
                    var command2 = commands[1];
                    switch (command2)
                    {
                        case "/помощь" or "помощь":
                            await CreateMenuInline(message.Chat.Id, cancellationToken);
                            break;
                        case "дневник":
                            if (commands[2] is { } foodDiarryCommand)
                            {
                                switch (foodDiarryCommand)
                                {
                                    case "питания":
                                        await CreateMenuFoodDiaryAsync(message.Chat.Id, cancellationToken);

                                        break;
                                    default:
                                        Console.WriteLine("Кто-то, что-то попутал.");
                                        break;
                                }
                            }
                            break;
                        case "прочитать":
                            if (commands[2] is { } foodDiarryFilling)
                            {
                                switch (foodDiarryFilling)
                                {
                                    case "дневник":
                                        await Console.Out.WriteLineAsync($"мы нажали прочитать дневник питания");
                                        var foodDiaryList = await _databaseService.ReadTheFoodDiaryForTheCurrentDay(message.Chat.Id, cancellationToken);
                                        await Pause(1000, 1500);
                                        await _botClient.SendTextMessageAsync(message.Chat.Id, DialogData.BORDER_FRUIT, cancellationToken: cancellationToken);
                                        foreach (var foodDiary in foodDiaryList)
                                        {
                                            await _botClient.SendTextMessageAsync(message.Chat.Id, foodDiary.ToString(), cancellationToken: cancellationToken);
                                        }
                                        await Pause(1000, 1500);
                                        await _botClient.SendTextMessageAsync(message.Chat.Id, DialogData.BORDER_FRUIT, cancellationToken: cancellationToken);

                                        break;
                                    default:
                                        Console.WriteLine("Кто-то, что-то попутал.");
                                        break;
                                }
                            }
                            break;
                        case "настройки":
                            if (commands[2] is { } settingsUserCommand)
                            {
                                switch (settingsUserCommand)
                                {
                                    case "пользователя":
                                        Console.WriteLine("Даем возможность пользователю, изменить настройки, перепрограммировать");
                                        await CreateMenuSettingsBotAsync(message.Chat.Id, cancellationToken);

                                        break;
                                    case "посмотреть":
                                        await Console.Out.WriteLineAsync($"Кто-то хочет узнать настройки, а значит их надо где-то хранить.");
                                        var settingsUser = await _databaseService.ReadUserBotSettings(message.Chat.Id, cancellationToken);

                                        await WriteUserBotSettingsAsync(message, settingsUser);

                                        break;
                                    default:
                                        Console.WriteLine("Кто-то, что-то попутал.");
                                        break;
                                }
                            }
                            break;
                        case "офлайн":
                            if (commands[2] is { } command3)
                            {
                                switch (command3)
                                {
                                    case "консультация":
                                        if (UserAutorization(message.Chat.Id))
                                        {
                                            var answer = await _botClient.SendTextMessageAsync(message.Chat.Id,
                                                $"{_userList[message.Chat.Id].FirstName}\nНа чем мы тут остановились?",
                                                replyMarkup: new ReplyKeyboardRemove());
                                            await Pause(1000);

                                            await _botClient.SendTextMessageAsync(message.Chat.Id, "🤖 " +
                                                DialogData.BOT_ANSWER_SMILE_PRANK_ARRAY[_random.Next(0, DialogData.BOT_ANSWER_SMILE_PRANK_ARRAY.Length)],
                                                parseMode: ParseMode.Html);
                                            await Pause(1000);

                                            _progressUsersList[message.Chat.Id].UpdateState = UpdateState.FullUpdate;
                                        }
                                        else
                                        {
                                            await _botClient.SendTextMessageAsync(message.Chat.Id, DialogData.CONSULTATION_OFFLINE_WELCOME);
                                            await CreateMenuKeyboardAuthUser(message.Chat.Id, cancellationToken);
                                        }
                                        break;
                                    default:
                                        await _botClient.SendTextMessageAsync(message.Chat.Id, "Извините, я не узнаю эту команду.");
                                        break;
                                }
                                break;
                            }
                            break;
                        case "/exit" or "exit" or "/выход" or "выход":
                            await _botClient.SendTextMessageAsync(message.Chat.Id, "До свидания!", replyMarkup: new ReplyKeyboardRemove());
                            break;
                        default:
                            await _botClient.SendTextMessageAsync(message.Chat.Id, "Извините, я не узнаю эту команду.");
                            break;
                    }
                }
            }
        }

        private async Task CreateMenuSettingsBotAsync(long chatId, CancellationToken cancellationToken)
        {
            var webApp = new WebAppInfo();
            webApp.Url = @"https://jevlogin.github.io/VPO/BotConfig.html";
            var buttonChangeSettings = new KeyboardButton("/🛠️ Изменить настройки");
            buttonChangeSettings.WebApp = webApp;

            var buttonReadSettings = new KeyboardButton("/🧑🏻‍💻 Настройки посмотреть");

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new[] { buttonChangeSettings, buttonReadSettings },
            })
            {
                ResizeKeyboard = true
            };

            await _botClient.SendTextMessageAsync(chatId, DialogData.CHOOSE_ONE_OF_THE_OPTIONS, replyMarkup: replyKeyboard);
        }

        private async Task SendMessageCommingSoonAsync(long id, CancellationToken cancellationToken)
        {
            await _botClient.SendTextMessageAsync(id, DialogData.THE_TECHNOLOGY_IS_UNDER_DEVELOPMENT, replyMarkup: new ReplyKeyboardRemove());
        }

        private bool UserAutorization(long id)
        {
            return _userList.ContainsKey(id);
        }

        private async Task CreateMenuInline(long chatId, CancellationToken cancellationToken)
        {
            var button1 = InlineKeyboardButton.WithCallbackData(text: "Инструкция участника VPO", callbackData: "/user_instruction");
            var button2 = InlineKeyboardButton.WithCallbackData(text: "Дневник питания - Старт", callbackData: "/user_bookstart");
            var button3 = InlineKeyboardButton.WithCallbackData(text: "Дневник питания - Квест", callbackData: "/user_bookquest");
            var button4 = InlineKeyboardButton.WithCallbackData(text: "Как заполнять дневник питания", callbackData: "/user_howto");

            List<List<InlineKeyboardButton>> buttons = new()
            {
                new List<InlineKeyboardButton>()
                {
                    button1,
                    button2,
                },
                new List<InlineKeyboardButton>()
                {
                    button3,
                    button4,
                },
            };
            var replyMarkup = new InlineKeyboardMarkup(buttons);

            await _botClient.SendTextMessageAsync(chatId, "🔎🆘", replyMarkup: new ReplyKeyboardRemove());
            await _botClient.SendTextMessageAsync(chatId, DialogData.CHOOSE_ONE_OF_THE_OPTIONS, replyMarkup: replyMarkup);
        }

        private async Task CreateMenuKeyboard(long chatId, CancellationToken cancellationToken)
        {
            var webAppInfo = new WebAppInfo();
            webAppInfo.Url = @"https://jevlogin.github.io/VPO/RequestForConsultation.html";
            var consultationOnlineButton = new KeyboardButton("/📞 Записаться на консультацию");
            consultationOnlineButton.WebApp = webAppInfo;

            var consultationOfflineButton = new KeyboardButton("/🧬 Офлайн консультация");
            var helpButton = new KeyboardButton("/🔍 Помощь");
            var exitButton = new KeyboardButton("/🏠 Выход");

            var foodDiarryButton = new KeyboardButton("/📖 Дневник питания");
            var settingsButton = new KeyboardButton("/☸ Настройки пользователя");

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new[] { consultationOnlineButton, consultationOfflineButton  },
                new[] { foodDiarryButton, settingsButton },
                new[] { helpButton, exitButton },
            })
            {
                ResizeKeyboard = true
            };

            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: DialogData.CHOOSE_ONE_OF_THE_OPTIONS,
                replyMarkup: replyKeyboard);
            return;

        }

        private async Task SendPhotoAsync(long chatId, string urlPhoto)
        {
            var msgInstructionHowto = await LoadedInstruction(chatId);

            var resultsPhotoSend = await _botClient.SendPhotoAsync(chatId, InputFile.FromUri(urlPhoto));

            if (resultsPhotoSend != null)
            {
                await _botClient.DeleteMessageAsync(chatId, msgInstructionHowto.MessageId);
            }
        }


        private async Task CreateMenuFoodDiaryAsync(long chatId, CancellationToken cancellationToken)
        {
            var webApp = new WebAppInfo();
            webApp.Url = @"https://jevlogin.github.io/VPO/FoodDiary.html";
            var buttonFillingFoodDiary = new KeyboardButton("/📖 Заполнить дневник");
            buttonFillingFoodDiary.WebApp = webApp;

            var buttonReadFoodDiary = new KeyboardButton("/📖 Прочитать дневник");

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new[] { buttonFillingFoodDiary, buttonReadFoodDiary },
            })
            {
                ResizeKeyboard = true
            };

            await _botClient.SendTextMessageAsync(chatId, DialogData.CHOOSE_ONE_OF_THE_OPTIONS, replyMarkup: replyKeyboard);
        }

        private async Task CreateMenuKeyboardAuthUser(long chatId, CancellationToken cancellationToken)
        {
            var webAppInfo = new WebAppInfo();
            webAppInfo.Url = @"https://jevlogin.github.io/VPO/IntroVPOBot.html";

            var button = new KeyboardButton("👽 Представиться 🤝");
            button.WebApp = webAppInfo;

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                button
            })
            {
                ResizeKeyboard = true
            };

            await _botClient.SendTextMessageAsync(chatId, "Кнопка предствиться ниже ⬇⬇⬇👇:", replyMarkup: replyKeyboard);
        }

        private async Task HandleCallBackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            if (callbackQuery.Message is not { } message)
            {
                return;
            }
            var chatId = message.Chat.Id;
            var data = callbackQuery.Data;

            await Console.Out.WriteLineAsync($"Обрабатываю данные {data} в чате {chatId}");

            switch (data)
            {
                case "/user_instruction":
                    await ActivateMethodUserSendInstructionVPO(chatId, cancellationToken);
                    break;
                case "/user_bookstart":
                    await ActivateMethodSendBookStart(chatId, cancellationToken);
                    break;
                case "/user_bookquest":
                    var response = $"Дневник заполняете в рамках программы VPO, в течении 21 дня.\nКаждый день.";
                    await _botClient.SendTextMessageAsync(chatId, response);
                    var msgInstructionBookQuest = await LoadedInstruction(chatId);
                    var fileUrlBookquest = "https://drive.google.com/uc?export=download&confirm=no_antivirus&id=1L5dNPZNZiq6-5y_82lo1DWzF_bE6Eu_E";
                    var instructionBookQuest = await _botClient.SendDocumentAsync(chatId, InputFile.FromUri(fileUrlBookquest), cancellationToken: cancellationToken);
                    if (instructionBookQuest != null)
                    {
                        await _botClient.DeleteMessageAsync(chatId, msgInstructionBookQuest.MessageId);
                    }
                    break;
                case "/user_howto":
                    await ActivateMethodUserOfHowToFillOutAFoodDiary(chatId, cancellationToken);
                    break;
                case "/user_continue":
                    if (_progressUsersList.Count > 0)
                    {
                        var msgAnswerCongrulatory = DialogData.USER_CONGRATILATORY_RESPONSES_ANSWER[_random.Next(0, DialogData.USER_CONGRATILATORY_RESPONSES_ANSWER.Length)];
                        await _botClient.SendTextMessageAsync(chatId, msgAnswerCongrulatory, parseMode: ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
                        await Pause(1000, 2000);
                        _progressUsersList[chatId].CurrentStep++;
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync($"Пользователей нет.");
                    }
                    break;
                case "/user_buyformulaone":
                    await _botClient.SendTextMessageAsync(chatId, DialogData.THANKSTOUSER,
                                        parseMode: ParseMode.Html, cancellationToken: cancellationToken);
                    await _botClient.SendTextMessageAsync(_adminList.FirstOrDefault().Key,
                        $"Пользователь - {_userList[chatId].FirstName}\n" +
                        $"Телефон - {_userList[chatId].Phone}\n\n" +
                        $"Желает приобрести продукт - Формула 1",
                        cancellationToken: cancellationToken);
                    break;
                case "/user_aboutformulaone":
                    //TODO - добавить PDF файл с описанием и картинками.
                    await _botClient.SendTextMessageAsync(chatId, "Это коктейль", cancellationToken: cancellationToken);
                    break;
            }
        }

        private async Task ActivateMethodUserOfHowToFillOutAFoodDiary(long chatId, CancellationToken cancellationToken)
        {
            await _botClient.SendTextMessageAsync(chatId, DialogData.USER_HOWTOFILLOUTAFOODDIARY);
            var msgInstructionHowto = await LoadedInstruction(chatId);

            var fileUrlHowto = "https://raw.githubusercontent.com/jevlogin/VPO/main/videoplayback.mp4";
            var instructionHowto = await _botClient.SendDocumentAsync(chatId, InputFile.FromUri(fileUrlHowto), cancellationToken: cancellationToken);

            if (instructionHowto != null)
            {
                await _botClient.DeleteMessageAsync(chatId, msgInstructionHowto.MessageId);
            }
        }

        private async Task ActivateMethodSendBookStart(long chatId, CancellationToken cancellationToken)
        {
            await _botClient.SendTextMessageAsync(chatId, DialogData.USER_BOOKSTART_SEND, parseMode: ParseMode.Html);
            var user_bookstart = await LoadedInstruction(chatId);

            var fileUrlBookstart = "https://drive.google.com/uc?export=download&confirm=no_antivirus&id=1TziuxE_YTzvTqqKLTLTQBeI9SIIG9HE7";
            var instructionBookstart = await _botClient.SendDocumentAsync(chatId,
                                InputFile.FromUri(fileUrlBookstart), cancellationToken: cancellationToken);
            if (instructionBookstart != null)
            {
                await _botClient.DeleteMessageAsync(chatId, user_bookstart.MessageId);
            }
        }

        private async Task ActivateMethodUserSendInstructionVPO(long chatId, CancellationToken cancellationToken)
        {
            await _botClient.SendTextMessageAsync(chatId, DialogData.USER_INSTRUCTION, parseMode: ParseMode.Html);

            var instructionMessage = await LoadedInstruction(chatId);

            var fileUrl = "https://drive.google.com/uc?export=download&confirm=no_antivirus&id=1SCEYzu1S_Jj8elBY2jEWZjzJ3ogVIItY";
            var instruction = await _botClient.SendDocumentAsync(chatId, InputFile.FromUri(fileUrl), cancellationToken: cancellationToken);
            if (instruction != null)
            {
                await _botClient.DeleteMessageAsync(chatId, instructionMessage.MessageId);
            }
        }

        private async Task<Message> LoadedInstruction(long chatId)
        {
            return await _botClient.SendTextMessageAsync(chatId, "<i>Загружаю инструкцию...</i>", parseMode: ParseMode.Html);
        }

        #endregion
    }
}