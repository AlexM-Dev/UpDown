using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UpDown.Core.Events;

namespace UpDown.Core {
    public static class Checker {
        /// <summary>
        /// Checks if a website is down or not, using the events
        /// to which methods are subscribed.
        /// </summary>
        /// <param name="website">The website to check.</param>
        /// <param name="client">The client to use to check</param>
        /// <returns>Is down/not, and which checks failed.</returns>
        public static async Task<(bool, List<string>)> IsDownAsync(
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
            CheckerEvents.CheckingWebsite(null, e);

            // Return whether the website is down, and each check which failed.
            return (e.IsDown, e.Sources);
        }
    }
}
