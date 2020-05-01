using Newtonsoft.Json;

namespace MiniTwit.Web.App.Models.Api
{
    public class GetLatestDto
    {
        public GetLatestDto(int latest)
        {
            Latest = latest;
        }

        [JsonProperty(PropertyName = "latest")]
        public int Latest { get; set; }
    }
}