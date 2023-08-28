using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class DatabaseService
    {
        #region Fields

        private readonly string _connectionString;
        private readonly ApplicationDbContext _dbContext;

        #endregion

        public DatabaseService(IConfigurationRoot configuration)
        {
            _connectionString = configuration.GetConnectionString(Configuration.DEFAULT_CONNECTION);

            var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            _dbContext = new ApplicationDbContext(optionBuilder.Options, configuration);
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

    }
}
