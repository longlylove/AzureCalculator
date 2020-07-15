using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utilities
{
    public class StopwatchHelper: IDisposable
    {
        private Stopwatch _stopwatch;

        public TimeSpan Elapsed => _stopwatch.Elapsed;

        public StopwatchHelper()
        {
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            _stopwatch = null;
        }
    }
}
