namespace Cuttage.Views;

public interface IMainView
{
    string InputFilePath { get; set; }
    string OutputFilePath { get; set; }
    TimeSpan StartTime { get; set; }
    TimeSpan EndTime { get; set; }
    bool IsProcessing { set; }
    string StatusText { set; }
    string DurationText { set; }

    void ShowMessage(string message, string title, bool isError = false);
}
