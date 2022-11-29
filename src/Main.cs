using System;
using System.Linq;
using Masked.DiscordNet;
using Masked.Sys.Extensions;
using Spectre.Console;

namespace DiscordBot;

public static class MainActivity
{
    public static async Task Main(string[] args)
    {
        StartStage programStages = new();

        //! |-------------------------|
        //! |     Pre-Init Code.      |     |
        //! |-------------------------|     V

        CommandHelper commandConstructor = await programStages.PreInitialization(args);

        //! |-------------------------|
        //! |       Init Code.        |     |
        //! |-------------------------|     V

        await programStages.Initialization(commandConstructor);

        //! |-------------------------|
        //! |     Post-Init Code.     |     |
        //! |-------------------------|     V

        await programStages.PostInitialization();

        static async Task LockConsole()
        {
            /* Lock Main Thread to avoid exiting. */
            await Task.Delay(-1);
        }

        await LockConsole();
    }
}