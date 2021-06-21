using System.Collections.Generic;
using System.IO;
using NF64.WebBrowser.Provider;

namespace NF64.WebBrowser
{
    public static class WebBrowserHistoryProviderHelper
    {
        public static IWebBrowserHistoryProvider CreateChromiumEdgeHistoryProvider()
        {
            if (!File.Exists(WebBrowserHistoryPath.ChromiumEdgePath))
                return null;
            return new ChromiumEdgeHistoryProvider(WebBrowserHistoryPath.ChromiumEdgePath);
        }

        public static IWebBrowserHistoryProvider CreateChromeHistoryProvider()
        {
            if (!File.Exists(WebBrowserHistoryPath.GoogleChromePath))
                return null;
            return new ChromeHistoryProvider(WebBrowserHistoryPath.GoogleChromePath);
        }

        public static IEnumerable<IWebBrowserHistoryProvider> CreateFirefoxHistoryProviders()
        {
            var paths = WebBrowserHistoryPath.GetFirefoxHistoryFilePath();
            var ret = new List<FirefoxHistoryProvider>();
            foreach (var path in paths)
            {
                if (File.Exists(path))
                    ret.Add(new FirefoxHistoryProvider(path));
            }
            return ret;
        }
    }
}
