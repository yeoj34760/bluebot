﻿using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using bluebot.Helper;
using Discord;
using System.Collections.Concurrent;
using Discord.Addons.Interactive;
using System.Diagnostics;
using Discord.Audio;

namespace bluebot.Services
{
    public class MusicService : OtherService
    {
        public InteractiveService m_Interactive; //상호작용
        private Dictionary<ulong, Guilds> m_Guild = new Dictionary<ulong, Guilds>();
        Youtube youtube;
        public async Task PlayAsync(string str)
        {
            if (m_Guild[context.Guild.Id].IsPlaying == true)
            {
                await ReplyChannel("현재 음악 재생중입니다!\n !중지, !추가 명령어를 이용하십시오");
                return;
            }
            m_Guild[context.Guild.Id].IsPlaying = true;
            youtube = new Youtube();
            Random random = new Random();
            string VideoId, Title, File = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".mp3";
            string YoutubeList = youtube.List(str).Result;
            if (YoutubeList == null)
            {

                await ReplyChannel("`검색 결과가 없습니다.`");
                return;
            }
            await ReplyChannel(YoutubeList);
            var response = await m_Interactive.NextMessageAsync(context);
            
            if (response != null)
            {
                int i = int.Parse(response.Content.ToString());
                VideoId = youtube.VideoId[i - 1];
                Title = youtube.Title[i - 1];
                Playlist playlist = new Playlist();
                await ReplyChannel("찾는중...");
                Console.WriteLine(VideoId + "알라봉따이");
                await youtube.Download(VideoId, File);
                playlist.IsDownload = true;
                playlist.Title = Title;
                playlist.VideoId = VideoId;
                playlist.File = File;
                await ReplyChannel("준비완료! 곧 음악이 재생됩니다.");
                m_Guild[context.Guild.Id].playlist.Enqueue(playlist);
                await m_Guild[context.Guild.Id].PlayStartAsync();
            }
        }

        public async Task ListAsync()
        {

            Playlist[] playlist = m_Guild[context.Guild.Id].playlist.ToArray();
            if (playlist.Length == 0)
            {
                await ReplyChannel("`없음!`");
                return;

            }
            string str = "```Markdown\n";
            for (int i = 1; playlist.Length >= i; i++)
                str += $"{i}. {playlist[i - 1].Title}\n";
            str += "```";
            await ReplyChannel(str);
        }
        public async Task PlayListAddAsync(string str)
        {
            if (m_Guild[context.Guild.Id].IsPlaying == false)
            {
                await ReplyChannel("`!플레이 <제목>`를 이용하십시오.");
                return;
            }
            youtube = new Youtube();
            Random random = new Random();
            string VideoId, Title, File = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".mp3";
            await ReplyChannel(youtube.List(str).Result);
            var response = await m_Interactive.NextMessageAsync(context);
            if (response != null)
            {
                Playlist playlist = new Playlist();
                int i = int.Parse(response.Content.ToString());
                VideoId = youtube.VideoId[i - 1];
                Title = youtube.Title[i - 1];
                playlist.IsDownload = false;
                playlist.Title = Title;
                playlist.VideoId = VideoId;
                playlist.File = File;
                m_Guild[context.Guild.Id].playlist.Enqueue(playlist);
                await ReplyChannel("추가완료");
            }

        }

        public async Task PlayStop()
        {
            m_Guild[context.Guild.Id].Stop();
            await ReplyChannel("음악을 중지합니다.");
        }
        public async Task PlaySkip()
        {
            m_Guild[context.Guild.Id].Skip();
            await ReplyChannel("다음 음악으로 재생합니다.");
        }
        public async Task PlayPause()
        {
            if (m_Guild[context.Guild.Id].PlayPause != true)
            {
                m_Guild[context.Guild.Id].PlayPause = true;
                await ReplyChannel("일시중지합니다.");
            }
            else
            {
                m_Guild[context.Guild.Id].PlayPause = false;
                await ReplyChannel("다시 재생합니다.");
            }
        }
        public async Task GuildAdd(ulong Id)
        {
            if (m_Guild.ContainsKey(Id) != true)
            {
                Guilds Guild = new Guilds();
                m_Guild.Add(Id, Guild);
            }
            else
                return;

        }

    }
}