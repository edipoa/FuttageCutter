using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuttage.Models
{
    public class VideoProcessingRequest
    {
        public string InputFilePath { get; set; }
        public string OutputFilePath { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(InputFilePath) &&
                   !string.IsNullOrEmpty(OutputFilePath) &&
                   StartTime < EndTime &&
                   System.IO.File.Exists(InputFilePath);
        }

        public TimeSpan Duration => EndTime - StartTime;
    }
}
