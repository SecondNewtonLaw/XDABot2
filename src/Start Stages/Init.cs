using System;
using System.Linq;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Masked.DiscordNet;
using Masked.Sys.Extensions;
using Spectre.Console;

namespace DiscordBot;

public partial class StartStage
{
    private readonly string token = File.ReadAllText("./TOKEN");
    public async Task Initialization(CommandHelper commandHelper)
    {
        AnsiConsole.MarkupLine("[maroon][[Init/INFO]] Running [green]Initialization[/].[/]");

        // Register listeners to events.

        AnsiConsole.MarkupLine("[maroon][[Init/INFO]] [yellow italic bold]Registering event listeners[/]...[/]");

        // Load commands.
        Shared.DiscordClient.SlashCommandExecuted += commandHelper.GetSlashCommandHandler();
        AnsiConsole.MarkupLine("[maroon][[Init/INFO]] [yellow italic bold]Registered listener for event [red bold underline]SlashCommandExecuted[/][/]...[/]");

        Shared.DiscordClient.Log += log =>
        {
            AnsiConsole.MarkupLine($"[[Discord.Net Log]] [red bold underline]{log.ToString().EscapeMarkup()}[/]");
            return Task.CompletedTask;
        };
        AnsiConsole.MarkupLine("[maroon][[Init/INFO]] [yellow italic bold]Registered listener for event [red bold underline]Log[/][/]...[/]");

        AnsiConsole.MarkupLine("[maroon][[Init/INFO]] [yellow italic bold]Logging in[/] with [red bold underline]token[/]...[/]");

        // Start Bot
        await Shared.DiscordClient.LoginAsync(TokenType.Bot, token: token, validateToken: true)
        .ContinueWith(async x =>
        {
            await x; // await previous task
            AnsiConsole.MarkupLine("[maroon][[Init/INFO]] [yellow italic bold]Starting Up[/]...[/]");
            await Shared.DiscordClient.StartAsync();
        }).ContinueWith(async x =>
        {
            await x;
            Shared.DiscordClient.GuildAvailable += async guild =>
            {
                // The bot is not Administrator, assume the Guild is a Production server, not testing server.
                if (!guild.GetUser(Shared.DiscordClient.CurrentUser.Id).Roles.Any(x => x.Permissions.Administrator))
                {
                    AnsiConsole.MarkupLine($"[maroon][[Init/WARN]] [yellow italic bold][/]Warning: Guild {guild.Name} has the bot without Administrative rights, not injecting developer command builder.[/]");
                    return;
                }

                AnsiConsole.MarkupLine("[maroon][[Init/INFO]] [yellow italic bold]Warming up Commands[/]...[/]");
                // SubmitCommandBuilder will only be done if the bot is ADMINISTRATOR else not.
                await commandHelper.SubmitCommandBuilder(guild);
            };
        });
    }
}