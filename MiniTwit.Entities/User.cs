using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MiniTwit.Entities
{
    public class User : IdentityUser<int>
    {
        [Required]
        public override string UserName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(5)]
        public override string Email { get; set; }
        
        public virtual ICollection<Follows> Follows { get; set;  }
        public virtual ICollection<Follows> FollowedBy { get; set;  }
    }
}