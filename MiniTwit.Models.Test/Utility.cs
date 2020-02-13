using Microsoft.EntityFrameworkCore;
using MiniTwit.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace MiniTwit.Models.Tests
{
    static class Utility
    {
        public static MiniTwitContext CreateMiniTwitContextContext([CallerMemberName] string testName = "", [CallerFilePath] string testNamePart2 = "")
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<MiniTwitContext>().UseSqlite(connection);
            return new MiniTwitContext(options.Options);
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
