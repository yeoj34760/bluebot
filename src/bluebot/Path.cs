using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bluebot
{
    class Path
    {
        public static string Info = (System.Environment.OSVersion.Platform.ToString() == "Win32NT") ?
            Directory.GetCurrentDirectory() + "\\Info\\UserBots.json" : Directory.GetCurrentDirectory() + "/Info/UserBots.json";
        public static string Setting = (System.Environment.OSVersion.Platform.ToString() == "Win32NT") ?
           Directory.GetCurrentDirectory() + "\\Info\\Setting.json" : Directory.GetCurrentDirectory() + "/Info/Setting.json";
    }
}
