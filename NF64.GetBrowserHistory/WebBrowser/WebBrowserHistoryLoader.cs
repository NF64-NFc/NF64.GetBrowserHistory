using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NF64.WebBrowser.Provider;

namespace NF64.WebBrowser
{
    public sealed class WebBrowserHistoryLoader
    {
        public string TemporaryDirectoryPath { get; }

        public IEnumerable<WebBrowserHistoryProvider> Providers { get; }


        public int TimeoutMilliseconds { get; set; } = -1;


        public WebBrowserHistoryLoader(string temporaryDirectoryPath, IEnumerable<WebBrowserHistoryProvider> providers)
        {
            if (string.IsNullOrEmpty(temporaryDirectoryPath))
                throw new ArgumentException($"{temporaryDirectoryPath} is null or empty", nameof(temporaryDirectoryPath));
            this.TemporaryDirectoryPath = temporaryDirectoryPath;
            this.Providers = providers ?? throw new ArgumentNullException(nameof(providers));
        }


        public IEnumerable<WebBrowserHistory> Load()
        {
            try
            {
                var tasks = this.Providers.Select(
                        provider => Task.Run(() => this.LoadHistory(provider))
                    ).ToArray();
                Task.WaitAll(tasks, this.TimeoutMilliseconds);

                var exceptions = tasks.Where(t => t.Exception != null).Select(t => t.Exception);
                if (exceptions.Any())
                    throw new AggregateException(exceptions);

                var ret = new List<WebBrowserHistory>();
                foreach (var task in tasks)
                    ret.AddRange(task.Result);
                return ret.OrderByDescending(history => history.VisitedTime);
            }
            finally
            {
                this.ClearDirectory(this.TemporaryDirectoryPath);
            }
        }

        private IEnumerable<WebBrowserHistory> LoadHistory(WebBrowserHistoryProvider provider)
        {
            var ret = new List<WebBrowserHistory>();

            var injectableProvider = provider as IWebBrowserHistoryPathInjectable;
            string copiedHistoryFilePath = null;
            try
            {
                copiedHistoryFilePath = this.CopyHistoryFile(provider);
                injectableProvider.InjectedHistoryFilePath = copiedHistoryFilePath;

                var histories = provider.GetHistories();
                if (histories?.Any() == true)
                    ret.AddRange(histories);
            }
            finally
            {
                injectableProvider.InjectedHistoryFilePath = null;

                if (copiedHistoryFilePath != null)
                {
                    if (File.Exists(copiedHistoryFilePath))
                        File.Delete(copiedHistoryFilePath);
                }
            }

            return ret;
        }

        private string CopyHistoryFile(WebBrowserHistoryProvider provider)
        {
            this.EnsureDirectory(this.TemporaryDirectoryPath);

            var sourceFilePath = provider.HistoryFilePath;
            if (!File.Exists(sourceFilePath))
                throw new FileNotFoundException("file not found.", sourceFilePath);

            var destFilePath = Path.Combine(this.TemporaryDirectoryPath, Guid.NewGuid().ToString());
            File.Copy(sourceFilePath, destFilePath);
            return destFilePath;
        }

        private void EnsureDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
                return;
            Directory.CreateDirectory(directoryPath);
        }

        private void ClearDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                return;

            foreach (var file in Directory.GetFiles(directoryPath))
                if (File.Exists(file))
                    File.Delete(file);
        }
    }
}
