using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly MiniTwitTestContext _context;
        private readonly UserRepository _userRepository;
        private readonly MessageRepository _messageRepository;

        public MessageRepositoryTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _context = CreateMiniTwitContext();
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _userRepository = new UserRepository(_context);
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
            await Assert.ThrowsAsync<DbUpdateException>(() =>
            {
                return _messageRepository.CreateAsync(new Message {Text = "qwdqg"});
            });
        }

        [Fact]
        public async Task Read_messages_in_PubDate_order()
        {
            await Add_dummy_data(_userRepository, _messageRepository);
            var result = await _messageRepository.ReadAsync();
            DateTime prev = DateTime.MinValue;
            foreach (var message in result)
            {
                Assert.True(DateTime.Compare(prev, message.PubDate) < 0);
                _testOutputHelper.WriteLine(message.PubDate.ToString(CultureInfo.InvariantCulture));
                prev = message.PubDate;
            
            }
        }
        
        [Fact]
        public async Task ReadCount_messages_in_PubDate_order()
        {
            await Add_dummy_data(_userRepository, _messageRepository);
            var result = await _messageRepository.ReadCountAsync(12);
            DateTime prev = DateTime.MinValue;
            foreach (var message in result)
            {
                Assert.True(DateTime.Compare(prev, message.PubDate) < 0);
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
            var result = await _messageRepository.ReadAsync();
            foreach (var message in result)
            {
                Assert.True(message.Flagged <= 0);
            }
        }
        
        [Fact]
        public async Task ReadCount_messages_contains_no_flagged()
        {
            await Add_dummy_data(_userRepository, _messageRepository);
            var result = await _messageRepository.ReadCountAsync(12);
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
    }
}
