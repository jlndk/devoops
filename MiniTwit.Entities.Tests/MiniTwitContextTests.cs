using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MiniTwit.Entities.Tests
{
    public class MiniTwitContextTests
    {
        [Fact]
        public void Can_Create_Context()
        {
            var context = new MiniTwitContext(new DbContextOptionsBuilder<MiniTwitContext>().Options);
            Assert.NotNull(context);
        }

        [Fact]
        public void Can_Read_Latest_In_Test_Context()
        {
            var options = new DbContextOptionsBuilder<MiniTwitContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            using(var context =  new MiniTwitContext(options))
            {
                context.Latest.Add(new Latest{Date = DateTime.Now, Value = 100});
                context.SaveChanges();
            }

            using(var context =  new MiniTwitContext(options))
            {
                Assert.Equal(100, context.Latest.Single().Value);
            }
        }
    }
}