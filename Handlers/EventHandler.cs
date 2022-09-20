using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Timers;
using Victoria;
using Victoria.EventArgs;

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
            _client.UserVoiceStateUpdated += OnUserVoiceStateUpdated;
            _lavaNode.OnTrackEnded += OnLavaTrackEnded;
            _lavaNode.OnTrackStarted += OnLavaTrackStarted;

            return Task.CompletedTask;
        }


        private static Task OnLavaTrackStarted(TrackStartEventArgs arg)
        {
            ulong guildId = arg.Player.VoiceChannel.GuildId;
            ConnectionTimerHandler.StopTimer(guildId);
            return Task.CompletedTask;
        }

        private static Task OnLavaTrackEnded(TrackEndedEventArgs arg)
        {
            ulong guildId = arg.Player.VoiceChannel.GuildId;
            System.Timers.Timer ConnectionTimer = new System.Timers.Timer();
            ConnectionTimer.Interval = 300 * 1000;
            ConnectionTimer.Enabled = false;
            ConnectionTimer.Elapsed += async (s, e) =>
            {
                ConnectionTimer.Stop();
                var voiceChannel = arg.Player.VoiceChannel;
                if (voiceChannel != null)
                {
                    await _lavaNode.LeaveAsync(voiceChannel);
                }
                else
                {
                    ConnectionTimer.Dispose();
                }

            };
            if (!ConnectionTimerHandler.guildConnecionTimer.ContainsKey(guildId))
            {                
                ConnectionTimerHandler.guildConnecionTimer.Add(guildId, ConnectionTimer);
            }
            else
            {
                ConnectionTimerHandler.guildConnecionTimer[guildId] = ConnectionTimer;
            }
            ConnectionTimerHandler.StartTimer(guildId);
            return Task.CompletedTask;
        }

        private static async Task OnUserVoiceStateUpdated(SocketUser user, SocketVoiceState before, SocketVoiceState after)
        {
            if (before.VoiceChannel is null) return;
            else if (!_lavaNode.HasPlayer(before.VoiceChannel.Guild)) return;
            else if (_lavaNode.HasPlayer(before.VoiceChannel.Guild))
            {

                var player = _lavaNode.GetPlayer(before.VoiceChannel.Guild);
                var vc = player.VoiceChannel as SocketVoiceChannel;
                if (vc.ConnectedUsers.Count == 1)
                {
                    try
                    {
                        if (player.PlayerState is Victoria.Enums.PlayerState.Playing) await player.StopAsync();
                        await _lavaNode.LeaveAsync(player.VoiceChannel);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR {ex.Message}");
                    }
                }
                
        }
            return;                  
        }


        private static async Task OnMessageReceived(SocketMessage msg)
        {

            var message = msg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            var argPos = 0;
            IResult result;
            if (message.Author.IsBot || message.Channel is IDMChannel 
                || message.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;


            if (!(message.HasStringPrefix(ConfigHandler.Config.Prefix, ref argPos)))
            {
                var krLength = ConfigHandler.Config.KrPrefix.Length;
                if (message.Content.Substring(0, krLength+1) == ConfigHandler.Config.KrPrefix + " ")
                {
                    result = await _commandService.ExecuteAsync(context, "말", ServiceHandler.ServiceProvider);
                }
                else return;

            }
            else
            {
                result = await _commandService.ExecuteAsync(context, argPos, ServiceHandler.ServiceProvider);
            };

            if (!result.IsSuccess)
            {
                if (result.Error == CommandError.UnknownCommand) return;
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
            await _client.SetGameAsync($"{ConfigHandler.Config.KrPrefix} \"메시지\" 로 말하기 / Prefix: {ConfigHandler.Config.Prefix}",
                streamUrl: null, ActivityType.Playing);
        }


    }
}
