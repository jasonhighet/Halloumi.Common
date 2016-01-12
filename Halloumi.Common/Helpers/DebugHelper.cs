using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Halloumi.Common.Helpers
{
    public static class DebugHelper
    {
        private static bool DebugMode { get; set; }

        static DebugHelper()
        {
            DebugMode = ApplicationHelper.IsDebugMode();
        }

        public static void WriteLine(object value)
        {
            WriteLine(value.ToString());
        }

        public static void WriteLine(string value)
        {
            if(!DebugMode) return;
            
            var timeStamp = DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);
            Console.WriteLine(timeStamp + @"    " + value);
        }

        public static void Write(string value)
        {
            if (!DebugMode) return;

            var timeStamp = DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);
            Console.Write(timeStamp + @"    " + value);
        }
    }
}