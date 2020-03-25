using MiniTwit.Entities;

namespace MiniTwit.Web.App.Models.Api
{
    public class PostRegisterDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Pwd { get; set; }
        public int? Latest { get; set; }

        public User ToUser()
        {
            return new User
            {
                UserName = Username,
                Email = Email
            };
        }
    }
}