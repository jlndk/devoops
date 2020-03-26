using Newtonsoft.Json;

namespace MiniTwit.Web.App.Models.Api
{
    public class GetLatestDTO
    {
        public GetLatestDTO(int latest)
        {
            Latest = latest;
        }
        
        [JsonProperty("latest")]
        public int Latest { get; set; }
    }
}