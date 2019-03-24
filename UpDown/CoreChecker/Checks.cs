using System;
using System.Collections.Generic;
using System.Text;

namespace UpDown.CoreChecker {
    public class Checks {
        public bool ResponseCode { get; set; }
        public List<int> AllowedResponseCodes { get; set; }
        public bool TryCatch { get; set; }
        public bool Keyword { get; set; }
        public bool KeywordRegex { get; set; }
        public string[] Keywords { get; set; }
        public bool Cloudflare { get; set; }
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
