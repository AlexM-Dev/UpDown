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
            var addons = await Loader.LoadAddons(ActiveConfig);
            Messenger.Initialise(addons);

            // The program should only be active while no input has been
            // given.
            while (!Console.KeyAvailable) ;

            // Send a shutdown request to all addons.
            Loader.ShutdownAddons(addons);
        }

        /// <summary>
        /// The monitor which checks whether a website is up or down.
        /// </summary
        static async void checkWebsitesAsync(object sender, ElapsedEventArgs e) {
            // Store each status.
            var statuses = new Dictionary<string, bool>();

            // Loop through each website to check.
            foreach (var w in ActiveConfig.Websites) {
                // Necessary when looping in an asynchronous context (?).
                string website = w;

                // Use the Core to check if a website is down. Uses addons.
                var isDown = await Checker.IsDownAsync(website, Client);

                // Add the website to the status.
                statuses.Add(website, isDown.Item1);

                // Use the logger to log the output.
                await Logger.Log($"{website} down: {isDown.Item1}");

                if (isDown.Item1)
                    await Logger.Log($"{website} failed checks: " +
                        String.Join(", ",
                        isDown.Item2.ConvertAll(s => Text.Trim(s, 65))));

                // Call CheckCompleted addon events.
                CheckerEvents.CheckCompleted(null,
                    new OnFinishCheckEventArgs(website,
                    isDown.Item1, isDown.Item2));
            }

            // Call AllChecksCompleted addon events.
            CheckerEvents.AllChecksCompleted(null,
                new OnAllChecksEventArgs(statuses));
        }
    }
}
