
using DSharpPlus;
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;
using bluebot.Module;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;

namespace bluebot
{
    class Program
    {
        public static JObject rss = JObject.Parse(File.ReadAllText(Path.Info));
        public static JObject Setting = JObject.Parse(File.ReadAllText(Path.Setting));
        public static DiscordClient discord;
        static CommandsNextModule commands;
        static InteractivityModule interactivity;
        static void Main(string[] args)
        {
            Console.WriteLine("실행완료");
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
           
                var Settingjson = JsonConvert.DeserializeObject<utility.SettingJson>(File.ReadAllText(Path.Setting));
                discord = new DiscordClient(new DiscordConfiguration
                {
                    Token = Settingjson.Token,
                    TokenType = TokenType.Bot
                });
                commands = discord.UseCommandsNext(new CommandsNextConfiguration
                {
                    StringPrefix = Settingjson.Prefix
                });
                interactivity = discord.UseInteractivity(new InteractivityConfiguration
                {
                    PaginationBehaviour = TimeoutBehaviour.Ignore,
                    PaginationTimeout = TimeSpan.FromMinutes(5),
                    Timeout = TimeSpan.FromMinutes(2)
                });
                discord.MessageCreated += Command.BotCommand.MessageCreated;
                discord.MessageDeleted += Command.BotCommand.MessageDeleted;
                commands.RegisterCommands<BotModule>();
                commands.RegisterCommands<CommandModule>();
                commands.RegisterCommands<EtcModule>();
                commands.RegisterCommands<LoggerModule>();
                await discord.ConnectAsync();
                await Task.Delay(-1);
        }

    }
}

