using Cuttage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuttage.Services
{
    public interface IVideoProcessingService
    {
        Task<VideoProcessingResult> CutVideoAsync(VideoProcessingRequest request,
            IProgress<string> progress = null);
        bool IsFFmpegAvailable();
    }
}
