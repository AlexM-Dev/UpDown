using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UpDown.Core;
using UpDown.Core.Events;

namespace UpDown.CoreChecker {
    internal class Registrar {
        /// <summary>
        /// The set of checks to use when checking websites.
        /// </summary>
        public Checks Checks { get; set; }

        /// <summary>
        /// Registers a base set of supported checks. Provides
        /// the simple core of the program.
        /// </summary>
        /// <param name="checks">The checks to use.</param>
        public void RegisterInternalChecker(Checker checker, Checks checks) {
            Checks = checks;

            // If the response code should be used to determine status:
            if (checks.ResponseCode) {
                checker.CheckingWebsite += checkResponseCode;
            }

            // If a set of keywords should be used to determine status:
            if (checks.Keyword) {
                checker.CheckingWebsite += checkKeywords;
            }

            // If an error occurs, that should be used to determine status,
            // regardless of type:
            if (checks.TryCatch) {
                checker.CheckingWebsite += checkError;
            }
        }

        private void checkResponseCode(object sender, OnCheckingEventArgs e) {
            // If not errored. 
            if (!e.Error) {
                // Get status code.
                int code = (int)e.Message.StatusCode;

                // Whitelisted codes. >= 400 are invalid codes.
                if (!Checks.AllowedResponseCodes.Contains(code)) {
                    if (code >= 400) {
                        e.IsDown = true;
                        e.Sources.Add($"Response code \"{code}\"");
                    }
                }
            }
        }

        private void checkKeywords(object sender, OnCheckingEventArgs e) {
            // Not errored.
            if (!e.Error) {
                // Check each keyword.
                foreach (var k in Checks.Keywords) {
                    // Use regex or not.
                    if (Checks.KeywordRegex) {
                        if (Regex.IsMatch(e.Content, k)) {
                            e.IsDown = true;
                            e.Sources.Add($"Regex contains \"{k}\"");
                        }
                    } else {
                        if (e.Content.Contains(k)) {
                            e.IsDown = true;
                            e.Sources.Add($"Contains \"{k}\"");
                        }
                    }
                }
            }
        }

        private void checkError(object sender, OnCheckingEventArgs e) {
            // Yes, there's an error!
            if (e.Error) {
                e.IsDown = true;
                e.Sources.Add($"Exception \"{e.Exception.Message}\"");
            }
        }
    }
}
