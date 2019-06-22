using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using bluebot.Services;
using Discord;
using Discord.Audio;
using System.Diagnostics;
using bluebot.Helper;
using Discord.Addons.Interactive;

namespace bluebot.Modules
{
    public class MusicModule : InteractiveBase
    {
        MusicService m_music;
        MusicModule(MusicService service)
        {
            m_music = service;
        }
        IAudioClient client;
        [Command("플레이", RunMode = RunMode.Async)]
        public async Task PingAsync(params string[] str)
        {
            await m_music.GuildAdd(Context.Guild.Id);
            m_music.m_Interactive = base.Interactive;
            await m_music.PlayAsync(string.Join(' ', str));
        }
        [Command("스킵", RunMode = RunMode.Async)]
        public async Task SkipAsync()
        {
            await m_music.GuildAdd(Context.Guild.Id);
            await m_music.PlaySkip();
        }
        [Command("일시중지", RunMode = RunMode.Async)]
        public async Task PauseAsync()
        {
            await m_music.GuildAdd(Context.Guild.Id);
            await m_music.PlayPause();
        }
        [Command("추가", RunMode = RunMode.Async)]
        public async Task PlayListAddCmd(params string[] str)
        {
            await m_music.GuildAdd(Context.Guild.Id);
            m_music.m_Interactive = base.Interactive;
            await m_music.PlayListAddAsync(string.Join(' ', str));
        }
        [Command("리스트")]
        public async Task PlayListAsync()
        {
            m_music.m_Interactive = base.Interactive;
            await m_music.ListAsync();
        }
        [Command("중지", RunMode = RunMode.Async)]
        public async Task StopChannel()
        {
            await m_music.PlayStop();
        }
    }
}
