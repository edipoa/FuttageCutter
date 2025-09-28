using Cuttage.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuttage.Services
{
    public class FFmpegVideoProcessingService : IVideoProcessingService
    {
        private const string FFMPEG_EXECUTABLE = "ffmpeg.exe";

        public bool IsFFmpegAvailable()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = FFMPEG_EXECUTABLE,
                        Arguments = "-version",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                process.WaitForExit();
                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<VideoProcessingResult> CutVideoAsync(VideoProcessingRequest request,
            IProgress<string> progress = null)
        {
            if (!request.IsValid())
            {
                return VideoProcessingResult.CreateError("Parâmetros inválidos para o processamento.");
            }

            if (!IsFFmpegAvailable())
            {
                return VideoProcessingResult.CreateError("FFmpeg não encontrado. " +
                    "Certifique-se de que o ffmpeg.exe está no diretório da aplicação ou no PATH do sistema.");
            }

            try
            {
                progress?.Report("Iniciando processamento...");

                var outputDir = Path.GetDirectoryName(request.OutputFilePath);
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                var arguments = BuildFFmpegArguments(request);

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = FFMPEG_EXECUTABLE,
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                progress?.Report("Executando FFmpeg...");

                process.Start();

                var output = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                if (process.ExitCode == 0)
                {
                    progress?.Report("Processamento concluído!");
                    return VideoProcessingResult.CreateSuccess();
                }
                else
                {
                    return VideoProcessingResult.CreateError(
                        "Erro durante o processamento do vídeo.", output);
                }
            }
            catch (Exception ex)
            {
                return VideoProcessingResult.CreateError(
                    "Erro inesperado durante o processamento.", ex.Message);
            }
        }

        private string BuildFFmpegArguments(VideoProcessingRequest request)
        {
            var inputPath = request.InputFilePath;
            var outputPath = request.OutputFilePath;
            var start = request.StartTime;
            var duration = request.Duration;

            return $"-i \"{inputPath}\" -ss {start:hh\\:mm\\:ss\\.fff} -t {duration:hh\\:mm\\:ss\\.fff} -c copy \"{outputPath}\"";
        }
    }
}
