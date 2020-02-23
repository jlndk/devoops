using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Entities;
using Xunit;
using static MiniTwit.Models.Tests.Utility;
using static MiniTwit.Models.Tests.MiniTwitTestContext;

namespace MiniTwit.Models.Tests
{
    public class MessageRepositoryTests
    {

        

        [Fact]
        public async Task Message_Is_Created_Succesfully()
        {
            MiniTwitContext context = CreateMiniTwitContext();
            MessageRepository repo = new MessageRepository(context);
            var userRepo = new UserRepository(context);
            MessageRepository messageRepo = new MessageRepository(context);
            await Add_dummy_data(userRepo, messageRepo);
            (_, int id) = await userRepo.CreateAsync(new User() {UserName ="Test", Email="qwfqwf@qdqw.qwf"});

            var result = await repo.CreateAsync(new Message() { Text ="qwdqg", AuthorId=id});

            Assert.Equal(Response.Created, result.response);
            Assert.Equal(10, result.messageId); //todo should probably find a way to figure out expected that doesn't break when dummydata is created.

        }

       [Fact]
        public async Task Message_without_author_fails()
        {
            MiniTwitContext context = CreateMiniTwitContext();
            MessageRepository repo = new MessageRepository(context);

            await Assert.ThrowsAsync<DbUpdateException>(() => repo.CreateAsync(new Message() { Text ="qwdqg"}));

        }
       

    }
}
