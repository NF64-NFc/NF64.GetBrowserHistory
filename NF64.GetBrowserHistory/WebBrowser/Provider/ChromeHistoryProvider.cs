namespace NF64.WebBrowser.Provider
{
    public sealed class ChromeHistoryProvider : ChromiumHistoryProvider
    {
        public ChromeHistoryProvider(string historyFilePath) : base("Google Chrome", historyFilePath) { }
    }
}
