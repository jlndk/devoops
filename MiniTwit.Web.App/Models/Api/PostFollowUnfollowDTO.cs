using Newtonsoft.Json;

namespace MiniTwit.Web.App.Models.Api
{
    public class PostFollowUnfollowDTO : BasePostDTO
    {
        [JsonProperty("follow")]   
        public string Follow { get; set; }
        [JsonProperty("unfollow")]
        public string UnFollow { get; set; }
    }
}