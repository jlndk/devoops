using System;
using MiniTwit.Entities;

namespace MiniTwit.Web.App.Models.Api
{
    public class PostMessageDTO
    {
        public string Content { get; set; }
        public int? Latest { get; set; }
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