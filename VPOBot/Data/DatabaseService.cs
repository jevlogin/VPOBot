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
        private readonly string _connectionString;

        #endregion

        public DatabaseService(IConfigurationRoot configuration)
        {
            _connectionString = configuration.GetConnectionString(Configuration.DEFAULT_CONNECTION);
            _configuration = configuration;
            _optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
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
    }
}
