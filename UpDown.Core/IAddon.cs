using System;
using System.Threading.Tasks;

namespace UpDown.Core {
    public interface IAddon {
        string Name { get; }
        void Initialise();
        void Shutdown();
        Task<object> SendMessage(IAddon source, object message);
    }
}
