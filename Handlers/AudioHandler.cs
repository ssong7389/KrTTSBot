using Discord;
using Victoria;
using Amazon.Polly;
using Victoria.Enums;
using Discord.WebSocket;
using Victoria.EventArgs;
using Amazon.Polly.Model;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace KrTTSBot.Handlers
{
    public class AudioHandler
    {
        private static readonly LavaNode _lavaNode = ServiceHandler.ServiceProvider.GetRequiredService<LavaNode>();
        private static string ResourcesFolder = "Resources";
        private static string TTS = "tts.mp3";
        private static string TTSPath =  ResourcesFolder + "/" + TTS;

        public static async Task JoinAsync(IGuild guild, IVoiceState voiceState, ITextChannel channel)
        {
            if (_lavaNode.HasPlayer(guild))
            {
                await channel.SendMessageAsync("이미 음성채팅에 연결되어 있어요");
                return;
            } 
            if (voiceState.VoiceChannel is null)
            {
                await channel.SendMessageAsync("먼저 음성채팅에 들어가 주세요");
                return;
            }

            try
            {
                await _lavaNode.JoinAsync(voiceState.VoiceChannel, channel);
                return;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] {ex.Message}");
            }
        }

        public static async Task LeaveAsync(IGuild guild)
        {
            try
            {
                var player = _lavaNode.GetPlayer(guild);
                if (player.PlayerState is PlayerState.Playing) await player.StopAsync();
                await _lavaNode.LeaveAsync(player.VoiceChannel);
            }
            catch(InvalidOperationException ex)
            {
                Console.WriteLine($"[{DateTime.Now}]\t {ex.Message}");
                return;
            }
        }

        public static async Task MakeTTS(string text)
        {
            Console.WriteLine(TTSPath);
            string AWSAccessKeyId = ConfigHandler.Config.AWSAccessKeyId;
            string AWSSecretKey = ConfigHandler.Config.AWSSecretKey;
            var pc = new AmazonPollyClient(AWSAccessKeyId, AWSSecretKey);
            var sreq = new SynthesizeSpeechRequest
            {
                Text = "<speak>" + text + "</speak>",
                OutputFormat = OutputFormat.Mp3,
                VoiceId = VoiceId.Seoyeon,
                LanguageCode = "ko-KR",
                TextType = TextType.Ssml
            };
            var sres = await pc.SynthesizeSpeechAsync(sreq);

            if (sres.HttpStatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("Http Error");
                return;
            }           

            if (File.Exists(TTSPath))
            {
                File.Delete(TTSPath);
            }
            FileStream fs = new FileStream(TTSPath, FileMode.Create);

            sres.AudioStream.CopyTo(fs);
            fs.Flush();
            fs.Close();
            await Task.CompletedTask;
            return;
        }

        public static async Task PlayAsync(SocketGuildUser user, IGuild guild, ITextChannel channel, string text)
        {
            Console.WriteLine(text);
            if (user.VoiceChannel is null)
                await channel.SendMessageAsync("음성 채널에 먼저 참가해주세요.");
            if (!_lavaNode.HasPlayer(guild))
                await JoinAsync(guild, user.VoiceState, channel);
            if(user.VoiceChannel != _lavaNode.GetPlayer(guild).VoiceChannel)
            {
                await channel.SendMessageAsync("다른 음성 채널에서 사용 중입니다.");
            }
            await MakeTTS(text);

            try
            {
                var player = _lavaNode.GetPlayer(guild);
                LavaTrack track;
                var search = await _lavaNode.SearchAsync(Victoria.Responses.Search.SearchType.Direct , @"C:\Users\sych7\Desktop\CSharp\Discord\KrTTSBot\KrTTSBot\bin\Debug\net6.0\Resources\tts.mp3");
                track = search.Tracks.FirstOrDefault();
                if(player.PlayerState is PlayerState.Stopped)
                {
                    player.Queue.Enqueue(track);
                }

                await player.PlayAsync(track);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] {ex.Message}");
                return;
            }
        }

    }
}
