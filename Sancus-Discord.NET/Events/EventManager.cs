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

        if (guildSetting is not { UserJoinLog: true }) return;

        var message = new LunarEmbed()
        {
            Title = $"Welcome {user.DisplayName} to the server",
            Color = LunarEmbed.InfoColor,
            ThumbnailUrl = user.GetDisplayAvatarUrl() ?? user.GetDefaultAvatarUrl()
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
            Author = new()
            {
                IconUrl = after.Author.GetAvatarUrl(),
                Name = after.Author.Username,
                Url = after.GetJumpUrl()
            },
            Title = $"A message has been edited",
            Color = LunarEmbed.InfoColor,
            Fields = new()
            {
                new EmbedFieldBuilder
                {
                    Name = "Old message",
                    Value = $"{beforeMessageContent}",
                    IsInline = false
                },
                new EmbedFieldBuilder
                {
                    Name = "Edited message",
                    Value = $"{after}",
                    IsInline = false
                }
            }
        };

        if (guildChannel.Guild.Channels.Where(x => x.Id == guildSetting.MessageEditLogChannel)
                .First() is SocketTextChannel logChannel)
            await logChannel.SendMessageAsync(embed: returnMessage.Build());
    }
}