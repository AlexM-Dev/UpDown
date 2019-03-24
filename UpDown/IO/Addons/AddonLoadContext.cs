using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace UpDown.IO.Addons {
    internal class AddonLoadContext : AssemblyLoadContext {
        private AssemblyDependencyResolver resolver;

        public AddonLoadContext(string path) {
            resolver = new AssemblyDependencyResolver(path);
        }

        protected override Assembly Load(AssemblyName name) {
            var path = resolver.ResolveAssemblyToPath(name);
            
            if (path != null) {
                return LoadFromAssemblyPath(path);
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string name) {
            var path = resolver.ResolveUnmanagedDllToPath(name);

            if (path != null) {
                return LoadUnmanagedDllFromPath(path);
            }

            return IntPtr.Zero;
        }
    }
}
