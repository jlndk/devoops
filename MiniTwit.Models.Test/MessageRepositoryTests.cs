using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Entities;
using Xunit;
using static MiniTwit.Models.Tests.Utility;


namespace MiniTwit.Models.Tests
{
    public class MessageRepositoryTests
    {

        

        [Fact]
        public async Task User_Is_Created_Succesfully()
        {
            MiniTwitContext context = CreateMiniTwitContextContext();
            MessageRepository repo = new MessageRepository(context);
            await Add_dummy_data(new UserRepository(context));

            var result = await repo.CreateAsync(new Message() { });

            Assert.Equal(Response.Created, result.response);
            Assert.Equal(1, result.messageId);

        }

       

    }
}
