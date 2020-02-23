using System;
using System.Data.SQLite;
using System.IO;
using System.Text;
using MiniTwit.FlagTool;
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

        private static string TestConnectionString
        {
            get
            {
                var stringBuilder = new SQLiteConnectionStringBuilder
                {
                    DataSource = "ProgramTestsInMemoryDb"
                };
                stringBuilder.Add("Cache", "Shared");
                stringBuilder.Add("Mode", "Memory");
                return stringBuilder.ToString();
            }
        }

        private void applySchema(SQLiteConnection connection)
        {
            const string sql = @"
                drop table if exists user;
                create table user (
                    user_id integer primary key autoincrement,
                    username string not null,
                    email string not null,
                    pw_hash string not null
                );

                drop table if exists follower;
                    create table follower (
                    who_id integer,
                    whom_id integer
                );

                drop table if exists message;
                create table message (
                    message_id integer primary key autoincrement,
                    author_id integer not null,
                    text string not null,
                    pub_date integer,
                    flagged integer
                );
            ";

            var command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
        }

        [Fact]
        public void Program_default_ConnectionString_is_correct()
        {
            var stringBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = "/tmp/MiniTwit.db"
            };
            var expected = stringBuilder.ToString();
            var program = new Program();
            Assert.Equal(expected, program.ConnectionString);
        }

        [Fact]
        public void Run_prints_all_tweets_and_authors_if_i_flag_is_supplied()
        {
            var connection = new SQLiteConnection(TestConnectionString);
            connection.Open();
            applySchema(connection);
            const string sql = @"
                INSERT INTO user (username, email, pw_hash)
                VALUES
                ('itu', 'itu@itu.dk', 'abc123');
                INSERT INTO message (author_id, text, flagged)
                VALUES
                (1, 'hello world', 0),
                (1, 'foobar', 0),
                (1, 'fricking heck', 1);
            ";
            var command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            var program = new Program
            {
                ConnectionString = TestConnectionString
            };
            var args = new[] {"-i"};
            program.Run(args);
            connection.Close();
            var actual = _output.ToString().Trim();
            var expected = new StringBuilder();
            expected.Append("1,1,hello world,0" + Environment.NewLine);
            expected.Append("2,1,foobar,0" + Environment.NewLine);
            expected.Append("3,1,fricking heck,1");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void Run_prints_flags_twit_if_id_is_supplied()
        {
            var connection = new SQLiteConnection(TestConnectionString);
            connection.Open();
            applySchema(connection);
            const string sql = @"
                INSERT INTO user (username, email, pw_hash)
                VALUES
                ('itu', 'itu@itu.dk', 'abc123');
                INSERT INTO message (author_id, text, flagged)
                VALUES
                (1, 'fricking heck', 0);
            ";
            var command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            var program = new Program
            {
                ConnectionString = TestConnectionString
            };
            var args = new[] {"1"};
            program.Run(args);
            
            //Assert output
            var actual = _output.ToString().Trim();
            Assert.Equal("Flagged entry: 1", actual);

            //Assert that the message has actually been flagged in db
            var assertSql = "SELECT COUNT(*) FROM message WHERE message_id = 1 AND flagged=1;";
            var assertCmd = new SQLiteCommand(assertSql, connection);
            var assertRes = (long) assertCmd.ExecuteScalar();
            Assert.True(assertRes == 1);

            connection.Close();
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