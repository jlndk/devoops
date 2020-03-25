using System;
using MiniTwit.Entities;
using MiniTwit.Utils.CustomJson;

namespace MiniTwit.Web.App.Models.Api
{
    public class GetMessageDTO
    {
        [JsonPropName("user")]
        public string User { get; set; }
        [JsonPropName("pub_date")]
        public DateTime PubDate { get; set; }
        [JsonPropName("content")]
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