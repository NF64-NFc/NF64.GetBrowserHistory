﻿using System;
using System.Collections.Generic;
using System.Linq;
using NF64.WebBrowser;

namespace NF64
{
    internal static class Program
    {
        private static readonly string TempDirectoryPath = @"D:\GetBrowserHistory";


        private static void Main()
        {
            var providers = new IWebBroserHistoryProvider[] {
                    new ChromiumHistoryProvider(WebBrowserHistoryPath.ChromiumEdgePath),
                    new ChromiumHistoryProvider(WebBrowserHistoryPath.GoogleChromePath),
                }.Concat(GetFireFoxProviders());

            var loader = new WebBrowserHistoryLoader(TempDirectoryPath, providers);
            var histories = loader.Load();

            foreach (var history in histories)
                Console.WriteLine(history);

            return;
        }


        private static IEnumerable<IWebBroserHistoryProvider> GetFireFoxProviders()
        {
            var paths = WebBrowserHistoryPath.GetFirefoxHistoryFilePath();
            var ret = new List<FirefoxHistoryProvider>();
            foreach (var path in paths)
                ret.Add(new FirefoxHistoryProvider(path));
            return ret;
        }
    }
}