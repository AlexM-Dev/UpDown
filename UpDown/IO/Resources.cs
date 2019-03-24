using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace UpDown.IO {
    internal class Resources {
        /// <summary>
        /// Provides documentation from the documentation resources. 
        /// </summary>
        /// <param name="docs">Documentation file names to load</param>
        /// <returns>Documentation.</returns>
        public static async Task<IEnumerable<string>> GetDocumentationAsync(
            params string[] docs) {
            // Current assembly & assembly path.
            var cur = Assembly.GetExecutingAssembly();
            var bname = cur.GetName().Name + ".Docs.";

            // Load each assembly resource.
            return await Task.WhenAll(from doc in docs
                                      select 
                                      getResourceAsync(cur, bname + doc + ".txt"));
        }

        /// <summary>
        /// Load an embedded resource.
        /// </summary>
        /// <param name="a">The assembly to load from.</param>
        /// <param name="name">The name of the resource to load.</param>
        /// <returns>The resource.</returns>
        private static async Task<string> getResourceAsync(Assembly a, string name) {
            using (var stream = a.GetManifestResourceStream(name))
            using (var reader = new StreamReader(stream))
                return await reader.ReadToEndAsync();
        }
        
        /// <summary>
        /// Gets TOS and Configuration resources.
        /// </summary>
        /// <returns>TOS & Configuration resources.</returns>
        public static async Task<string[]> GetDocumentationAsync() {
            var documentation = (await GetDocumentationAsync(
               "tos", "configuration")).ToArray();

            return documentation;
        }
    }
}
