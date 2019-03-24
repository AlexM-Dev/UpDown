using System;
using System.Collections.Generic;
using System.Text;

namespace UpDown {
    partial class Program {
        /// <summary>
        /// Provides a suitable entry point for UpDown.
        /// I can't stand this being in the main class, so it's in isolation.
        /// (It's synchronous... oops.)
        /// </summary>
        /// <param name="args">Program arugments.</param>
        static void Main(string[] args) => MainAsync(args).Wait();
    }
}
