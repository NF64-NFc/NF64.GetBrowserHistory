using System.Collections.Generic;

namespace NF64.WebBrowser.Provider
{
    public interface IWebBrowserHistoryProvider
    {
        string BrowserName { get; }


        IEnumerable<WebBrowserHistory> GetHistories();
    }
}
