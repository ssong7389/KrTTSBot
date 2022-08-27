using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

namespace KrTTSBot.Handlers
{
    public class CommandHandler
    {
        private static CommandService _commandService = ServiceHandler.GetService<CommandService>();
        private static DiscordSocketClient _client = ServiceHandler.GetService<DiscordSocketClient>();


        public static async Task LoadCommandAsync()
        {
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), ServiceHandler.ServiceProvider);
            foreach (var command in _commandService.Commands)
                Console.WriteLine($"Command {command.Name} loaded.");
        }

        public static async Task HelpCommandAsync(IGuild guild, ITextChannel channel)
        {
            var embedBuilder = new EmbedBuilder();
            embedBuilder.WithTitle(_client.CurrentUser.Username);
            embedBuilder.WithThumbnailUrl(_client.CurrentUser.GetAvatarUrl());
            embedBuilder.WithColor(169, 250, 250);

            string Description = "TTS 메세지를 출력해주는 " + _client.CurrentUser.Username + "입니다. " + Handlers.ConfigHandler.Config.KrPrefix
            + " \'메시지\' 로 메시지를 TTS로 만들어 재생시켜줍니다. TTS 명령어 사용시 자동으로 음성채널에 연결해 재생합니다." +
            " 기본 Prefix는 " + Handlers.ConfigHandler.Config.Prefix + "입니다.";

            embedBuilder.WithDescription(Description);
            
            foreach (var c in _commandService.Commands)
            {
                if (c.Name == "말")
                {
                    embedBuilder.AddField(Handlers.ConfigHandler.Config.KrPrefix, c.Remarks, false);
                }
                else
                {
                    var aliases = string.Empty;
                    foreach(var alias in c.Aliases)
                    {
                        aliases += alias;
                        if (alias != c.Aliases.Last())
                            aliases += ", ";
                    }
                    embedBuilder.AddField(aliases, c.Remarks, false);
                }    
                    
            }
            var footer = new EmbedFooterBuilder();
            footer.WithText("\n사용법 예시: 말ㄹ 할 말 / ~tts 할 말 \n -> \'할 말\'이 TTS로 재생됩니다. (먼저 음성 채널에 입장은 필수)");
            embedBuilder.WithFooter(footer);
            await channel.SendMessageAsync("", false, embedBuilder.Build());
        }
    }
}
