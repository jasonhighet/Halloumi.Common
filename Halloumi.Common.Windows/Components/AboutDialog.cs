//-----------------------------------------------------------------------------------------------
//  Name:            AboutDialog.cs
//  Description:     Displays an about dialog form
//  Author:          Jason Highet
//  Date Created:    12 February 2008
//------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Halloumi.Common.Windows.Forms;
using System.Drawing;
using System.Windows.Forms;

namespace Halloumi.Common.Windows.Controls
{
    /// <summary>
    /// Displays an about dialog form
    /// </summary>
    public partial class AboutDialog : Component
    {
        #region Private Variables

        /// <summary>
        /// The picture shown in the about dialog form.
        /// </summary>
        private Image _image = null;

        /// <summary>
        /// The starting position of the about dialog form. 
        /// </summary>
        private FormStartPosition _startPosition = FormStartPosition.CenterParent;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AboutDialog class.
        /// </summary>
        public AboutDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the AboutDialog class, given the parent container.
        /// </summary>
        /// <param name="container">The parent container.</param>
        public AboutDialog(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the picture shown in the about dialog form.
        /// </summary>
        [Category("Appearance")]
        [Description("The picture shown in the about dialog form.")]
        [DefaultValue(null)]
        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }

        /// <summary>
        /// Gets or sets the picture shown in the about dialog form.
        /// </summary>
        [Category("Behavior")]
        [Description("The starting position the about dialog form.")]
        [DefaultValue(FormStartPosition.CenterParent)]
        public FormStartPosition StartPosition
        {
            get { return _startPosition; }
            set { _startPosition = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Shows the about dialog form.
        /// </summary>
        public void Show()
        {
            using (frmAbout aboutForm = new frmAbout())
            {
                aboutForm.Image = this.Image;
                aboutForm.StartPosition = this.StartPosition;
                aboutForm.ShowDialog();
            }
        }

        #endregion
    }
}
