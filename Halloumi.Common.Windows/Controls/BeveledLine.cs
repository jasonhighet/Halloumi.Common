using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using ComponentFactory.Krypton.Toolkit;
using Halloumi.Common.Windows.Helpers;   

namespace Halloumi.Common.Windows.Controls
{
    public class BeveledLine : UserControl
    {
        Color _lightColor = SystemColors.ControlLightLight;
        Color _darkColor = SystemColors.ControlDark;

        public BeveledLine()
            : base()
        {
            base.TabStop = false;
            KryptonManager.GlobalPaletteChanged += new EventHandler(KryptonManager_GlobalPaletteChanged);
            SetThemeColors();
        }

        private void SetThemeColors()
        {
            PaletteMode palette = KryptonHelper.GetCurrentPalette();

            if (palette == PaletteMode.SparkleBlue
                || palette == PaletteMode.SparkleOrange
                || palette == PaletteMode.SparklePurple)
            {

                _lightColor = SystemColors.ControlDark;
                _darkColor = Color.FromArgb(24, 32, 48);
            }
            else
            {
                _lightColor = KryptonManager.GetPaletteForMode(palette).GetBackColor2(PaletteBackStyle.TabCustom3, PaletteState.Normal);
                _darkColor = SystemColors.ControlDark;
            }
            this.Invalidate();
        }

        
        /// <summary>
        /// Paints the control
        /// </summary>
        /// <param name="e">The graphics object to draw on</param>
        [DebuggerStepThroughAttribute]
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(_darkColor, 1), new Point(0, 0), new Point(this.Width, 0));
            e.Graphics.DrawLine(new Pen(_lightColor, 1), new Point(0, 1), new Point(this.Width, 1));
        }

        /// <summary>
        /// Gets/sets the size of the line
        /// </summary>
        [Category("Layout")]
        [Description("The size of the control in pixels.")]
        public new Size Size
        {
            get { return base.Size; }
            set { base.Size = new Size(value.Width, 2); }
        }

        /// <summary>
        /// Called when the control is resized
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            if (base.Height != 2)
            {
                base.Size = new Size(this.Width, 2);
            }
            base.OnResize(e);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can give the focus to this control using the TAB key.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool TabStop
        { 
            get { return false; }
            set {  }
        }

        private void KryptonManager_GlobalPaletteChanged(object sender, EventArgs e)
        {
            SetThemeColors();
        }
    }
}
