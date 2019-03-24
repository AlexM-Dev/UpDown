using System;
using System.Collections.Generic;
using System.Text;

namespace UpDown.Core.Events {
    public class OnFinishCheckEventArgs : EventArgs {
        /// <summary>
        /// The website which has been checked.
        /// </summary>
        public string Website { get; }

        /// <summary>
        /// Whether the website is down or not.
        /// </summary>
        public bool IsDown { get; }

        /// <summary>
        /// The sources from which the checks failed, if applicable.
        /// </summary>
        public List<string> Sources { get; }

        public OnFinishCheckEventArgs(string website, bool isDown, 
            List<string> sources) {
            this.Website = website;
            this.IsDown = isDown;
            this.Sources = sources;
        }
    }
}
