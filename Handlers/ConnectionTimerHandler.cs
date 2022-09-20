using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Victoria;

namespace KrTTSBot.Handlers
{
    public class ConnectionTimerHandler
    {
        public static Dictionary<ulong, System.Timers.Timer> guildConnecionTimer = new Dictionary<ulong, System.Timers.Timer>();

        public static void StartTimer(ulong guildId)
        {
            if(guildConnecionTimer.TryGetValue(guildId, out var timer))
            {
                timer.Start();
            }
        }

        public static void StopTimer(ulong guildId)
        {
            if (guildConnecionTimer.TryGetValue(guildId, out var timer))
            {
                timer.Stop();
                timer.Dispose();
            }
        }

    }
}
