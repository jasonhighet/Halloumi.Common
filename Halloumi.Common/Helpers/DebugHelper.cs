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
        public static void WriteLine(object value)
        {
            WriteLine(value.ToString());
        }

        public static void WriteLine(string value)
        {
#if DEBUG
            var timeStamp = DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);
            Console.WriteLine(timeStamp + "\t" + value);
#endif
        }

        public static void Write(string value)
        {
#if DEBUG
            var timeStamp = DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);
            Console.Write(timeStamp + "\t" + value);
#endif
        }
    }
}