using System;
using System.Data;
using System.Data.SQLite;

namespace NF64.WebBrowser.Provider
{
    public abstract class ChromiumHistoryProvider : SQLiteWebBrowserHistoryProvider
    {
        public ChromiumHistoryProvider(string browserName, string historyFilePath) : base(browserName, historyFilePath) { }


        protected override WebBrowserHistory GetHistoryItem(DataRow historyRow)
            => new WebBrowserHistory() {
                Url = Convert.ToString(historyRow["url"]),
                Title = Convert.ToString(historyRow["title"]),
                VisitedTime = this.UtcTimeToLocalTime(historyRow),
                BrowserName = this.BrowserName,
            };

        protected override SQLiteConnection CreateConnection(string hisotryFilePath)
            => new SQLiteConnection($"Data Source={hisotryFilePath}; Version=3; New=False; Compress=True;");

        protected override SQLiteDataAdapter CreateDataAdapter(SQLiteConnection connection)
            => new SQLiteDataAdapter($"SELECT * FROM urls ORDER BY last_visit_time DESC", connection);


        private DateTime UtcTimeToLocalTime(DataRow historyRow)
        {
            var utcMicroSeconds = Convert.ToInt64(historyRow["last_visit_time"]);
            var gmtTime = DateTime.FromFileTimeUtc(utcMicroSeconds * 10);
            return TimeZoneInfo.ConvertTimeFromUtc(gmtTime, TimeZoneInfo.Local);
        }
    }
}
