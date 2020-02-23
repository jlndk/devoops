using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace MiniTwit.FlagTool
{
    public class Program
    {
        public static readonly string DocString =
@"ITU-MiniTwit Tweet Flagging Tool
Usage:
    flag_tool <tweet_id>...
    flag_tool -i
    flag_tool -h
Options:
    -h            Show this screen.
    -i            Dump all tweets and authors to STDOUT.";

        private string _connectionString;

        public string ConnectionString
        {
            get
            {
                if (_connectionString != null)
                {
                    return _connectionString;
                }

                var stringBuilder = new SQLiteConnectionStringBuilder
                {
                    DataSource = "/tmp/MiniTwit.db"
                };
                _connectionString = stringBuilder.ToString();

                return _connectionString;
            }
            set => _connectionString = value;
        }

        public void Run(string[] args)
        {
            var cmd = args.ElementAtOrDefault(0) ?? "";

            switch (cmd)
            {
                case "-i":
                    PrintAllMessages();
                    break;
                case "-h":
                    PrintHelp();
                    break;
                default:
                    if (cmd == "")
                        PrintHelp();
                    else
                        FlagMessage(int.Parse(cmd));
                    break;
            }
        }

        private void FlagMessage(int messageId)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            const string sql = @"UPDATE message SET flagged=1 WHERE message_id=@messageId";
            var cmd = new SQLiteCommand(sql, connection);
            var idParam = new SQLiteParameter("@messageId", DbType.Int32)
            {
                Value = messageId
            };
            cmd.Parameters.Add(idParam);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"Flagged entry: {messageId}");
        }

        private static void PrintHelp()
        {
            Console.WriteLine(DocString);
        }

        private void PrintAllMessages()
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            const string sql = @"SELECT message_id, author_id, text, flagged FROM message;";
            var cmd = new SQLiteCommand(sql, connection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader["message_id"]},{reader["author_id"]},{reader["text"]},{reader["flagged"]}");
            }
        }

        public static void Main(string[] args)
        {
            var p = new Program();
            p.Run(args);
        }
    }
}