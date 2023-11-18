namespace CineLinkBE.Models
{
    public class Review
    {
        public int Id { get; set; } 
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }
        public int Rating { get; set; }
    }
}
