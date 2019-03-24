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
        private Dictionary<string, bool> data;
        private List<IAddon> subscribed { get; set; }

        private Config config;
        bool force = false;
        public string Name => "Refinery";

        public async void Initialise() {
            subscribed = new List<IAddon>();

            config = await FileLoader.LoadAsync("refinery.json",
                new Config());
            force = config.ForceLog;

            if (config.Enabled) {
                CheckerEvents.AllChecksCompleted += allChecksCompleted;
            }
        }

        public void Shutdown() {

        }

        private async Task logData(Dictionary<string, bool> data) {
            var output = data.Select(w => $"{w.Key} : " +
                (w.Value ? "down" : "up")).ToList();

            foreach (var status in output)
                await this.Log(status, force);

            if (config.UseOutputFile && output.Count > 0) {
                output.Insert(0, $"[{DateTime.UtcNow.ToString("F")}]");
                output.Add("");

                await File.AppendAllLinesAsync(config.OutputFilePath, output);
            }
        }

        private async void allChecksCompleted(object sender, OnAllChecksEventArgs e) {
            var newData = e.WebsitesDown;

            if (data == null) {
                data = newData;
                sendStatusEvent(data);

                await logData(data);

                return;
            }

            if (data != newData) {
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

                await logData(diffData);

                data = newData;
                sendStatusEvent(diffData);
            }
        }

        private async void sendStatusEvent(Dictionary<string, bool> data) {
            foreach (var addon in subscribed) {
                var response = addon.SendMessage(this, data);

                if (typeof(bool) == response.GetType()) {
                    var addonStatus = (bool)(await response);

                    if (addonStatus) {
                        await this.Log($"Status event successful, " +
                            $" sent to {addon.Name}", force);
                    }
                }
            }
        }

        public async Task<object> SendMessage(IAddon source, object message) {
            if (typeof(string) == message.GetType()) {
                switch (message) {
                    case "subscribe wud":
                        await this.Log($"{source.Name} subscribed" +
                            " to the Refinery.");
                        subscribed.Add(source);
                        return true;
                }
            }

            return false;
        }
    }
}
