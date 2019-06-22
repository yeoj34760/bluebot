using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
namespace bluebot.Services
{
    public class OtherService
    {
        public static SocketCommandContext context;
        public async Task ReplyChannel(string str)
        {
            await context.Channel.SendMessageAsync(str);
        }
        public async Task ReplyUser(string str)
        {
            await context.User.SendMessageAsync(str);
        }
    }
}
