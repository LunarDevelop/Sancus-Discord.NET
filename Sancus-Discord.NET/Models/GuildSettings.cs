using System.ComponentModel.DataAnnotations;
using MongoDbService.Models;

namespace Sancus_Discord.NET.Models;

public class GuildSettings : EntityBase
{
    [Required]
    public ulong GuildId { get; set; }

    public bool UserJoinLog { get; set; } = false;
    public ulong? UserJoinedLogChannel { get; set; }

    public bool MessageEditLog { get; set; } = false;
    public ulong? MessageEditLogChannel { get; set; }
}