using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KrTTSBot.Commands
{
    public class TTSCommand : ModuleBase<SocketCommandContext>
    {
        [Command("join")]
        public async Task JoinCommand()
            => await Handlers.AudioHandler.JoinAsync(Context.Guild, Context.User as IVoiceState,
                Context.Channel as ITextChannel);

        [Command("leave")]
        public async Task LeaveCommand()
            => await Handlers.AudioHandler.LeaveAsync(Context.Guild);

        [Command("tts")]
        public async Task PlayCommand([Remainder] string text)
        {
            Console.WriteLine("잉?");
            await Handlers.AudioHandler.PlayAsync(Context.User as SocketGuildUser, Context.Guild, Context.Channel as ITextChannel, text);
        }

        [Command("kr")]
        public async Task KrCommand()
        {
            Console.WriteLine("잉?");
            await Handlers.AudioHandler.PlayAsync(Context.User as SocketGuildUser, Context.Guild, Context.Channel as ITextChannel, Context.Message.ToString());
        }

    }
}
