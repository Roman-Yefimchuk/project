namespace Server.Models
{
    public class EntityModel
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string EntityTypes { get; set; }

        public static EntityModel[] FindAll()
        {
            return Database.FindAllEntityModels();
        }

        public static EntityModel FindById(int id)
        {
            return Database.FindEntityModelById(id);
        }
    }
}
