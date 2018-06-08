namespace MyWebServer.GameStore.Data.Models
{
    public class OrderGame
    {
        public int GameId { get; set; }
        public Game Game { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}