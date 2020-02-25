using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using MiniTwit.Entities;
using MiniTwit.Models;

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

        private readonly IMessageRepository _messageRepo;
        
        public Program(IMiniTwitContext context = null)
        {
            _messageRepo = new MessageRepository(context ?? new MiniTwitContext());
        }

        public async Task Run(string[] args)
        {
            var cmd = args.ElementAtOrDefault(0) ?? "";
            switch (cmd)
            {
                case "-i":
                    await PrintAllMessages();
                    break;
                case "-h":
                    PrintHelp();
                    break;
                default:
                    if (cmd == "")
                        PrintHelp();
                    else
                        await FlagMessage(int.Parse(cmd));
                    break;
            }
        }

        private async Task FlagMessage(int messageId)
        {
            var message = await _messageRepo.ReadAsync(messageId);
            if (message == null)
            {
                Console.WriteLine($"No post with id {messageId}");
                return;
            }
            message.Flagged = 1;
            var response = await _messageRepo.UpdateAsync(message);
            if (response != Response.Updated)
            {
                Console.WriteLine($"Message wasn't updated, got response: {response}");
                return;
            }
            Console.WriteLine($"Flagged post with id {messageId}");
        }

        private static void PrintHelp()
        {
            Console.WriteLine(DocString);
        }

        private async Task PrintAllMessages()
        {
            var messages = await _messageRepo.ReadAsync(true);
            foreach (var message in messages)
            {
                Console.WriteLine($"{message.Id},{message.AuthorId},{message.Text},{message.Flagged}");
            }
        }

        public static async Task Main(string[] args)
        {
            var p = new Program();
            await p.Run(args);
        }
    }
}