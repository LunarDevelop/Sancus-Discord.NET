using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sancus_Discord.NET;

public class Program
{
    private readonly IConfiguration _config;
    private readonly IServiceProvider _serviceProvider;

    public Program()
    {
        _config = CreateConfiguration();
        _serviceProvider = CreateProvider();
    }
    
    public static Task Main(string[] args) => new Program().MainAsync();

    private static IConfiguration CreateConfiguration()
    {
        // Builds a config for getting secrets from settings 
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            #if DEBUG
            .AddJsonFile($"appsettings.Development.json", optional: false);
            #else
            .AddJsonFile($"appsetting.Production.json", optional: false);
            #endif
        return builder.Build();
    }

    private static IServiceProvider CreateProvider()
    {
        var socketConfig = new DiscordSocketConfig();

        var interactionConfig = new InteractionServiceConfig()
        {
            AutoServiceScopes = true
        };
        
        var collection = new ServiceCollection()
            .AddSingleton(socketConfig)
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton(interactionConfig)
            .AddSingleton<InteractionService>();
        
        return collection.BuildServiceProvider();
    }
    
    private async Task MainAsync()
    {
        var client = _serviceProvider.GetRequiredService<DiscordSocketClient>();

        var unused = new LoggingService(client);

        client.Ready += OnReady;

        var token = _config["Discord:Token"];
        await client.LoginAsync(TokenType.Bot, token);
        
        await client.StartAsync();
        
        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private async Task OnReady()
    {
        var client = _serviceProvider.GetRequiredService<DiscordSocketClient>();
        var interactionService = _serviceProvider.GetRequiredService<InteractionService>();

        await interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
        
        #if DEBUG   
        await interactionService.RegisterCommandsToGuildAsync(780211278614364160);
        #else   
        await interactionService.RegisterCommandsGloballyAsync();
        #endif  
        
        client.InteractionCreated += async interaction =>
        {
            var ctx = new SocketInteractionContext(client, interaction);
            await interactionService.ExecuteCommandAsync(ctx, _serviceProvider);
        };

    }
}