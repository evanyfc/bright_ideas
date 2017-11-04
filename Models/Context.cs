using Microsoft.EntityFrameworkCore;
namespace bright_ideas.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options){}
        public DbSet<User> users { get;set; }
        public DbSet<Post> posts { get;set; }
        public DbSet<Like> likes { get;set; }
    }
}