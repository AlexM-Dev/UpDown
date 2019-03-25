using System;
using System.Threading.Tasks;
using UpDown.Core.Messaging;

namespace UpDown.Core {
    public interface IAddon {
        /// <summary>
        /// The name of the addon.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Initialise/make the addon ready.
        /// </summary>
        void Initialise(Checker checker, Logger logger);

        /// <summary>
        /// Send a shutdown signal, make the addon unload.
        /// </summary>
        void Shutdown();

        /// <summary>
        /// Send a message to the addon. Inter-addon only.
        /// </summary>
        /// <param name="source">The addon source.</param>
        /// <param name="message">The message object to send.</param>
        /// <returns>Asynchronous object message to return.</returns>
        Task<object> SendMessage(IAddon source, object message);
    }
}
