using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UpDown.Core;
using UpDown.Core.Events;
using UpDown.Core.Messaging;

namespace UpDown.DemoVisualAddon {
    class DemoVisualEntryPoint : IAddon {
        private frmMain mainForm;
        private bool useRefinery;

        public string Name => "Demo Visual";

        public void Initialise() {
            CheckerEvents.MessengerInitialised += addonsReady;

            // Provide a suitable entry point.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            mainForm = new frmMain();
            new Thread(() => mainForm.ShowDialog()).Start();
        }

        private async Task<bool> tryUseRefinery() {
            var result = await this.SendMessage("Refinery", "subscribe wud");

            if (result.Length == 1 && typeof(bool) == result[0].GetType()) {
                return (bool)result[0];
            }

            return false;
        }

        private async void addonsReady(object sender, EventArgs e) {
            useRefinery = await tryUseRefinery();

            await this.Log("Found Refinery addon? " + useRefinery);

            if (!useRefinery) {
                CheckerEvents.CheckingWebsite += checking;
                CheckerEvents.CheckCompleted += checkFinished;
                CheckerEvents.AllChecksCompleted += allChecksFinished;
            }
        }

        private void checking(object sender, OnCheckingEventArgs e) {
            mainForm.SetStatus($"Checking {e.Website}.");
        }

        private void checkFinished(object sender, OnFinishCheckEventArgs e) {
            mainForm.SetData(e.Website, e.IsDown);
        }

        private void allChecksFinished(object sender, OnAllChecksEventArgs e) {
            // All checks have been completed.
        }

        public void Shutdown() {
            mainForm.ThreadClose();
        }

        public async Task<object> SendMessage(IAddon source, object message) {
            if (useRefinery && source.Name == "Refinery") {
                // The refinery has completed checks & compared differences.
                if (typeof(Dictionary<string, bool>) == message.GetType()) {
                    var data = message as Dictionary<string, bool>;

                    mainForm.UpdateData(data);

                    return true;
                }
            }

            return false;
        }
    }
}
