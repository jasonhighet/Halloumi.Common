using System;
using System.Windows.Forms;

namespace Halloumi.Common.Windows.Helpers
{
    public static class WindowsApplicationHelper
    {
        public static void InitialiseWindowsApplication()
        {
            if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
