using DSharpPlus.CommandsNext;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace bluebot
{
    class utility
    {
        static public string Randoms(string[] vs)
        {
            Random random = new Random();
            return vs[random.Next(0, vs.Length)];
        }
        static public string NoBlank(string str)
        {
            return str.Replace(" ", null);
        }
        public static void NewUser(CommandContext ctx) //최초유저일시 등록
        {
            JObject rss2 = Program.rss["User"] as JObject;
            if (!rss2.ContainsKey(ctx.User.Id.ToString()))
            {
                rss2.Add(ctx.User.Id.ToString(),
                    new JObject(
                        new JProperty(
                            "CustomBot", new JObject())));

            }
        }
        public static void NewServer(CommandContext ctx)
        {
            JObject rss = JObject.Parse(File.ReadAllText(Path.Logger));
            if (!rss.ContainsKey(ctx.Guild.Id.ToString()))
            {
                rss.Add(ctx.Guild.Id.ToString(), new JObject(
                    new JProperty("Channel", null),
                    new JProperty("Logger", false)));
                File.WriteAllText(Path.Logger, rss.ToString());
            }

        }
        public struct SettingJson
        {
            [JsonProperty("Token")]
            public string Token { get; private set; }
            [JsonProperty("Prefix")]
            public string Prefix { get; private set; }
        }
    }
}
