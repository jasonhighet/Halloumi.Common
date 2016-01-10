using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace Halloumi.Common.Windows.Controls
{
    [DefaultEvent("Click")]
    public partial class Button : UserControl, IButtonControl
    {
        #region Private Variables

        /// <summary>
        /// the value returned to the parent form when the button is clicked.
        /// </summary>
        private DialogResult _dialogResult = DialogResult.None;

        /// <summary>
        /// 
        /// </summary>
        private ButtonStyle _style = ButtonStyle.Primary;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        public Button()
        {
            InitializeComponent();

            base.Text = "Button";
            
            KryptonManager.GlobalPaletteChanged += new EventHandler(KryptonManager_GlobalPaletteChanged);

            btnKrypton.MouseDown += new MouseEventHandler(btnKrypton_MouseDown);
            btnSystem.MouseDown += new MouseEventHandler(btnSystem_MouseDown);
            btnKrypton.MouseUp += new MouseEventHandler(btnKrypton_MouseUp);
            btnSystem.MouseUp += new MouseEventHandler(btnSystem_MouseUp);

            SetThemeColors();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the button text 
        /// </summary>
        [Category("Appearance")]
        [Description("The text displayed on the button")]
        [DefaultValue("Button")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                btnKrypton.Text = value;
                btnSystem.Text = value;
            }
        }

        /// <summary>
        /// Gets/sets the button image 
        /// </summary>
        [Category("Appearance")]
        [Description("The image displayed on the button")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set
            {
                base.BackgroundImage = value;
                btnKrypton.Values.Image = value;
                btnSystem.Image = value;
            }
        }

        /// <summary>
        /// Gets or sets the value returned to the parent form when the button is clicked.
        /// </summary>
        [Category("Behavior")]
        [Description("The value returned to the parent form when the button is clicked.")]
        [DefaultValue(DialogResult.None)]
        public DialogResult DialogResult
        {
            get { return _dialogResult; }
            set
            {
                _dialogResult = value;
                btnKrypton.DialogResult = _dialogResult;
                btnSystem.DialogResult = _dialogResult;
            }
        }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        public override System.Drawing.Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                btnSystem.Font = base.Font;
            }
        }

        [DefaultValue(ButtonStyle.Primary )]
        [Category("Appearance")]
        [Description("The display style of the title")]
        public ButtonStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                SetThemeColors();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates a Click event for the control.
        /// </summary>
        public void PerformClick()
        {
            if (Click != null)
            {
                Click(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Notifies a control that it is the default button so that 
        /// its appearance and behavior is adjusted accordingly.
        /// </summary>
        public void NotifyDefault(bool value)
        {
            if (btnKrypton.Visible)
            {
                btnKrypton.NotifyDefault(value);
            }
            else
            {
                btnSystem.NotifyDefault(value);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the theme colors.
        /// </summary>
        private void SetThemeColors()
        {
            // show the standard button if in system pallete, otherwise show krypton button
            if (KryptonManager.CurrentGlobalPalette == KryptonManager.PaletteProfessionalSystem
                || KryptonManager.CurrentGlobalPalette == KryptonManager.PaletteProfessionalOffice2003)
            {
                btnSystem.Visible = true;
                btnKrypton.Visible = false;
            }
            else
            {
                btnSystem.Visible = false;
                btnKrypton.Visible = true;
            }

            if (this.Style == ButtonStyle.Primary) btnKrypton.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            else btnKrypton.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Gallery;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the btnKrypton control.
        /// </summary>
        private void btnKrypton_Click(object sender, EventArgs e)
        {
            PerformClick();
        }

        /// <summary>
        /// Handles the Click event of the btnSystem control.
        /// </summary>
        private void btnSystem_Click(object sender, EventArgs e)
        {
            PerformClick();
        }


        /// <summary>
        /// Handles the GlobalPaletteChanged event of the KryptonManager control.
        /// </summary>
        private void KryptonManager_GlobalPaletteChanged(object sender, EventArgs e)
        {
            SetThemeColors();
        }

        /// <summary>
        /// Handles the MouseDown event of the btnSystem control.
        /// </summary>
        private void btnSystem_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseDown != null) MouseDown(this, e);
        }

        /// <summary>
        /// Handles the MouseDown event of the btnKrypton control.
        /// </summary>
        private void btnKrypton_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseDown != null) MouseDown(this, e);
        }

        /// <summary>
        /// Handles the MouseUp event of the btnSystem control.
        /// </summary>
        private void btnSystem_MouseUp(object sender, MouseEventArgs e)
        {
            if (MouseUp != null) MouseUp(this, e);
        }

        /// <summary>
        /// Handles the MouseUp event of the btnKrypton control.
        /// </summary>
        private void btnKrypton_MouseUp(object sender, MouseEventArgs e)
        {
            if (MouseUp != null) MouseUp(this, e);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the control is clicked.
        /// </summary>
        [Browsable(true), Category("Action")]
        public new event EventHandler Click;


        /// <summary>
        /// Occurs when the control is clicked.
        /// </summary>
        [Browsable(true), Category("Action")]
        public new event MouseEventHandler MouseDown;

        /// <summary>
        /// Occurs when the control is clicked.
        /// </summary>
        [Browsable(true), Category("Action")]
        public new event MouseEventHandler MouseUp;

        #endregion
    }

    public enum ButtonStyle
    {
        Primary,
        Secondary
    }
}

