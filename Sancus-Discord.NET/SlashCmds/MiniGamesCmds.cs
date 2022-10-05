using Discord.Interactions;
using Discord.WebSocket;

namespace Sancus_Discord.NET.SlashCmds;

public class MiniGamesCmds : InteractionModuleBase
{
    private readonly InteractionService _interaction;
    private Random _random = new Random();
    
    public MiniGamesCmds(InteractionService interaction)
    {
        _interaction = interaction;
    }

    [SlashCommand("rock_paper_scissors", "Play a classic game of Rock Paper Scissors with the bot")]
    public async Task RockPaperScissors(
        [Choice("Rock", "r"), Choice("Paper", "p"), Choice("Scissors", "s")] string option)
    {
        string[] choices = new[] { "r", "p", "s" };
        string botChoice = choices[_random.Next(choices.Length)];

        await RespondAsync(botChoice);
    }
}