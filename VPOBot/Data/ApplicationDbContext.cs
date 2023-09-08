using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;


namespace WORLDGAMEDEVELOPMENT
{
    internal class ApplicationDbContext : DbContext
    {
        #region Fields

        private readonly IConfigurationRoot _configuration;

        #endregion


        #region Properties

        public DbSet<UserVPO> Users { get; set; }
        public DbSet<UserAdmin> UsersAdmin { get; set; }
        public DbSet<ProgressUsers> ProgressUsers { get; set; }
        public DbSet<FoodDiaryEntry> FoodDiary { get; set; }
        public DbSet<UserBotSettings> UserBotSettings { get; set; }

        #endregion


        #region ClassLifeCycles

        public ApplicationDbContext(DbContextOptions options, IConfigurationRoot configuration) : base(options)
        {
            _configuration = configuration;
            if (Database.EnsureCreated())
            {
                Console.WriteLine("База данных была создана.");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString(Configuration.DEFAULT_CONNECTION);
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAdmin>(ConfigureAdmin);
            modelBuilder.Entity<ProgressUsers>(ConfigureProgress);
            modelBuilder.Entity<UserVPO>(ConfigureUsers);
            modelBuilder.Entity<FoodDiaryEntry>(ConfigureFoodDiary);
            modelBuilder.Entity<UserBotSettings>(ConfigureUserBotSettings);
        }

        #endregion


        #region Methods

        private void ConfigureUserBotSettings(EntityTypeBuilder<UserBotSettings> entity)
        {
            entity.ToTable(nameof(UserBotSettings));
            entity.HasKey(fd => fd.UserId);
        }

        private void ConfigureFoodDiary(EntityTypeBuilder<FoodDiaryEntry> entity)
        {
            entity.ToTable(nameof(FoodDiaryEntry));
            entity.HasKey(fd => fd.Id);
        }

        private void ConfigureAdmin(EntityTypeBuilder<UserAdmin> entity)
        {
            entity.ToTable(nameof(UserAdmin));
            entity.HasKey(u => u.UserId);
        }

        private void ConfigureUsers(EntityTypeBuilder<UserVPO> entity)
        {
            entity.ToTable(nameof(UserVPO));
            entity.HasKey(u => u.UserId);
        }

        private void ConfigureProgress(EntityTypeBuilder<ProgressUsers> entity)
        {
            entity.ToTable(nameof(ProgressUsers));
            entity.HasKey(u => u.UserId);
        }

        #endregion
    }
}
