using CloudFlareUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UpDown.Core;
using UpDown.Core.IO;
using UpDown.Core.Messaging;
using UpDown.CoreChecker;
using UpDown.IO;
using UpDown.IO.Addons;

namespace UpDown {
    partial class Program {
        internal const string Config = "config.json";

        // Components
        internal static Timer Mon = new Timer();
        internal static HttpClient Client;
        internal static Config ActiveConfig;

        public static Checker Checker;
        public static Messenger Messenger;
        public static Logger Logger;

        internal static Loader Loader;
        internal static Registrar Registrar;

        /// <summary>
        /// Initialise all required components for the program to function.
        /// </summary>
        /// <returns>Whether initialisation was successful.</returns>
        private static async Task<bool> initAsync() {
            try {
                // Initialise the configuration file - failure = false.
                if (!await initConfigAsync()) {
                    return false;
                }

                // Initialise the logger, for core and addons.
                Checker = new Checker();
                Logger = new Logger(ActiveConfig.EnableLog, 
                    ActiveConfig.EnableLogFile, 
                    ActiveConfig.LogFilePath);

                // Initialise the monitor.
                Mon.Elapsed += checkWebsitesAsync;
                Mon.Interval = ActiveConfig.Interval * 1000;
                Mon.Start();

                // Initialise the request client.
                initClient();

                // Register all appropriate checks.
                Registrar = new Registrar();
                Registrar.RegisterInternalChecker(Checker,
                    ActiveConfig.CheckSeries);
            } catch {
                // If something went wrong, return false.
                return false;
            }

            // Everything went smoothly.
            return true;
        }

        /// <summary>
        /// Initialises the config.
        /// </summary>
        /// <returns>Successful or not.</returns>
        private static async Task<bool> initConfigAsync() {
            // Load the config file.
            var config = await FileLoader.LoadAsync(Config, new Config());

            // Failed to load.
            if (config == null) return false;

            // Share the config.
            ActiveConfig = config;

            // Prompt the TOS if not accepted already.
            if (!await promptTosAsync()) return false;

            // It went smoothly.
            return true;
        }

        /// <summary>
        /// Initialises the request client.
        /// </summary>
        private static void initClient() {
            // Use cloudflare or not.
            if (ActiveConfig.CheckSeries.Cloudflare) {
                // Create the cloudflare client.
                var handler = new ClearanceHandler() {
                    MaxRetries = ActiveConfig.CheckSeries.CfMaxRetries
                };

                Client = new HttpClient(handler);
            } else {
                // Create the standard client.
                Client = new HttpClient();
            }
        }

        /// <summary>
        /// Prompts the TOS (to accept/not).
        /// </summary>
        /// <returns>Whether it was accepted.</returns>
        private static async Task<bool> promptTosAsync() {
            if (!ActiveConfig.AcceptedTOS) {
                // Print TOS.
                Console.WriteLine(Text.GetHeader("TERMS OF USE"));
                Console.WriteLine(Resources.TOS);

                // Print configuration information.
                Console.WriteLine(Text.GetHeader("CONFIGURATION"));
                Console.WriteLine(Resources.Configuration);

                var k = UserInput.Choice("Do you understand the terms & information",
                    ConsoleKey.Y,
                    ConsoleKey.N);

                if (k == ConsoleKey.Y) {
                    // Successful.
                    ActiveConfig.AcceptedTOS = true;
                    await ActiveConfig.SaveAsync(Config);
                } else {
                    // The user hates us.
                    return false;
                }
            }

            // Everything went smoothly.
            return true;
        }
    }
}
