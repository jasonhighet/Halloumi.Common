using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Halloumi.Common.Windows.Helpers
{
    public static class WallpaperHelper
    {
        /// <summary>
        /// Sets the current desktop wallpaper to the specified image.
        /// </summary>
        /// <param name="imagePath">The path of the image to set as the wallpaper.</param>
        public static void SetWallpaper(string imagePath)
        {
            SetWallpaper(imagePath, WallpaperStyle.Stretched);
        }

        /// <summary>
        /// Sets the current desktop wallpaper to the specified image.
        /// </summary>
        /// <param name="imagePath">The path of the image to set as the wallpaper.</param>
        /// <param name="style">The style.</param>
        public static void SetWallpaper(string imagePath, WallpaperStyle style)
        {
            // calculate registry values
            var wallpaperStyle = "1";
            var tileWallpaper = "0";
            if (style == WallpaperStyle.Stretched) wallpaperStyle = "2";
            if (style == WallpaperStyle.Tiled) tileWallpaper = "1";

            // set registry values
            var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            key.SetValue(@"WallpaperStyle", wallpaperStyle);
            key.SetValue(@"TileWallpaper", tileWallpaper);
            
            // set desktop backround
            const int SPI_SETDESKWALLPAPER = 20;
            const int SPIF_UPDATEINIFILE = 0x01;
            const int SPIF_SENDWININICHANGE = 0x02;
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 
                0,
                imagePath, 
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		private static extern int SystemParametersInfo (int uAction, int uParam, string lpvParam, int fuWinIni);

        /// <summary>
        /// Wallpaper display style
        /// </summary>
		public enum WallpaperStyle
		{
			Tiled,
			Centered,
			Stretched
        }    
    
    }
}
