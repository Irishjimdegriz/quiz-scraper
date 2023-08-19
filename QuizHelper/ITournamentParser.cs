using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizDb.Scraper
{
    public interface ITournamentParser
    {
        Task LoadNewTournaments();
        Task LoadNewQuestions();
    }
}
