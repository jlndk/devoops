using System;
using MiniTwit.Entities;
using Newtonsoft.Json;

namespace MiniTwit.Web.App.Models.Api
{
    public class GetMessageDTO
    {
        [JsonProperty("user")]
        public string User { get; set; }
        [JsonProperty("pub_date")]
        public DateTime PubDate { get; set; }
        [JsonProperty("content")]
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