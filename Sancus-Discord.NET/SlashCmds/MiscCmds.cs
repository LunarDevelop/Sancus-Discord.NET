using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json;
using Sancus_Discord.NET.Elements;

namespace Sancus_Discord.NET.SlashCmds;

public class MiscCmds : InteractionModuleBase
{
    private readonly InteractionService _interaction;
    private readonly DiscordSocketClient _client;

    public MiscCmds(InteractionService interaction, 
        DiscordSocketClient client)
    {
        _interaction = interaction;
        _client = client;
    }

    [SlashCommand("ping", "Latency of the bot")]
    public async Task PingCmd()
    {
        var generalPingField = new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "General Ping",
            Value = _client.Latency
        };
        
        var embed = new LunarEmbed()
        {
            Color = LunarEmbed.InfoColor,
            Title = "Ping Cmd",
            Description = "Pong!",
            Fields = new List<EmbedFieldBuilder>()
            {
                generalPingField
            }
        };
        await RespondAsync(embed: embed.Build());
    }

    [SlashCommand("create-poll", "Create a strawpoll that people can vote on.")]
    public async Task CreatePoll() => await RespondWithModalAsync<CreatePollModal>("create_poll_modal");

    [ModalInteraction("create_poll_modal")]
    public async Task ModalResponse(CreatePollModal modal)
    {
        var uri = "https://api.strawpoll.com/v3/polls";

        var answers =  modal.Answers.Split(",");

        var pollAnswers = new List<Dictionary<string, object>>();

        foreach (var answer in answers)
        {
            pollAnswers.Add(new Dictionary<string, object>()
            {
                {"type", "text"},
                {"value", answer}
            });
        }
        
        var body = new Dictionary<string, object>()
        {
            { "title", modal.Question },
            { "poll_config", new Dictionary<string, object>()
            {
                {"is_private", true}
            }},
            { "poll_options", pollAnswers }
        };

        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(uri);

        var req = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            Content = new StringContent(JsonConvert.SerializeObject(body))
        };

        var res = httpClient.Send(req);
        var pollObj = JsonConvert.DeserializeObject<Dictionary<string, Object>>(await res.Content.ReadAsStringAsync());

        var pollId = pollObj["id"];
        var pollUri = $"https://strawpoll.com/polls/{pollId}";
        
        await RespondAsync($"Your poll link is: {pollUri}"); 
    }
}

public class CreatePollModal : IModal
{
    public string Title => "Create Poll";

    [InputLabel("Question?")]
    [ModalTextInput("poll_question", placeholder: "What is your question?")]
    public string Question { get; set; } = string.Empty;

    [InputLabel("Comma seperated list of answers.")]
    [ModalTextInput("poll_answers", placeholder: "yes, no, maybe")]
    public string Answers { get; set; } = string.Empty;
}