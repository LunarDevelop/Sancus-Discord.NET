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
        var choices = new List<string[]> { new[] {"r", "Rock"}, new [] { "p", "Paper" }, new [] { "s", "Scissors" } };
        var randomNumber = _random.Next(choices.Count);
        var botResponse = choices[randomNumber];

        // If user wins
        if (((botResponse[0] == "r") && (option == "p")) ||
            (botResponse[0] == "p" && option == "s") ||
            (botResponse[0] == "s" && option == "r"))
        {
            await RespondAsync($"Bot chose {botResponse[1]}\nYOU WON!!");
        }
        // If user and bot tie
        else if (botResponse[0] == option)
        {
            await RespondAsync($"Bot chose {botResponse[1]}\nYou have the same word, tie.");
        }
        //  If user loses
        else
        {
            await RespondAsync($"Bot chose {botResponse[1]}\nYou lost. :frowning:");
        }
    }

    [SlashCommand("flip_coin", "Get to flip a coin and guess the answer")]
    public async Task FlipCoin(
        [Choice("Heads", "h"), Choice("Tails", "t")] string userInput)
    {
        var choices = new List<string[]> { new[] {"h", "Heads"}, new[] { "t", "Tails" } };

        var botResponse = choices[_random.Next(choices.Count)];

        if (botResponse[0] == userInput)
        {
            await RespondAsync($"The coin flipped onto {botResponse[1]}\nYou guess correctly, well done!");
        }
        else
        {
            await RespondAsync($"The coin flipped onto {botResponse[1]}\nYou guessed incorrect, better luck next time.");
        }
    }
}