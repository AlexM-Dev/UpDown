using System;
using System.Collections.Generic;
using System.Text;

namespace UpDown.Core.Events {
    public class OnAllChecksEventArgs {
        /// <summary>
        /// Data for each checked website that is up or down.
        /// </summary>
        public Dictionary<string, bool> WebsitesDown { get; }

        public OnAllChecksEventArgs(Dictionary<string, bool> websites) {
            this.WebsitesDown = websites;
        }
    }
}
