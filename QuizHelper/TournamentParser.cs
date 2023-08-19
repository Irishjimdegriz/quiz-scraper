using QuizDb.Module;
using Microsoft.Extensions.DependencyInjection;
using QuizDB.DAL.Services;
using QuizDB.DAL.Services.Interfaces;
using QuizDb.Module.Services.Interfaces;
using QuizDB.DAL.Entities;
using QuizDb.Module.Models;

namespace QuizDb.Scraper
{
    public class TournamentParser: ITournamentParser
    {
        private const int DEFAULT_PAGES_COUNT = 2;

        private readonly IDatabaseService m_DatabaseService;
        private readonly IQuizDbService m_QuizDbService;

        public TournamentParser(IDatabaseService databaseService, IQuizDbService quizDbService)
        {
            m_DatabaseService = databaseService;
            m_QuizDbService = quizDbService;
        }

        public async Task LoadNewTournaments()
        {
            var lastTournamentName = await m_DatabaseService.GetLastTournamentName();

            var tournaments = await m_QuizDbService.GetTournaments(DEFAULT_PAGES_COUNT, lastTournamentName);

            var lastTournament = tournaments.Find(x => x.Name == lastTournamentName);

            if (lastTournament != null) 
            {
                var lastTournamentIndex = tournaments.IndexOf(lastTournament);
                tournaments.RemoveRange(lastTournamentIndex, tournaments.Count - lastTournamentIndex);
            }

            if (!tournaments.Any())
            {
                return;
            }

            tournaments.Reverse();
            var tournamentsToSave = tournaments.Select(x => x.ConvertToTournament()).ToList();

            await m_DatabaseService.SaveNewTournaments(tournamentsToSave);

            tournaments.ForEach(x => Console.WriteLine(x.ToString()));
        }

        public async Task LoadNewQuestions()
        {
            var tournamentsToHandle = await m_DatabaseService.GetTournamentsWithoutQuestions();

            var tournamentQuestions = new List<Question>();

            foreach (var tournament in tournamentsToHandle)
            {
                var tournamentQuestionModel = await m_QuizDbService.GetQuestions(tournament);
                
                if (tournamentQuestionModel is null)
                {
                    continue;
                }

                tournament.Loaded = true;

                tournamentQuestions.AddRange(tournamentQuestionModel.Questions.Select(x => x.ConvertToQuestion()));
            }

            await m_DatabaseService.SaveNewQuestions(tournamentsToHandle.Where(x => x.Loaded).ToList(), tournamentQuestions);
        }
    }
}
