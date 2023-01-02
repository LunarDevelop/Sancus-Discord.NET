using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MongoDbService;
using Sancus_Discord.NET.Elements;
using Sancus_Discord.NET.Models;

namespace Sancus_Discord.NET.SlashCmds;

[EnabledInDm(false)]
[DefaultMemberPermissions(GuildPermission.ManageGuild)]
[Group("admin", "Admin commands can be found here.")]
public class AdminCmds : InteractionModuleBase
{
    private readonly InteractionService _interaction;
    private readonly IEntityBaseRepository<GuildSettings> _guildSettings;
    private readonly IEntityBaseRepository<BotSettings> _botSettings;
    private readonly Utilities _utilities;
    
    public AdminCmds(InteractionService interaction, IServiceProvider services, Utilities utilities)
    {
        _interaction = interaction;
        _utilities = utilities;

        var scope = services.CreateScope();
        var provider = scope.ServiceProvider;
        _guildSettings = provider.GetRequiredService<IEntityBaseRepository<GuildSettings>>();
    }

    [Group("users", "User settings")]
    public class UserSettings: InteractionModuleBase
    {
        private readonly InteractionService _interaction;
        private readonly IEntityBaseRepository<GuildSettings> _guildSettings;
        private readonly IEntityBaseRepository<BotSettings> _botSettings;
        private readonly Utilities _utilities;

        public UserSettings(InteractionService interaction, IServiceProvider services, Utilities utilities)
        {
            _interaction = interaction;
            _utilities = utilities;

            var scope = services.CreateScope();
            var provider = scope.ServiceProvider;
            _guildSettings = provider.GetRequiredService<IEntityBaseRepository<GuildSettings>>();
        }

        [SlashCommand("joinlogging", "Enable/Disable user joining logging")]
        public async Task JoinLoggingToggle(StandardEnums.TrueFalseChoice choice)
        {
            var guild = Context.Guild;
            var guildSettings = _utilities.GetGuildSettings(guild);

            guildSettings.UserJoinLog = choice.Value();
            _guildSettings.Update(guildSettings);

            var message = new LunarEmbed()
            {
                Title = $"User joining logging settings have been updated",
                Color = LunarEmbed.InfoColor,
            };

            await RespondAsync(embed: message.Build());
        }

        [SlashCommand("joinchannel", "Change User Join Log Channel")]
        public async Task JoinLogChannelChange(SocketTextChannel newChannel)
        {
            try
            {
                var joinChannelMessage = new LunarEmbed()
                {
                    Title = "Welcome channel!",
                    Description = "This is the channel where I will welcome new users to the channel",
                    Color = LunarEmbed.InfoColor
                };
                await newChannel.SendMessageAsync(embed: joinChannelMessage.Build());
            }
            catch (Exception e)
            {
                var errorMessage = new LunarEmbed()
                {
                    Title = "Permission Error",
                    Description = "I am missing the permissions to write to that channel",
                    Color = LunarEmbed.ErrorColor
                };
                await RespondAsync(embed: errorMessage.Build());
                Console.WriteLine(e);
                return;
            }

            var guild = Context.Guild;
            var guildSettings = _utilities.GetGuildSettings(guild);

            guildSettings.UserJoinedLogChannel = newChannel.Id;
            _guildSettings.Update(guildSettings);

            var message = new LunarEmbed()
            {
                Title = $"User join log channel updated",
                Color = LunarEmbed.InfoColor,
            };


            await RespondAsync(embed: message.Build());
        }

        [SlashCommand("leftchannel", "Change User Leave Log Channel")]
        public async Task UserLeaveLogChannelChange(SocketTextChannel newChannel)
        {
            try
            {
                var joinChannelMessage = new LunarEmbed()
                {
                    Title = "Leaving channel!",
                    Description = "This is the channel where I will let you know who has left the server",
                    Color = LunarEmbed.InfoColor
                };
                await newChannel.SendMessageAsync(embed: joinChannelMessage.Build());
            }
            catch (Exception e)
            {
                var errorMessage = new LunarEmbed()
                {
                    Title = "Permission Error",
                    Description = "I am missing the permissions to write to that channel",
                    Color = LunarEmbed.ErrorColor
                };
                await RespondAsync(embed: errorMessage.Build());
                Console.WriteLine(e);
                return;
            }

            var guild = Context.Guild;
            var guildSettings = _utilities.GetGuildSettings(guild);

            guildSettings.UserLeftLogChannel = newChannel.Id;
            _guildSettings.Update(guildSettings);

            var message = new LunarEmbed()
            {
                Title = $"User left log channel updated",
                Color = LunarEmbed.InfoColor,
            };

            await RespondAsync(embed: message.Build());
        }

        [SlashCommand("leftlogging", "Enable/Disable user leaving logging")]
        public async Task LeftLoggingToggle(StandardEnums.TrueFalseChoice choice)
        {
            var guild = Context.Guild;
            var guildSettings = _utilities.GetGuildSettings(guild);

            guildSettings.UserLeftLog = choice.Value();
            _guildSettings.Update(guildSettings);

            var message = new LunarEmbed()
            {
                Title = $"User leaving logging settings have been updated",
                Color = LunarEmbed.InfoColor,
            };

            await RespondAsync(embed: message.Build());
        }
    }

}