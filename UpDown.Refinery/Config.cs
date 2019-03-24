using System;
using System.Collections.Generic;
using System.Text;

namespace UpDown.Refinery {
    internal class Config {
        public bool Enabled { get; set; }
        public bool ForceLog { get; set; }
        public bool UseOutputFile { get; set; }
        public string OutputFilePath { get; set; }
        
        public Config() {
            this.Enabled = true;
            this.ForceLog = true;

            this.UseOutputFile = false;
            this.OutputFilePath = "refinery.log";
        }
    }
}
