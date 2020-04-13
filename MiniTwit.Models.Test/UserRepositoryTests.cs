using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniTwit.Entities;
using Moq;
using Xunit;
using Xunit.Abstractions;
using static MiniTwit.Models.Tests.Utility;
using static MiniTwit.Models.Tests.MiniTwitTestContext;
// ReSharper disable IdentifierTypo

namespace MiniTwit.Models.Tests
{
    public class UserRepositoryTests
    {
        private ITestOutputHelper _testOutputHelper;
        private ILogger<UserRepository> _loggerUser;

        public UserRepositoryTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _loggerUser = Mock.Of<ILogger<UserRepository>>();
           
        }
        [Fact]
        public async Task CreateAsync_creates_user_with_properties()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
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
            var repo = new UserRepository(context, _loggerUser);
            var user1 = new User
            {
                UserName = "user1",
                Email = "user1@kanban.com"
            };
            await repo.CreateAsync(user1);
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
            var repo = new UserRepository(context, _loggerUser);
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
            var repo = new UserRepository(context, _loggerUser);
            var user = await repo.ReadAsync(2);
            Assert.Null(user);
        }

        [Fact]
        public async Task ReadAsync_given_non_existing_user_returns_Null()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            var user = await repo.ReadAsync(42);
            Assert.Null(user);
        }

        [Fact]
        public async Task ReadAsync_maps_user_properties()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(repo, messageRepo);
            var users = await repo.ReadManyAsync();
            var user = users.First();
            Assert.Equal(1, user.Id);
            Assert.Equal("user1", user.UserName);
            Assert.Equal("user1@kanban.com", user.Email);
        }

        [Fact]
        public async Task ReadAsync_returns_all_sorted_by_name()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(repo, messageRepo);
            var users = await repo.ReadManyAsync();
            var usernames = users
                .Where(u => u.Id < 4)
                .Select(u => u.UserName);
            Assert.Equal(new[] {"user1", "user2", "user3"}, usernames);
        }

        [Fact]
        public async Task ReadAsync_returns_all_users()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(repo, messageRepo);
            var users = await repo.ReadManyAsync();
            Assert.Equal(9, users.Count());
        }


        [Fact]
        public async Task Update_given_non_existing_user_returns_NotFound()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            var user = new User {Id = 42};
            var response = await repo.UpdateAsync(user);
            Assert.Equal(Response.NotFound, response);
        }

        [Fact]
        public async Task Update_given_user_with_another_users_emailAddress_returns_Conflict()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(repo, messageRepo);
            var user = new User
            {
                Id = 1,
                UserName = "user120",
                Email = "user2@kanban.com"
            };
            var response = await repo.UpdateAsync(user);
            Assert.Equal(Response.Conflict, response);
        }

        [Fact]
        public async Task Update_returns_Updated()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(repo, messageRepo);
            var user = new User
            {
                Id = 2,
                UserName = "newuser2",
                Email = "newuser2@kanban.com"
            };
            var response = await repo.UpdateAsync(user);
            Assert.Equal(Response.Updated, response);
        }

        [Fact]
        public async Task Update_updates_user()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(repo, messageRepo);
            var user = new User
            {
                Id = 2,
                UserName = "newuser2",
                Email = "newuser2@kanban.com"
            };

            await repo.UpdateAsync(user);
            var entity = await context.Users.FindAsync(2);
            Assert.Equal("newuser2", entity.UserName);
            Assert.Equal("newuser2@kanban.com", entity.Email);
        }


        [Fact]
        public async Task User_Is_Created_Successfully()
        {
            var context = CreateMiniTwitContext();
            var userRepo = new UserRepository(context, _loggerUser);
            var (response, userId) = await userRepo.CreateAsync(new User
            {
                UserName = "TestCreate",
                Email = "qwdq@gqqw.com"
            });
            Assert.Equal(Response.Created, response);
            Assert.Equal(1, userId);
        }

        [Fact]
        public async Task User_without_mails_fails()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            await Assert.ThrowsAsync<DbUpdateException>(() =>
            {
                return repo.CreateAsync(new User {UserName = "TestCreate"});
            });
        }
        
        [Fact]
        public async Task Can_Follow()
        {
            var context = CreateMiniTwitContext();
            var userRepo = new UserRepository(context, _loggerUser);
            var follower = new User
            {
                UserName = "Follower",
                Email = "qwdq@gqqw.com"
            };
            var (_, followerReturnedId) = await userRepo.CreateAsync(follower);
            var followee = new User
            {
                UserName = "Followee",
                Email = "qwdq@gqqqwdw.com"
            };
            var (_, followeeReturnedId) = await userRepo.CreateAsync(followee);

            await userRepo.AddFollowerAsync(followerId: followerReturnedId, followeeId: followeeReturnedId);
            
            Assert.Contains(followee.FollowedBy, (f => f.FollowerId == followerReturnedId));
            Assert.Contains(follower.Follows, (f => f.FolloweeId == followeeReturnedId));
        }
        
        [Fact]
        public async Task Can_follow_multiple()
        {
            var context = CreateMiniTwitContext();
            var userRepo = new UserRepository(context, _loggerUser);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(userRepo, messageRepo);
            var followee = new User
            {
                UserName = "Followee",
                Email = "qwdq@gqqqwdw.com"
            };
      
            var (_, followeeReturnedId) = await userRepo.CreateAsync(followee);
            
            await foreach (var user in context.Users)
            {
                if (user.Id == followeeReturnedId) 
                    continue;
                await userRepo.AddFollowerAsync(followerId: user.Id, followeeId: followeeReturnedId);
            }
            
            
            Assert.Equal(9, followee.FollowedBy.Count());
            Assert.True(followee.FollowedBy.All(f => f.Follower.Follows.Any(ff => ff.FolloweeId == followeeReturnedId)));
        }
        
        [Fact]
        public async Task Can_check_follow()
        {
            var context = CreateMiniTwitContext();
            var userRepo = new UserRepository(context, _loggerUser);
            var follower = new User
            {
                UserName = "Follower",
                Email = "qwdq@gqqw.com"
            };
            var (_, followerReturnedId) = await userRepo.CreateAsync(follower);
            var followee = new User
            {
                UserName = "Followee",
                Email = "qwdq@gqqqwdw.com"
            };
            var (_, followeeReturnedId) = await userRepo.CreateAsync(followee);

            await userRepo.AddFollowerAsync(followerId: followerReturnedId, followeeId: followeeReturnedId);

            var actual = await userRepo.IsUserFollowing(followerReturnedId, followeeReturnedId);
            Assert.True(actual);
        }
        
        [Fact]
        public async Task Can_delete()
        {
            var context = CreateMiniTwitContext();
            var userRepo = new UserRepository(context, _loggerUser);
            var follower = new User
            {
                UserName = "Follower",
                Email = "qwdq@gqqw.com"
            };
            var (_, followerReturnedId) = await userRepo.CreateAsync(follower);

            var response = await userRepo.DeleteAsync(followerReturnedId);
            
            var actual = await context.Users.FindAsync(followerReturnedId);
            Assert.Equal(Response.Deleted, response);
            Assert.Null(actual);
        }
        
        [Fact]
        public async Task Delete_given_nonexistant_returns_NotFound()
        {
            var context = CreateMiniTwitContext();
            var userRepo = new UserRepository(context, _loggerUser);
            var follower = new User
            {
                UserName = "Follower",
                Email = "qwdq@gqqw.com"
            };
            var (_, followerReturnedId) = await userRepo.CreateAsync(follower);
            
            await userRepo.DeleteAsync(followerReturnedId);
            var response = await userRepo.DeleteAsync(followerReturnedId);
            
            Assert.Equal(Response.NotFound, response);
        }
        
        [Fact]
        public async Task Can_delete_follow()
        {
            var context = CreateMiniTwitContext();
            var userRepo = new UserRepository(context, _loggerUser);
            var user = new User
            {
                UserName = "Follower",
                Email = "qwdq@gqqw.com"
            };
            var (_, userId) = await userRepo.CreateAsync(user);
            Assert.Contains(user, context.Users);
            await userRepo.DeleteAsync(userId);
            Assert.DoesNotContain(user, context.Users);
        }
        
        [Fact]
        public async Task GetFollows_returns_follows()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            var followee = new User
            {
                UserName = "Followee",
                Email = "qwdq@gqqqwdw.com"
            };
            var (_, followeeId) = await repo.CreateAsync(followee);
            
            var follower = new User
            {
                UserName = "Follower",
                Email = "qwd@gqqqwdw.com"
            };
            var (_, followerId) = await repo.CreateAsync(follower);
            await repo.AddFollowerAsync(followerId, followeeId);
            var actual = await repo.GetFollowsAsync(followerId);
            Assert.Contains(followee, actual);
        }
        
        [Fact]
        public async Task GetFollowedby_returns_followedby()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(repo, messageRepo);
            var followee = new User
            {
                UserName = "Followee",
                Email = "qwdq@gqqqwdw.com"
            };
      
            var (_, followeeReturnedId) = await repo.CreateAsync(followee);
            
            await foreach (var user in context.Users)
            {
                if (user.Id == followeeReturnedId) 
                    continue;
                await repo.AddFollowerAsync(followeeId: followeeReturnedId, followerId: user.Id);
            }
            var actual = await repo.GetFollowersAsync(followeeReturnedId);
            Assert.NotEmpty(actual);
        }
        
        [Fact]
        public async Task ReadAsyncByUsername_returns_user()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            var user = new User
            {
                UserName = "User",
                Email = "qwdq@gqqqwdw.com"
            };
            
            await repo.CreateAsync(user);
            
            var actual = await repo.ReadAsyncByUsername(user.UserName);
            Assert.Equal(user, actual);
        }
        
        [Fact]
        public async Task RemoveFollower_removes_followers()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            var followee = new User
            {
                UserName = "Followee",
                Email = "qwdq@gqqqwdw.com"
            };
            var (_, followeeId) = await repo.CreateAsync(followee);
            
            var follower = new User
            {
                UserName = "Follower",
                Email = "qwd@gqqqwdw.com"
            };
            var (_, followerId) = await repo.CreateAsync(follower);
            await repo.AddFollowerAsync(followerId, followeeId);
            await repo.RemoveFollowerAsync(followerId, followeeId);
            var actual = await repo.GetFollowsAsync(followerId);
            Assert.DoesNotContain(followee, actual);
        }
        
        [Fact]
        public async Task AddFollower_following_same_twice_returns_conflict()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            var followee = new User
            {
                UserName = "Followee",
                Email = "qwdq@gqqqwdw.com"
            };
            var (_, followeeId) = await repo.CreateAsync(followee);
            
            var follower = new User
            {
                UserName = "Follower",
                Email = "qwd@gqqqwdw.com"
            };
            var (_, followerId) = await repo.CreateAsync(follower);
            
            await repo.AddFollowerAsync(followeeId, followerId);
            var actual = await repo.AddFollowerAsync(followeeId, followerId);
            Assert.Equal(Response.Conflict, actual);
        }
        
        [Fact]
        public async Task AddFollower_given_1_follower_returns_conflict()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            var followee = new User
            {
                UserName = "Followee",
                Email = "qwdq@gqqqwdw.com"
            };
            var (_, followeeId) = await repo.CreateAsync(followee);
            
            var actual = await repo.AddFollowerAsync(followeeId, followeeId);
            Assert.Equal(Response.Conflict, actual);
        }
        
        [Fact]
        public async Task RemoveFollower_following_same_twice_returns_conflict()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            var followee = new User
            {
                UserName = "Followee",
                Email = "qwdq@gqqqwdw.com"
            };
            var (_, followeeId) = await repo.CreateAsync(followee);
            
            var follower = new User
            {
                UserName = "Follower",
                Email = "qwd@gqqqwdw.com"
            };
            var (_, followerId) = await repo.CreateAsync(follower);
            
            var actual = await repo.RemoveFollowerAsync(followeeId, followerId);
            Assert.Equal(Response.Conflict, actual);
        }
        
        [Fact]
        public async Task RemoveFollower_given_1_follower_returns_conflict()
        {
            var context = CreateMiniTwitContext();
            var repo = new UserRepository(context, _loggerUser);
            var followee = new User
            {
                UserName = "Followee",
                Email = "qwdq@gqqqwdw.com"
            };
            var (_, followeeId) = await repo.CreateAsync(followee);
            
            var actual = await repo.RemoveFollowerAsync(followeeId, followeeId);
            Assert.Equal(Response.Conflict, actual);
        }
    }
}