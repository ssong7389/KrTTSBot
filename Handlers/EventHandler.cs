using Discord;
using Victoria;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

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
            // var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot || message.Channel is IDMChannel) return;

            var argPos = 0;
            //if (message.Content.Substring(0,2) == ConfigHandler.Config.KrPrefix)
            //{
            //    Console.WriteLine(message.Content.Substring(0, 2));
            //    Console.WriteLine(message.Content);
            //    await _commandService.ExecuteAsync(context, "tts", ServiceHandler.ServiceProvider);
            //}
            //if (!(message.HasStringPrefix(ConfigHandler.Config.Prefix, ref argPos)) ||
            //    message.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;


            //var result = await _commandService.ExecuteAsync(context, argPos, ServiceHandler.ServiceProvider);
            //if (!result.IsSuccess)
            //{
            //    if (result.Error == CommandError.UnknownCommand) return;
            //}
            if(!(message.HasStringPrefix(ConfigHandler.Config.Prefix, ref argPos))){
                if(message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.Author.IsBot) return;
                else
                {
                    if (message.ToString().Substring(0, 4) == "tts ")
                    {
                        var context = new SocketCommandContext(_client, message);
                        Console.WriteLine("in 말ㄹ");
                        await _commandService.ExecuteAsync(
                            context: context,
                            input: "kr",
                            services: ServiceHandler.ServiceProvider);
                    }
                }
            }
            else
            {
                var context = new SocketCommandContext(_client, message);
                await _commandService.ExecuteAsync(context, argPos, ServiceHandler.ServiceProvider);
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
            await _client.SetGameAsync($"Prefix: {ConfigHandler.Config.Prefix}",
                streamUrl: null, ActivityType.Listening);
        }

    }
}
