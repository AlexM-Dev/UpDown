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
        public static async Task<(bool, List<string>)> IsDownAsync(
            string website, HttpClient client) {
            OnCheckingEventArgs e;

            try {
                var response = await client.GetAsync(website);
                var content = await response.Content.ReadAsStringAsync();

                var code = (int)response.StatusCode;

                e = new OnCheckingEventArgs(website, client,
                    response, content, false, null);
            } catch (Exception ex) {
                e = new OnCheckingEventArgs(website, client, null,
                    null, true, ex);
            }

            CheckerEvents.CheckingWebsite(null, e);
            return (e.IsDown, e.Sources);
        }
    }
}
