using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UpDown.Core.IO {
    public static class FileLoader {
        /// <summary>
        /// Load a JSON file.
        /// </summary>
        /// <typeparam name="T">The type to use.</typeparam>
        /// <param name="path">The file path to load.</param>
        /// <param name="def">The default value for the instance.</param>
        /// <returns></returns>
        public static async Task<T> LoadAsync<T>(string path, T def = default) {
            try {
                if (!File.Exists(path)) {
                    await SaveAsync(def, path); 
                }
                var contents = await File.ReadAllTextAsync(path);

                return JsonConvert.DeserializeObject<T>(contents);
            } catch {
                return default;
            }
        }

        /// <summary>
        /// Save a JSON file.
        /// </summary>
        /// <param name="o">The object to save.</param>
        /// <param name="path">The file to save to.</param>
        /// <returns></returns>
        public static async Task<bool> SaveAsync(this object o, string path) {
            try {
                var contents = JsonConvert.SerializeObject(o,
                    Formatting.Indented);

                await File.WriteAllTextAsync(path, contents);

                return true;
            } catch {
                return false;
            }
        }
    }
}
