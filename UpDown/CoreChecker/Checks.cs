using System;
using System.Collections.Generic;
using System.Text;

namespace UpDown.CoreChecker {
    public class Checks {
        /// <summary>
        /// Use a response code (>=400) to determine 
        /// whether a website is down.
        /// </summary>
        public bool ResponseCode { get; set; }

        /// <summary>
        /// A list of all allowed response codes, if Response Codes
        /// are enabled.
        /// </summary>
        public List<int> AllowedResponseCodes { get; set; }

        /// <summary>
        /// Use exception catch to determine if a site is down.
        /// </summary>
        public bool TryCatch { get; set; }

        /// <summary>
        /// Use a set of keywords to determine if a site is down.
        /// </summary>
        public bool Keyword { get; set; }

        /// <summary>
        /// Use Regex instead of a basic Contains() check.
        /// </summary>
        public bool KeywordRegex { get; set; }

        /// <summary>
        /// Keywords to use for determining if a site is down.
        /// </summary>
        public string[] Keywords { get; set; }

        /// <summary>
        /// Use Cloudflare for cloudflare-enabled sites.
        /// </summary>
        public bool Cloudflare { get; set; }

        /// <summary>
        /// How many maximum retries to do when trying to solve
        /// a CloudFlare challenge.
        /// </summary>
        public int CfMaxRetries { get; set; }

        public Checks() {
            this.ResponseCode = true;
            this.AllowedResponseCodes = new List<int>();
            this.TryCatch = true;
            this.Keyword = false;
            this.KeywordRegex = false;
            this.Cloudflare = false;
            this.CfMaxRetries = 5;

            this.Keywords = new string[0];
        }
    }
}
