namespace bright_ideas.Models
{
    public class Like : BaseEntity
    {
        public int likeId { get; set; }
        public int userId { get; set; }
        public User User { get; set; }
        public int postId { get; set; }
        public Post Post { get; set; }
    }
}