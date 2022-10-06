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
        // Sets arrays for each object 
        string[] choices = new[] { "r", "p", "s" };
        string[] choicesName = new[] { "Rock", "Paper", "Scissors" };
        int randomNumber = _random.Next(choices.Length);
        string botChoice = choices[randomNumber];
        string botWord = choicesName[randomNumber];

        // If user wins
        if (((botChoice == "r") && (option == "p")) ||
            (botChoice == "p" && option == "s") ||
            (botChoice == "s" && option == "r"))
        {
            await RespondAsync($"Bot choose {botWord}\nYOU WON!!");
        }
        // If user and bot tie
        else if (botChoice == option)
        {
            await RespondAsync($"Bot choose {botWord}\nYou have the same word, tie.");
        }
        //  If user loses
        else
        {
            await RespondAsync($"Bot choose {botWord}\nYou lost, :(");
        }
    }
}