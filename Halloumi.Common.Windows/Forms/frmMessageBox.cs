using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace Halloumi.Common.Windows.Forms
{
    /// <summary>
    /// A message-box style form that auto-resizes itself depending on its text
    /// </summary>
    internal partial class frmMessageBox : BaseForm
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the frmMessageBox class.
        /// </summary>
        public frmMessageBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the frmMessageBox class.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <param name="title">The title of the message box form.</param>
        /// <param name="buttons">The buttons to display.</param>
        /// <param name="icon">The icon to display.</param>
        public frmMessageBox(string text, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            InitializeComponent();

            this.Text = title;
            SetText(text);
            SetButtons(buttons);
            SetIcon(icon);

            // if showing error form with only 'OK' button, 
            // change text to 'Close' (as errors are never OK.)
            if (icon == MessageBoxIcon.Error && buttons == MessageBoxButtons.OK)
            {
                btnYes.Text = "&Close";
            }

            // start in center screen if no parent window
            if (this.Parent == null)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the state of the buttons that should be displayed.
        /// </summary>
        /// <param name="buttons">The buttons to be displayed.</param>
        private void SetButtons(MessageBoxButtons buttons)
        {
            if (buttons == MessageBoxButtons.OK)
            {
                btnYes.Text = "&OK";
                btnYes.DialogResult = DialogResult.OK;
                this.CancelButton = btnYes;
                btnNo.Visible = false;
                btnCancel.Visible = false;
            }
            else if (buttons == MessageBoxButtons.OKCancel)
            {
                btnYes.Text = "&OK";
                btnYes.DialogResult = DialogResult.OK;
                btnNo.Visible = false;
            }
            else if (buttons == MessageBoxButtons.YesNo)
            {
                btnCancel.Visible = false;
                this.CancelButton = btnNo;
            }
        }

        /// <summary>
        /// Sets the icon to be displayed.
        /// </summary>
        /// <param name="icon">The icon to be displayed.</param>
        private void SetIcon(MessageBoxIcon icon)
        {
            if (icon == MessageBoxIcon.Error)
            {
                imgIcon.Image = imageList.Images["Error"];
            }
            else if (icon == MessageBoxIcon.Exclamation || icon == MessageBoxIcon.Warning)
            {
                imgIcon.Image = imageList.Images["Exclamation"];
            }
            else if (icon == MessageBoxIcon.Question)
            {
                imgIcon.Image = imageList.Images["Question"];
            }
            else
            {
                imgIcon.Image = imageList.Images["Information"];
            }        
        }

        /// <summary>
        /// Sets the text to display, and resizes the form to fit it.
        /// </summary>
        /// <param name="text">The text to display.</param>
        private void SetText(string text)
        {
            // resize form to be 2/3rds screen width
            this.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.66);

            // Set maximum layout size.
            SizeF maxSize = new SizeF(lblText.Width, Screen.PrimaryScreen.WorkingArea.Height);

            using (Graphics graphics = this.CreateGraphics())
            {
                // measure the string.
                SizeF size = graphics.MeasureString(text, lblText.Font, maxSize);
                
                // resize form to fit string
                if (size.Height > lblText.Height)
                {
                    this.Size = new Size(this.Size.Width, this.Size.Height + ((int)size.Height - lblText.Height));
                }
                if (size.Width < lblText.Width)
                {
                    this.Size = new Size((this.Size.Width - (lblText.Width - (int)size.Width)) + 20, this.Height);
                }

                lblText.Text = text;
            }
        }
        
        #endregion
    }
}