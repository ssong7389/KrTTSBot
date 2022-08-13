using Newtonsoft.Json;

namespace KrTTSBot.Handlers
{
    public class ConfigHandler
    {
        private static string ResourcesFolder = "Resources";
        private static string ConfigFile = "config.json";
        private static string ConfigPath = ResourcesFolder + "/" + ConfigFile;
        
        public static BotConfig Config { get; private set; }

        static ConfigHandler()
        {
            if(!Directory.Exists(ResourcesFolder))
                Directory.CreateDirectory(ResourcesFolder);
            if (!File.Exists(ConfigPath))
            {
                Config = new BotConfig(); 
                var json = JsonConvert.SerializeObject(Config, Formatting.Indented);
                File.WriteAllText(ConfigPath, contents: json);
            }
            else
            {
                var json = File.ReadAllText(ConfigPath);
                Config = JsonConvert.DeserializeObject<BotConfig>(json);
            }
                
        }
    }
    public struct BotConfig
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
        [JsonProperty("krPrefix")]
        public string KrPrefix { get; private set; }
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }
        [JsonProperty("AWSAccessKeyId")]
        public string AWSAccessKeyId { get; private set; }
        [JsonProperty("AWSSecretKey")]
        public string AWSSecretKey { get; private set; }
    }
}
