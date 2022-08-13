using System;

namespace KrTTSBot
{
    class Program
    {
        public static void Main(string[] args)
            => new Bot().MainAsync().GetAwaiter().GetResult();
    }
}