using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MiniTwit.Web.App.Models.Api
{
    public class GetFollowsDTO
    {
        public GetFollowsDTO(IEnumerable<string> follows)
        {
            Follows = follows;
        }

        [JsonPropertyName("follows")]
        public IEnumerable<string> Follows { get; set; }
    }
}