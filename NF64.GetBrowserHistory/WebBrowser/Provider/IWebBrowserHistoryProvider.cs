using System.Collections.Generic;

namespace NF64.WebBrowser.Provider
{
    public interface IWebBrowserHistoryProvider
    {
        IEnumerable<WebBrowserHistory> GetHistories();
    }
}
