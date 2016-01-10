using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Halloumi.Common.Windows.Forms;

namespace Halloumi.Common.Windows.Helpers
{
    /// <summary>
    /// Helper class for displaying dialogs and text boxes
    /// </summary>
    public static class MessageBoxHelper
    {
        #region Public Methods

        /// <summary>
        /// Shows a messagebox displaying the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public static void Show(string text)
        {
            Show(text, ApplicationTitle());
        }

        /// <summary>
        /// Shows a messagebox displaying the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="title">The window title.</param>
        public static void Show(string text, string title)
        {
            Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows a warning messagebox displaying the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public static void Warn(string text)
        {
            Warn(text, "Warning");
        }

        /// <summary>
        /// Shows a warning messagebox displaying the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="title">The window title.</param>
        public static void Warn(string text, string title)
        {
            Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Displays a yes/no message box
        /// </summary>
        /// <param name="text">The main text to display</param>
        /// <returns>True if the user selects 'yes'</returns>
        public static bool Confirm(string text)
        {
            return Confirm(text, ApplicationTitle());
        }


        /// <summary>
        /// Displays a yes/no message box
        /// </summary>
        /// <param name="title">The window title of the message box dialog</param>
        /// <returns>True if the user selects 'yes'</returns>
        public static bool Confirm(string text, string title)
        {
            DialogResult result = Show(text, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return (result == DialogResult.Yes);
        }

        /// <summary>
        /// Displays a yes/no/cancel message box
        /// </summary>
        /// <param name="title">The window title of the message box dialog</param>
        /// <returns>True if the user selects 'yes'</returns>
        public static DialogResult ConfirmOrCancel(string text, string title)
        {
            return Show(text, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// Displays a yes/no/cancel message box
        /// </summary>
        /// <param name="text">The main text to display</param>
        /// <returns>True if the user selects 'yes'</returns>
        public static DialogResult ConfirmOrCancel(string text)
        {
            return ConfirmOrCancel(text, "Confirm");
        }


        /// <summary>
        /// Displays an error message in a message box.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public static void ShowError(string message)
        {
           Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Displays an error message in a message box.
        /// </summary>
        /// <param name="exception">The exception error to display.</param>
        public static void ShowError(Exception exception)
        {
            string message = exception.Message
                + Environment.NewLine
                + Environment.NewLine
                + exception.StackTrace;

            ShowError(message);
        }

        /// <summary>
        /// Shows a message box on the screen
        /// </summary>
        /// <param name="text">The text of the message.</param>
        /// <param name="title">The title of the form.</param>
        /// <param name="buttons">The buttons to display.</param>
        /// <param name="icon">The icon to display.</param>
        /// <returns></returns>
        public static DialogResult Show(string text, 
            string title, 
            MessageBoxButtons buttons, 
            MessageBoxIcon icon)
        {
            using (frmMessageBox messageBoxForm = new frmMessageBox(text, title, buttons, icon))
            {
                return messageBoxForm.ShowDialog();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the application title
        /// </summary>
        private static string ApplicationTitle()
        {
            // get executing assembly
            Assembly assembly = Assembly.GetEntryAssembly();

            // get all title attributes on this assembly
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

            // if there is at least one title attribute
            if (attributes.Length > 0)
            {
                // select the first one
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];

                // if it is not an empty string, return it
                if (titleAttribute.Title != "")
                {
                    return titleAttribute.Title;
                }
            }

            // if there was no title attribute, or if the title attribute was the empty string, return the .exe name
            return Path.GetFileNameWithoutExtension(assembly.CodeBase);
        }
        
        #endregion
    }
}
