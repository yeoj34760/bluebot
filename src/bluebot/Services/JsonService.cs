using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace bluebot.Services
{
    class JsonService
    {
        private static string JsonPath = File.ReadAllText("config.json");
        public static ConfigValue Config = JsonConvert.DeserializeObject<ConfigValue>(JsonPath);
        internal class ConfigValue
        {
            public string Token { get; set; }
            public string Prefix { get; set; }
        }
    }
}
