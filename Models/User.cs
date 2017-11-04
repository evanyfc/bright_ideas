using System;
namespace bright_ideas.Models
{
    public class User : BaseEntity
    {
        public int userId { get; set; }
        public string name { get; set; }
        public string alias { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}