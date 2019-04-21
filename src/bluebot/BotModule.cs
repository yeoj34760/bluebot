
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace bluebot
{
    class BotModule : Program
    {
        public static void NewUser(CommandContext ctx) //최초유저일시 등록
        {
            JObject rss2 = rss["User"] as JObject;
            if (!rss2.ContainsKey(ctx.User.Id.ToString()))
            {
                rss2.Add(ctx.User.Id.ToString(),
                    new JObject(
                        new JProperty(
                            "CustomBot", new JObject())));
            }
        }
        [Group("봇")]
        public class Bot
        {
            [Command("생성")]
            public async Task BotAdd(CommandContext ctx, string Botname, string Botnick) // this command takes no arguments
            {
                NewUser(ctx);
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
                NewUser(ctx);
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
            [Group("명령어")]
            public class Command
            {
                [Command("추가")]
                public async Task CommandAdd(CommandContext ctx, string Botname, string strq, string stra)
                {

                    NewUser(ctx);
                    JObject rssbot = rss["User"][ctx.User.Id.ToString()]["CustomBot"] as JObject;
                    if (!rssbot.ContainsKey(Botname))
                        await ctx.RespondAsync($"'{ctx.User.Username}님의 {Botname}'봇이 존재하지 않습니다.");
                    else
                    {

                        if (strq.StartsWith("\"") && stra.StartsWith("\"")) await ctx.RespondAsync("큰따옴표가 있는지 확인하세요");
                        else
                        {

                            JObject rsscommands = rss["User"][ctx.User.Id.ToString()]["CustomBot"][Botname]["commands"] as JObject;

                            rsscommands.Add(new JProperty(utility.NoBlank(strq),
                                new JArray(utility.NoBlank(stra))));
                            File.WriteAllText(Path.Info, rss.ToString()); //파일 저장
                            await ctx.RespondAsync("추가 완료");
                        }

                    }
                }
                [Command("답변추가")]
                public async Task AnswerAdd(CommandContext ctx, string Botname, string strq, string stra)
                {
                    NewUser(ctx);
                    JObject rssbot = rss["User"][ctx.User.Id.ToString()]["CustomBot"] as JObject;
                    if (!rssbot.ContainsKey(Botname))
                        await ctx.RespondAsync($"'{ctx.User.Username}님의 {Botname}'봇이 존재하지 않습니다.");
                    else
                    {

                        if (strq.StartsWith("\"") && stra.StartsWith("\"")) await ctx.RespondAsync("큰따옴표가 있는지 확인하세요");
                        else
                        {
                            JObject rsscommands = rss["User"][ctx.User.Id.ToString()]["CustomBot"][Botname]["commands"] as JObject; //검사용
                            JArray rssArray = rss["User"][ctx.User.Id.ToString()]["CustomBot"][Botname]["commands"][strq] as JArray;
                            if (!rsscommands.ContainsKey(strq))
                            {
                                await ctx.RespondAsync($"'{strq}'라는 질문이 등록되어 있지 않습니다.");
                            }
                            else
                            {
                                rssArray.Add(stra);
                                File.WriteAllText(Path.Info, rss.ToString()); //파일 저장
                                await ctx.RespondAsync("추가 완료");
                            }

                        }

                    }

                }
                [Command("제거")]
                public async Task CommandDel(CommandContext ctx, string Botname, string strq)
                {

                    NewUser(ctx);
                    JObject rssbot = rss["User"][ctx.User.Id.ToString()]["CustomBot"] as JObject;
                    if (!rssbot.ContainsKey(Botname))
                        await ctx.RespondAsync($"'{ctx.User.Username}님의 {Botname}'봇이 존재하지 않습니다.");
                    else
                    {

                        if (strq.StartsWith("\"")) await ctx.RespondAsync("큰따옴표가 있는지 확인하세요");
                        else
                        {

                            JObject rsscommands = rss["User"][ctx.User.Id.ToString()]["CustomBot"][Botname]["commands"] as JObject;
                            if (!rsscommands.ContainsKey(strq))
                            {
                                await ctx.RespondAsync($"'{strq}'라는 질문이 등록되어 있지 않습니다.");
                            }
                            else
                            {
                                rsscommands.Remove(strq);
                                File.WriteAllText(Path.Info, rss.ToString()); //파일 저장
                                await ctx.RespondAsync("제거 완료");
                            }

                        }

                    }
                }
                /* [Command("답변제거")]
                 public async Task AnswerDel(CommandContext ctx, string Botname, string strq, string stra)
                 {

                     NewUser(ctx);
                     JObject rssbot = rss["User"][ctx.User.Id.ToString()]["CustomBot"] as JObject;
                     if (!rssbot.ContainsKey(Botname))
                         await ctx.RespondAsync($"'{ctx.User.Username}님의 {Botname}'봇이 존재하지 않습니다.");
                     else
                     {

                         if (strq.StartsWith("\"")) await ctx.RespondAsync("큰따옴표가 있는지 확인하세요");
                         else
                         {

                             JObject rsscommands = rss["User"][ctx.User.Id.ToString()]["CustomBot"][Botname]["commands"] as JObject;
                             JArray rssArray = rss["User"][ctx.User.Id.ToString()]["CustomBot"][Botname]["commands"][strq] as JArray;
                             if (!rsscommands.ContainsKey(strq))
                             {
                                 await ctx.RespondAsync($"'{strq}'라는 질문이 등록되어 있지 않습니다.");
                             }
                             else
                             {

                                 Console.WriteLine(rssArray.ToString());
                                 File.WriteAllText(Path.Info, rss.ToString()); //파일 저장
                                 await ctx.RespondAsync("제거 완료");
                             }

                         }

                     }
                 }*/
            }


        }
    }
}
