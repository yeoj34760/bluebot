using System;
using DSharpPlus;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using Newtonsoft.Json.Linq;
using System.IO;
using DSharpPlus.Entities;

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

            JObject rss = JObject.Parse(File.ReadAllText(Path.Logger));
            if (!rss.ContainsKey(e.Guild.Id.ToString())) return;
            if ((bool)rss[e.Guild.Id.ToString()]["Logger"] == false) return;
            if (rss[e.Guild.Id.ToString()]["Channel"].ToString() == "") return;
            DiscordChannel Channel = Program.discord.GetChannelAsync((ulong)rss[e.Guild.Id.ToString()]["Channel"]).Result;
            var embed = new DiscordEmbedBuilder
            {
                Title = "Logger",
                Description = $"이름 : {e.Message.Author.Username}\n내용 : {e.Message.Content}",
                ThumbnailUrl = e.Message.Author.AvatarUrl,
                Color = DiscordColor.Blue

            };
            await Program.discord.SendMessageAsync(Channel, null, false, embed);
        }
    }
}
