using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace UpDown.IO {
    internal class Resources {
        public static async Task<IEnumerable<string>> GetDocumentationAsync(
            params string[] docs) {
            var cur = Assembly.GetExecutingAssembly();
            var bname = cur.GetName().Name + ".Docs.";

            return await Task.WhenAll(from doc in docs
                                      select 
                                      getResourceAsync(cur, bname + doc + ".txt"));
        }

        private static async Task<string> getResourceAsync(Assembly a, string name) {
            using (var stream = a.GetManifestResourceStream(name))
            using (var reader = new StreamReader(stream))
                return await reader.ReadToEndAsync();
        }
        
        internal static async Task<string[]> GetDocumentationAsync() {
            var documentation = (await GetDocumentationAsync(
               "tos", "configuration")).ToArray();

            return documentation;
        }
    }
}
