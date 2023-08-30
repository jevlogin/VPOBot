using Telegram.Bot.Types;


namespace WORLDGAMEDEVELOPMENT
{
    internal interface IMessageHandler
    {
        public Task HandlePollingErrorAsync(Exception exception, CancellationToken cancellationToken);

        public Task HandleUpdateAsync(Update update, CancellationToken cancellationToken);

        public bool CanHandle(long userId, CancellationToken cancellationToken);
    }
}
