using System;
using MiniTwit.Entities;
using Newtonsoft.Json;

namespace MiniTwit.Web.App.Models.Api
{
    public class PostMessageDTO : BasePostDTO
    {
        [JsonProperty("content")]
        public string Content { get; set; }
        
        public Message ToMessage(User user) {
            return new Message
            {
                Author = user,
                AuthorId = user.Id,
                Flagged = 0,
                PubDate = DateTime.Now,
                Text = Content
            };
        }
    }
}