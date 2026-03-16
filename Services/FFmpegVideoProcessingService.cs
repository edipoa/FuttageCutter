using Cuttage.Models;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Cuttage.Services;

public sealed partial class FFmpegVideoProcessingService : IVideoProcessingService
{
    private static readonly string? _ffmpegPath = FindFFmpegPath();

    private static string? FindFFmpegPath()
    {
        // 1. Check app subfolder ffmpeg/ffmpeg.exe (bundled)
        var bundled = Path.Combine(AppContext.BaseDirectory, "ffmpeg", "ffmpeg.exe");
        if (File.Exists(bundled)) return bundled;

        // 2. Check app directory directly
        var local = Path.Combine(AppContext.BaseDirectory, "ffmpeg.exe");
        if (File.Exists(local)) return local;

        // 3. Rely on PATH
        return "ffmpeg";
    }

    public bool IsFFmpegAvailable()
    {
        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _ffmpegPath ?? "ffmpeg",
                    Arguments = "-version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit(3000);
            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<VideoProcessingResult> CutVideoAsync(
        VideoProcessingRequest request,
        IProgress<string>? progress = null,
        CancellationToken cancellationToken = default)
    {
        if (!request.IsValid())
            return VideoProcessingResult.Fail("Parâmetros inválidos para o processamento.");

        if (_ffmpegPath is null || !IsFFmpegAvailable())
            return VideoProcessingResult.Fail(
                "FFmpeg não encontrado.\n" +
                "Coloque o ffmpeg.exe na pasta 'ffmpeg/' da aplicação ou adicione ao PATH.");

        try
        {
            progress?.Report("Iniciando processamento...");

            var outputDir = Path.GetDirectoryName(request.OutputFilePath);
            if (!string.IsNullOrEmpty(outputDir))
                Directory.CreateDirectory(outputDir);

            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _ffmpegPath,
                    Arguments = BuildArguments(request),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            process.Start();

            var errorOutput = new StringBuilder();
            var readTask = ReadProgressAsync(
                process.StandardError,
                request.Duration,
                progress,
                errorOutput,
                cancellationToken);

            try
            {
                await process.WaitForExitAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                try { process.Kill(entireProcessTree: true); } catch { }
                await readTask.ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
                TryDeletePartialOutput(request.OutputFilePath);
                return VideoProcessingResult.Fail("Processamento cancelado pelo usuário.");
            }

            await readTask;

            return process.ExitCode == 0
                ? VideoProcessingResult.Ok()
                : VideoProcessingResult.Fail("Erro durante o processamento do vídeo.", errorOutput.ToString());
        }
        catch (OperationCanceledException)
        {
            return VideoProcessingResult.Fail("Processamento cancelado pelo usuário.");
        }
        catch (Exception ex)
        {
            return VideoProcessingResult.Fail("Erro inesperado durante o processamento.", ex.Message);
        }
    }

    private static async Task ReadProgressAsync(
        StreamReader stderr,
        TimeSpan totalDuration,
        IProgress<string>? progress,
        StringBuilder errorOutput,
        CancellationToken ct)
    {
        string? line;
        while ((line = await stderr.ReadLineAsync(ct).ConfigureAwait(false)) is not null)
        {
            errorOutput.AppendLine(line);

            if (progress is null || totalDuration.TotalSeconds <= 0) continue;

            var match = TimePattern().Match(line);
            if (!match.Success) continue;

            if (!TimeSpan.TryParseExact(match.Value, @"hh\:mm\:ss\.ff", null, out var current))
                continue;

            var pct = (int)Math.Min(current.TotalSeconds / totalDuration.TotalSeconds * 100, 99);
            progress.Report($"Processando... {pct}%");
        }
    }

    private static string BuildArguments(VideoProcessingRequest request) =>
        $"-y -i \"{request.InputFilePath}\" " +
        $"-ss {request.StartTime:hh\\:mm\\:ss\\.fff} " +
        $"-t {request.Duration:hh\\:mm\\:ss\\.fff} " +
        $"-c copy \"{request.OutputFilePath}\"";

    private static void TryDeletePartialOutput(string path)
    {
        try { if (File.Exists(path)) File.Delete(path); } catch { }
    }

    [GeneratedRegex(@"\d{2}:\d{2}:\d{2}\.\d{2}")]
    private static partial Regex TimePattern();
}
