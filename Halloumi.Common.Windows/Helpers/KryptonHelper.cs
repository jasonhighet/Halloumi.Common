using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ComponentFactory.Krypton.Toolkit;
using System.Drawing;
using Halloumi.Common.Windows.Controls;

namespace Halloumi.Common.Windows.Helpers
{
    /// <summary>
    /// Helper functionality around the krypton toolkit
    /// </summary>
    public static class KryptonHelper
    {
        /// <summary>
        /// Gets the current krypton palette.
        /// </summary>
        /// <returns>The current krypton palette.</returns>
        public static PaletteMode GetCurrentPalette()
        {
            if (KryptonManager.CurrentGlobalPalette == KryptonManager.PaletteOffice2007Blue) return PaletteMode.Office2007Blue;
            if (KryptonManager.CurrentGlobalPalette == KryptonManager.PaletteOffice2007Black) return PaletteMode.Office2007Black;
            if (KryptonManager.CurrentGlobalPalette == KryptonManager.PaletteOffice2007Silver) return PaletteMode.Office2007Silver;
            if (KryptonManager.CurrentGlobalPalette == KryptonManager.PaletteOffice2010Blue) return PaletteMode.Office2010Blue;
            if (KryptonManager.CurrentGlobalPalette == KryptonManager.PaletteOffice2010Black) return PaletteMode.Office2010Black;
            if (KryptonManager.CurrentGlobalPalette == KryptonManager.PaletteOffice2010Silver) return PaletteMode.Office2010Silver;
            if (KryptonManager.CurrentGlobalPalette == KryptonManager.PaletteSparkleBlue) return PaletteMode.SparkleBlue;
            if (KryptonManager.CurrentGlobalPalette == KryptonManager.PaletteSparkleOrange) return PaletteMode.SparkleOrange;
            if (KryptonManager.CurrentGlobalPalette == KryptonManager.PaletteSparklePurple) return PaletteMode.SparklePurple;
            if (KryptonManager.CurrentGlobalPalette == KryptonManager.PaletteProfessionalSystem) return PaletteMode.ProfessionalSystem;
            if (KryptonManager.CurrentGlobalPalette == KryptonManager.PaletteProfessionalOffice2003) return PaletteMode.ProfessionalOffice2003;
            return PaletteMode.Office2007Blue;
        }

        public static Color GetErrorBackgroundColor()
        {
            //PaletteMode palette = KryptonHelper.GetCurrentPalette();
            //var buttonColor = KryptonManager.GetPaletteForMode(palette).GetBackColor2(PaletteBackStyle.ButtonStandalone, PaletteState.Pressed);
            //return ImageHelper.AverageColor(buttonColor, Color.White);
            return Color.LemonChiffon;
        }


        /// <summary>
        /// Gets the color of the border.
        /// </summary>
        /// <returns>The color of the border.</returns>
        public static Color GetBorderColor()
        {
            var palette = GetCurrentPalette();
            var color = KryptonManager.GetPaletteForMode(palette).GetBorderColor1(PaletteBorderStyle.HeaderPrimary, PaletteState.Normal);
            return color;
        }

        /// <summary>
        /// Gets the color of the panel.
        /// </summary>
        /// <param name="panelStyle">The panel style.</param>
        /// <returns>The color of the panel</returns>
        public static Color GetPanelColor(PanelStyle panelStyle)
        {
            var palette = GetCurrentPalette();

            Color contentBackColour = Color.WhiteSmoke;
            Color backgroundBackColour = Color.White;
            Color buttonStripBackColour = SystemColors.ControlDark;

            if (palette == PaletteMode.Office2007Black
                || palette == PaletteMode.Office2007Blue
                || palette == PaletteMode.Office2007Silver)
            {
                contentBackColour = Color.WhiteSmoke;
                backgroundBackColour = Color.White;
                if (palette == PaletteMode.Office2007Black) backgroundBackColour = KryptonManager.GetPaletteForMode(palette).GetBackColor1(PaletteBackStyle.PanelAlternate, PaletteState.Normal);
                buttonStripBackColour = KryptonManager.GetPaletteForMode(palette).GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);
            }
            else if (palette == PaletteMode.ProfessionalOffice2003
                || palette == PaletteMode.ProfessionalSystem)
            {
                contentBackColour = SystemColors.Control;
                backgroundBackColour = Color.White;
                buttonStripBackColour = SystemColors.ControlDark;
            }
            else if (palette == PaletteMode.SparkleBlue
                || palette == PaletteMode.SparkleOrange
                || palette == PaletteMode.SparklePurple)
            {
                contentBackColour = Color.Black;
                backgroundBackColour = Color.FromArgb(24, 32, 48);
                buttonStripBackColour = Color.FromArgb(24, 32, 48);
            }
            else if (palette == PaletteMode.Office2010Black
                || palette == PaletteMode.Office2010Blue
                || palette == PaletteMode.Office2010Silver)
            {
                contentBackColour = KryptonManager.GetPaletteForMode(palette).GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);
                backgroundBackColour = KryptonManager.GetPaletteForMode(palette).GetBackColor1(PaletteBackStyle.PanelAlternate, PaletteState.Normal);
                buttonStripBackColour = KryptonManager.GetPaletteForMode(palette).GetBackColor1(PaletteBackStyle.PanelAlternate, PaletteState.Normal);
            }

            if (panelStyle == PanelStyle.Content)
            {
                return contentBackColour;
            }
            else if (panelStyle == PanelStyle.Background)
            {
                return backgroundBackColour;
            }
            else if (panelStyle == PanelStyle.ButtonStrip)
            {
                return buttonStripBackColour;
            }

            return SystemColors.Control;
        }
    }
}
