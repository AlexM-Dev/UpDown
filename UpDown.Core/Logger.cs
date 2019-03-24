using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UpDown.Core {
    public static class Logger {
        private static bool enableOutput;
        private static bool enableFile;
        private static string filePath;

        public static void Initialise(bool enableOutput,
            bool enableFile, string filePath) {

            Logger.enableOutput = enableOutput;
            Logger.enableFile = enableFile;
            Logger.filePath = filePath;
        }

        public static async Task Log(string message, bool force = false) {
            await Log(null, message, force);
        }

        public static async Task Log(this IAddon source, 
            string message, bool force = false) {
            if (enableOutput || force) {
                var dt = DateTime.UtcNow;

                var src = source != null ? $"/{source.Name}" : "";
                var str = $"[LOG{src}] [{dt.ToString("F")}] {message}";

                Console.WriteLine(str);

                if (enableFile)
                    await File.AppendAllLinesAsync(filePath, new[] { str });
            }
        }
    }
}
