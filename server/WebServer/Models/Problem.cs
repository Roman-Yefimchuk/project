namespace Server.Models
{
    public class Problem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string EntityTypes { get; set; }

        public static Problem[] FindAll()
        {
            return Database.FindAllProblems();
        }

        public static Problem FindById(int id)
        {
            return Database.FindProblemById(id);
        }
    }
}
