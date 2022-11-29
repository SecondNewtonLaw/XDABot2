using Masked.Scraper.SearchEngines;

public class ScraperAbstractor
{
    /// <summary>
    /// Obtains the search results of the scraped google query in a fast and easy way. Used for quick requests only.
    /// </summary>
    /// <param name="googleQuery">The query</param>
    /// <param name="googleURL">Custom Google URL, if any.</param>
    /// <returns></returns>
    public static List<ScrapedSearchResult>? GetGoogleQuery(string googleQuery, string googleURL = "https://google.com/search?gl=us&")
    {
        if (googleURL.Last() != '&')
            googleURL += "&"; // Add the &.
        else if (googleURL.Last() == '/')
            googleURL += "?"; // Add the ?, as no apparent other items passed as url params.

        FormUrlEncodedContent url = new(new Dictionary<string, string>()
        {
            ["q"] = googleQuery
        });

        GoogleScraper googleScraper = new(new Uri($"https://google.com/search?gl=us&{url.ReadAsStringAsync().Result}"));

        try
        {
            return googleScraper.GetResults();
        }
        catch (NoResultsException)
        {
            return null;
        }
    }
}