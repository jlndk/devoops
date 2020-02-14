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
    public class MiniTwitTestContext : MiniTwitContext
    {
        private readonly SqliteConnection _connection;
        public MiniTwitTestContext(DbContextOptions<MiniTwitContext> options, SqliteConnection connection) : base(options)
        {
            _connection = connection;
        }

        public static MiniTwitTestContext CreateMiniTwitContext([CallerMemberName] string testName = "", [CallerFilePath] string testNamePart2 = "")
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<MiniTwitContext>().UseSqlite(connection);
            var context = new MiniTwitTestContext(options.Options, connection);
            context.Database.EnsureCreated();
            return context;
        }

        public override void Dispose(){
            Dispose();
            if (_connection != null){
                _connection.Dispose();
            }
        }
    }
}
