using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuttage.Views
{
    public interface IMainView
    {
        string InputFilePath { get; set; }
        string OutputFilePath { get; set; }
        TimeSpan StartTime { get; set; }
        TimeSpan EndTime { get; set; }
        bool IsProcessing { set; }

        void ShowMessage(string message, string title, bool isError = false);
        void UpdateProgress(string message);
    }
}
