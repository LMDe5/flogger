namespace HttpServer.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price  { get; set; }
        public string Rarity { get; set; }
        public Item() { }
    }
}