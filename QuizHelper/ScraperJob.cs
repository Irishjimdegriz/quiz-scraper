using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizDb.Scraper
{
    public class ScraperJob : IJob
    {
        private readonly ITournamentParser m_TournamentParser;

        public ScraperJob(ITournamentParser tournamentParser)
        {
            m_TournamentParser = tournamentParser;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await m_TournamentParser.LoadNewTournaments();
            await m_TournamentParser.LoadNewQuestions();

            Console.WriteLine("Finished loading tournaments and questions");
        }
    }
}
