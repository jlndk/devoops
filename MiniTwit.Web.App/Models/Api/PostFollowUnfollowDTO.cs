using System.Text.Json.Serialization;

namespace MiniTwit.Web.App.Models.Api
{
    public class PostFollowUnfollowDTO : BasePostDTO
    {
        [JsonPropertyName("follow")]   
        public string Follow { get; set; }
        [JsonPropertyName("unfollow")]
        public string UnFollow { get; set; }
    }
}