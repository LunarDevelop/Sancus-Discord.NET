using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Sancus_Discord.NET.Elements;

namespace Sancus_Discord.NET.SlashCmds;

public class MiscCmds : InteractionModuleBase
{
    private readonly InteractionService _interaction;
    private readonly DiscordSocketClient _client;

    public MiscCmds(InteractionService interaction, 
        DiscordSocketClient client)
    {
        _interaction = interaction;
        _client = client;
    }

    [SlashCommand("ping", "Latency of the bot")]
    public async Task PingCmd()
    {
        var generalPingField = new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "General Ping",
            Value = _client.Latency
        };
        
        var embed = new LunarEmbed()
        {
            Color = LunarEmbed.InfoColor,
            Title = "Ping Cmd",
            Description = "Pong!",
            Fields = new List<EmbedFieldBuilder>()
            {
                generalPingField
            }
        };
        await ReplyAsync(embed: embed.Build());
    }
}