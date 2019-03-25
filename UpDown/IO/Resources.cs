using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace UpDown.IO {
    internal class Resources {
        static Resources() {
            var assembly = Assembly.GetExecutingAssembly();
            var name = assembly.GetName().Name;

            string tosPath = $"{name}.Docs.tos.txt";
            string configurationPath = $"{name}.Docs.configuration.txt";

            TOS = getResource(assembly, tosPath);
            Configuration = getResource(assembly, configurationPath);
        }

        public static string TOS { get; private set; }
        public static string Configuration { get; private set; }

        /// <summary>
        /// Load an embedded resource.
        /// </summary>
        /// <param name="a">The assembly to load from.</param>
        /// <param name="name">The name of the resource to load.</param>
        /// <returns>The resource.</returns>
        private static string getResource(Assembly a, string name) {
            using (var stream = a.GetManifestResourceStream(name))
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}
