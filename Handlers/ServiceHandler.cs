using Microsoft.Extensions.DependencyInjection;

namespace KrTTSBot.Handlers
{
    public class ServiceHandler
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void SetProvider(ServiceCollection collection)
            => ServiceProvider = collection.BuildServiceProvider();

        public static T GetService<T>() where T : new()
            => ServiceProvider.GetRequiredService<T>();
    }
}
