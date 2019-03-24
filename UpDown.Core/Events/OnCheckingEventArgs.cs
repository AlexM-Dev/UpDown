using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace UpDown.Core.Events {
    public class OnCheckingEventArgs {
        /// <summary>
        /// The website currently being checked.
        /// </summary>
        public string Website { get; }

        /// <summary>
        /// The client being used to check the website.
        /// </summary>
        public HttpClient Client { get; }

        /// <summary>
        /// The received message from the check.
        /// </summary>
        public HttpResponseMessage Message { get; }

        /// <summary>
        /// The string representation of the message's content.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Whether the result is errored.
        /// </summary>
        public bool Error { get; }

        /// <summary>
        /// The exception, if an error is received.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Sources from which the checks failed.
        /// </summary>
        public List<string> Sources { get; }

        /// <summary>
        /// Whether the website is down or not.
        /// </summary>
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
