using System.Text.Json.Serialization;

namespace MiniTwit.Web.App.Models.Api
{
    public class GetLatestDTO
    {
        public GetLatestDTO(int latest)
        {
            Latest = latest;
        }
        
        [JsonPropertyName("latest")]
        public int Latest { get; set; }
    }
}