using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using bluebot.Services;
using Newtonsoft.Json;

namespace bluebot.Modules
{
    public class BasicModule : ModuleBase
    {
        [Command("도움말")]
        public async Task PingAsync() => await ReplyAsync($"```Markdown\n{System.IO.File.ReadAllText("Help.md")}```");
        
    }
}
