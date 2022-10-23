using Discord;
using Discord.WebSocket;

namespace Sancus_Discord.NET;

public partial class ConsoleApplication
{
    // Todo: write message to channel for information
    private async Task UserJoined(SocketGuildUser user)
    {
        Console.WriteLine("TEST");
        if (user.Guild.Id == 780211278614364160)
        {
            throw new NotImplementedException();
        }
    }
}