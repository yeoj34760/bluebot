using System;
using DSharpPlus;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using Newtonsoft.Json.Linq;
using System.IO;

namespace bluebot.Command
{
    class BotCommand : Program
    {

        public static async Task MessageCreated(MessageCreateEventArgs e)
        {

            if (Program.discord.CurrentUser.Id == e.Author.Id) return;

            JObject Bot = rss["User"][e.Author.Id.ToString()]["CustomBot"] as JObject;

            foreach (var B_name in Bot.Properties())
            {
                foreach (string B_Nickname in Bot[B_name.Name]["Nickname"] as JArray)
                {
                    if (e.Message.Content.ToString().StartsWith(B_Nickname))
                    {
                        string str = e.Message.Content.ToString().Substring(B_Nickname.Length + 1); //봇 호칭을 문자열에서 제외함
                        str = utility.NoBlank(str);
                        JArray items = (JArray)Bot[B_name.Name]["commands"][str];
                        int i = 0;
                        string[] vs = new string[items.Count];
                        foreach (string str2 in items)
                        {
                            vs[i] = str2;
                            i++;
                        }
                        await e.Message.RespondAsync(utility.Randoms(vs).ToString());
                    }
                }
            }

        }

        public static async Task MessageDeleted(MessageDeleteEventArgs e)
        {

        }
    }
}
