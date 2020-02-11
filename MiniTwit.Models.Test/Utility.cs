using Microsoft.EntityFrameworkCore;
using MiniTwit.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MiniTwit.Models.Tests
{
    static class Utility
    {
        public static MiniTwitContext CreateMiniTwitContextContext([CallerMemberName] string testName = "", [CallerFilePath] string testNamePart2 = "")
        {
            var options = new DbContextOptionsBuilder<MiniTwitContext>()
                .UseInMemoryDatabase(databaseName: testName + testNamePart2)
                .Options;

            return new MiniTwitContext(options);
        }

        public static async Task Add_dummy_data(UserRepository repository)
        {
            for (int i = 1; i < 10; i++)
            {
                var user1 = new User
                {
                    Username = "user" + i,
                    Email = "user" + i + "@kanban.com"
                };
                var (_, _) = await repository.CreateAsync(user1);
            }
        }
    }
}
