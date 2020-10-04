using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using ComponentFactory.Krypton.Toolkit;
using Halloumi.Common.Windows.Helpers;

namespace Halloumi.Common.Windows.Controls
{
    public partial class Title : System.Windows.Forms.Label
    {
        #region Private Variables

        /// <summary>
        /// The title text for the control
        /// </summary>
        private string _text = "";

        /// <summary>
        /// A title text as a blurred bitmap
        /// </summary>
        private Bitmap _textBitmap = null;

        /// <summary>
        /// Amount to blur text (i.e. number of pixels to smear around)
        /// </summary>
        private const int _blurAmount = 2;

        /// <summary>
        /// The style
        /// </summary>
        private TitleStyle _style = TitleStyle.Custom;

        /// <summary>
        /// The fore color
        /// </summary>
        private Color _foreColor = SystemColors.ControlText;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Title class.
        /// </summary>
        public Title()
            : base()
        {
            this.Style = TitleStyle.Custom;
            this.GlassTableTop = false;
            KryptonManager.GlobalPaletteChanged += new EventHandler(KryptonManager_GlobalPaletteChanged);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [DefaultValue("")]
        public new string Text
        {
            get { return _text; }
            set
            {
                // do not set the text value of the base label
                // as this will be drawn manually
                _text = value;
                GenerateBitmap();
                Invalidate();
            }

        }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        public new Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                GenerateBitmap();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the foreground color of the control.
        /// </summary>
        public new Color ForeColor
        {
            get { return _foreColor; }
            set
            {
                _foreColor = value;
                GenerateBitmap();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        public new Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                GenerateBitmap();
                Invalidate();
            }
        }

        [DefaultValue(TitleStyle.Custom)]
        [Category("Appearance")]
        [Description("The display style of the title")]
        public TitleStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                GenerateBitmap();
                Invalidate();
            }
        }


        [DefaultValue(false)]
        [Category("Appearance")]
        [Description("If true, show 'glass table top' reflection underneath")]
        public bool GlassTableTop
        {
            get;
            set;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates the blurred text bitmap.
        /// </summary>
        private void GenerateBitmap()
        {
            // dispose of any existing bitmap
            if (_textBitmap != null)
            {
                _textBitmap.Dispose();
            }

            if (_text == "")
            {
                // if no text, create empty bitmap
                _textBitmap = new Bitmap(1, 1);
                return;
            }

            var foreColor = GetForeColor();

            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                var size = graphics.MeasureString(_text, this.Font, this.Width);
                var rectangle = new RectangleF(0, 0, ClientRectangle.Width, ClientRectangle.Height);
                using (var bitmap = new Bitmap((int)(size.Width * 1.2F), (int)(size.Height * 1.25F)))
                using (var bitmapGraphics = Graphics.FromImage(bitmap))
                using (var backBrush = new SolidBrush(Color.FromArgb(16, this.BackColor.R, this.BackColor.G, this.BackColor.B)))
                using (var foreBrush = new SolidBrush(foreColor))
                {
                    // write text to bitmap
                    bitmapGraphics.SmoothingMode = SmoothingMode.HighQuality;
                    bitmapGraphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    bitmapGraphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    bitmapGraphics.DrawString(_text, this.Font, backBrush, rectangle);

                    // create text bitmap 
                    //_textBitmap = new Bitmap(bitmap.Width + _blurAmount, bitmap.Height + _blurAmount);
                    _textBitmap = new Bitmap(bitmap.Width + _blurAmount, bitmap.Height - 8);

                    using (var bitmapGraphicsOut = Graphics.FromImage(_textBitmap))
                    {
                        bitmapGraphicsOut.SmoothingMode = SmoothingMode.HighQuality;
                        bitmapGraphicsOut.InterpolationMode = InterpolationMode.HighQualityBilinear;
                        bitmapGraphicsOut.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                        // smear image of background of text about to 
                        // make blurred background "halo"
                        for (var x = 0; x <= _blurAmount; x++)
                        {
                            for (var y = 0; y <= _blurAmount; y++)
                            {
                                bitmapGraphicsOut.DrawImageUnscaled(bitmap, x, y);
                            }
                        }

                        // draw actual text
                        rectangle.X = _blurAmount / 2;
                        rectangle.Y = _blurAmount / 2;
                        bitmapGraphicsOut.DrawString(_text, this.Font, foreBrush, rectangle);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the forecolor of the text.
        /// </summary>
        /// <returns>The fore color</returns>
        private Color GetForeColor()
        {
            if (this.Style == TitleStyle.Custom)
            {
                return _foreColor;
            }

            var palette = KryptonHelper.GetCurrentPalette();
            var headingColor = Color.Black;
            var subheadingColor = Color.Black;

            if (palette == PaletteMode.Office2007Black
                || palette == PaletteMode.Office2007Blue
                || palette == PaletteMode.Office2007Silver)
            {
                headingColor = KryptonManager.GetPaletteForMode(palette).GetContentShortTextColor1(PaletteContentStyle.ButtonStandalone, PaletteState.Normal);
                subheadingColor = KryptonManager.GetPaletteForMode(palette).GetContentShortTextColor1(PaletteContentStyle.ButtonStandalone, PaletteState.Normal);
            }
            else if (palette == PaletteMode.ProfessionalOffice2003
                || palette == PaletteMode.ProfessionalSystem)
            {
                headingColor = SystemColors.ControlText;
                subheadingColor = SystemColors.ControlText;
            }
            else if (palette == PaletteMode.SparkleBlue
                || palette == PaletteMode.SparkleOrange
                || palette == PaletteMode.SparklePurple)
            {
                headingColor = Color.White;
                subheadingColor = Color.White;
            }
            else if (palette == PaletteMode.Office2010Black
                || palette == PaletteMode.Office2010Blue
                || palette == PaletteMode.Office2010Silver)
            {
                headingColor = KryptonManager.GetPaletteForMode(palette).GetContentShortTextColor1(PaletteContentStyle.ButtonStandalone, PaletteState.Normal);
                subheadingColor = KryptonManager.GetPaletteForMode(palette).GetContentShortTextColor1(PaletteContentStyle.ButtonStandalone, PaletteState.Normal);
            }

            if (this.Style == TitleStyle.Subheading)
            {
                return subheadingColor;
            }
            else if (this.Style == TitleStyle.Heading)
            {
                return headingColor;
            }

            return _foreColor;
        }

        #endregion

        #region Overidden Methods

        /// <summary>
        /// Called when the label is repainting itself.
        /// </summary>
        /// <param name="e">A PaintEventArgs object that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // draw the blurred text bitmap
            if (_textBitmap != null)
            {
                var x = (this.Padding.Left) - (_blurAmount / 2);
                var y = (this.Padding.Top) - (_blurAmount / 2);
                e.Graphics.DrawImageUnscaled(_textBitmap, x, y);

                if (this.GlassTableTop)
                {
                    var reflection = ImageHelper.GlassTableTopReflection(_textBitmap, _textBitmap.Height / 2, this.BackColor) as Bitmap;
                    e.Graphics.DrawImageUnscaled(reflection, x, y + _textBitmap.Height);
                }
            }
        }

        /// <summary>
        /// Releases the resources used by the Label
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (_textBitmap != null)
            {
                _textBitmap.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the GlobalPaletteChanged event of the KryptonManager control.
        /// </summary>
        private void KryptonManager_GlobalPaletteChanged(object sender, EventArgs e)
        {
            GenerateBitmap();
            Invalidate();
        }

        #endregion
    }

    public enum TitleStyle
    {
        Custom,
        Heading,
        Subheading
    }
}
