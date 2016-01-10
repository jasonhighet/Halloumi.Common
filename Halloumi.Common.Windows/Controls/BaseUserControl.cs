using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Common.Helpers;

namespace Halloumi.Common.Windows.Controls
{
    public class BaseUserControl : UserControl
    {
        public BaseUserControl()
            : base()
        { }

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
    }
}
