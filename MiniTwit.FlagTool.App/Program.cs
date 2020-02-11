using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace MiniTwit.FlagTool.App
{
    public class Program
    {
        public static readonly string docStr =
                "ITU-MiniTwit Tweet Flagging Tool\n\n" +
                "Usage:\n" +
                "  flag_tool <tweet_id>...\n" +
                "  flag_tool -i\n" +
                "  flag_tool -h\n" +
                "Options:\n" +
                "-h            Show this screen.\n" +
                "-i            Dump all tweets and authors to STDOUT.";

        private string _connectionString = null;
        public string ConnectionString {
            get {
                if(_connectionString == null) {
                    var b = new SQLiteConnectionStringBuilder();
                    b.DataSource = "/tmp/MiniTwit.db";
                    _connectionString = b.ToString();
                }

                return _connectionString;
            }
            set {
                _connectionString = value;
            }
        }

        public void Run(string[] args) {
            var cmd = args.ElementAtOrDefault(0) ?? "";

            switch(cmd) {
                case "-i":
                    printAllMessages();
                    break;
                case "-h":
                    printHelp();
                    break;
                default:
                    if(cmd == "") {
                        printHelp();
                    }
                    else {
                        flagMessage(int.Parse(cmd));
                    }
                    break;
            }
        }

        private void flagMessage(int messageId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var sql = "UPDATE message SET flagged=1 WHERE message_id=@messageId";

                var cmd = new SQLiteCommand(sql, connection);

                //Use prepared statement to prevent sql injection
                var idParam = new SQLiteParameter("@messageId", DbType.Int32);
                idParam.Value = messageId;
                cmd.Parameters.Add(idParam);

                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Flagged entry: {0}", messageId);
        }

        private void printHelp()
        {
            Console.WriteLine(docStr);
        }

        private void printAllMessages()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var sql = "SELECT message_id, author_id, text, flagged FROM message;";

                var cmd = new SQLiteCommand(sql, connection);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var messageId = (Int64) reader["message_id"];
                    var authorId = (Int64) reader["author_id"];
                    var text = reader["text"] as string;
                    var flagged = (Int64) reader["flagged"];

                    Console.WriteLine("{0},{1},{2},{3}", messageId, authorId, text, flagged);
                }
            }
        }

        public static void Main(string[] args)
        {
            var p = new Program();
            p.Run(args);
        }
    }
}
