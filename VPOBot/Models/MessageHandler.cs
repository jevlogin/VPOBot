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

        public abstract Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);

        public abstract Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

        public abstract bool CanHandle(long? userId, CancellationToken cancellationToken);
    }
}