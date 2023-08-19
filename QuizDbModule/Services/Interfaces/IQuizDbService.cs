using QuizDb.Module.Models;
using QuizDB.DAL.Entities;

namespace QuizDb.Module.Services.Interfaces
{
    public interface IQuizDbService
    {
        Task<List<TournamentModel>> GetTournaments(int pagesCount, string lastTournamentName);
        Task<TournamentQuestionsModel> GetQuestions(Tournament tournament);
    }
}
