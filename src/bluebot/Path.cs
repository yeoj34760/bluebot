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
        public static string Help = (System.Environment.OSVersion.Platform.ToString() == "Win32NT") ?
      Directory.GetCurrentDirectory() + "\\Info\\Help.md" : Directory.GetCurrentDirectory() + "/Info/Help.md";
        public static string Logger = (System.Environment.OSVersion.Platform.ToString() == "Win32NT") ?
      Directory.GetCurrentDirectory() + "\\Info\\Logger.json" : Directory.GetCurrentDirectory() + "/Info/Logger.json";
    }
}
