using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace Halloumi.Common.Windows.Contollers
{
    public partial class ThemeMenuController : Component
    {
        #region Private Variables

        /// <summary>
        /// A file menu
        /// </summary>
        private ToolStripMenuItem _themeMenu = null;

        /// <summary>
        /// 
        /// </summary>
        private KryptonManager _kryptonManager = null;

        #endregion

        #region Constructors

        public ThemeMenuController()
        {
            InitializeComponent();

            Initialize();
        }

        public ThemeMenuController(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            int itemCount = contextMenu.Items.Count;
            for (int i = 0; i < itemCount; i++)
            {
                contextMenu.Items[i].Click += new EventHandler(ColorTheme_Click);
            }

            KryptonManager.GlobalPaletteChanged += new EventHandler(KryptonManager_GlobalPaletteChanged);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the file menu.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(null)]
        [Description("Gets or sets the menu the document menu options should appear in.")]
        public ToolStripMenuItem ThemeMenu
        {
            get { return _themeMenu; }
            set
            {
                _themeMenu = value;
                if (_themeMenu != null)
                {
                    int itemCount = contextMenu.Items.Count;
                    for (int i = 0; i < itemCount; i++)
                    {
                        _themeMenu.DropDownItems.Insert(i, contextMenu.Items[0]);
                    }
                    _themeMenu.DropDownOpening += new EventHandler(ThemeMenu_DropDownOpening);
                }
            }
        }

        /// <summary>
        /// Gets or sets the krypton manager associated with the parent form/application - used to change theme
        /// </summary>
        [Category("Behavior")]
        [Description("The krypton manager associated with the parent form/application - used to change theme")]
        [DefaultValue(null)]
        public KryptonManager KryptonManager
        {
            get { return _kryptonManager; }
            set
            {
                _kryptonManager = value;

                if (_kryptonManager != null)
                {
                    // only show custom form chrome if OS is not vista
                    _kryptonManager.GlobalAllowFormChrome = (Environment.OSVersion.Version.Major < 6);
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the palette mode.
        /// </summary>
        /// <param name="paletteMode">The palette mode to set.</param>
        private void SetPalleteMode(PaletteModeManager paletteMode)
        {
            if (_kryptonManager == null) return;
            _kryptonManager.GlobalPaletteMode = paletteMode;
        }

        #endregion

        #region Events
        
        /// <summary>
        /// Occurs when the document title is changed
        /// </summary>
        [Category("Behavior")]
        public event EventHandler ThemedChanged;

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the DropDownOpening event of the ThemeMenu control - checks the menu item for the current theme
        /// </summary>
        private void ThemeMenu_DropDownOpening(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            if (_kryptonManager != null)
            {
                mnuO7Black.Checked = (_kryptonManager.GlobalPaletteMode == PaletteModeManager.Office2007Black);
                mnuO7Silver.Checked = (_kryptonManager.GlobalPaletteMode == PaletteModeManager.Office2007Silver);
                mnuO7Blue.Checked = (_kryptonManager.GlobalPaletteMode == PaletteModeManager.Office2007Blue);
                mnuO10Blue.Checked = (_kryptonManager.GlobalPaletteMode == PaletteModeManager.Office2010Blue);
                mnuO10Silver.Checked = (_kryptonManager.GlobalPaletteMode == PaletteModeManager.Office2010Silver);
                mnuO10Black.Checked = (_kryptonManager.GlobalPaletteMode == PaletteModeManager.Office2010Black);
                mnuSystem.Checked = (_kryptonManager.GlobalPaletteMode == PaletteModeManager.ProfessionalSystem);
                mnuProfessional.Checked = (_kryptonManager.GlobalPaletteMode == PaletteModeManager.ProfessionalOffice2003);
                mnuWMPBlue.Checked = (_kryptonManager.GlobalPaletteMode == PaletteModeManager.SparkleBlue);
                mnuWMPOrange.Checked = (_kryptonManager.GlobalPaletteMode == PaletteModeManager.SparkleOrange);
                mnuWMPPurple.Checked = (_kryptonManager.GlobalPaletteMode == PaletteModeManager.SparklePurple);
            }
        }

        /// <summary>
        /// Handles the Click event of the ColorTheme control.
        /// </summary>
        private void ColorTheme_Click(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            Application.DoEvents();
            if (sender == mnuO7Silver) SetPalleteMode(PaletteModeManager.Office2007Silver);
            if (sender == mnuO7Black) SetPalleteMode(PaletteModeManager.Office2007Black);
            if (sender == mnuO7Blue) SetPalleteMode(PaletteModeManager.Office2007Blue);
            if (sender == mnuO10Silver) SetPalleteMode(PaletteModeManager.Office2010Silver);
            if (sender == mnuO10Black) SetPalleteMode(PaletteModeManager.Office2010Black);
            if (sender == mnuO10Blue) SetPalleteMode(PaletteModeManager.Office2010Blue);
            if (sender == mnuProfessional) SetPalleteMode(PaletteModeManager.ProfessionalOffice2003);
            if (sender == mnuSystem) SetPalleteMode(PaletteModeManager.ProfessionalSystem);
            if (sender == mnuWMPBlue) SetPalleteMode(PaletteModeManager.SparkleBlue);
            if (sender == mnuWMPOrange) SetPalleteMode(PaletteModeManager.SparkleOrange);
            if (sender == mnuWMPPurple) SetPalleteMode(PaletteModeManager.SparklePurple);
        }

        /// <summary>
        /// Handles the GlobalPaletteChanged event of the KryptonManager control.
        /// </summary>
        private void KryptonManager_GlobalPaletteChanged(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            if (ThemedChanged != null)
            {
                ThemedChanged(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
