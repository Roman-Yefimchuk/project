namespace Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public static User Find(string name, string password)
        {
            return Database.FindUser(name, password);
        }

        public static User Create(string name, string password)
        {
            return Database.CreateUser(name, password);
        }
    }
}
