namespace Server.Models
{
    public class Accessory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EntityTypes { get; set; }
        public string Ports { get; set; }

        public static Accessory[] FindAll()
        {
            return Database.FindAllEntityAccessories();
        }

        public static Accessory FindById(int id)
        {
            return Database.FindEntityAccessoryById(id);
        }
    }
}
