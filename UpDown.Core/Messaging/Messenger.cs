using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UpDown.Core.Events;
using System.Linq;

namespace UpDown.Core.Messaging {
    public static class Messenger {
        // The addons which the messenger should be able to access.
        public static IEnumerable<IAddon> Addons { get; private set; }

        public static void Initialise(Checker checker, IEnumerable<IAddon> addons) {
            Addons = addons;

            checker.MessengerInitialised(null, EventArgs.Empty);
        }

        /// <summary>
        /// Sends a message to every addon with a given name/regex.
        /// </summary>
        /// <param name="source">The source addon.</param>
        /// <param name="nameRegex">The name of the addon. Supports regex.</param>
        /// <param name="message">The message object to send</param>
        /// <returns>Asynchronous results from each matched addon.</returns>
        public static async Task<object[]> SendMessage(this IAddon source,
            string name, object message) {
            if (Addons == null) return null;

            // Send the message to each matched addon (by name).
            return await Task.WhenAll(Addons
                .Where(a => Regex.IsMatch(a.Name, name))
                .Select(a => a.SendMessage(source, message)));
        }
    }
}
