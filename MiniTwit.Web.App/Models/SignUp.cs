namespace WebPlayground.Models
{
    public class SignUp : Login
    {
        public string PasswordRepeat { get; set; }
        public string Email { get; set; }
    }
}