using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace UpDown.IO {
    internal class UserInput {
        public static ConsoleKey Choice(string q, params ConsoleKey[] keys) {
            string options = "(";
            for (int i = 0; i < keys.Length; i++) {
                options += keys[i];
                if (i < keys.Length - 1)
                    options += "/";
            }
            options += ")";

            Console.Write($"{q}? {options}: ");

            bool valid = false;
            ConsoleKeyInfo rk = default;

            while (!valid) {
                rk = Console.ReadKey(true);

                foreach (var k in keys) {
                    if (k == rk.Key) {
                        Console.Write(rk.KeyChar);
                        valid = true;
                        Console.WriteLine();
                    }
                }
            }

            return rk.Key;
        }
    }
}
