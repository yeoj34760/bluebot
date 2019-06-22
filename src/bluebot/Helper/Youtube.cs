﻿using bluebot.Services;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.Addons.Interactive;
using Discord.Commands;
using System.Threading.Tasks;
using System.Diagnostics;

namespace bluebot.Helper
{
    public class Youtube : OtherService
    {
        public List<string>
            Title = new List<string>(),
            VideoId = new List<string>();

        public async Task<string> List(string str) //유튜브 리스트를 불러옵니다.
        {
             var youtube = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "Key", // 키 지정
                ApplicationName = "bluebotAPI"
            });
            var request = youtube.Search.List("snippet");
            request.Q = str; //검색 예시 : 진진자라
            request.MaxResults = 5; //최대 검색량
            request.Type = "video"; //비디오 타입
            var result = request.Execute(); //결과
            if (result.Items.Count == 0)
                return null;
            string text = "```Markdown\n";
            for (int i = 0; result.Items.Count > i; i++)
            {
                Google.Apis.YouTube.v3.Data.SearchResult searchResult = result.Items[i];
                VideoId.Add(searchResult.Id.VideoId);
                Title.Add(searchResult.Snippet.Title);
                text += $"{i + 1}. {searchResult.Snippet.Title}\n";
            }
                
            text += "```";
            return text;
        }
        public async Task Download(string VideoId, string File)
        {
            Process p = new Process();
            p.StartInfo.FileName = "youtube-dl";
            p.StartInfo.Arguments = $@"--extract-audio -o {File} --audio-format mp3 https://www.youtube.com/watch?v={VideoId}";
            p.Start();
            p.WaitForExit();
        }
    }
}
