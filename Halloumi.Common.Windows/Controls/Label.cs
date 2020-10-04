using System;
using System.ComponentModel;
using System.Drawing;
using ComponentFactory.Krypton.Toolkit;
using Halloumi.Common.Windows.Helpers;

namespace Halloumi.Common.Windows.Controls
{
    public class Label : System.Windows.Forms.Label
    {
        private LabelStyle _style = LabelStyle.Custom;
        private Color _foreColor = SystemColors.ControlText;

        public Label() : base()
        {
            this.Style = LabelStyle.Custom;
            KryptonManager.GlobalPaletteChanged += new EventHandler(KryptonManager_GlobalPaletteChanged);
        }

        #region Properties

        /// <summary>
        /// Gets or sets the background color of the control
        /// </summary>
        public new Color ForeColor
        {
            get
            {
                return _foreColor;
            }
            set
            {
                _foreColor = value;
                SetForeColor();
            }
        }
        
        [DefaultValue(LabelStyle.Custom)]
        [Category("Appearance")]
        [Description("The display style of the panel")]
        public LabelStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                SetForeColor();
            }
        }

        #endregion

        #region Public Methods


        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the background colour.
        /// </summary>
        private void SetForeColor()
        {
            if (this.Style == LabelStyle.Custom)
            {
                base.ForeColor = _foreColor;
                return;
            }

            var palette = KryptonHelper.GetCurrentPalette();
            var textColor = Color.Black;
            var captionColor = Color.Black;
            var headingColor = Color.Black;

            if (palette == PaletteMode.Office2007Black
                || palette == PaletteMode.Office2007Blue
                || palette == PaletteMode.Office2007Silver)
            {
                textColor = Color.FromArgb(64, 64, 64);
                captionColor = KryptonManager.GetPaletteForMode(palette).GetContentShortTextColor1(PaletteContentStyle.ButtonStandalone, PaletteState.Normal);
                //headingColor = KryptonManager.GetPaletteForMode(palette).GetContentShortTextColor1(PaletteContentStyle.ButtonStandalone, PaletteState.Normal);
                headingColor = Color.Black;
            }
            else if (palette == PaletteMode.ProfessionalOffice2003
                || palette == PaletteMode.ProfessionalSystem)
            {
                textColor = SystemColors.ControlText;
                captionColor = SystemColors.ControlText;
                headingColor = SystemColors.ControlText;
            }
            else if (palette == PaletteMode.SparkleBlue
                || palette == PaletteMode.SparkleOrange
                || palette == PaletteMode.SparklePurple)
            {
                textColor = Color.White;
                captionColor = Color.White;
                headingColor = Color.White;
            }
            else if (palette == PaletteMode.Office2010Black
                || palette == PaletteMode.Office2010Blue
                || palette == PaletteMode.Office2010Silver)
            {
                textColor = Color.FromArgb(64, 64, 64);
                if(palette == PaletteMode.Office2010Black) textColor = Color.White;
                
                captionColor = KryptonManager.GetPaletteForMode(palette).GetContentShortTextColor1(PaletteContentStyle.ButtonStandalone, PaletteState.Normal);
                headingColor = headingColor = Color.Black;
            }

            if (this.Style == LabelStyle.Text)
            {
                base.ForeColor = textColor;
            }
            else if (this.Style == LabelStyle.Caption)
            {
                base.ForeColor = captionColor;
            }
            else if (this.Style == LabelStyle.Heading)
            {
                base.ForeColor = headingColor;
            }
        }

        #endregion

        private void KryptonManager_GlobalPaletteChanged(object sender, EventArgs e)
        {
            SetForeColor();        
        }


    }

    public enum LabelStyle
    { 
        Custom,
        Text,
        Caption,
        Heading
    }
}
