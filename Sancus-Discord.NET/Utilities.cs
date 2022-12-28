using Discord;
using Discord.WebSocket;
using MongoDbService;
using Sancus_Discord.NET.Models;

namespace Sancus_Discord.NET;

public class Utilities
{
    private readonly IEntityBaseRepository<GuildSettings> _guildSettings;

    public Utilities(IEntityBaseRepository<GuildSettings> guildSettings)
    {
        _guildSettings = guildSettings;
    }

    public GuildSettings GetGuildSettings(IGuild guild)
    {
        var settingsList = _guildSettings.SearchFor(x => x.GuildId == guild.Id).ToList();
        if (settingsList.Count == 0)
            return new GuildSettings()
            {
                GuildId = guild.Id
            };
        return settingsList[0];
    }
}