using Amazon.Polly;
using Amazon.Polly.Model;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Victoria;
using Victoria.Enums;

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
            if (voiceState.VoiceChannel is null)
            {
                await channel.SendMessageAsync("음성 채널에 먼저 참가해주세요.");
                return;
            }
            else
            {
                if (!_lavaNode.HasPlayer(guild))
                {
                    try
                    {
                        await _lavaNode.JoinAsync(voiceState.VoiceChannel, channel);
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[{DateTime.Now}] {ex.Message}");
                    }
                }
                else if (voiceState.VoiceChannel != _lavaNode.GetPlayer(guild).VoiceChannel)
                {
                    await channel.SendMessageAsync("다른 음성 채널에서 사용 중입니다.");
                    return;
                }
            }

        }

        public static async Task LeaveAsync(IGuild guild, IVoiceState voiceState, ITextChannel channel)
        {
            if (voiceState.VoiceChannel is null) return;
            if (voiceState.VoiceChannel != _lavaNode.GetPlayer(guild).VoiceChannel)
            {
                await channel.SendMessageAsync("같은 음성 채널에 참가해주세요.");
                return;
            }
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

        public static async Task PlayAsync(IGuild guild, IVoiceState voiceState, ITextChannel channel, string text)
        {

            await JoinAsync(guild, voiceState, channel);
            await MakeTTS(text);

            try
            {
                var player = _lavaNode.GetPlayer(guild);
                LavaTrack track;
                var search = await _lavaNode.SearchAsync(Victoria.Responses.Search.SearchType.Direct , Path.GetFullPath(TTSPath));
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

        public static async Task PlayAsync(IGuild guild, IVoiceState voiceState, ITextChannel channel, string text, int krLength)
        {

            text = text.Substring(krLength);
            await JoinAsync(guild, voiceState, channel);
            await MakeTTS(text);

            try
            {
                var player = _lavaNode.GetPlayer(guild);
                LavaTrack track;
                var search = await _lavaNode.SearchAsync(Victoria.Responses.Search.SearchType.Direct, Path.GetFullPath(TTSPath));
                track = search.Tracks.FirstOrDefault();
                if (player.PlayerState is PlayerState.Stopped)
                {
                    player.Queue.Enqueue(track);
                }

                await player.PlayAsync(track);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] {ex.Message}");
                return;
            }
        }

        public static async Task StopAsnyc(IGuild guild, IVoiceState voiceState, ITextChannel channel)
        {
            var player = _lavaNode.GetPlayer(guild);

            if (voiceState.VoiceChannel is null)
            {
                await channel.SendMessageAsync("음성 채널에 먼저 참가해주세요.");
                return;
            }
            else
            {
                if (!_lavaNode.HasPlayer(guild))
                {
                        return;
                }
                else if (voiceState.VoiceChannel != player.VoiceChannel)
                {
                    await channel.SendMessageAsync("다른 음성 채널에서 사용 중입니다.");
                    return;
                }
                else if (voiceState.VoiceChannel == player.VoiceChannel)
                {
                    if (_lavaNode.GetPlayer(guild).PlayerState is PlayerState.Playing)
                    {
                        await player.StopAsync();
                        await channel.SendMessageAsync("음성 출력을 중단하였습니다.");
                    }
                    else return;
                }
            }
        }

    }
}
