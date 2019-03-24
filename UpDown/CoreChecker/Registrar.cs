using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UpDown.Core.Events;

namespace UpDown.CoreChecker {
    internal class Registrar {
        public static Checks Checks { get; set; }

        public static void RegisterInternalChecker(Checks checks) {
            Checks = checks;

            if (checks.ResponseCode) {
                CheckerEvents.CheckingWebsite += checkResponseCode;
            }

            if (checks.Keyword) {
                CheckerEvents.CheckingWebsite += checkKeywords;
            }

            if (checks.TryCatch) {
                CheckerEvents.CheckingWebsite += checkError;
            }
        }

        private static void checkResponseCode(object sender, OnCheckingEventArgs e) {
            if (!e.Error) {
                int code = (int)e.Message.StatusCode;

                if (!Checks.AllowedResponseCodes.Contains(code)) {
                    if (code >= 400) {
                        e.IsDown = true;
                        e.Sources.Add($"Response code \"{code}\"");
                    }
                }
            }
        }

        private static void checkKeywords(object sender, OnCheckingEventArgs e) {
            if (!e.Error) {
                foreach (var k in Checks.Keywords) {
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

        private static void checkError(object sender, OnCheckingEventArgs e) {
            if (e.Error) {
                e.IsDown = true;
                e.Sources.Add($"Exception \"{e.Exception.Message}\"");
            }
        }
    }
}
