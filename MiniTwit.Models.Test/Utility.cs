using System;
using System.Threading.Tasks;
using MiniTwit.Entities;

namespace MiniTwit.Models.Tests
{
    internal static class Utility
    {
        public static async Task Add_dummy_data(UserRepository userRepository, MessageRepository messageRepository)
        {
            var extraUser = new User();
            for (var i = 1; i < 10; i++)
            {
                var user1 = new User
                {
                    UserName = "user" + i,
                    Email = "user" + i + "@kanban.com"
                };
                var message1 = new Message
                {
                    Author = user1,
                    PubDate = new DateTime(2019, 1, i),
                    Text = "waddup" + i
                };
                extraUser = user1;
                await userRepository.CreateAsync(user1);
                await messageRepository.CreateAsync(message1);
            }

            var extraMessage = new Message
            {
                Author = extraUser,
                PubDate = new DateTime(2018, 1, 1),
                Text = "waddup"
            };
            await messageRepository.CreateAsync(extraMessage);
            var extraMessage2 = new Message
            {
                Author = extraUser,
                PubDate = new DateTime(2018, 1, 2),
                Text = "waddup",
                Flagged = 1
            };
            await messageRepository.CreateAsync(extraMessage2);
        }
    }
}