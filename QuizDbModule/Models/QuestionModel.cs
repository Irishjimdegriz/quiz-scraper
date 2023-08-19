using QuizDb.Module.Extensions;
using QuizDB.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizDb.Module.Models
{
    public class QuestionModel
    {
        public string Extra { get; set; } = "";
        public string Text { get; set; }
        public string Answer { get; set; }
        public string Comment { get; set; }
        public string Sources { get; set; }
        public string Author { get; set; }
        public int TournamentId { get; set; }

        public Question ConvertToQuestion()
        {
            return new Question()
            {
                Extra = Extra.TrimWhitespace(),
                Text = Text.TrimWhitespace(),
                Answer = Answer.RemoveParagraphTitle().TrimWhitespace(),
                Comment = Comment.RemoveParagraphTitle().TrimWhitespace(),
                Source = Sources.RemoveParagraphTitle().TrimWhitespace(),
                Author = Author.RemoveParagraphTitle().TrimWhitespace(),
                TournamentId = TournamentId,
                CreateDate = DateTime.Now,
            };
        }
    }
}
