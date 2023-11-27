namespace CineLinkBE.Models
{
    public class Post
    {
        public int Id { get; set; } 
        public int UserId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Length { get; set; }
        public DateTime DatePosted { get; set; }
        public List<Genre> Genres { get; set; } 
        public List<User> Users { get; set; } 
    }
}
