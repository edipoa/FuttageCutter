using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuttage.Extensions
{
    public static class ProcessExtensions
    {
        public static async Task WaitForExitAsync(this Process process)
        {
            await Task.Run(() => process.WaitForExit());
        }
    }
}
