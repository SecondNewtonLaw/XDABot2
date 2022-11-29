using System.Collections.Specialized;
using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using Masked.DiscordNet.Extensions;
using Masked.Scraper.SearchEngines;
using Masked.Sys.Extensions;
using Spectre.Console;

namespace DiscordBot;

#pragma warning disable MA0004 // Disable -> Use ConfigurateAwait(false); as no SyncCtx is needed.
public static partial class Commands
{
    enum SearchBackend
    {
        Google, Official
    }
    /// <summary>
    /// A command that searchs something in the XDA Developers Forum.
    /// </summary>
    /// <param name="sockCommand">The interaction socket.</param>
    /// <returns>A Task representing the on-going asynchronous operation</returns>
    public static async Task XDADevelopers(SocketSlashCommand sockCommand)
    {
        // TODO: Make an alternative backend that uses Google.com and site:forum.xda-developers.com
        // Defer.
        await sockCommand.DeferAsync();
        var illegal = (await File.ReadAllTextAsync("ILLEGALWORDS")).Split(',');

        string query = (string)sockCommand.Data.Options.ElementAt(0).Value;
        XDASearchOrder searchOrder = ((string)sockCommand.Data.Options.ElementAt(1).Value) == "Date" ? XDASearchOrder.Date : XDASearchOrder.Relevance;
        SearchBackend searchBackend = (string)sockCommand.Data.Options.ElementAt(2).Value == "Custom" ? SearchBackend.Google : SearchBackend.Official;

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

        var scraper = searchBackend == SearchBackend.Google ? await GoogleBackend() : OfficialBackend();

        var queryResults = scraper.GetResultsAsync();

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

        for (int i = 0; i < queryResults.Result.Count; i++)
        {
            if (cnt == MAX_CNT)
                break;

            if (queryResults.Result[i]!.URL.HasIllegal(illegal) || queryResults!.Result[i].Title!.HasIllegal(illegal))
            {
                await ogAns.ModifyAsync(x => x.Content = "Oops. The search results contained things that were not allowed! :sob: :x:");
                return;
            }
            searchResults.AddField(queryResults.Result[i].Title?.RemoveIllegal(new string[] { "&amp" }), queryResults.Result[i]!.URL);
            cnt++;
        }

        await ogAns.ModifyAsync(x =>
        {
            x.Content = "";
            x.Embed = searchResults.Build();
        });

        ISearchScrape OfficialBackend()
        {
            return new XDADevelopersScraper(query, searchOrder);
        }

        async Task<ISearchScrape> GoogleBackend()
        {
            System.Net.Http.FormUrlEncodedContent gurl = new(new Dictionary<string, string>()
            {
                ["q"] = query + " site:forum.xda-developers.com"
            });
            return new GoogleScraper(new Uri($"https://google.com/search?gl=us&{await gurl.ReadAsStringAsync()}"));
        }
    }
}