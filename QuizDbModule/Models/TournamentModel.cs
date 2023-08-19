using QuizDB.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizDb.Module.Models
{
    public class TournamentModel
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string PlayedDate { get; set; }
        public DateTime AddedDate { get; set; }

        public TournamentModel() { }
        public TournamentModel(string name, string url, string datePlayed, DateTime dateAdded) => (Name, Link, PlayedDate, AddedDate) = (name, url, datePlayed, dateAdded);

        public override string ToString()
        {
            return $"Name: {Name}, link: {Link}";
        }

        public Tournament ConvertToTournament()
        {
            if (DateTime.TryParseExact(PlayedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var tournamentDate))
            {
                PlayedDate = tournamentDate.ToString("dd.MM.yyyy");
            }

            return new Tournament()
            { 
                Name = Name,
                Link = Link,
                PlayedDate = PlayedDate,
                CreateDate = DateTime.Now,
                AddedDate = AddedDate,
            };
        }
    }
}
