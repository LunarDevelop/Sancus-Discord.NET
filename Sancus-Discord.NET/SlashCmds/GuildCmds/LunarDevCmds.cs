using Discord.Interactions;

namespace Sancus_Discord.NET.SlashCmds.GuildCmds;

public class LunarDevCmds : InteractionModuleBase
{
    private readonly InteractionService _interaction;
    
    public LunarDevCmds(InteractionService interaction)
    {
        _interaction = interaction;
    }
    
    
}