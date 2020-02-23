using System;
using MiniTwit.Entities;
using System.Threading.Tasks;

namespace MiniTwit.Models.Tests
{
    static class Utility
    {
        

        public static async Task Add_dummy_data(UserRepository userRepository, MessageRepository messageRepository)
        {
            var extrauser = new User();
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
                extrauser = user1;
                await userRepository.CreateAsync(user1);
                await messageRepository.CreateAsync(message1);
            }

            var extraMessage = new Message
            {
                Author = extrauser,
                PubDate = new DateTime(2018, 1, 1),
                Text = "waddup"
            };
            await messageRepository.CreateAsync(extraMessage);
            var extraMessage2 = new Message
            {
                Author = extrauser,
                PubDate = new DateTime(2018, 1, 2),
                Text = "waddup",
                Flagged = 1
            };
            await messageRepository.CreateAsync(extraMessage2);
        }
    }
}
