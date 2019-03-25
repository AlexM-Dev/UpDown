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
using UpDown.Core.Messaging;

namespace UpDown.IO.Addons {
    internal class Loader {
        private Checker checker;
        private Messenger messenger;
        private Logger logger;

        public List<IAddon> LoadedAddons { get; set; } = new List<IAddon>();

        public Loader(Checker checker, Messenger messenger, Logger logger) {
            this.checker = checker;
            this.messenger = messenger;
            this.logger = logger;
        }

        /// <summary>
        /// Executes each addon's shutdown method.
        /// </summary>
        /// <param name="addons">The addons to send the message to.</param>
        public void ShutdownAddons() {
            foreach (var addon in LoadedAddons) {
                addon.Shutdown();
            }
        }

        /// <summary>
        /// Load each addon and return a list of them.
        /// </summary>
        /// <param name="conf">The configuration file to use.</param>
        /// <returns>List of all loaded addons.</returns>
        public async Task LoadAddons(Config conf) {
            // Get all files from the directory.
            var files = getFiles(conf);

            // List of all addons.
            IEnumerable<IAddon> addons = new List<IAddon>();

            // Record the time it takes to load.
            var sw = Stopwatch.StartNew();

            // Try to retrieve the addons.
            try {
                await logger.Log("Resolving addons.", true);

                addons = getAddons(files);
            } catch (Exception ex) {
                // Something horrible happened.
                await logger.Log($"Failed to resolve addons: {ex.Message}",
                    true);
            }
            // Space it out.
            await logger.Log("", true);

            // Initialise each addon.
            var initAddons = new List<IAddon>();
            foreach (var a in addons) {
                await logger.Log($"Initialising {a.Name}.", true);
                try {
                    a.Initialise(checker, messenger, logger);
                    await logger.Log("Initialised.", true);
                    initAddons.Add(a);
                } catch (Exception ex) {
                    await logger.Log($"Failed to initialise: {ex.Message}",
                        true);
                }
                await logger.Log("");
            }

            // Return & dump.
            sw.Stop();

            await logger.Log("", true);
            await logger.Log($"Loaded {addons.Count()} addons " +
                $"in {sw.ElapsedMilliseconds} ms.", true);

            this.LoadedAddons = initAddons;
        }

        /// <summary>
        /// Return all valid addon assemblies within the addon folder.
        /// </summary>
        /// <param name="conf">The configuration file to read from.</param>
        /// <returns>List of all addon assemblies.</returns>
        private string[] getFiles(Config conf) {
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

        /// <summary>
        /// Get a set of addons from each assembly file.
        /// </summary>
        /// <param name="assemblies">Assemblies to load addons from.</param>
        /// <returns>List of addons.</returns>
        private static IEnumerable<IAddon> getAddons(string[] assemblies) {
            // Return all assemblies specified, and create addons from each.
            return assemblies.SelectMany(path => {
                var assembly = loadAddon(path);
                return createAddons(assembly);
            });
        }

        /// <summary>
        /// Loads an assembly.
        /// </summary>
        /// <param name="path">The assembly path to load from.</param>
        /// <returns>The loaded assembly.</returns>
        private static Assembly loadAddon(string path) {
            // Load from here.
            string fullPath = Path.GetFullPath(path);

            // Find the addons within the addons folder.
            var context = new AddonLoadContext(fullPath);

            return context.LoadFromAssemblyName(new
                AssemblyName(Path.GetFileNameWithoutExtension(path)));
        }

        /// <summary>
        /// Create instances of each addon from an assembly.
        /// </summary>
        /// <param name="assembly">The assembly to create addons from.</param>
        /// <returns>List of addons.</returns>
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
