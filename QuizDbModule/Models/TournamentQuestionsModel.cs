using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizDb.Module.Models
{
    public class TournamentQuestionsModel
    {
        public List<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
        public DateTime PlayedDate { get; set; }
    }
}
