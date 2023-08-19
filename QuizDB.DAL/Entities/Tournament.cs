namespace QuizDB.DAL.Entities
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public DateTime CreateDate{ get; set; }
        public string PlayedDate{ get; set; }
        public bool Loaded { get; set; }
        public DateTime AddedDate { get; set; }


        public List<Question> Questions { get; set; }
    }
}
