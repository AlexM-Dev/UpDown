using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UpDown.Core.Events;
using System.Linq;

namespace UpDown.Core.Messaging {
    public static class Messenger {
        private static bool initialised = false;

        private static IEnumerable<IAddon> addons;

        public static void Initialise(IEnumerable<IAddon> addons) {
            Messenger.initialised = true;
            Messenger.addons = addons;

            CheckerEvents.MessengerInitialised(null, EventArgs.Empty);
        }

        public async static Task<object[]> SendMessage(this IAddon source,
            string nameRegex, object message) {
            if (!initialised) return null;

            return await Task.WhenAll(addons
                .Where(a => Regex.IsMatch(a.Name, nameRegex))
                .Select(a => a.SendMessage(source, message)));
        }
    }
}
