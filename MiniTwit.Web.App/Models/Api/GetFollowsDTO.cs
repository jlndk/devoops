using System.Collections.Generic;
using Newtonsoft.Json;

namespace MiniTwit.Web.App.Models.Api
{
    public class GetFollowsDTO
    {
        public GetFollowsDTO(IEnumerable<string> follows)
        {
            Follows = follows;
        }

        [JsonProperty("follows")]
        public IEnumerable<string> Follows { get; set; }
    }
}