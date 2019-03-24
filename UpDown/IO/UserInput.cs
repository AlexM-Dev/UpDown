using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace UpDown.IO {
    internal class UserInput {
        /// <summary>
        /// Forces a user to provide limited key-based input.
        /// Kind of like CMD's choice.
        /// </summary>
        /// <param name="q">The question to provide.</param>
        /// <param name="keys">The available keys.</param>
        /// <returns>The selected key.</returns>
        public static ConsoleKey Choice(string q, params ConsoleKey[] keys) {
            // Represent each available key as a string.
            string options = "(";
            for (int i = 0; i < keys.Length; i++) {
                options += keys[i];
                if (i < keys.Length - 1)
                    options += "/";
            }
            options += ")";

            // Pose the question.
            Console.Write($"{q}? {options}: ");

            // Checking whether input is valid.
            bool valid = false;

            // Track current key.
            ConsoleKeyInfo rk = default;

            // While invalid - i.e. a loop that executes forever if always
            // invalid, no restriction.
            while (!valid) {
                // Read the input.
                rk = Console.ReadKey(true);

                // Check if the input is valid.
                foreach (var k in keys) {
                    if (k == rk.Key) {
                        Console.Write(rk.KeyChar);
                        valid = true;
                        Console.WriteLine();
                    }
                }
            }

            // Return the choice.
            return rk.Key;
        }
    }
}
