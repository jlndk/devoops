using System.ComponentModel.DataAnnotations;

namespace MiniTwit.Web.App.Models
{
    public class Login
    {
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}