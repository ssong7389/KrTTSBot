using Discord;
using Discord.Commands;

namespace KrTTSBot.Commands
{
    public class TTSCommand : ModuleBase<SocketCommandContext>
    {
        [Command("말")]
        [Remarks("TTS를 재생하는 한글 명령어입니다. 한/영 전환 없이 한글로 간편하게 사용해요.")]
        public async Task KrCommand()
        {
            await Handlers.AudioHandler.PlayAsync(Context.Guild, Context.User as IVoiceState,
                Context.Channel as ITextChannel, Context.Message.Content, Handlers.ConfigHandler.Config.KrPrefix.Length + 1);
        }

        [Command("tts")]
        [Remarks("TTS 명령어입니다.")]
        public async Task PlayCommand([Remainder] string text)
        {
            await Handlers.AudioHandler.PlayAsync(Context.Guild, Context.User as IVoiceState,
                Context.Channel as ITextChannel, text);
        }

        [Command("join")]
        [Alias("드루와")]
        [Remarks("음성 채널에 참가합니다. join 명령어로 참가시키지 않아도 말하기 명령으로 자동으로 참가해요.")]
        public async Task JoinCommand()
        {
            await Handlers.AudioHandler.JoinAsync(Context.Guild, Context.User as IVoiceState,
                Context.Channel as ITextChannel);
        }

        [Command("leave")]
        [Alias("나가")]
        [Remarks("음성 채널에서 나가게 합니다.")]
        public async Task LeaveCommand()
        { 
            await Handlers.AudioHandler.LeaveAsync(Context.Guild, Context.User as IVoiceState,
                Context.Channel as ITextChannel);
        }

        [Command("stop")]
        [Alias("멈춰")]
        [Remarks("TTS 재생을 중단합니다.")]
        public async Task StopCommand()
        {
            await Handlers.AudioHandler.StopAsnyc(Context.Guild, Context.User as IVoiceState,
                Context.Channel as ITextChannel);
        }

    }
}
