using System;
using System.Collections.Generic;
using UrlHistoryLibrary;

namespace NF64.WebBrowser.Provider
{
    public sealed class IEHistoryProvider : IWebBrowserHistoryProvider
    {
        public string BrowserName { get; }


        public IEHistoryProvider()
        {
            this.BrowserName = "Internet Explorer";
        }


        public IEnumerable<WebBrowserHistory> GetHistories()
        {
            var ret = new List<WebBrowserHistory>();
            var urlHistory = new UrlHistoryWrapperClass();

            var enumerator = urlHistory.GetEnumerator();
            while (enumerator.MoveNext())
                ret.Add(this.GetHistoryItem(enumerator));
            enumerator.Reset();

            return ret;
        }


        private WebBrowserHistory GetHistoryItem(UrlHistoryWrapperClass.STATURLEnumerator enumerator)
            => new WebBrowserHistory() {
                Url = enumerator.Current.URL,
                Title = enumerator.Current.Title,
                VisitedTime = this.IETimeToLocalTime(enumerator),
                BrowserName = this.BrowserName,
            };


        private DateTime IETimeToLocalTime(UrlHistoryWrapperClass.STATURLEnumerator enumerator)
        {
            var timeLong = ((long)enumerator.Current.ftLastVisited.dwHighDateTime) << 32 | (uint)enumerator.Current.ftLastVisited.dwLowDateTime;
            var dateTime = DateTime.FromFileTime(timeLong);
            return dateTime;
        }
    }
}
