using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Entities;
using Xunit;
using Xunit.Abstractions;
using static MiniTwit.Models.Tests.Utility;
using static MiniTwit.Models.Tests.MiniTwitTestContext;

namespace MiniTwit.Models.Tests
{
    public class MessageRepositoryTests
    {
        public MessageRepositoryTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _context = CreateMiniTwitContext();
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _userRepository = new UserRepository(_context);
            _messageRepository = new MessageRepository(_context);
        }

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly MiniTwitTestContext _context;
        private readonly UserRepository _userRepository;
        private readonly MessageRepository _messageRepository;


        [Fact]
        public async Task Message_Is_Created_Successfully()
        {
            await Add_dummy_data(_userRepository, _messageRepository);
            var (_, id) = await _userRepository.CreateAsync(new User {
                UserName = "Test",
                Email = "qwfqwf@qdqw.qwf"
            });
            var (response, messageId) = await _messageRepository.CreateAsync(new Message
            {
                Text = "qwdqg",
                AuthorId = id
            });
            Assert.Equal(Response.Created, response);
            // TODO: Should probably find a way to figure out expected that doesn't break when dummy data is created.
            Assert.Equal(12, messageId);
        }

        [Fact]
        public async Task Message_without_author_fails()
        {
            await Assert.ThrowsAsync<DbUpdateException>(() =>
            {
                return _messageRepository.CreateAsync(new Message {Text = "qwdqg"});
            });
        }

        [Fact]
        public async Task Read_messages_contains_no_flagged()
        {
            await Add_dummy_data(_userRepository, _messageRepository);
            var result = await _messageRepository.ReadAsync();
            foreach (var message in result)
            {
                Assert.True(message.Flagged <= 0);
            }
        }

        [Fact]
        public async Task Read_messages_in_pubDate_order()
        {
            await Add_dummy_data(_userRepository, _messageRepository);
            var result = await _messageRepository.ReadAsync();
            var prev = DateTime.MinValue;
            foreach (var message in result)
            {
                Assert.True(DateTime.Compare(prev, message.PubDate) < 0);
                _testOutputHelper.WriteLine(message.PubDate.ToString(CultureInfo.InvariantCulture));
                prev = message.PubDate;
            }
        }
    }
}