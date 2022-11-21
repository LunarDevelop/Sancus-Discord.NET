using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDbService;
using Sancus_Discord.NET.Elements;
using Sancus_Discord.NET.Models;

namespace Sancus_Discord.NET.Events;

public class EventManager
{
    private readonly IEntityBaseRepository<GuildSettings> _guildSettings;

    public EventManager(IServiceProvider services)
    {
        var scope = services.CreateScope();
        var provider = scope.ServiceProvider;
        _guildSettings = provider.GetRequiredService<IEntityBaseRepository<GuildSettings>>();
    }

    public async Task UserJoined(SocketGuildUser user)
    {
        var guildSetting = _guildSettings.SearchFor(x => x.GuildId == user.Guild.Id)[0];
        Console.WriteLine(guildSetting.GuildId);

        if (guildSetting is not { UserJoinLog: true }) return;

        var message = new LunarEmbed()
        {
            Author = new EmbedAuthorBuilder()
            {
                IconUrl = user.GetGuildAvatarUrl(),
                Name = user.Guild.Name
            },
            Title = $"Welcome {user.Nickname} to the server",
            ThumbnailUrl = user.GetDisplayAvatarUrl()
        };

        var logChannel = user.Guild.Channels.Where(x => x.Id == guildSetting.UserJoinedLogChannel)
            .First() as SocketTextChannel;

        if (logChannel is not null)
            await logChannel.SendMessageAsync(embed: message.Build());
    }

    public async Task MessageEdit(Cacheable<IMessage, ulong> before, SocketMessage after,
        ISocketMessageChannel channel)
    {
        var guildChannel = channel as SocketGuildChannel;
        var guildSetting = _guildSettings.SearchFor(x => x.GuildId == guildChannel.Guild.Id)[0];

        if (guildSetting is not { MessageEditLog: true }) return;

        var beforeMessageContent = await before.GetOrDownloadAsync();

        var returnMessage = new LunarEmbed()
        {
            Author = new EmbedAuthorBuilder()
            {
                IconUrl = after.Author.GetAvatarUrl(),
                Name = after.Author.Username
            },
            Title = $"A message has been edited, {after.Id}",
            Color = LunarEmbed.InfoColor,
            Fields = new List<EmbedFieldBuilder>()
            {
                new EmbedFieldBuilder()
                {
                    Name = "Old message",
                    Value = $"{beforeMessageContent}",
                    IsInline = false
                },
                new EmbedFieldBuilder()
                {
                    Name = "Edited message",
                    Value = $"{after}",
                    IsInline = false
                }
            }
        };
        var logChannel = guildChannel.Guild.Channels.Where(x => x.Id == guildSetting.MessageEditLogChannel)
            .First() as SocketTextChannel;

        if (logChannel is not null)
            await logChannel.SendMessageAsync(embed: returnMessage.Build());
    }
}