namespace PostService.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        
        public int UserID { get; set; }
    }
}
