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
        public DbSet<FeedbackResponse> FeedbackResponses { get; set; }
        public DbSet<ResponseData> ResponseDatas { get; set; }
        public DbSet<QuestionAnswerPair> QuestionAnswerPairs { get; set; }

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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        #endregion
    }
}
