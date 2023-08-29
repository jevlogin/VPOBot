using Telegram.Bot;
using Telegram.Bot.Types;


namespace WORLDGAMEDEVELOPMENT
{
    internal class UserMessageHandler : MessageHandler
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
        }

        #endregion


        #region MessageHandler

        public override bool CanHandle(long? userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}