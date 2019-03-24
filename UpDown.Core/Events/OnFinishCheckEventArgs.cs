using System;
using System.Collections.Generic;
using System.Text;

namespace UpDown.Core.Events {
    public class OnFinishCheckEventArgs : EventArgs {
        public string Website { get; }
        public bool IsDown { get; }
        public List<string> Sources { get; }
        public OnFinishCheckEventArgs(string website, bool isDown, 
            List<string> sources) {
            this.Website = website;
            this.IsDown = isDown;
            this.Sources = sources;
        }
    }
}
