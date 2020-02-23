using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Entities;
using Xunit;
using static MiniTwit.Models.Tests.Utility;
using static MiniTwit.Models.Tests.MiniTwitTestContext;

namespace MiniTwit.Models.Tests
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task CreateAsync_creates_user_with_properties()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context);

            var user = new User
            {
                UserName = "user4",
                Email = "user4@kanban.com"
            };

            var (_, id) = await repo.CreateAsync(user);

            var entity = await context.Users.FindAsync(id);

            Assert.Equal("user4", entity.UserName);
            Assert.Equal("user4@kanban.com", entity.Email);
        }

        [Fact]
        public async Task CreateAsync_given_existing_user_returns_Conflict_and_id_0()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context);

            var user1 = new User
            {
                UserName = "user1",
                Email = "user1@kanban.com"
            };
            var (_, _) = await repo.CreateAsync(user1);

            var user = new User
            {
                UserName = "user1",
                Email = "user1@kanban.com"
            };

            var (response, id) = await repo.CreateAsync(user);

            Assert.Equal(Response.Conflict, response);
            Assert.Equal(0, id);
        }

        [Fact]
        public async Task CreateAsync_returns_Created_and_id()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(repo, messageRepo);


            var user = new User
            {
                UserName = "user11",
                Email = "user11@kanban.com"
            };

            var (response, id) = await repo.CreateAsync(user);

            Assert.Equal(Response.Created, response);
            Assert.Equal(10, id);
        }

        [Fact]
        public async Task ReadAsync_given_non_existing_user_returns_null()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context);
            var user = await repo.ReadAsync(2);

            Assert.Null(user);
        }

        [Fact]
        public async Task ReadAsync_given_non_existing_user_returns_Null()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context);
            var user = await repo.ReadAsync(42);

            Assert.Null(user);
        }

        [Fact]
        public async Task ReadAsync_maps_user_properties()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(repo, messageRepo);

            var users = await repo.ReadAsync();

            var user = users.First();

            Assert.Equal(1, user.Id);
            Assert.Equal("user1", user.UserName);
            Assert.Equal("user1@kanban.com", user.Email);
        }

        [Fact]
        public async Task ReadAsync_returns_all_sorted_by_name()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(repo, messageRepo);

            var users = await repo.ReadAsync();

            Assert.Equal(new[] {"user1", "user2", "user3"}, users.Where(u => u.Id < 4).Select(u => u.UserName));
        }

        [Fact]
        public async Task ReadAsync_returns_all_users()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(repo, messageRepo);

            var users = await repo.ReadAsync();

            Assert.Equal(9, users.Count());
        }


        [Fact]
        public async Task Update_given_non_existing_user_returns_NotFound()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context);
            var user = new User {Id = 42};

            var response = await repo.UpdateAsync(user);

            Assert.Equal(Response.NotFound, response);
        }

        [Fact]
        public async Task Update_given_user_with_another_users_emailAddress_returns_Conflict()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(repo, messageRepo);

            var user = new User {Id = 1, UserName = "user120", Email = "user2@kanban.com"};

            var response = await repo.UpdateAsync(user);

            Assert.Equal(Response.Conflict, response);
        }

        [Fact]
        public async Task Update_returns_Updated()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(repo, messageRepo);

            var user = new User {Id = 2, UserName = "newuser2", Email = "newuser2@kanban.com"};

            var response = await repo.UpdateAsync(user);

            Assert.Equal(Response.Updated, response);
        }

        [Fact]
        public async Task Update_updates_user()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(repo, messageRepo);

            var user = new User {Id = 2, UserName = "newuser2", Email = "newuser2@kanban.com"};

            await repo.UpdateAsync(user);

            var entity = await context.Users.FindAsync(2);

            Assert.Equal("newuser2", entity.UserName);
            Assert.Equal("newuser2@kanban.com", entity.Email);
        }


        [Fact]
        public async Task User_Is_Created_Succesfully()
        {
            var context = CreateMiniTwitContext();
            var userRepo = new UserRepository(context);

            var result = await userRepo.CreateAsync(new User {UserName = "TestCreate", Email = "qwdq@gqqw.com"});

            Assert.Equal(Response.Created, result.response);
            Assert.Equal(1, result.userId);
        }

        [Fact]
        public async Task User_without_mails_fails()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context);

            await Assert.ThrowsAsync<DbUpdateException>(() => repo.CreateAsync(new User {UserName = "TestCreate"}));
        }
    }
}