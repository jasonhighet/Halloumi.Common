using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Halloumi.Common.Windows.Helpers;
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
            string versionNumber = ApplicationHelper.GetVersionNumber();
            string compileDate = ApplicationHelper.GetCompilationDate().ToShortDateString();
            return "Version " + versionNumber + " (" + compileDate + ")";
        }

        /// <summary>
        /// Locates and runs system info appplication
        /// </summary>
        private void DisplaySystemInfo()
        {
            try
            {
                string path = string.Empty;
                object value = null;
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;

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
                    System.IO.FileInfo info = new System.IO.FileInfo(path);
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
            int screenWidth = Screen.GetBounds(new Point(0, 0)).Width;
            int screenHeight = Screen.GetBounds(new Point(0, 0)).Height;

            Bitmap screenShot = new Bitmap(screenWidth, screenHeight);
            Graphics graphics = Graphics.FromImage((Image)screenShot);
            graphics.CopyFromScreen(0, 0, 0, 0, new Size(screenWidth, screenHeight));

            EffectsForm effectsForm = new EffectsForm();
            graphics = effectsForm.CreateGraphics();

            int x = 0;
            int y = 0;
            int width = screenWidth;
            int height = screenHeight;

            while (width >= 0)
            {
                graphics.DrawImage((Image)screenShot, x, y, width, height);
                x++;
                y++;
                width -= 2;
                height -= 2;
            }

            while (width <= screenWidth)
            {
                graphics.DrawImage((Image)screenShot, x, y, width, height);
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
