using System;
using System.Linq;
using NF64.WebBrowser;
using NF64.WebBrowser.Provider;

namespace NF64
{
    internal static class Program
    {
        private static readonly string TempDirectoryPath = @"D:\GetBrowserHistory";


        private static void Main()
        {
            var providers = new IWebBrowserHistoryProvider[] {
                    new IEHistoryProvider(),
                    WebBrowserHistoryProviderHelper.CreateChromiumEdgeHistoryProvider(),
                    WebBrowserHistoryProviderHelper.CreateChromeHistoryProvider(),
                    null
                }.Concat(WebBrowserHistoryProviderHelper.CreateFirefoxHistoryProviders());

            var loader = new WebBrowserHistoryLoader(TempDirectoryPath, providers);
            var histories = loader.Load();

            foreach (var history in histories)
                Console.WriteLine(history);

            return;
        }
    }
}
