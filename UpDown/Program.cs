using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Timers;
using UpDown.IO.Addons;
using UpDown.Core.Events;
using UpDown.Core;
using UpDown.Core.Messaging;
using UpDown.IO;

namespace UpDown {
    partial class Program {
        /// <summary>
        /// Asynchronous entry point.
        /// </summary>
        /// <param name="args">Program arugments.</param>
        static async Task MainAsync(string[] args) {
            // Attempt to load the configuration.
            bool initialised = await initAsync();

            // Failed to load the configuration, or the TOS was declined.
            if (!initialised) {
                Console.WriteLine("Something went wrong when trying to" +
                    "initialise components (config, etc.).");

                return;
            }

            // Load all addons, and notify the messenger system
            // that all addons have been loaded.
            Loader = new Loader(Checker, Logger);
            await Loader.LoadAddons(ActiveConfig);
            await Loader.InitialiseAddons();

            Messenger.Initialise(Checker, Loader.LoadedAddons);

            // The program should only be active while no input has been
            // given.
            while (!Console.KeyAvailable) ;

            // Send a shutdown request to all addons.
            Loader.ShutdownAddons();
        }

        /// <summary>
        /// The monitor which checks whether a website is up or down.
        /// </summary
        static async void checkWebsitesAsync(object sender, ElapsedEventArgs e) {
            await Checker.IsDownAsync(ActiveConfig.Websites, Client);
        }
        
        static async void checkedWebsite(object sender, OnFinishCheckEventArgs e) {
            // Use the logger to log the output.
            await Logger.Log($"{e.Website} down: {e.IsDown}");

            if (e.IsDown)
                await Logger.Log($"{e.Website} failed checks: " +
                    String.Join(", ",
                    e.Sources.ConvertAll(s => Text.Trim(s, 65))));
        }
    }
}
