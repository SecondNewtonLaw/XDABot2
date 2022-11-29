using System;
using System.Linq;
using Discord;
using Masked.Sys.Extensions;
using Spectre.Console;

namespace DiscordBot;

public partial class StartStage
{
    public async Task PostInitialization()
    {
        AnsiConsole.MarkupLine("[maroon][[INFO]] Running [yellow]Post-Initialization[/].[/]");

        AnsiConsole.MarkupLine("[maroon][[INFO/Post-Init]] [red]Injecting[/] [yellow]Autocompletition Service[/] for 'order' option of command [red bold underline]xda[/].[/]");

        // ORDER OF SEARCH AUTOCOMPLETE XDA CMD.
        Shared.DiscordClient.AutocompleteExecuted += x =>
        {
            if (x.Data.CommandName != "xda" || x.Data.Current.Name != "order")
                return Task.CompletedTask; // Early return.

            AutocompleteResult result = new();
            AutocompleteResult result2 = new();

            result.Name = "Order by Date";
            result2.Name = "Order by Relevance (Default)";

            result.Value = "Date";
            result2.Value = "Relevance";

            x.RespondAsync(RequestOptions.Default, result, result2);
            return Task.CompletedTask;
        };

        AnsiConsole.MarkupLine("[maroon][[INFO/Post-Init]] [yellow]Autocompletition Service[/] [red]Injected[/].[/]");

        AnsiConsole.MarkupLine("[maroon][[INFO/Post-Init]] [red]Injecting[/] [yellow]Autocompletition Service[/] for 'backend' option of command [red bold underline]xda[/].[/]");
        // BACKEND AUTOCOMPLETE XDA CMD
        Shared.DiscordClient.AutocompleteExecuted += x =>
        {
            if (x.Data.CommandName != "xda" || x.Data.Current.Name != "backend")
                return Task.CompletedTask; // Early return.

            AutocompleteResult result = new();
            AutocompleteResult result2 = new();

            result.Name = "Official Search Backend";
            result2.Name = "Custom Search Backend (Default)";

            result.Value = "Official";
            result2.Value = "Custom";

            x.RespondAsync(RequestOptions.Default, result, result2);
            return Task.CompletedTask;
        };
        AnsiConsole.MarkupLine("[maroon][[INFO/Post-Init]] [yellow]Autocompletition Service[/] [red]Injected[/].[/]");

    }
}