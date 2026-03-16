namespace Cuttage.Models;

public sealed record VideoProcessingRequest(
    string InputFilePath,
    string OutputFilePath,
    TimeSpan StartTime,
    TimeSpan EndTime)
{
    public TimeSpan Duration => EndTime - StartTime;

    public bool IsValid() =>
        !string.IsNullOrEmpty(InputFilePath) &&
        !string.IsNullOrEmpty(OutputFilePath) &&
        StartTime < EndTime &&
        File.Exists(InputFilePath);
}
