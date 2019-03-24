using System;
using System.Collections.Generic;
using System.Text;

namespace UpDown.Core.Events {
    public static class CheckerEvents {
        /// <summary>
        /// Event for when one website has been checked.
        /// </summary>
        public static EventHandler<OnFinishCheckEventArgs> CheckCompleted =
            (o, e) => { };

        /// <summary>
        /// Event for when every website has been checked.
        /// </summary>
        public static EventHandler<OnAllChecksEventArgs> AllChecksCompleted =
            (o, e) => { };

        /// <summary>
        /// Event for when checking a website. This can be used to alter
        /// the result for a check.
        /// </summary>
        public static EventHandler<OnCheckingEventArgs> CheckingWebsite =
            (o, e) => { };

        /// <summary>
        /// Event for when the messenger is ready to be used.
        /// </summary>
        public static EventHandler MessengerInitialised = (o, e) => { };
    }
}
