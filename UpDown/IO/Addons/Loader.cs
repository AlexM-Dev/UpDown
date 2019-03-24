using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Diagnostics;
using UpDown.Core;
using System.Threading.Tasks;
using UpDown.CoreChecker;
using UpDown.Core.Events;

namespace UpDown.IO.Addons {
    internal class Loader {
        public static void ShutdownAddons(List<IAddon> addons) {
            foreach (var addon in addons) {
                addon.Shutdown();
            }
        }

        public static async Task<List<IAddon>> LoadAddons(Config conf) {
            // Get all files from the directory.
            var files = getFiles(conf);

            // List of all addons.
            IEnumerable<IAddon> addons = new List<IAddon>();

            // Record the time it takes to load.
            var sw = Stopwatch.StartNew();

            // Try to retrieve the addons.
            try {
                await Logger.Log("Resolving addons.", true);

                addons = getAddons(files);
            } catch (Exception ex) {
                // Something horrible happened.
                await Logger.Log($"Failed to resolve addons: {ex.Message}",
                    true);
            }
            // Space it out.
            await Logger.Log("", true);

            // Initialise each addon.
            var initAddons = new List<IAddon>();
            foreach (var a in addons) {
                await Logger.Log($"Initialising {a.Name}.", true);
                try {
                    a.Initialise();
                    await Logger.Log("Initialised.", true);
                    initAddons.Add(a);
                } catch (Exception ex) {
                    await Logger.Log($"Failed to initialise: {ex.Message}",
                        true);
                }
                await Logger.Log("");
            }

            // Return & dump.
            sw.Stop();

            await Logger.Log("", true);
            await Logger.Log($"Loaded {addons.Count()} addons " +
                $"in {sw.ElapsedMilliseconds} ms.", true);

            return initAddons;
        }

        private static string[] getFiles(Config conf) {
            // Find the absolute path of the path given in config.
            var loc = Path.GetFullPath(conf.AddonPath);

            // If the directory doesn't exist, create it.
            if (!Directory.Exists(loc)) {
                Directory.CreateDirectory(loc);
            }

            // Return all files (matching format) in the path.
            return Directory.GetFiles(loc, conf.AddonsFormat,
                SearchOption.AllDirectories);
        }

        private static IEnumerable<IAddon> getAddons(string[] assemblies) {
            // Return all assemblies specified, and create addons from each.
            return assemblies.SelectMany(path => {
                var assembly = loadAddon(path);
                return createAddons(assembly);
            });
        }

        private static Assembly loadAddon(string path) {
            // Load from here.
            string fullPath = Path.GetFullPath(path);

            // Find the addons within the addons folder.
            var context = new AddonLoadContext(fullPath);

            return context.LoadFromAssemblyName(new
                AssemblyName(Path.GetFileNameWithoutExtension(path)));
        }

        private static IEnumerable<IAddon> createAddons(Assembly assembly) {
            // List all types in the assembly.
            foreach (var type in assembly.GetTypes()) {
                // If the type is an addon, try to load it.
                if (typeof(IAddon).IsAssignableFrom(type)) {
                    // Create an instance of the addon.
                    var addon = Activator.CreateInstance(type) as IAddon;

                    // Return the created addon.
                    if (addon != null)
                        yield return addon;
                }
            }
        }
    }
}
