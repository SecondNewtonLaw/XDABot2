using Discord;
using Masked.DiscordNet;

namespace DiscordBot;

public static class CommandLoader
{
    public static CommandHelper LoadCommands(CommandHelper commandHelper)
    {
        commandHelper.AddToCommandList(new SlashCommandBuilder
        {
            Name = "ping",
            Description = "A simple command to show the latency between the server and the client (Discord Gateway) <---> (Bot)",
        }.Build(), Commands.Ping);

        var gCommand = new SlashCommandBuilder
        {
            Name = "google",
            Description = "Search something in Google",
        };
        gCommand.AddOption("query", ApplicationCommandOptionType.String, "The query to search for in google", true);

        commandHelper.AddToCommandList(gCommand.Build(), Commands.Google);

        var xdaCommand = new SlashCommandBuilder
        {
            Name = "xda",
            Description = "Search something in XDA Forums",
        };
        xdaCommand.AddOption(name: "query", ApplicationCommandOptionType.String, "The query to search for in XDA Forums", true);
        xdaCommand.AddOption(name: "order", ApplicationCommandOptionType.String, "How should the answers be ordered", true, null, true);
        xdaCommand.AddOption(name: "backend", ApplicationCommandOptionType.String, "What backend should the bot use", true, null, true);

        commandHelper.AddToCommandList(xdaCommand.Build(), Commands.XDADevelopers);
        return commandHelper;
    }
}