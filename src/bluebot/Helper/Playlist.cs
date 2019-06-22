using System;
using System.Collections.Generic;
using System.Text;

namespace bluebot.Helper
{
    class Playlist
    {
        public string File { get; set; } //파일
        public string Title { get; set; } //타이틀
        public string VideoId { get; set; } //https://www.youtube.com/watch?v= <- 이부분을 말하는거임
        public bool IsDownload { get; set; } //다운로드를 했는가?
        public string IsUser { get; set; } //누가 신청했는가?
    }
}
