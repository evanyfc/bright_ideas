namespace bright_ideas.Models
{
    public class Post : BaseEntity
    {
        public int postId { get; set; }
        public int userId { get; set; }
        public User User { get; set; }
        public string content { get; set; }
        public int likecount { get; set; }
    }
}