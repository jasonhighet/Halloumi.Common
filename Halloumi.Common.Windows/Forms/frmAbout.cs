using System;
using System.Drawing;
using System.Windows.Forms;
using Halloumi.Common.Helpers;

namespace Halloumi.Common.Windows.Forms
{
    /// <summary>
    /// About dialog form
    /// </summary>
    internal partial class frmAbout : BaseForm
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public frmAbout()
        {
            InitializeComponent();

            this.Text = "About " + ApplicationHelper.GetTitle();
            this.lblProductName.Text = ApplicationHelper.GetProductName();
            this.lblVersion.Text = GetVersionDetails();
            this.lblCopyright.Text = ApplicationHelper.GetCopyrightMessage();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the logo image.
        /// </summary>
        public Image Image
        {
            get { return imgLogo.Image; }
            set { imgLogo.Image = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the application version details.
        /// </summary>
        /// <returns>The application version details</returns>
        public static string GetVersionDetails()
        {
            var versionNumber = ApplicationHelper.GetVersionNumber();
            var compileDate = ApplicationHelper.GetCompilationDate().ToShortDateString();
            return "Version " + versionNumber + " (" + compileDate + ")";
        }

        /// <summary>
        /// Locates and runs system info appplication
        /// </summary>
        private void DisplaySystemInfo()
        {
            try
            {
                var path = string.Empty;
                object value = null;
                var key = Microsoft.Win32.Registry.LocalMachine;

                // attempt to find path to MSInfo in registry
                key = key.OpenSubKey("Software\\Microsoft\\Shared Tools\\MSInfo");
                if (key != null)
                {
                    value = key.GetValue("Path");
                }
                if (value == null)
                {
                    key = key.OpenSubKey("Software\\Microsoft\\Shared Tools Location");
                    if (key != null)
                    {
                        value = key.GetValue("MSInfo");
                        if (value != null)
                        {
                            path = System.IO.Path.Combine(value.ToString(), "MSInfo32.exe");
                        }
                    }
                }
                else
                {
                    path = value.ToString();
                }

                // if path found, verifiy exists
                if (path != string.Empty)
                {
                    var info = new System.IO.FileInfo(path);
                    if (info.Exists)
                    {
                        // if exists, run
                        System.Diagnostics.Process.Start(path);
                    }
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Displays fancy screen effect
        /// </summary>
        private void DisplayScreenEffects()
        {
            var screenWidth = Screen.GetBounds(new Point(0, 0)).Width;
            var screenHeight = Screen.GetBounds(new Point(0, 0)).Height;

            var screenShot = new Bitmap(screenWidth, screenHeight);
            var graphics = Graphics.FromImage(screenShot);
            graphics.CopyFromScreen(0, 0, 0, 0, new Size(screenWidth, screenHeight));

            var effectsForm = new EffectsForm();
            graphics = effectsForm.CreateGraphics();

            var x = 0;
            var y = 0;
            var width = screenWidth;
            var height = screenHeight;

            while (width >= 0)
            {
                graphics.DrawImage(screenShot, x, y, width, height);
                x++;
                y++;
                width -= 2;
                height -= 2;
            }

            while (width <= screenWidth)
            {
                graphics.DrawImage(screenShot, x, y, width, height);
                x--;
                y--;
                width += 2;
                height += 2;
            }
            effectsForm.Close();
        }

        #endregion

        #region Private Classes

        /// <summary>
        /// Used by DisplayScreenEffects method to draw fancy screen effecr
        /// </summary>
        private class EffectsForm : Form
        {
            public EffectsForm()
            {
                this.WindowState = FormWindowState.Maximized;
                this.BackColor = Color.Black;
                this.FormBorderStyle = FormBorderStyle.None;
                this.TopMost = true;
                this.Show();
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the buttonSystemInfo control.
        /// </summary>
        private void buttonSystemInfo_Click(object sender, EventArgs e)
        {
            DisplaySystemInfo();
        }


        /// <summary>
        /// Handles the Click event of the imgLogo control.
        /// </summary>
        private void imgLogo_Click(object sender, EventArgs e)
        {
            DisplayScreenEffects();
        }

        #endregion
    }
}
