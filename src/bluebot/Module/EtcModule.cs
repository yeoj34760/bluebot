using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.IO;
using DSharpPlus.Interactivity;
using System.Timers;
using System.Threading;

namespace bluebot.Module
{
    class EtcModule
    {
        [Command("도움말")]
        [Aliases("도와줘")]
        public async Task Help(CommandContext ctx)
        {
            string str = File.ReadAllText(Path.Help);
            await ctx.RespondAsync($"```Markdown\n{str}```");
        }

        [Command("랜덤")]
        public async Task Random_(CommandContext ctx, params string[] vs)
        {
            Random random = new Random();
            await ctx.RespondAsync(vs[random.Next(0, vs.Length)]);
        }

        [Command("타이머")]
        public async Task Timer(CommandContext ctx, int Time) //테스트용
        {
            var mag = await ctx.RespondAsync(Time + "초 남았습니다.");
            int i = 1;
           while (true)
            {
                Thread.Sleep(1000);
                await mag.ModifyAsync($"{Time - i}초 남았습니다.");
                if (Time == i) break;
                i++;
            }
            await ctx.RespondAsync( ctx.User.Mention + " 타이머 종료됨!");
        }
    }
}
