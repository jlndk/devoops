using System;
using System.ComponentModel.DataAnnotations;

namespace MiniTwit.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MinLength(5)]
        public string Email { get; set; }
        public string PwHash { get; set; }

        //can't remember how many-to-many relations are handled in EFC

    }
}