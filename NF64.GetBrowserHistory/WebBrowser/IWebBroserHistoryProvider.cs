using System.Collections.Generic;

namespace NF64.WebBrowser
{
    public interface IWebBroserHistoryProvider
    {
        string HistoryFilePath { get; }

        IEnumerable<WebBrowserHistory> GetHistories();
    }
}
