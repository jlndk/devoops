using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Entities;
using MiniTwit.FlagTool;
using MiniTwit.Models;
using Xunit;

namespace MiniTwit.FlagTool.Tests
{
    public class ProgramTests
    {
        public ProgramTests()
        {
            _output = new StringBuilder();
            Console.SetOut(new StringWriter(_output));
        }

        private readonly StringBuilder _output;

        [Fact]
        public async Task Run_prints_all_tweets_and_authors_if_i_flag_is_supplied()
        {
            var context = MiniTwitTestContext.CreateMiniTwitContext();
            context.Users.Add(new User
            {
                UserName = "itu",
                Email = "itu@itu.dk",
                PasswordHash = "abc123"
            });
            context.Messages.Add(new Message
            {
                AuthorId = 1,
                Text = "hello world",
                Flagged = 0,
            });
            context.Messages.Add(new Message
            {
                AuthorId = 1,
                Text = "foobar",
                Flagged = 0,
            });
            context.Messages.Add(new Message
            {
                AuthorId = 1,
                Text = "fricking heck",
                Flagged = 1,
            });
            context.SaveChanges();
            

            var p = new Program(context);
            await p.Run(new[] {"-i"});

            var actual = _output.ToString().Trim();
            var expected = new StringBuilder();
            expected.Append("1,1,hello world,0" + Environment.NewLine);
            expected.Append("2,1,foobar,0" + Environment.NewLine);
            expected.Append("3,1,fricking heck,1");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public async Task Run_prints_flags_twit_if_id_is_supplied()
        {
            var context = MiniTwitTestContext.CreateMiniTwitContext();
            
            context.Users.Add(new User
            {
                UserName = "itu",
                Email = "itu@itu.dk",
                PasswordHash = "abc123"
            });
            context.Messages.Add(new Message
            {
                AuthorId = 1,
                Text = "fricking heck",
                Flagged = 0,
            });
            context.SaveChanges();
            
            var p = new Program(context);
            await p.Run(new[] {"1"});
            
            //Assert output
            var actual = _output.ToString().Trim();
            Assert.Equal("Flagged post with id 1", actual);

            //Assert that the message has actually been flagged in db
            var messages = context.Messages.Where(m => m.Flagged == 1 && m.Id == 1);
            Assert.Equal(1, messages.Count());
        }

        [Fact]
        public void Run_prints_instructions_if_h_flag_is_supplied()
        {
            var args = new[] {"-h"};
            var program = new Program();
            program.Run(args);
            var actual = _output.ToString().Trim();
            Assert.Equal(Program.DocString, actual);
        }

        [Fact]
        public void Run_prints_instructions_if_no_args_supplied()
        {
            var args = new string[0];
            var program = new Program();
            program.Run(args);
            var actual = _output.ToString().Trim();
            Assert.Equal(Program.DocString, actual);
        }
    }
}