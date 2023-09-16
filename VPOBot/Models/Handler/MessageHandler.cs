using Telegram.Bot;
using Telegram.Bot.Types;


namespace WORLDGAMEDEVELOPMENT
{
    internal abstract class MessageHandler : IMessageHandler
    {
        protected readonly ITelegramBotClient _botClient;

        protected MessageHandler(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public abstract Task HandlePollingErrorAsync(Exception exception, CancellationToken cancellationToken);

        public abstract Task HandleUpdateAsync(Update update, CancellationToken cancellationToken);

        public abstract bool CanHandle(long userId, CancellationToken cancellationToken);
    }
}