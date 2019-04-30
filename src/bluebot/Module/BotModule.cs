
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using DSharpPlus.Interactivity;
namespace bluebot.Module
{
    class BotModule : Program
    {

        [Group("봇")]
        public class Bot
        {
            [Command("생성")]
            public async Task BotAdd(CommandContext ctx, string Botname, string Botnick) // this command takes no arguments
            {
                utility.NewUser(ctx);
                if (Botnick == "블루야") await ctx.RespondAsync($"'블루야'는 시스템상 설정불가능합니다.");
                else
                {
                    JObject _rss = rss["User"][ctx.User.Id.ToString()]["CustomBot"] as JObject; //피싱
                    _rss.Add(Botname, new JObject(
                        new JProperty("Nickname", new JArray(Botnick)),
                        new JProperty("commands", new JObject())));
                    await ctx.RespondAsync($"'{ctx.User.Username}님의 {Botname}'봇 생성완료"); //성공했다면 유저에게 성공했다고 전달함
                    File.WriteAllText(Path.Info, rss.ToString()); //파일 저장
                }
            }
            [Command("제거")]
            public async Task BotDel(CommandContext ctx, string Botname)
            {
                utility.NewUser(ctx);
                JObject _rss = rss["User"][ctx.User.Id.ToString()]["CustomBot"] as JObject; //피싱
                if (!_rss.ContainsKey(Botname))
                    await ctx.RespondAsync($"'{ctx.User.Username}님의 {Botname}'봇이 존재하지 않습니다.");
                else
                {
                    _rss.Remove(Botname);
                    await ctx.RespondAsync($"'{ctx.User.Username}님의 {Botname}'봇을 삭제하였습니다."); //성공했다면 유저에게 성공했다고 전달함
                    File.WriteAllText(Path.Info, rss.ToString()); //파일 저장
                }

            }
            [Command("목록")]
            public async Task List(CommandContext ctx)
            {
                utility.NewUser(ctx);
                JObject rssbot = rss["User"][ctx.User.Id.ToString()]["CustomBot"] as JObject;
                string str = "```Markdown\n";
                foreach (var item in rssbot.Properties()) str += $"* {item.Name}\n";
                str += "```";
                await ctx.RespondAsync(str);
            }
        }
    }
}
