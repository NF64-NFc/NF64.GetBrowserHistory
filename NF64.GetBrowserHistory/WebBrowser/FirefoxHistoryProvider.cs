﻿using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace NF64.WebBrowser
{
    public sealed class FirefoxHistoryProvider : ChromiumHistoryProvider
    {
        public FirefoxHistoryProvider(string historyFilePath) : base(historyFilePath) { }


        protected override WebBrowserHistory GetHistoryItem(DataRow historyRow)
            => new WebBrowserHistory() {
                    Url = Convert.ToString(historyRow["url"]),
                    Title = Convert.ToString(historyRow["title"]),
                    VisitedTime = this.UnixTimeToLocalTime(historyRow),
                };

        protected override SQLiteDataAdapter CreateDataAdapter(SQLiteConnection connection)
            => new SQLiteDataAdapter($"SELECT * FROM moz_places ORDER BY last_visit_date DESC", connection);


        private DateTime UnixTimeToLocalTime(DataRow historyRow)
        {
            var unixTimeMicroSeconds = Convert.ToInt64(historyRow["last_visit_date"]) / 1000000;
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var gmtTime = epoch.AddSeconds(unixTimeMicroSeconds);
            return TimeZoneInfo.ConvertTimeFromUtc(gmtTime, TimeZoneInfo.Local);
        }
    }
}