using System;

namespace NF64.WebBrowser
{
    public sealed class WebBrowserHistory
    {
        public string Title { get; internal set; }

        public string Url { get; internal set; }

        public DateTime VisitedTime { get; internal set; }

        public string BrowserName { get; internal set; }


        public override string ToString()
            => string.Join(", ", new[] {
                    $"{nameof(Title)}={Title}",
                    $"{nameof(Url)}={Url}",
                    $"{nameof(VisitedTime)}={VisitedTime}",
                    $"{nameof(BrowserName)}={BrowserName}",
            });
    }
}
