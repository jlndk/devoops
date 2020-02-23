using System;
using System.ComponentModel.DataAnnotations;

namespace MiniTwit.Entities
{
    public class Message
    {
        public int Id { get; set;}
        public int AuthorId { get; set;}
        [Required]
        public User Author { get; set;}
        [Required]
        public string Text { get; set;} 
        public DateTime Pubdate { get; set; }

        public int Flagged { get; set;}
        
    }
}