using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace UpDown.Core.Events {
    public class OnCheckingEventArgs {
        public string Website { get; }
        public HttpClient Client { get; }
        public HttpResponseMessage Message { get; }
        public string Content { get; }
        public bool Error { get; }
        public Exception Exception { get; }
        public List<string> Sources { get; }
        public bool IsDown { get; set; }
        public OnCheckingEventArgs(string website, HttpClient client,
            HttpResponseMessage message, string content,
            bool error, Exception exception) {

            this.Website = website;
            this.Client = client;
            this.Message = message;
            this.Content = content;
            this.Error = error;
            this.Exception = exception;
            this.Sources = new List<string>();
        }
    }
}
