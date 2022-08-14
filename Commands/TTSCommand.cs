using Discord;
using Discord.Commands;

namespace KrTTSBot.Commands
{
    public class TTSCommand : ModuleBase<SocketCommandContext>
    {
        [Command("join")]
        public async Task JoinCommand()
        {
            await Handlers.AudioHandler.JoinAsync(Context.Guild, Context.User as IVoiceState,
                Context.Channel as ITextChannel);
        }

        [Command("leave")]
        public async Task LeaveCommand()
        { 
            await Handlers.AudioHandler.LeaveAsync(Context.Guild, Context.User as IVoiceState,
                Context.Channel as ITextChannel);
        }
        [Command("tts")]
        public async Task PlayCommand([Remainder] string text)
        {
            await Handlers.AudioHandler.PlayAsync(Context.Guild, Context.User as IVoiceState,
                Context.Channel as ITextChannel, text);
        }

        [Command("말")]
        public async Task KrCommand()
        {
            await Handlers.AudioHandler.PlayAsync(Context.Guild, Context.User as IVoiceState,
                Context.Channel as ITextChannel, Context.Message.Content, Handlers.ConfigHandler.Config.KrPrefix.Length + 1);
        }

        [Command("stop")]
        public async Task StopCommand()
        {
            await Handlers.AudioHandler.StopAsnyc(Context.Guild, Context.User as IVoiceState,
                Context.Channel as ITextChannel);
        }

    }
}
