using Telegram.Bot;
using Telegram.Bot.Polling;
using Microsoft.Extensions.Configuration;


namespace WORLDGAMEDEVELOPMENT
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var botKey = configuration[key: Configuration.BOT_KEY_NAME];
            await Console.Out.WriteLineAsync($"BotKey: {botKey}");

            var botClient = new TelegramBotClient(botKey);
            var databaseService = new DatabaseService(configuration);
            await databaseService.MigrateAsync();

            var adminList = await databaseService.LoadAdminListAsync();
            var userList = await databaseService.LoadUserListAsync();

            var adminMessageHandler = new AdminMessageHandler(botClient, databaseService, adminList, userList);
            var userMessageHandler = new UserMessageHandler(botClient, databaseService, adminList, userList);

            var updateDispatcher = new UpdateDispatcher();
            updateDispatcher.AddHandler(adminMessageHandler);
            updateDispatcher.AddHandler(userMessageHandler);

            using CancellationTokenSource cancellationToken = new CancellationTokenSource();

            ReceiverOptions receiverOptions = new ReceiverOptions();

            botClient.StartReceiving(
                updateDispatcher,
                receiverOptions: receiverOptions,
                cancellationToken: cancellationToken.Token);

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Начало работы бота {me.Username}");
            Console.ReadLine();
        }
    } 
}