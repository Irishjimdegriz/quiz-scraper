using QuizDb.Module.Models;
using QuizDb.Module.Services.Interfaces;
using QuizDb.QuizDbModule;
using QuizDB.DAL.Entities;

namespace QuizDb.Module.Services
{
    public class QuizDbService: IQuizDbService
    {
        public QuizDbService() { }

        public async Task<List<TournamentModel>> GetTournaments(int pagesCount, string lastTournamentName)
        {
            var result = new List<TournamentModel>();

            try
            {
                for (var i = 0; i < pagesCount; i++)
                {
                    var htmlContent = await QuizDbGetter.GetTournamentList(i);
                    var loadedTournaments = QuizDbParser.GetTournaments(htmlContent);

                    result.AddRange(loadedTournaments);

                    if (loadedTournaments.Exists(x => x.Name == lastTournamentName))
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public async Task<TournamentQuestionsModel> GetQuestions(Tournament tournament)
        {
            var result = new TournamentQuestionsModel();

            try
            {
                var htmlContent = await QuizDbGetter.GetTournamentQuestions(tournament.Link);

                result = QuizDbParser.GetTournamentQuestions(htmlContent, tournament.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }
    }
}