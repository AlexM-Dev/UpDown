using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UpDown.IO;

namespace UpDown.CoreChecker {
    public class Config {
        /// <summary>
        /// The path to load addons from.
        /// </summary>
        public string AddonPath { get; set; }

        /// <summary>
        /// The format of the addon names.
        /// </summary>
        public string AddonsFormat { get; set; }

        /// <summary>
        /// Enable verbose logging.
        /// </summary>
        public bool EnableLog { get; set; }

        /// <summary>
        /// Enable file logging.
        /// </summary>
        public bool EnableLogFile { get; set; }

        /// <summary>
        /// The file to log to.
        /// </summary>
        public string LogFilePath { get; set; }

        /// <summary>
        /// Whether the TOS has been accepted or not.
        /// </summary>
        public bool AcceptedTOS { get; set; }

        /// <summary>
        /// The interval, in seconds, to check websites..
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// The websites to check.
        /// </summary>
        public List<string> Websites { get; set; }

        /// <summary>
        /// The base checks to perform for each website.
        /// </summary>
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
