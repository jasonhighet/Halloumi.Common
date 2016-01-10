using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Halloumi.Common.Windows.Helpers;
using System.Reflection;
using Halloumi.Common.Helpers;

namespace Halloumi.Common.Windows.Forms
{
    public partial class BaseForm : KryptonForm
    {
        #region Private Variables

        /// <summary>
        /// If set to true, the form will use the default application icon for its icon
        /// </summary>
        private bool _useApplicationIcon = false;

        #endregion

        #region Properties

        /// <summary>
        /// If set to true, the form will use the default application icon for its icon
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        [Description("If set to true, the form will use the default application icon for its own icon")]
        public bool UseApplicationIcon
        {
            get { return _useApplicationIcon; }
            set
            {
                _useApplicationIcon = value;
                SetIcon();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Logs an exception, displays a message to the user, 
        /// and reverts the cursor if neccessary.
        /// </dsummary>
        public void HandleException(string userErrorMessage, Exception exception)
        {
            this.Cursor = Cursors.Default;

#if DEBUG
#else
            // log error to event log
            EventLogHelper.LogError(userErrorMessage, exception);
#endif

            userErrorMessage += Environment.NewLine
                + Environment.NewLine
                + exception.ToString();

#if DEBUG

            // show extended error message in debug mode
            userErrorMessage += Environment.NewLine
                + Environment.NewLine
                + exception.StackTrace;

#endif

            try
            {
                MessageBoxHelper.ShowError(userErrorMessage);
            }
            catch
            { }
        }

        /// <summary>
        /// Logs an exception, displays a message to the user, 
        /// and reverts the cursor if neccessary.
        /// </summary>
        public void HandleException(Exception exception)
        {
            this.Cursor = Cursors.Default;
            MessageBoxHelper.ShowError(exception);

#if DEBUG
#else
            // log error to event log
            EventLogHelper.LogError(exception);
#endif
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the form icon to the application icon if neccesary
        /// </summary>
        private void SetIcon()
        {
            if (_useApplicationIcon && !this.DesignMode)
            {
                var icon = ApplicationHelper.GetIcon();
                if (icon != null) base.Icon = icon;
            }
        }

        /// <summary>
        /// Raises the Load event.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            // if not design mode, set the form icon if necessary
            if (!this.DesignMode)
            {
                SetIcon();
            }

            // if not design mpde, set the minimum 
            // size for the form to the default size
            if (!this.DesignMode && this.WindowState == FormWindowState.Normal)
            {
                this.MinimumSize = this.Size;
            }

            base.OnLoad(e);
        }

        #endregion
    }
}
