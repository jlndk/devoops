using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MiniTwit.Web.App.Models.Api
{
    public class GetFollowsDto
    {
        public GetFollowsDto(IEnumerable<string> follows)
        {
            Follows = follows;
        }

        [JsonPropertyName("follows")]
        public IEnumerable<string> Follows { get; set; }
    }
}