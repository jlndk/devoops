using System;
using System.Text.Json.Serialization;
using MiniTwit.Entities;

namespace MiniTwit.Web.App.Models.Api
{
    public class GetMessageDTO
    {
        [JsonPropertyName("user")]
        public string User { get; set; }
        [JsonPropertyName("pub_date")]
        public DateTime PubDate { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        
        public static GetMessageDTO FromMessage(Message message)
        {
            return new GetMessageDTO
            {
                Content = message.Text,
                PubDate = message.PubDate,
                User = message.Author.UserName
            };
        }
    }
}