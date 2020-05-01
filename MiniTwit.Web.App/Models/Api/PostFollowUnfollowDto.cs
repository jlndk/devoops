using System.Text.Json.Serialization;

namespace MiniTwit.Web.App.Models.Api
{
    public class PostFollowUnfollowDto : BasePostDto
    {
        [JsonPropertyName("follow")]   
        public string Follow { get; set; }
        [JsonPropertyName("unfollow")]
        public string UnFollow { get; set; }
    }
}