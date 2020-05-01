using System;
using System.Text.Json.Serialization;
using MiniTwit.Entities;

namespace MiniTwit.Web.App.Models.Api
{
    public class PostMessageDto : BasePostDto
    {
        [JsonPropertyName("content")]
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