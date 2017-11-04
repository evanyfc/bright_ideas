using System.ComponentModel.DataAnnotations;

namespace bright_ideas.Models
{
    public class UserViewModel : BaseEntity
    {
        [Required(ErrorMessage = "Please enter your name!")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name should be letters only!")]
        [MinLength(2, ErrorMessage = "Too short! Name should be at least 2 letters!")]
        public string name { get; set; }

        [Required(ErrorMessage = "Please enter your alias!")]
        [RegularExpression(@"^[\w\s]+$", ErrorMessage = "Alias is invalid!")]
        [MinLength(4, ErrorMessage = "Too short! Alias should be greater than 3!")]
        [MaxLength(19, ErrorMessage = "Too long! Alias should be less than 20!")]
        public string alias { get; set; }

        [Required(ErrorMessage = "Please enter your email!")]
        [RegularExpression(@"^[a-zA-Z0-9\.\+_-]+@[a-zA-Z0-9\._-]+\.[a-zA-Z]+$", ErrorMessage="Please enter a valid email!")]
        public string email { get; set; }

        [Required(ErrorMessage = "Please create a new password!")]
        [MinLength(8, ErrorMessage = "Too short! Passwords must be at least 8 characters.")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Compare("password", ErrorMessage = "The passwords entered don't match!")]
        [DataType(DataType.Password)]
        public string confirm { get; set; }
    }
}