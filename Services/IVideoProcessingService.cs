using Cuttage.Models;

namespace Cuttage.Services;

public interface IVideoProcessingService
{
    Task<VideoProcessingResult> CutVideoAsync(
        VideoProcessingRequest request,
        IProgress<string>? progress = null,
        CancellationToken cancellationToken = default);

    bool IsFFmpegAvailable();
}
