namespace QuizDB.DAL.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Extra { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
        public string Comment { get; set; }
        public string Author { get; set; }
        public string Source { get; set; }
        public QuestionType State { get; set; }

        public Tournament Tournament { get; set; }
    }
}
