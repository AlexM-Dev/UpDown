using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpDown.Core;
using UpDown.Core.Events;
using UpDown.Core.IO;
using System.Linq;
using System.IO;

namespace UpDown.Refinery {
    public class RefineryCore : IAddon {
        // Currently stored data to provide differentials.
        private Dictionary<string, bool> data;

        // List of subscribed addons to receive events from Refinery.
        private List<IAddon> subscribed { get; set; } = new List<IAddon>();

        // Configuration options.
        private Config config;

        // Whether to force logging, even when the logging option is disabled
        // in the main program.
        bool force = false;

        public string Name => "Refinery";

        public async void Initialise() {
            // Load the config from the refinery.json file.
            config = await FileLoader.LoadAsync("refinery.json",
                new Config());
            // Whether to force logging.
            force = config.ForceLog;

            // Check whether to register events.
            if (config.Enabled) {
                CheckerEvents.AllChecksCompleted += allChecksCompleted;
            }
        }

        public void Shutdown() { }

        // Logs data to the log file. By default, this is enabled.
        // Refinery itself does not provide custom outputs, that is handled by
        // other addons.
        private async Task logData(Dictionary<string, bool> data) {
            // Convert each data entry to a loggable format.
            var output = data.Select(w => $"{w.Key} : " +
                (w.Value ? "down" : "up")).ToList();

            // Log each entry to the console.
            foreach (var status in output)
                await this.Log(status, force);

            // If Refinery should log to the file, and there is data to log,
            // then log it.
            if (config.UseOutputFile && output.Count > 0) {
                output.Insert(0, $"[{DateTime.UtcNow.ToString("F")}]");
                output.Add("");

                await File.AppendAllLinesAsync(config.OutputFilePath, output);
            }
        }

        // Update all data as a whole. Waits for a set of checks to complete
        // in order to provide a whole differential.
        private async void allChecksCompleted(object sender, OnAllChecksEventArgs e) {
            var newData = e.WebsitesDown;

            // If this is the first time, then set the data.
            // In the future, the logging file should be able to be used
            // as a previous point, to compare all statuses.
            if (data == null) {
                data = newData;
                sendStatusEvent(data);

                await logData(data);

                return;
            }

            // If something's changed:
            if (data != newData) {
                // Create a differential.
                var diffData = new Dictionary<string, bool>();

                foreach (var website in newData) {
                    // If the old data does not contain the new entry.
                    if (!data.ContainsKey(website.Key)) {
                        diffData.Add(website.Key, website.Value);
                        continue;
                    }

                    // If new value is not the old value.
                    if (website.Value != data[website.Key]) {
                        diffData.Add(website.Key, website.Value);
                    }
                }

                // Log the appropriate data.
                await logData(diffData);

                // Send event to subscribed addons.
                data = newData;
                sendStatusEvent(diffData);
            }
        }

        // Sends a message to all subscribed addons.
        private async void sendStatusEvent(Dictionary<string, bool> data) {
            // Loop through each subscribed addon.
            foreach (var addon in subscribed) {
                // Send a message to each addon.
                var response = await addon.SendMessage(this, data);

                // Requires response 'true' to indicate the event was successful.
                if (typeof(bool) == response.GetType()) {
                    var addonStatus = (bool)response;

                    if (addonStatus) {
                        await this.Log($"Status event successful, " +
                            $" sent to {addon.Name}", force);
                    }
                }
            }
        }

        // On received a message.
        public async Task<object> SendMessage(IAddon source, object message) {
            if (typeof(string) == message.GetType()) {
                switch (message) {
                    case "subscribe wud":
                        // Subscribe the source addon to the Refinery.
                        subscribed.Add(source);
                        await this.Log($"{source.Name} subscribed" +
                            " to the Refinery.");

                        // Return successful event.
                        return true;
                }
            }

            // Unrecognised command: return unsuccessful.
            return false;
        }
    }
}
