using System;
using System.Collections.Generic;
using System.Text;

namespace UpDown.IO {
    internal class Text {
        /// <summary>
        /// Fancify some message in the form of a header.
        /// </summary>
        /// <param name="message">Header message.</param>
        /// <returns>Fancified message.</returns>
        public static string GetHeader(string message) {
            int len = (int)(message.Length * 1.5);
            int m = (len - message.Length) / 2;

            string header = new string('#', len);
            string space = new string(' ', m);

            return $"{header}\n{space}{message}\n{header}";
        }

        /// <summary>
        /// Trim a string to a length and append '...'
        /// </summary>
        /// <param name="message">The message to trim.</param>
        /// <param name="len">The length to trim to.</param>
        /// <returns>Trimmed string.</returns>
        public static string Trim(string message, int len) {
            if (message.Length <= len)
                return message;

            string add = "...";

            string trim = message.Substring(0, len + 3) + add;
            return trim;
        }
    }
}
