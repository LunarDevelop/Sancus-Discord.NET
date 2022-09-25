using Discord.Interactions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sancus_Discord.NET.Elements;

namespace Sancus_Discord.NET.SlashCmds;

public class MovieCmds : InteractionModuleBase
{
    private readonly IConfiguration _config;
    private readonly string token;
    
    public MovieCmds(IConfiguration config)
    {
        _config = config;
        token = config["Movies:Token"];
    }

    [SlashCommand("movie", "Get a recommendation on a movie from a genre")]
    public async Task GetMovieRecommendation(
        [Choice("Action", 28), 
         Choice("Adventure", 12), 
         Choice("Animation", 16),
         Choice("Comedy", 35),
         Choice("Crime", 80),
         Choice("Documentary", 99),
         Choice("Drama", 18),
         Choice("Family", 10751),
         Choice("Fantasy", 14),
         Choice("History", 36),
         Choice("Horror", 27),
         Choice("Music", 10402),
         Choice("Mystery", 9648),
         Choice("Romance", 10749),
         Choice("Science Fiction", 878),
         Choice("TV Movie", 10770),
         Choice("Thriller", 53),
         Choice("War", 10752),
         Choice("Western", 37)] int genreId)
    {
        var httpClient = new HttpClient();

        var getPagesReq = new HttpRequestMessage()
        {
            RequestUri = new Uri($"https://api.themoviedb.org/3/discover/movie?with_genres={genreId}&api_key={token}")
        };
        var getPagesRes = await httpClient.SendAsync(getPagesReq);
        var resObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(await getPagesRes.Content.ReadAsStringAsync());
        var totalPages = Int64.Parse(resObj["total_pages"].ToString());
        var random = new Random();
        long page;
        
        if ((int)totalPages > 500)
        {
            page = random.Next(500);
        }
        else
        {
            page = random.NextInt64(totalPages);
        }

        var getPageReq = new HttpRequestMessage()
        {
            RequestUri = new Uri($"https://api.themoviedb.org/3/discover/movie?with_genres={genreId}&api_key={token}&page={page}")
        };
        
        var getPageRes = await httpClient.SendAsync(getPageReq);
        var pageResObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(await getPageRes.Content.ReadAsStringAsync());
        var resultList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(pageResObj["results"].ToString());
        var result = resultList[random.Next(resultList.Count())];

        var title = (string)result["title"];
        var overview = (string)result["overview"];
        var imagePath = (string)result["poster_path"];

        var embed = new LunarEmbed()
        {
            Author = new()
            {
                Name = "Provided by the movie DB",
                Url = "https://www.themoviedb.org/"
            },
            Title = title,
            Description = overview,
            Color = LunarEmbed.InfoColor
        };

        if (imagePath != null)
        {
            embed.ThumbnailUrl = $"https://www.themoviedb.org/t/p/w500/{imagePath}";
        }
        
        await RespondAsync(embed: embed.Build());
    }
}