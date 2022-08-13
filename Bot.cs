using Discord;
using Victoria;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace KrTTSBot
{
    public class Bot
    {
        private DiscordSocketClient _client;
        private CommandService _commandService;

        public Bot()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Debug
            });

            _commandService = new CommandService(new CommandServiceConfig()
            {
                LogLevel = LogSeverity.Debug,
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async,
                IgnoreExtraArgs = true,
            });

            var collection = new ServiceCollection();
            collection.AddSingleton(_client);
            collection.AddSingleton(_commandService);
            collection.AddLavaNode();

            Handlers.ServiceHandler.SetProvider(collection);
        }

        public async Task MainAsync()
        {
            if (string.IsNullOrWhiteSpace(Handlers.ConfigHandler.Config.Token)) return;

            await Handlers.CommandHandler.LoadCommandAsync();
            await Handlers.EventHandler.LoadCommands();
            await _client.LoginAsync(TokenType.Bot, Handlers.ConfigHandler.Config.Token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }
    }
}
