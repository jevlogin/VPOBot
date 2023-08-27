using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;


namespace WORLDGAMEDEVELOPMENT
{
    internal abstract class MessageHandler : IUpdateHandler
    {
        protected readonly ITelegramBotClient _botClient;

        protected MessageHandler(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}