using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Framework.Models;

namespace Framework.Utilities
{
    public class Utils
    {
        public static string BuildDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public readonly string ReportsDirectory = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"\Reports");
        private static Contexts _context;

        public Utils() { }

        public Utils(Contexts context)
        {
            _context = context;
        }

        public void Log(string message, string header = "")
        {
            var log = $"{header}\r\n\t{message}";
            Console.WriteLine("-> " + log);

            if (_context != null && _context.Logs != null)
            {
                log = "\t" + log;
                _context.Logs.Add(log.Replace("\r\n", "<br />&emsp;").Replace("\t", "&emsp;"));
            }
        }

        public string GetAppSettings(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }

        public void WaitInMilliseconds(int milliseconds)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(milliseconds ));
        }
    }
}
