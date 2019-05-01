using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace bluebot.Module
{
    class CommandModule : Program
    {
        [Group("명령어")]
        public class Command
        {
            [Command("추가")]
            public async Task CommandAdd(CommandContext ctx, string Botname)
            {
                utility.NewUser(ctx);
                var interactivity = ctx.Client.GetInteractivityModule();
                string QuestionResult = null, AnswerResult;
                JObject rssbot = rss["User"][ctx.User.Id.ToString()]["CustomBot"] as JObject;
                if (!rssbot.ContainsKey(Botname))
                    await ctx.RespondAsync($"'{ctx.User.Username}님의 {Botname}'봇이 존재하지 않습니다.");
                else
                {
                    await ctx.RespondAsync($" {ctx.User.Mention}님 1시간안에 질문을 적으세요 \n예시 : 안녕");
                    var Question = await interactivity.WaitForMessageAsync(xm => xm.Content != null && ctx.User.Id == xm.Author.Id, TimeSpan.FromHours(1));
                    if (Question != null)
                    {
                        QuestionResult = Question.Message.Content;
                        JObject items = rssbot[Botname]["commands"] as JObject;
                        foreach (var item in items.Properties())
                        {
                            if (items[item.Name].ToString() == QuestionResult)
                            {
                                await ctx.RespondAsync("이미 등록이 되어있습니다. '답변추가'를 이용해주세요.");
                                return;
                            }
                        }
                        await ctx.RespondAsync($"{ctx.User.Mention}님 1시간안에 봇이 말할 답변을 적으세요 \n예시 : 방가워!");
                        var Answer = await interactivity.WaitForMessageAsync(xm => xm.Content != null && ctx.User.Id == xm.Author.Id, TimeSpan.FromHours(1));
                        if (Answer != null)
                        {
                            AnswerResult = Answer.Message.Content;
                            JObject rsscommands = rss["User"][ctx.User.Id.ToString()]["CustomBot"][Botname]["commands"] as JObject;
                            rsscommands.Add(new JProperty(utility.NoBlank(QuestionResult),
                                new JArray(AnswerResult)));
                            File.WriteAllText(Path.Info, rss.ToString()); //파일 저장
                            await ctx.RespondAsync("추가 완료");
                        }
                        else await ctx.RespondAsync($"{ctx.User.Mention}님 한 시간이 넘었습니다.");
                    }
                    else await ctx.RespondAsync($"{ctx.User.Mention}님 한 시간이 넘었습니다.");
                }
            }
            [Command("답변추가")]
            public async Task AnswerAdd(CommandContext ctx, string Botname)
            {
                utility.NewUser(ctx);
                var interactivity = ctx.Client.GetInteractivityModule();
                JObject rssbot = rss["User"][ctx.User.Id.ToString()]["CustomBot"] as JObject;
                if (!rssbot.ContainsKey(Botname))
                    await ctx.RespondAsync($"'{ctx.User.Username}님의 {Botname}'봇이 존재하지 않습니다.");
                else
                {

                    JObject items = rssbot[Botname]["commands"] as JObject;
                    string[] vs = new string[items.Count];
                    string str = "```Markdown\n";
                    int i = 0;
                    foreach (var item in items.Properties())
                    {
                        vs[i] = item.Name;
                        str += $"{i}. {item.Name}\n";
                        i++;
                    }
                    str += "```";
                    await ctx.RespondAsync(str);
                    await ctx.RespondAsync($"{ctx.User.Mention}님 1시간안에 질문(번호)을 적으세요");
                    var Question = await interactivity.WaitForMessageAsync(xm => xm.Content != null && ctx.User.Id == xm.Author.Id, TimeSpan.FromHours(1));
                    if (Question != null)
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}님 1시간안에 봇이 말할 답변을 적으세요 \n예시 : 방가워!");
                        var Answer = await interactivity.WaitForMessageAsync(xm => xm.Content != null && ctx.User.Id == xm.Author.Id, TimeSpan.FromHours(1));
                        if (Answer != null)
                        {
                            JArray Answers = items[vs[int.Parse(Question.Message.Content)]] as JArray;
                            Answers.Add(Answer.Message.Content);
                            File.WriteAllText(Path.Info, rss.ToString());
                            await ctx.RespondAsync($"추가완료");
                        }
                        else await ctx.RespondAsync($"{ctx.User.Mention}님 한 시간이 넘었습니다.");
                    }
                    else await ctx.RespondAsync($"{ctx.User.Mention}님 한 시간이 넘었습니다.");
                }

            }
            [Command("제거")]
            public async Task CommandDel(CommandContext ctx, string Botname)
            {

                utility.NewUser(ctx);
                var interactivity = ctx.Client.GetInteractivityModule();
                JObject rssbot = rss["User"][ctx.User.Id.ToString()]["CustomBot"] as JObject;
                if (!rssbot.ContainsKey(Botname))
                    await ctx.RespondAsync($"'{ctx.User.Username}님의 {Botname}'봇이 존재하지 않습니다.");
                else
                {
                    JObject items = rssbot[Botname]["commands"] as JObject;
                    string[] vs = new string[items.Count];
                    string str = "```Markdown\n";
                    int i = 0;
                    foreach (var item in items.Properties())
                    {
                        vs[i] = item.Name;
                        str += $"{i}. {item.Name}\n";
                        i++;
                    }
                    str += "```";
                    await ctx.RespondAsync(str);
                    await ctx.RespondAsync($"{ctx.User.Mention}님 1시간안에 제거할 번호를 적으세요");
                    var Question = await interactivity.WaitForMessageAsync(xm => xm.Content != null && ctx.User.Id == xm.Author.Id, TimeSpan.FromHours(1));
                    if (Question != null)
                    {
                        items.Remove(vs[int.Parse(Question.Message.Content)]);
                        File.WriteAllText(Path.Info, rss.ToString());
                        await ctx.RespondAsync("제거 완료");
                    }
                    else await ctx.RespondAsync($"{ctx.User.Mention}님 한 시간이 넘었습니다.");
                }
            }
            [Command("목록")]
            public async Task List(CommandContext ctx, string Botname)
            {
                utility.NewUser(ctx);
                JObject rssbot = rss["User"][ctx.User.Id.ToString()]["CustomBot"] as JObject;
                if (!rssbot.ContainsKey(Botname))
                    await ctx.RespondAsync($"'{ctx.User.Username}님의 {Botname}'봇이 존재하지 않습니다.");
                else
                {
                    string str = "```Markdown\n";
                    JObject items = rssbot[Botname]["commands"] as JObject;
                    foreach (var item in items.Properties())
                    {
                        str += $"* {item.Name}\n";
                        JArray Answeritems = items[item.Name] as JArray;
                        foreach (string Answeritem in Answeritems) str += $"> {Answeritem}\n";
                    }
                    str += "```";
                    await ctx.RespondAsync(str);
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
