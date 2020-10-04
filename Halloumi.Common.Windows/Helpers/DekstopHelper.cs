using System.Drawing;
using System.Windows.Forms;

namespace Halloumi.Common.Windows.Helpers
{
    public static class DesktopHelper
    {
        #region Public Methods

        /// <summary>
        /// Gets the size of the desktop (including multiple monitors)
        /// </summary>
        /// <returns>The size of the desktop</returns>
        public static Size GetDesktopSize()
        {
            // calculate desktop size
            var desktopWidth = 0;
            var desktopHeight = 0;
            for (var i = 0; i < Screen.AllScreens.Length; i++)
            {
                desktopWidth += Screen.AllScreens[i].WorkingArea.Width;
                if (Screen.AllScreens[i].WorkingArea.Height > desktopHeight)
                {
                    desktopHeight = Screen.AllScreens[i].WorkingArea.Height;
                }
            }
            return new Size(desktopWidth, desktopHeight);
        }

        /// <summary>
        /// Gets the size of the primary desktop 
        /// </summary>
        /// <returns>The size of the desktop</returns>
        public static Size GetPrimaryDesktopSize()
        {
            return new Size(Screen.PrimaryScreen.WorkingArea.Width, 
                Screen.PrimaryScreen.WorkingArea.Height);
        }

        /// <summary>
        /// Determines whether a location in the bounds of the desktop
        /// </summary>
        /// <param name="location">The location to check.</param>
        /// <returns>True if the location in in the desktop, otherwise false.</returns>
        public static bool IsLocationInDesktop(Point location)
        {
            var desktop = GetDesktopSize();

            if (location.X > desktop.Width
                || location.Y > desktop.Height
                || location.X < 0
                || location.Y < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        #endregion
    }
}
