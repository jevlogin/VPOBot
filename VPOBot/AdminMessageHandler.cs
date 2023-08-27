using Telegram.Bot;


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
    }
}