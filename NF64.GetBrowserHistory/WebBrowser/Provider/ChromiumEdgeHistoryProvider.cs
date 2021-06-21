namespace NF64.WebBrowser.Provider
{
    public sealed class ChromiumEdgeHistoryProvider : ChromiumHistoryProvider
    {
        public ChromiumEdgeHistoryProvider(string historyFilePath) : base("Edge (Chromium)", historyFilePath) { }
    }
}
