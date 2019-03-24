using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UpDown.IO;

namespace UpDown.CoreChecker {
    public class Config {
        public string AddonPath { get; set; }
        public string AddonsFormat { get; set; }
        public bool EnableLog { get; set; }
        public bool EnableLogFile { get; set; }
        public string LogFilePath { get; set; }
        public bool AcceptedTOS { get; set; }
        public int Interval { get; set; }
        public List<string> Websites { get; set; }
        public Checks CheckSeries { get; set; }

        public Config() {
            this.AddonPath = "addons";
            this.AddonsFormat = "*.dll";

            this.EnableLog = true;
            this.EnableLogFile = true;
            this.LogFilePath = "output.log";

            this.AcceptedTOS = false;
            this.Interval = 300;
            this.Websites = new List<string>();
            this.CheckSeries = new Checks();
        }
    }
}
