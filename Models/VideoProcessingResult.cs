using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuttage.Models
{
    public class VideoProcessingResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ErrorDetails { get; set; }

        public static VideoProcessingResult CreateSuccess(string message = "Vídeo processado com sucesso!")
        {
            return new VideoProcessingResult
            {
                Success = true,
                Message = message
            };
        }

        public static VideoProcessingResult CreateError(string message, string errorDetails = null)
        {
            return new VideoProcessingResult
            {
                Success = false,
                Message = message,
                ErrorDetails = errorDetails
            };
        }
    }
}
