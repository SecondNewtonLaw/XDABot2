using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using Discord;
using Discord.WebSocket;

namespace DiscordBot;

/// <summary>
/// Class containing objects that are shared throughout the program.
/// </summary>
public static class Shared
{
    /// <summary>
    /// The Discord Client that communicates with the Discord API.
    /// </summary>
    public static DiscordSocketClient DiscordClient { get; } = new(new DiscordSocketConfig()
    {
        // During development we want  V e r b o s e  logs to diagnose stuff
        LogLevel = LogSeverity.Debug,
        AlwaysDownloadUsers = true,
        GatewayIntents = GatewayIntents.AllUnprivileged,
        LogGatewayIntentWarnings = true,

        // Wait for servers in a time frame of 25 seconds, else fire READY event anyways.
        MaxWaitBetweenGuildAvailablesBeforeReady = 25000,

        // Small cache.
        MessageCacheSize = 15,
    });
    /// <summary>
    /// HTTPClient used to make any type of HTTP requests.
    /// </summary>
    public static HttpClient HttpClient { get; } = new(new HttpClientHandler()
    {
        SslProtocols = SslProtocols.Tls12,
        UseCookies = false,
        UseProxy = false,
        AutomaticDecompression = DecompressionMethods.All,
        AllowAutoRedirect = true,
    });
    /// <summary>
    /// Determines if the program is set to 'testing mode'
    /// </summary>
    public static bool TestingMode { get; private set; } = false;
    /// <summary>
    /// Inverse the state of TestingMode,
    /// if TestingMode is true it will become false,
    /// if TestingMode is false it will become true.
    /// </summary>
    public static void ChangeTestingMode()
    {
#if DEBUG
        // If it is on release mode, this method will not do anything.
        // Also known as, disable testing mode completely if not on debug.
        TestingMode = !TestingMode;
#endif
    }
}