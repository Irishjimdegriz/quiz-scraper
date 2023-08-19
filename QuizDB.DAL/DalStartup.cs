using Microsoft.Extensions.Configuration;

namespace QuizDB.DAL
{
    public class DalStartup
    {
        private readonly IConfiguration configuration;

        public DalStartup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetConnectionString()
        {
            return configuration.GetConnectionString("DefaultConnection");
        }
    }
}
