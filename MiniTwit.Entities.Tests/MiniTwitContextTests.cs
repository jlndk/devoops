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
    }
}