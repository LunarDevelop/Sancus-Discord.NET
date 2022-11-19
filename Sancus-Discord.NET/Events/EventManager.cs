using Discord.WebSocket;

namespace Sancus_Discord.NET.Events;

public static class EventManager
{
    
    public static async Task UserJoined(SocketGuildUser user)
    {
        var acceptGuilds = new List<ulong>()
        {
            780211278614364160
        };

        if (!acceptGuilds.Contains(user.Guild.Id))
            return;
        
        if (user.Guild.Id == 780211278614364160)
        {
            throw new NotImplementedException();
        }
    }
}