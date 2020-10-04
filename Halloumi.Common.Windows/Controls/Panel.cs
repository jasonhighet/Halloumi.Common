using System;
using System.ComponentModel;
using System.Drawing;
using ComponentFactory.Krypton.Toolkit;
using Halloumi.Common.Windows.Helpers;
using System.Windows.Forms;

namespace Halloumi.Common.Windows.Controls
{
    public class Panel : System.Windows.Forms.Panel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Panel class.
        /// </summary>
        public Panel()
            : base()
        {
            this.Style = PanelStyle.Custom;
            KryptonManager.GlobalPaletteChanged += new EventHandler(KryptonManager_GlobalPaletteChanged);
            _borderColor = KryptonHelper.GetBorderColor();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            DrawBorder(e.Graphics);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawBorder(e.Graphics);
        }

        private void DrawBorder(Graphics g)
        {
            if (this.BorderStyle == BorderStyle.FixedSingle || this.BorderStyle == BorderStyle.Fixed3D)
            {
                if (this.Padding.Left == 0 || this.Padding.Right == 0 || this.Padding.Top == 0 || this.Padding.Bottom == 0) SetPadding();
                ControlPaint.DrawBorder(g, this.ClientRectangle, _borderColor, ButtonBorderStyle.Solid);
            }
        }

        private void SetPadding()
        {
            var left = this.Padding.Left;
            var right = this.Padding.Right;
            var top = this.Padding.Top;
            var bottom = this.Padding.Bottom;

            if (left == 0) left = 1;
            if (right == 0) right = 1;
            if (top == 0) top = 1;
            if (bottom == 0) bottom = 1;

            this.Padding = new Padding(left, top, right, bottom);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the background color of the control
        /// </summary>
        public new Color BackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
                SetBackgroundColour();
            }
        }
        private Color _backColor = SystemColors.Control;

        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        [DefaultValue(PanelStyle.Custom)]
        [Category("Apperance")]
        [Description("The display style of the panel")]
        public PanelStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                SetBackgroundColour();
            }
        }
        private PanelStyle _style = PanelStyle.Custom;

        /// <summary>
        /// Gets or sets the border style
        /// </summary>
        [DefaultValue(BorderStyle.None)]
        [Category("Apperance")]
        [Description("The display style of the panel")]
        public new BorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                _borderStyle = value;
                Invalidate();
            }
        }
        private BorderStyle _borderStyle = BorderStyle.None;

        private Color _borderColor = SystemColors.ControlDark;

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the background colour.
        /// </summary>
        private void SetBackgroundColour()
        {
            if (this.Style == PanelStyle.Custom)
            {
                base.BackColor = _backColor;
                return;
            }
            else
            {
                base.BackColor = KryptonHelper.GetPanelColor(this.Style);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the GlobalPaletteChanged event of the KryptonManager control.
        /// </summary>
        private void KryptonManager_GlobalPaletteChanged(object sender, EventArgs e)
        {
            _borderColor = KryptonHelper.GetBorderColor();
            SetBackgroundColour();
        }

        #endregion
    }

    /// <summary>
    /// Panel style
    /// </summary>
    public enum PanelStyle
    {
        Custom,
        Background,
        Content,
        ButtonStrip
    }
}
