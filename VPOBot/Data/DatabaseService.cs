using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class DatabaseService
    {
        #region Fields

        private readonly DbContextOptionsBuilder<ApplicationDbContext> _optionBuilder;
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfigurationRoot _configuration;

        #endregion

        public DatabaseService(IConfigurationRoot configuration)
        {
            _optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            _configuration = configuration;

            _dbContext = new ApplicationDbContext(_optionBuilder.Options, configuration);
        }


        #region MigrateAsync

        public async Task MigrateAsync()
        {
            //Надо научиться понимать, были ли изменения. Если их не было, то и миграцию запускать не надо. а вообще, она может сама это делать...
            await _dbContext.Database.MigrateAsync();

            await AddFirstAdminAsync();
        }


        private async Task AddFirstAdminAsync()
        {
            var firstAdminId = Configuration.FIRST_ADMIN_ID;

            var firstAdmin = await _dbContext.UsersAdmin.FindAsync(firstAdminId);

            if (firstAdmin == null)
            {
                var newAdmin = new UserAdmin
                {
                    UserId = firstAdminId,
                    UserName = "VPO_Consultant"
                };

                _dbContext.UsersAdmin.Add(newAdmin);

                await _dbContext.SaveChangesAsync();
            }
        }

        #endregion


        #region LoadAdminListAsync

        internal async Task<Dictionary<long, string>> LoadAdminListAsync()
        {
            var adminList = await _dbContext.UsersAdmin.ToDictionaryAsync(admin => admin.UserId, admin => admin.UserName);
            return adminList;
        }

        #endregion


        #region LoadUserList

        internal async Task<Dictionary<long, UserVPO>> LoadUserListAsync()
        {
            var userList = await _dbContext.Users.ToDictionaryAsync(u => u.UserId, user => user);
            return userList;
        }

        #endregion


        #region AddAdminAsync

        internal async Task AddAdminAsync(UserAdmin admin)
        {
            var adminInServer = await _dbContext.UsersAdmin.FindAsync(admin.UserId);
            if (adminInServer == null)
            {
                await _dbContext.UsersAdmin.AddAsync(admin);
            }
            else
            {
                _dbContext.Entry(adminInServer).CurrentValues.SetValues(admin);
            }

            await _dbContext.SaveChangesAsync();
        }

        #endregion


        #region LoadProgressUsers

        internal async Task<Dictionary<long, ProgressUsers>> LoadProgressUsersAsync()
        {
            await Console.Out.WriteLineAsync($"Загрузка прогресса пользователей.");
            var progressList = await _dbContext.ProgressUsers.ToDictionaryAsync(progress => progress.UserId, progress => progress);
            return progressList;
        }

        #endregion


        #region UpdateProgressAsync

        internal async Task<bool> UpdateUserProgressAsync(ProgressUsers progress)
        {
            using (var dbContext = new ApplicationDbContext(_optionBuilder.Options, _configuration))
            {
                var existProgress = await dbContext.ProgressUsers.FindAsync(progress.UserId);

                if (existProgress == null)
                {
                    await dbContext.ProgressUsers.AddAsync(progress);
                    await Console.Out.WriteLineAsync($"Был добавлен прогресс пользователя - {DialogData.SUCCESS}");
                }
                else
                {
                    dbContext.Entry(existProgress).CurrentValues.SetValues(progress);
                }

                await dbContext.SaveChangesAsync();
            }
            return true;
        }

        #endregion


        #region AddUserAsync

        internal async Task AddUserAsync(UserVPO user)
        {
            var existingUsers = await _dbContext.Users.FindAsync(user.UserId);
            if (existingUsers == null)
            {
                await _dbContext.Users.AddAsync(user);
                await Console.Out.WriteLineAsync($"Был добавлен новый пользователь в базу даннных");
            }
            else
            {
                _dbContext.Entry(existingUsers).CurrentValues.SetValues(user);
            }

            await _dbContext.SaveChangesAsync();
        }

        #endregion


        #region AddOrUpdateFoodDiaryAsync

        internal async Task AddOrUpdateFoodDiaryAsync(FoodDiaryEntry foodDiary)
        {
            var existingFoodDiary = await _dbContext.FoodDiary.SingleOrDefaultAsync(x => x.Id == foodDiary.Id && x.UserId == foodDiary.UserId);
            if (existingFoodDiary == null)
            {
                await _dbContext.FoodDiary.AddAsync(foodDiary);
                await Console.Out.WriteLineAsync($"Была добавлена новая запис в дневник питания, в базу данных");
            }
            else
            {
                _dbContext.Entry(existingFoodDiary).CurrentValues.SetValues(foodDiary);
            }

            await _dbContext.SaveChangesAsync();
        }

        internal async Task<List<FoodDiaryEntry>> ReadTheFoodDiaryForTheCurrentDay(long id, CancellationToken cancellationToken)
        {
            var foodDiaryEntries = _dbContext.FoodDiary.Where(entry => entry.UserId == id && entry.Date == DateTime.Now.Date).ToList();
            return foodDiaryEntries;
        }

        #endregion


        #region AddOrUpdateBotSettingsAsync

        internal async Task AddOrUpdateBotSettingsAsync(UserBotSettings userBotSettings)
        {
            var existingSettings = await _dbContext.UserBotSettings.SingleOrDefaultAsync(botConfig => botConfig.UserId == userBotSettings.UserId);
            if (existingSettings == null)
            {
                await _dbContext.UserBotSettings.AddAsync(userBotSettings);
                await Console.Out.WriteLineAsync($"Была добавлена новая запис в дневник питания, в базу данных");
            }
            else
            {
                _dbContext.Entry(existingSettings).CurrentValues.SetValues(userBotSettings);
            }

            await _dbContext.SaveChangesAsync();
        }

        internal async Task<EmptyBotSettings> ReadUserBotSettings(long id, CancellationToken cancellationToken)
        {
            var userBotSettings = await _dbContext.UserBotSettings.FindAsync(id);
            if (userBotSettings == null)
            {
                return new EmptyBotSettings();
            }
            return userBotSettings;
        }
        #endregion
    }
}
