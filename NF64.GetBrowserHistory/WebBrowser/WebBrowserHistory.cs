using System;

namespace NF64.WebBrowser
{
    public sealed class WebBrowserHistory
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public DateTime VisitedTime { get; set; }


        public override string ToString()
            => string.Join(", ", new[] {
                    $"{nameof(Title)}={Title}",
                    $"{nameof(Url)}={Url}",
                    $"{nameof(VisitedTime)}={VisitedTime}",
            });
    }
}
