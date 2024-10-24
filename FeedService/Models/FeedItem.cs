namespace FeedService.Models
{
    public class FeedItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Content { get; set; }


    }
}
