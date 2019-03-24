using System;
using System.Collections.Generic;
using System.Text;

namespace UpDown.Core.Events {
    public class OnAllChecksEventArgs {
        public Dictionary<string, bool> WebsitesDown { get; }

        public OnAllChecksEventArgs(Dictionary<string, bool> websites) {
            this.WebsitesDown = websites;
        }
    }
}
