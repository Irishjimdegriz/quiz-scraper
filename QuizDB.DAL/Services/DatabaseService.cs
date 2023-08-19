using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizDB.DAL.Entities;
using QuizDB.DAL.Services.Interfaces;

namespace QuizDB.DAL.Services
{
    public class DatabaseService: IDatabaseService
    {
        private readonly ApplicationDbContext m_DbContext;

        public DatabaseService(ApplicationDbContext dbContext)
        {
            this.m_DbContext = dbContext;
        }

        public async Task<string> GetLastTournamentName()
        {
            var lastTournament = await m_DbContext.Tournaments.OrderByDescending(x => x.CreateDate).FirstOrDefaultAsync();

            return lastTournament is null ? string.Empty : lastTournament.Name;
        }

        public async Task SaveNewTournaments(List<Tournament> tournaments)
        {
            m_DbContext.AddRange(tournaments);

            await m_DbContext.SaveChangesAsync();
        }

        public async Task<List<Tournament>> GetTournamentsWithoutQuestions()
        {
            return await m_DbContext.Tournaments.Where(x => !x.Loaded).ToListAsync();
        }

        public async Task SaveNewQuestions(List<Tournament> tournaments, List<Question> questions)
        {
            m_DbContext.UpdateRange(tournaments);
            m_DbContext.AddRange(questions);

            await m_DbContext.SaveChangesAsync();
        }
    }
}
