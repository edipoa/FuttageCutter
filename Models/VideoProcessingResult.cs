namespace Cuttage.Models;

public sealed record VideoProcessingResult(
    bool Success,
    string Message,
    string? ErrorDetails = null)
{
    public static VideoProcessingResult Ok(string message = "Vídeo cortado com sucesso!") =>
        new(true, message);

    public static VideoProcessingResult Fail(string message, string? details = null) =>
        new(false, message, details);
}
