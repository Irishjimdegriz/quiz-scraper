using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuizDB.DAL.Entities;

namespace QuizDB.DAL
{
    public class ApplicationDbContext: DbContext
    {
        private readonly IConfiguration configuration;

        public ApplicationDbContext(): base()
        {
            configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options) 
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var startup = new DalStartup(configuration);
                optionsBuilder.UseSqlServer(startup.GetConnectionString());
            }
        }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Question> Questions { get; set; }
    }
}
