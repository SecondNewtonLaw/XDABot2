using System.Collections.Specialized;
using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using Masked.DiscordNet.Extensions;
using Masked.Scraper.SearchEngines;
using Spectre.Console;

namespace DiscordBot;

#pragma warning disable MA0004 // Disable -> Use ConfigurateAwait(false); as no SyncCtx is needed.
public static partial class Commands
{
    /// <summary>
    /// A command that googles something in the internet.
    /// </summary>
    /// <param name="sockCommand">The interaction socket.</param>
    /// <returns>A Task representing the on-going asynchronous operation</returns>
    public static async Task Google(SocketSlashCommand sockCommand)
    {
        // Defer.
        await sockCommand.DeferAsync();
        var illegal = (await File.ReadAllTextAsync("ILLEGALWORDS")).Split(',');

        string query = (string)sockCommand.Data.Options.First().Value;
        var illegalQuery = query.HasIllegal(illegal);

        // Respond.
        var ogAns = await sockCommand.FollowupAsync("Processing Query...");

        if (illegalQuery)
        {
            await ogAns.ModifyAsync(x => x.Content = "Oops. Searching for that is not allowed. :sob: :x:");
            return;
        }
        System.Net.Http.FormUrlEncodedContent url = new(new Dictionary<string, string>()
        {
            ["q"] = query
        });
        GoogleScraper googleScraper = new(new Uri($"https://google.com/search?gl=us&{await url.ReadAsStringAsync()}"));

        var queryResults = googleScraper.GetResultsAsync();
        try
        {
            await queryResults;
        }
        catch (NoResultsException)
        {
            await ogAns.ModifyAsync(x => x.Content = $"Query Failed: No results found for '{query}'");
            return;
        }

        EmbedBuilder searchResults = new();
        int cnt = 0;
        const int MAX_CNT = 5;
        searchResults.SetRandomColor();
        searchResults.Title = $"Search Results for '{query}'";

        queryResults.Result.ToArray().FastIterator(async (x, _) =>
        {
            if (cnt == MAX_CNT)
                return;

            if (x.URL.HasIllegal(illegal) || x.Title!.HasIllegal(illegal))
            {
                await ogAns.ModifyAsync(x => x.Content = "Oops. The search results contained things that were not allowed! :sob: :x:");
                return;
            }

            searchResults.AddField(x.Title, x.URL);
            cnt++;
        });

        /*for (int i = 0; i < queryResults.Result.Count; i++)
        {
            if (cnt == MAX_CNT)
                break;

            if (queryResults.Result[i]!.URL.HasIllegal(illegal) || queryResults!.Result[i].Title!.HasIllegal(illegal))
            {
                await ogAns.ModifyAsync(x => x.Content = "Oops. The search results contained things that were not allowed! :sob: :x:");
                return;
            }

            searchResults.AddField(queryResults.Result[i].Title, queryResults.Result[i].URL);
            cnt++;
        }*/

        await ogAns.ModifyAsync(x =>
        {
            x.Content = "";
            x.Embed = searchResults.Build();
        });
    }
}