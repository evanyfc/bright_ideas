using System;

namespace bright_ideas.Models
{
    public abstract class BaseEntity
    {
        public DateTime created_at { get; set; } = DateTime.Now;
        public DateTime updated_at { get; set; } = DateTime.Now;
    }
}