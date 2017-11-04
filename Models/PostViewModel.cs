using System.ComponentModel.DataAnnotations;

namespace bright_ideas.Models
{
    public class PostViewModel : BaseEntity
    {
        
        [Required(ErrorMessage = "Please submit a post!")]
        public string content { get; set; }
    }
}