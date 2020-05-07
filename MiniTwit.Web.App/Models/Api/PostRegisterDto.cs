using MiniTwit.Entities;
using System.Text.Json.Serialization;

namespace MiniTwit.Web.App.Models.Api
{
    public class PostRegisterDto : BasePostDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("pwd")]
        public string Password { get; set; }

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