using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniTwit.Entities;
using Moq;
using Xunit;
using Xunit.Abstractions;
using static MiniTwit.Models.Tests.Utility;
using static MiniTwit.Models.Tests.MiniTwitTestContext;

namespace MiniTwit.Models.Tests
{
    public class MessageRepositoryTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly MiniTwitTestContext _context;
        private readonly UserRepository _userRepository;
        private readonly MessageRepository _messageRepository;
        private ILogger<UserRepository> _loggerUser;

        public MessageRepositoryTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _loggerUser = Mock.Of<ILogger<UserRepository>>();
            _context = CreateMiniTwitContext();
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _userRepository = new UserRepository(_context, _loggerUser);
            _messageRepository = new MessageRepository(_context);
        }

        [Fact]
        public async Task Message_Is_Created_Successfully()
        {
            await Add_dummy_data(_userRepository, _messageRepository);
            var (_, id) = await _userRepository.CreateAsync(new User() {UserName ="Test", Email="qwfqwf@qdqw.qwf"});

            var (response, messageId) = await _messageRepository.CreateAsync(new Message
            {
                Text ="qwdqg", AuthorId=id
            });

            Assert.Equal(Response.Created, response);
            // TODO: should probably find a way to figure out expected that doesn't break when dummy data is created.
            Assert.Equal(12, messageId);

        }

       [Fact]
        public async Task Message_without_author_fails()
        {
            await Assert.ThrowsAsync<DbUpdateException>(() => _messageRepository.CreateAsync(new Message {Text = "qwdqg"}));
        }

        [Fact]
        public async Task Read_messages_in_PubDate_order()
        {
            await Add_dummy_data(_userRepository, _messageRepository);
            var result = await _messageRepository.ReadManyAsync(100);
            DateTime prev = DateTime.MaxValue;
            foreach (var message in result)
            {
                Assert.True(DateTime.Compare(prev, message.PubDate) >= 0);
                _testOutputHelper.WriteLine(message.PubDate.ToString(CultureInfo.InvariantCulture));
                prev = message.PubDate;
            
            }
        }
        
        [Fact]
        public async Task ReadCount_messages_in_PubDate_order()
        {
            await Add_dummy_data(_userRepository, _messageRepository);
            var result = await _messageRepository.ReadManyAsync(12);
            DateTime prev = DateTime.MaxValue;
            foreach (var message in result)
            {
                Assert.True(DateTime.Compare(prev, message.PubDate)  >= 0);
                _testOutputHelper.WriteLine(message.PubDate.ToString(CultureInfo.InvariantCulture));
                prev = message.PubDate;
            }
        }
        
        [Fact]
        public async Task ReadAllMessagesFromUser_messages_in_PubDate_order()
        {
            await Add_dummy_data(_userRepository, _messageRepository);
            var result = await _messageRepository.ReadAllMessagesFromUserAsync(1);
            DateTime prev = DateTime.MinValue;
            foreach (var message in result)
            {
                Assert.True(DateTime.Compare(prev, message.PubDate) < 0);
                _testOutputHelper.WriteLine(message.PubDate.ToString(CultureInfo.InvariantCulture));
                prev = message.PubDate;
            }
        }
       
        [Fact]
        public async Task Read_messages_contains_no_flagged()
        {
            await Add_dummy_data(_userRepository, _messageRepository);
            var result = await _messageRepository.ReadManyAsync(100);
            foreach (var message in result)
            {
                Assert.True(message.Flagged <= 0);
            }
        }
        
        [Fact]
        public async Task ReadCount_messages_contains_no_flagged()
        {
            await Add_dummy_data(_userRepository, _messageRepository);
            var result = await _messageRepository.ReadManyAsync(12);
            foreach (var message in result)
            {
                Assert.True(message.Flagged <= 0);
            }
        }
        
        [Fact]
        public async Task ReadMessagesFromUser_messages_contains_no_flagged()
        {
            await Add_dummy_data(_userRepository, _messageRepository);
            var result = await _messageRepository.ReadAllMessagesFromUserAsync(1);
            foreach (var message in result)
            {
                Assert.True(message.Flagged <= 0);
            }
        }
        
        [Fact]
        public async Task No_Messages_If_Not_Following_Anyone()
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
            Assert.Null(followee.Follows);
            Assert.Empty((await messageRepo.ReadAllMessagesFromFollowedAsync(followeeReturnedId)));
        }
        
        [Fact]
        public async Task All_Messages_If_Following_Everyone()
        {
            var context = CreateMiniTwitContext();
            var userRepo = new UserRepository(context, _loggerUser);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(userRepo, messageRepo);
            var follower = new User
            {
                UserName = "Followee",
                Email = "qwdq@gqqqwdw.com"
            };
      
            var (_, followerReturnedId) = await userRepo.CreateAsync(follower);
            
            foreach (var user in context.Users.Where(user => user.Id != followerReturnedId))
            {
                await userRepo.AddFollowerAsync(followerReturnedId, user.Id);
            }
            Assert.Equal(9, follower.Follows.Count);
            Assert.Equal(11, (await messageRepo.ReadAllMessagesFromFollowedAsync(followerReturnedId)).Count());
        }
        
        [Fact]
        public async Task ReadManyWithinDate_Gives_Dates_Before()
        {
            var context = CreateMiniTwitContext();
            var userRepo = new UserRepository(context, _loggerUser);
            var messageRepo = new MessageRepository(context);
            var user = new User
            {
                UserName = "TimeTraveler",
                Email = "hej@hej"
            };
            await userRepo.CreateAsync(user);
            for (var i = 1; i < 25; i++)
            {
                DateTime dt = new DateTime(2020, 2, i);
                var message = new Message
                {
                    Author = user,
                    Flagged = 0,
                    PubDate = dt,
                    Text = $"Im great at this {i}th day in a row"
                };
                await messageRepo.CreateAsync(message);
            }
            var beforeDate = new DateTime(2020, 2, 10);
            var messages = await messageRepo.ReadManyWithinTimeAsync(100, beforeDate);
            foreach (var message in messages)
            {
                Assert.True(message.PubDate < beforeDate);
            }
        }
        
        [Fact]
        public async Task ReadManyWithinDate_Gives_Dates_After()
        {
            var context = CreateMiniTwitContext();
            var userRepo = new UserRepository(context, _loggerUser);
            var messageRepo = new MessageRepository(context);
            var user = new User
            {
                UserName = "TimeTraveler",
                Email = "hej@hej"
            };
            await userRepo.CreateAsync(user);
            for (var i = 1; i < 25; i++)
            {
                DateTime dt = new DateTime(2020, 2, i);
                var message = new Message
                {
                    Author = user,
                    Flagged = 0,
                    PubDate = dt,
                    Text = $"Im great at this {i}th day in a row"
                };
                await messageRepo.CreateAsync(message);
            }
            var afterDate = new DateTime(2020, 2, 10);
            var messages = await messageRepo.ReadManyWithinTimeAsync(100, null, afterDate);
            foreach (var message in messages)
            {
                Assert.True(message.PubDate > afterDate);
            }
        }
        
        [Fact]
        public async Task ReadManyFromUserWithinDate_Gives_Dates_Before()
        {
            var context = CreateMiniTwitContext();
            var userRepo = new UserRepository(context, _loggerUser);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(userRepo, messageRepo);
            var user = new User
            {
                UserName = "TimeTraveler",
                Email = "hej@hej"
            };
            var (_, userId) = await userRepo.CreateAsync(user);
            for (int i = 1; i < 25; i++)
            {
                DateTime dt = new DateTime(2020, 2, i);
                var message = new Message
                {
                    Author = user,
                    Flagged = 0,
                    PubDate = dt,
                    Text = $"Im great at this {i}th day in a row"
                };
                await messageRepo.CreateAsync(message);
            }

            DateTime beforeDate = new DateTime(2020, 2, 10);
            var messages = await messageRepo.ReadManyFromUserWithinTimeAsync(userId, 100, beforeDate);
            foreach (var message in messages)
            {
                Assert.True(message.PubDate < beforeDate);
            }
        }
        
        [Fact]
        public async Task ReadManyFromUserWithinDate_Gives_Dates_After()
        {
            var context = CreateMiniTwitContext();
            var userRepo = new UserRepository(context, _loggerUser);
            var messageRepo = new MessageRepository(context);
            await Add_dummy_data(userRepo, messageRepo);
            var user = new User
            {
                UserName = "TimeTraveler",
                Email = "hej@hej"
            };
            var (_, userId) = await userRepo.CreateAsync(user);
            for (int i = 1; i < 25; i++)
            {
                DateTime dt = new DateTime(2020, 2, i);
                var message = new Message
                {
                    Author = user,
                    Flagged = 0,
                    PubDate = dt,
                    Text = $"Im great at this {i}th day in a row"
                };
                await messageRepo.CreateAsync(message);
            }

            DateTime afterDate = new DateTime(2020, 2, 10);
            var messages = await messageRepo.ReadManyFromUserWithinTimeAsync(userId, 100, null, afterDate);
            foreach (var message in messages)
            {
                Assert.True(message.PubDate > afterDate);
            }
        }
    }
}
