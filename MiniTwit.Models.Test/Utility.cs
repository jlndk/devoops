using System;
using MiniTwit.Entities;
using System.Threading.Tasks;

namespace MiniTwit.Models.Tests
{
    static class Utility
    {
        

        public static async Task Add_dummy_data(UserRepository userRepository, MessageRepository messageRepository)
        {
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
                    Pubdate = new DateTime(2019, 1, i),
                    Text = "waddup" + i
                };
                await userRepository.CreateAsync(user1);
                await messageRepository.CreateAsync(message1);
            }
        }
    }
}
