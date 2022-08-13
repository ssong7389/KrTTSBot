using Discord.Commands;
using System.Reflection;

namespace KrTTSBot.Handlers
{
    public class CommandHandler
    {
        private static CommandService _commandService = ServiceHandler.GetService<CommandService>();

        public static async Task LoadCommandAsync()
        {
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), ServiceHandler.ServiceProvider);
            foreach (var command in _commandService.Commands)
                Console.WriteLine($"Command {command.Name} loaded.");
        }
    }
}
