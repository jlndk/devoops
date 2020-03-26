using MiniTwit.Entities;
using Newtonsoft.Json;

namespace MiniTwit.Web.App.Models.Api
{
    public class PostRegisterDTO : BasePostDTO
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("pwd")]
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