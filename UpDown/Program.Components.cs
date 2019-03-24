using CloudFlareUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UpDown.Core;
using UpDown.Core.IO;
using UpDown.CoreChecker;
using UpDown.IO;

namespace UpDown {
    partial class Program {
        internal const string Config = "config.json";

        // Components
        internal static Timer Mon = new Timer();
        internal static HttpClient Client;
        internal static Config ActiveConfig;

        private static async Task<bool> initAsync() {
            try {
                // CONFIGURATION FILE
                if (!await initConfigAsync()) {
                    return false;
                }

                // LOGGER
                Logger.Initialise(ActiveConfig.EnableLog,
                    ActiveConfig.EnableLogFile,
                    ActiveConfig.LogFilePath);

                // MONITOR TIMER.
                Mon.Elapsed += checkWebsitesAsync;
                Mon.Interval = ActiveConfig.Interval * 1000;
                Mon.Start();

                // REQUEST CLIENT
                initClient();

                // REGISTER THE INTERNAL CHECKS.
                Registrar.RegisterInternalChecker(
					ActiveConfig.CheckSeries);
            } catch {
                return false;
            }

            return true;
        }

        private static async Task<bool> initConfigAsync() {
            var config = await FileLoader.LoadAsync(Config, new Config());

            if (config == null) {
                return false;
            }

            ActiveConfig = config;
            await promptTosAsync();

            return true;
        }

        private static void initClient() {
            if (ActiveConfig.CheckSeries.Cloudflare) {
                var handler = new ClearanceHandler() {
                    MaxRetries = ActiveConfig.CheckSeries.CfMaxRetries
                };

                Client = new HttpClient(handler);
            } else {
                Client = new HttpClient();
            }
        }


        private static async Task promptTosAsync() {
            if (!ActiveConfig.AcceptedTOS) {
                var docs = await Resources.GetDocumentationAsync();

                // Print TOS.
                Console.WriteLine(Text.GetHeader("TERMS OF USE"));
                Console.WriteLine(docs[0]);

                // Print configuration information.
                Console.WriteLine(Text.GetHeader("CONFIGURATION"));
                Console.WriteLine(docs[1]);

                var k = UserInput.Choice("Do you understand the terms & information",
                    ConsoleKey.Y,
                    ConsoleKey.N);

                if (k == ConsoleKey.Y) {
                    ActiveConfig.AcceptedTOS = true;
                    await ActiveConfig.SaveAsync(Config);
                } else {
                    return;
                }
            }
        }
    }
}
