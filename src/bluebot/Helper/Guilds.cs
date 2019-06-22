using bluebot.Services;
using Discord;
using Discord.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace bluebot.Helper
{
    class Guilds : OtherService
    {
        public Queue<Playlist> playlist = new Queue<Playlist>();
        public bool PlayStop = false; //true라면 음악을 멈춤
        public bool PlayPause = false; //true라면 일시중지함
        public bool IsPlaying = false;
        public bool PlaySkip = false;
        Process ffmpeg;
        int blockSize;
        byte[] buffer;
        Playlist List;
        public async Task PlayStartAsync()
        {
            IAudioClient client = await (context.User as IVoiceState).VoiceChannel.ConnectAsync();
            var discord = client.CreatePCMStream(AudioApplication.Mixed);
            blockSize = 7680;
            buffer = new byte[blockSize];
            while (playlist.Count != 0)
            {
                PlayStop = false;
                PlaySkip = false;
                List = playlist.Dequeue();
                if (List.IsDownload == false)
                {
                    Youtube youtube = new Youtube();
                    await youtube.Download(List.VideoId, List.File);
                }
                ffmpeg = CreateStream(List.File);
                await Play(discord);
                if (PlayStop)
                    break;
            }
            playlist.Clear();
            IsPlaying = false;
            PlaySkip = false;
            PlayStop = false;
            PlayPause = false;
            await client.StopAsync();

        }
        public async Task Play(AudioOutStream discord)
        {
            int byteCount;
         
            while (true)
            {
                if (PlaySkip) //스킵
                    break;

                if (PlayStop) //중지
                    break;
                byteCount = await ffmpeg.StandardOutput.BaseStream.ReadAsync(buffer, 0, blockSize);
                if (byteCount <= 0) //음악이 끝나면
                {
                    System.IO.File.Delete(List.File);
                    break;
                }
                if (ffmpeg == null)
                {
                    System.IO.File.Delete(List.File);
                    break;
                }
                if (PlayPause) //일시중지
                    continue;
                await discord.WriteAsync(buffer, 0, byteCount);
            }
        }

        public void Stop()
        {
            Kill();
            PlayStop = true;
        }
        public void Skip()
        {
            Kill();
            PlaySkip = true;
        }
        private void Kill()
        {
            ffmpeg.Kill();
            ffmpeg.WaitForExit();
            System.IO.File.Delete(List.File);
        }
        private Process CreateStream(string File)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i {File} -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }
    }
}
