using System;
using System.Collections.Generic;
using System.Text;

namespace UpDown.Core.Events {
    public static class CheckerEvents {
        public static EventHandler<OnFinishCheckEventArgs> CheckCompleted =
            (o, e) => { };

        public static EventHandler<OnAllChecksEventArgs> AllChecksCompleted =
            (o, e) => { };

        public static EventHandler<OnCheckingEventArgs> CheckingWebsite =
            (o, e) => { };

        public static EventHandler MessengerInitialised = (o, e) => { };
    }
}
