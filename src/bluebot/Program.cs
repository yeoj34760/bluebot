using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Audio;
using Microsoft.Extensions.DependencyInjection;
using bluebot.Services;
using Discord.Addons.Interactive;

namespace bluebot
{
    class Program
    {
        public static void Main(string[] args)
             => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync() 
        {
            // You should dispose a service provider created using ASP.NET
            // when you are finished using it, at the end of your app's lifetime.
            // If you use another dependency injection framework, you should inspect
            // its documentation for the best way to do this.
            IServiceProvider services = ConfigureServices();
                var client = services.GetRequiredService<DiscordSocketClient>();

                client.Log += LogAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;

                // Tokens should be considered secret data and never hard-coded.
                // We can read from the environment variable to avoid hardcoding.
                await client.LoginAsync(TokenType.Bot, JsonService.Config.Token);
                await client.StartAsync();

                // Here we initialize the logic required to register our commands.
                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                await Task.Delay(-1);
            
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());

            return Task.CompletedTask;
        }

        private IServiceProvider ConfigureServices()
        {
            ServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddSingleton<DiscordSocketClient>();
            serviceDescriptors.AddSingleton<CommandService>();
            serviceDescriptors.AddSingleton<CommandHandlingService>();
            serviceDescriptors.AddSingleton<MusicService>();
            serviceDescriptors.AddSingleton<InteractiveService>();
            return serviceDescriptors.BuildServiceProvider();
        }
    }
}
