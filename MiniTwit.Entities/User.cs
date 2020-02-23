using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MiniTwit.Entities
{
    public class User : IdentityUser<int>
    {
        [Required] public override string UserName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(5)]
        public override string Email { get; set; }

        //can't remember how many-to-many relations are handled in EFC
    }
}