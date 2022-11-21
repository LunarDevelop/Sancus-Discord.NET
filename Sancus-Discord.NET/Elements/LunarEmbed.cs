using Discord;

namespace Sancus_Discord.NET.Elements;

public class LunarEmbed : EmbedBuilder
{
    public static Color SuccessColor = new Color(0x33FF7D);
    public static Color ErrorColor = new Color(0xF64545);
    public static Color InfoColor = new Color(0x4BDCE9);

    public LunarEmbed()
    {
        Footer = new EmbedFooterBuilder()
        {
            Text = "By Lunar Development",
            // TODO eventually switch this to use the Lunar CDN for the icon
            IconUrl = "https://cdn.discordapp.com/attachments/789247201678327838/879862055404965938/stage_1629845776.jpeg"
        };
        Timestamp = DateTimeOffset.Now;
    }
}