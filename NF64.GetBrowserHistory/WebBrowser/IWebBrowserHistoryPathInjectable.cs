namespace NF64.WebBrowser
{
    internal interface IWebBrowserHistoryPathInjectable
    {
        string HistoryFilePath { get; }

        string InjectedHistoryFilePath { get; set; }


        string GetEnsuredHistoryFilePath();
    }
}
