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
        [Command("move")]
        [Alias("이동")]
        [Remarks("봇을 다른 채널에서 사용중일 때 사용자가 접속중인 음성 채널로 옮겨올 수 있어요.")]
        public async Task MoveCommand()
        {
            await Handlers.AudioHandler.ChangeCannelAsync(Context.Guild, Context.User as IVoiceState, Context.Channel as ITextChannel);
        }
    }
}
