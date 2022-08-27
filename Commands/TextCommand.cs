using Discord;
using Discord.Commands;

namespace KrTTSBot.Commands
{
    public class TextCommand :ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Alias("도움")]
        [Remarks("사용법을 알려드려요.")]
        public async Task HelpCommand()
        {
            await Handlers.CommandHandler.HelpCommandAsync(Context.Guild, Context.Channel as ITextChannel);
        }
    }
}
