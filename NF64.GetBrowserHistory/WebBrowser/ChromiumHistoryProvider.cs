using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace NF64.WebBrowser
{
    public class ChromiumHistoryProvider : IWebBroserHistoryProvider, IWebBrowserHistoryPathInjectable
    {
        public string HistoryFilePath { get; }


        public ChromiumHistoryProvider(string historyFilePath)
        {
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


        protected virtual WebBrowserHistory GetHistoryItem(DataRow historyRow)
            => new WebBrowserHistory() {
                Url = Convert.ToString(historyRow["url"]),
                Title = Convert.ToString(historyRow["title"]),
                VisitedTime = this.UtcTimeToLocalTime(historyRow)
            };

        protected virtual SQLiteConnection CreateConnection(string hisotryFilePath)
            => new SQLiteConnection($"Data Source={hisotryFilePath}; Version=3; New=False; Compress=True;");

        protected virtual SQLiteDataAdapter CreateDataAdapter(SQLiteConnection connection)
            => new SQLiteDataAdapter($"SELECT * FROM urls ORDER BY last_visit_time DESC", connection);


        private DateTime UtcTimeToLocalTime(DataRow historyRow)
        {
            var utcMicroSeconds = Convert.ToInt64(historyRow["last_visit_time"]);
            var gmtTime = DateTime.FromFileTimeUtc(utcMicroSeconds * 10);
            return TimeZoneInfo.ConvertTimeFromUtc(gmtTime, TimeZoneInfo.Local);
        }


        string IWebBrowserHistoryPathInjectable.InjectedHistoryFilePath { get; set; }

        string IWebBrowserHistoryPathInjectable.GetEnsuredHistoryFilePath()
            => (this as IWebBrowserHistoryPathInjectable).InjectedHistoryFilePath ?? this.HistoryFilePath;


        private string GetEnsuredHistoryFilePath()
            => (this as IWebBrowserHistoryPathInjectable).GetEnsuredHistoryFilePath();
    }
}
