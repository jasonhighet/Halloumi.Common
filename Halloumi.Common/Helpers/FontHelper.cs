using System.Drawing;

namespace Halloumi.Common.Helpers
{
    public static class FontHelper
    {
        /// <summary>
        /// Emboldens the font.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <returns>The same font, but bold</returns>
        public static Font EmboldenFont(Font font)
        {
            return new Font(font.FontFamily, font.Size, FontStyle.Bold);
        }

        /// <summary>
        /// Benormals a font.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <returns>The same font, but regular (ie not bold/italic etc.)</returns>
        public static Font BenormalFont(Font font)
        {
            return new Font(font.FontFamily, font.Size, FontStyle.Regular);
        }

        /// <summary>
        /// Emboldens the font.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <returns>The same font, but bold</returns>
        public static Font ItalicizeFont(Font font)
        {
            return new Font(font.FontFamily, font.Size, FontStyle.Italic);
        }
    }
}