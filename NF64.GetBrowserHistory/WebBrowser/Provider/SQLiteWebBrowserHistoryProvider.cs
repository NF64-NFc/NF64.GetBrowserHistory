using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace NF64.WebBrowser.Provider
{
    public abstract class SQLiteWebBrowserHistoryProvider : IWebBrowserHistoryProvider, IWebBrowserHistoryPath
    {
        public string BrowserName { get; }

        public string HistoryFilePath { get; }


        protected SQLiteWebBrowserHistoryProvider(string browserName, string historyFilePath)
        {
            if (string.IsNullOrEmpty(browserName))
                throw new ArgumentException($"{browserName} is null or empty", nameof(browserName));
            this.BrowserName = browserName;

            if (string.IsNullOrEmpty(historyFilePath))
                throw new ArgumentException($"{historyFilePath} is null or empty", nameof(historyFilePath));
            this.HistoryFilePath = historyFilePath;
        }


        public IEnumerable<WebBrowserHistory> GetHistories()
        {
            var historyFilePath = this.GetEnsuredHistoryFilePath();
            using (var historyDataTable = this.LoadHistoryDataTable(historyFilePath))
            {
                if (historyDataTable == null)
                    return new WebBrowserHistory[] { };

                var ret = new List<WebBrowserHistory>();
                foreach (DataRow historyRow in historyDataTable.Rows)
                    ret.Add(this.GetHistoryItem(historyRow));
                return ret;
            }
        }

        private DataTable LoadHistoryDataTable(string historyFilePath)
        {
            if (string.IsNullOrEmpty(historyFilePath))
                throw new ArgumentException($"{historyFilePath} is null or empty", nameof(historyFilePath));
            if (!File.Exists(historyFilePath))
                throw new FileNotFoundException("file not found.", historyFilePath);

            using (var connection = this.CreateConnection(historyFilePath))
            {
                connection.Open();

                var dataTable = new DataTable();
                using (var adapter = this.CreateDataAdapter(connection))
                    adapter.Fill(dataTable);
                return dataTable;
            }
        }


        protected abstract WebBrowserHistory GetHistoryItem(DataRow historyRow);

        protected abstract SQLiteConnection CreateConnection(string hisotryFilePath);

        protected abstract SQLiteDataAdapter CreateDataAdapter(SQLiteConnection connection);


        string IWebBrowserHistoryPath.InjectedHistoryFilePath { get; set; }

        string IWebBrowserHistoryPath.GetEnsuredHistoryFilePath()
            => (this as IWebBrowserHistoryPath).InjectedHistoryFilePath ?? this.HistoryFilePath;


        private string GetEnsuredHistoryFilePath()
            => (this as IWebBrowserHistoryPath).GetEnsuredHistoryFilePath();
    }
}
