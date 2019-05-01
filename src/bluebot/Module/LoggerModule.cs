using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json.Linq;

namespace bluebot.Module
{
    class LoggerModule
    {
        [Group("로그")]
        [Hidden]
        [RequirePermissions(Permissions.ManageGuild)]
        public class Logger
        {
            [Command("채널")]
            public async Task Channel(CommandContext ctx)
            {
                utility.NewServer(ctx);
                JObject rss = JObject.Parse(File.ReadAllText(Path.Logger));
                rss[ctx.Guild.Id.ToString()]["Channel"] = ctx.Channel.Id;
                File.WriteAllText(Path.Logger, rss.ToString());
                await ctx.RespondAsync("이제 이 곳에서 로그가 남습니다.");
            }
            [Command("활성화")]
            public async Task On(CommandContext ctx)
            {
                utility.NewServer(ctx);
                JObject rss = JObject.Parse(File.ReadAllText(Path.Logger));
                if ((bool)rss[ctx.Guild.Id.ToString()]["Logger"] == false)
                {
                    rss[ctx.Guild.Id.ToString()]["Logger"] = true;
                    await ctx.RespondAsync("로그 활성화되었습니다.\n블루봇인해 발생하는 문제들은 책임 치지 않습니다.");
                }
                else
                {
                    rss[ctx.Guild.Id.ToString()]["Logger"] = false;
                    await ctx.RespondAsync("로그 비활성화되었습니다.");
                }
                File.WriteAllText(Path.Logger, rss.ToString());
            }
            [Command("정보")]
            public async Task Info(CommandContext ctx)
            {
                utility.NewServer(ctx);
                JObject rss = JObject.Parse(File.ReadAllText(Path.Logger));
                string Channel = (rss[ctx.Guild.Id.ToString()]["Channel"].ToString() == "") ? "없음" : Program.discord.GetChannelAsync((ulong)rss[ctx.Guild.Id.ToString()]["Channel"]).Result.Name;
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Logger 정보",
                    Description = $"활성화 상태 : {rss[ctx.Guild.Id.ToString()]["Logger"].ToString()}\n채널 : {Channel}"

                };
                await ctx.RespondAsync(null, false, embed);
            }

        }
    }

}

