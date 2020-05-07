using System.Text.Json.Serialization;

namespace MiniTwit.Web.App.Models.Api
{
    public abstract class BasePostDto
    {
        [JsonPropertyName("latest")]
        public int? Latest { get; set; }
    }
}