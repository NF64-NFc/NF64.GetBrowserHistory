namespace NF64.WebBrowser.Provider
{
    internal interface IWebBrowserHistoryPath
    {
        string HistoryFilePath { get; }

        string InjectedHistoryFilePath { get; set; }


        string GetEnsuredHistoryFilePath();
    }
}
