using Microsoft.Extensions.DependencyInjection;
using QuizDB.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizDB.DAL.Services.Interfaces
{
    public interface IDatabaseService
    {
        Task<string> GetLastTournamentName();
        Task SaveNewTournaments(List<Tournament> tournaments);
        Task<List<Tournament>> GetTournamentsWithoutQuestions();
        Task SaveNewQuestions(List<Tournament> tournaments, List<Question> questions);
    }
}
