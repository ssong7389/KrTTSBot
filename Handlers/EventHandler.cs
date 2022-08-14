using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Victoria;

namespace KrTTSBot.Handlers
{
    public static class EventHandler
    {
        private static LavaNode _lavaNode = ServiceHandler.ServiceProvider.GetRequiredService<LavaNode>();
        private static DiscordSocketClient _client = ServiceHandler.GetService<DiscordSocketClient>();
        private static CommandService _commandService = ServiceHandler.GetService<CommandService>();

        public static Task LoadCommands()
        {
            _client.Log += message =>
            {
                Console.WriteLine($"[{DateTime.Now}]\t{message.Source}\t{message.Message}");
                return Task.CompletedTask;
            };

            _commandService.Log += message =>
            {
                Console.WriteLine($"[{DateTime.Now}]\t{message.Source}\t{message.Message}");
                return Task.CompletedTask;
            };

            _client.Ready += OnReady;
            _client.MessageReceived += OnMessageReceived;
            return Task.CompletedTask;
        }
        private static async Task OnMessageReceived(SocketMessage msg)
        {

            var message = msg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            var argPos = 0;
            IResult result;
            if (message.Author.IsBot || message.Channel is IDMChannel 
                || message.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;


            if (!(message.HasStringPrefix(ConfigHandler.Config.Prefix, ref argPos)))
            {
                var krLength = ConfigHandler.Config.KrPrefix.Length;
                if (message.Content.Substring(0, krLength+1) == ConfigHandler.Config.KrPrefix + " ")
                {
                    result = await _commandService.ExecuteAsync(context, "말", ServiceHandler.ServiceProvider);
                }
                else return;

            }
            else
            {
                result = await _commandService.ExecuteAsync(context, argPos, ServiceHandler.ServiceProvider);
            };

            if (!result.IsSuccess)
            {
                if (result.Error == CommandError.UnknownCommand) return;
            }

        }

    

        private static async Task OnReady()
        {
            try
            {
                await _lavaNode.ConnectAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Console.WriteLine($"[{DateTime.Now}\t(READY)\tBot is Ready");
            await _client.SetStatusAsync(Discord.UserStatus.Online);
            await _client.SetGameAsync($"{ConfigHandler.Config.KrPrefix} \"메시지\" 로 말하기 / Prefix: {ConfigHandler.Config.Prefix}",
                streamUrl: null, ActivityType.Playing);
        }

    }
}
