
using DSharpPlus;
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using DSharpPlus.CommandsNext;

namespace bluebot
{
    class Program
    {
        public static JObject rss = JObject.Parse(File.ReadAllText(Path.Info));
        public static DiscordClient discord;
        static CommandsNextModule commands;
        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "",
                TokenType = TokenType.Bot
            });
            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = "블루야 "
            });

            discord.MessageCreated += Command.BotCommand.MessageCreated;
            discord.MessageDeleted += Command.BotCommand.MessageDeleted;
            commands.RegisterCommands<BotModule>();
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
