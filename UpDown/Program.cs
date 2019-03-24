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
        static async Task MainAsync(string[] args) {
            // Attempt to load the configuration.
            bool initialised = await initAsync();

            if (!initialised) {
                Console.WriteLine("Something went wrong when trying to" +
                    "initialise components (config, etc.).");

                return;
            }

            var addons = await Loader.LoadAddons(ActiveConfig);
            Messenger.Initialise(addons);

            while (!Console.KeyAvailable) ;

            Loader.ShutdownAddons(addons);
        }

        static async void checkWebsitesAsync(object sender, ElapsedEventArgs e) {
            var statuses = new Dictionary<string, bool>();

            foreach (var w in ActiveConfig.Websites) {
                string website = w;

                var isDown = await Checker.IsDownAsync(website, Client);

                statuses.Add(website, isDown.Item1);

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
