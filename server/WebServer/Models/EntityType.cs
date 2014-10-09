namespace Server.Models
{
    public class EntityType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public static EntityType[] FindAll()
        {
            return Database.FindAllEntityTypes();
        }

        public static EntityType FindById(int id)
        {
            return Database.FindEntityTypeById(id);
        }
    }
}
