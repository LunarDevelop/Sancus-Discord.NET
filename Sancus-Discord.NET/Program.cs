using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using fluxpoint_sharp;
using Sancus_Discord.NET.SlashCmds.GuildCmds;

namespace Sancus_Discord.NET;

public class Program
{
    public static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<ConsoleApplication>();
            })
            .RunConsoleAsync();
    }
}

public partial class ConsoleApplication : IHostedService {
    private readonly IConfiguration _config;
    private readonly IServiceProvider _serviceProvider;

    public ConsoleApplication()
    {
        _config = CreateConfiguration();
        _serviceProvider = CreateProvider();
    }
    

    private static IConfiguration CreateConfiguration()
    {
        // Builds a config for getting secrets from settings 
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", optional: true)
            #if DEBUG
            .AddJsonFile($"appsettings.Development.json", optional: true);
            #else
            .AddJsonFile($"appsettings.Production.json", optional: true);
            #endif
        
        return builder.Build();
    }

    private IServiceProvider CreateProvider()
    {
        var socketConfig = new DiscordSocketConfig();

        var interactionConfig = new InteractionServiceConfig()
        {
            AutoServiceScopes = true
        };

        var fluxClient = new FluxpointClient("Sancus", _config["FluxPoint:Token"]);

        var collection = new ServiceCollection()
            .AddSingleton(socketConfig)
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton(interactionConfig)
            .AddSingleton<InteractionService>()
            .AddSingleton(_config)
            .AddSingleton(fluxClient);

        return collection.BuildServiceProvider();
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var client = _serviceProvider.GetRequiredService<DiscordSocketClient>();

        var unused = new LoggingService(client);

        client.Ready += OnReady;
        
        // TODO: fix this so that it actually displays the messages or at least puts a message in the console
        client.UserJoined += UserJoined;

        var token = _config["Discord:Token"];
        await client.LoginAsync(TokenType.Bot, token);
        
        await client.StartAsync();
        
        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Console exited");
        return Task.CompletedTask;
    }

    private async Task OnReady()
    {
        var client = _serviceProvider.GetRequiredService<DiscordSocketClient>();

        var interactionService = _serviceProvider.GetRequiredService<InteractionService>();

        await interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);

        if (Convert.ToBoolean(_config["IsDev"]))
            await interactionService.RegisterCommandsToGuildAsync(780211278614364160);

        else
        {
            var lunarDevCmdsModule = interactionService.GetModuleInfo<LunarDevCmds>();
            await interactionService.AddModulesToGuildAsync(789941733998854176,
                true,
                lunarDevCmdsModule);
            await interactionService.RegisterCommandsGloballyAsync();
        }


        client.InteractionCreated += async interaction =>
        {
            var ctx = new SocketInteractionContext(client, interaction);
            await interactionService.ExecuteCommandAsync(ctx, _serviceProvider);
        };

    }
}