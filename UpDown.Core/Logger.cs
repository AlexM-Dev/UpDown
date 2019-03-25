using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UpDown.Core {
    public class Logger {
        private bool enableOutput;
        private bool enableFile;
        private string filePath;

        /// <summary>
        /// Initialise the logger for performing logging operations.
        /// </summary>
        /// <param name="enableOutput">Whether to enable verbose output.</param>
        /// <param name="enableFile">Whether to enable file output.</param>
        /// <param name="filePath">The path to the output file.</param>
        public Logger(bool enableOutput, bool enableFile, string filePath) {
            this.enableOutput = enableOutput;
            this.enableFile = enableFile;
            this.filePath = filePath;
        }

        /// <summary>
        /// Log a mainstream message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="force">Whether to force logging.</param>
        public async Task Log(string message, bool force = false) {
            await Log(null, message, force);
        }

        /// <summary>
        /// Log a message, from mainstream or addon.
        /// </summary>
        /// <param name="source">The addon source.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="force">Whether to force logging.</param>
        /// <returns></returns>
        public async Task Log(IAddon source, 
            string message, bool force = false) {
            if (enableOutput || force) {
                // Get the appropriate UTC date.
                // UTC = universal and it's more manageable.
                var dt = DateTime.UtcNow;

                var src = source != null ? $"/{source.Name}" : "";
                var str = $"[LOG{src}] [{dt.ToString("F")}] {message}";

                // Write the output to console.
                Console.WriteLine(str);
                
                // If file output is enabled, log to the file.
                if (enableFile)
                    await File.AppendAllLinesAsync(filePath, new[] { str });
            }
        }
    }
}
