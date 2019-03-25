using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UpDown.Core.Events;
using System.Linq;

namespace UpDown.Core {
    public class Checker {
        /// <summary>
        /// Event for when one website has been checked.
        /// </summary>
        public EventHandler<OnFinishCheckEventArgs> CheckCompleted =
            (o, e) => { };

        /// <summary>
        /// Event for when every website has been checked.
        /// </summary>
        public EventHandler<OnAllChecksEventArgs> AllChecksCompleted =
            (o, e) => { };

        /// <summary>
        /// Event for when checking a website. This can be used to alter
        /// the result for a check.
        /// </summary>
        public EventHandler<OnCheckingEventArgs> CheckingWebsite =
            (o, e) => { };

        /// <summary>
        /// Event for when the messenger is ready to be used.
        /// </summary>
        public EventHandler MessengerInitialised = (o, e) => { };

        public async Task<Dictionary<string, bool>> IsDownAsync(
            IEnumerable<string> websites, HttpClient client) {

            var result = new Dictionary<string, bool>();

            foreach (var website in websites) {
                var down = await IsDownAsync(website, client);

                result.Add(website, down.Item1);
            }

            // Call the event, as all checks completed.
            AllChecksCompleted(this, new OnAllChecksEventArgs(result));

            return result;
        }

        /// <summary>
        /// Checks if a website is down or not, using the events
        /// to which methods are subscribed.
        /// </summary>
        /// <param name="website">The website to check.</param>
        /// <param name="client">The client to use to check</param>
        /// <returns>Is down/not, and which checks failed.</returns>
        public async Task<(bool, List<string>)> IsDownAsync(
            string website, HttpClient client) {
            OnCheckingEventArgs e;

            try {
                // Get response & content of response.
                var response = await client.GetAsync(website);
                var content = await response.Content.ReadAsStringAsync();

                // Set the event arguments to success, with content.
                e = new OnCheckingEventArgs(website, client,
                    response, content, false, null);
            } catch (Exception ex) {
                // Set the event arguments to fail, without content.
                e = new OnCheckingEventArgs(website, client, null,
                    null, true, ex);
            }

            // Run the event for each check to pass/fail.
            CheckingWebsite(null, e);

            // Return whether the website is down, and each check which failed.
            var result = (e.IsDown, e.Sources);

            // Run the event, as the check finished.
            CheckCompleted(this,
                new OnFinishCheckEventArgs(website, e.IsDown, e.Sources));

            return result;
        }
    }
}
