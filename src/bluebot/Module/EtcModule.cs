using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.IO;
namespace bluebot
{
    class EtcModule
    {
        [Command("도움말")]
        [Aliases("도와줘")]
        public async Task Help(CommandContext ctx) // this command takes no arguments
        {
            string str = File.ReadAllText(Path.Help);
            await ctx.RespondAsync($"```Markdown\n{str}```");
        }
        [Command("랜덤")]
        public async Task Random_(CommandContext ctx, params string[] vs) // this command takes no arguments
        {
            Random random = new Random();
            await ctx.RespondAsync(vs[random.Next(0, vs.Length)]);
        }
    }
}
