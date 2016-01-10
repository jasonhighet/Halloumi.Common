using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Win32;

namespace Halloumi.Common.Helpers
{
    /// <summary>
    /// Helper methods around the executing application details
    /// </summary>
    public static class ApplicationHelper
    {
        #region Private Variables

        /// <summary>
        /// The path to the area in the registry that contains applications to launch on Windows startup
        /// </summary>
        private const string _startUpPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the application title.
        /// </summary>
        /// <returns>The application title</returns>
        public static string GetTitle()
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

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        /// <returns>The name of the product</returns>
        public static string GetProductName()
        {
            // get executing assembly
            Assembly assembly = Assembly.GetEntryAssembly();

            // get all product attributes on this assembly
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);

            // if there aren't any product attributes, return an empty string
            if (attributes.Length == 0)
            {
                return "";
            }

            // if there is a product attribute, return its value
            return ((AssemblyProductAttribute)attributes[0]).Product;
        }

        /// <summary>
        /// Gets the application version number.
        /// </summary>
        /// <returns>The application version number</returns>
        public static string GetVersionNumber()
        {
            return Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Gets the application executable path.
        /// </summary>
        /// <returns>The application executable path</returns>
        public static string GetExecutablePath()
        {
            return Assembly.GetEntryAssembly().CodeBase.Replace("file:///", "").Replace("/", "\\");
        }

        /// <summary>
        /// Gets the folder of the application executable.
        /// </summary>
        /// <returns>The folder of the application executable</returns>
        public static string GetExecutableFolder()
        {
            return Path.GetDirectoryName(GetExecutablePath());
        }

        public static string GetUserDataPath()
        {
            var applicationName = ApplicationHelper.GetTitle();
            if (applicationName == "") applicationName = "Halloumi";

            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folder = Path.Combine(folder, applicationName);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            return folder;
        }

        /// <summary>
        /// Gets the application compilation date.
        /// </summary>
        /// <returns>The application compilation date</returns>
        public static DateTime GetCompilationDate()
        {
            return File.GetLastWriteTime(GetExecutablePath());
        }

        /// <summary>
        /// Gets the application copyright message.
        /// </summary>
        /// <returns>The application copyright message</returns>
        public static string GetCopyrightMessage()
        {
            // get executing assembly
            Assembly assembly = Assembly.GetEntryAssembly();

            // get all copyright attributes on this assembly
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

            // if there aren't any copyright attributes, return an empty string
            if (attributes.Length == 0)
            {
                return "";
            }

            // if there is a copyright attribute, return its value
            return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }

        /// <summary>
        /// Gets the command line parameters.
        /// </summary>
        /// <returns>The command line parameters</returns>
        public static string GetCommandLineParameters()
        {
            string parameters = string.Join(" ", Environment.GetCommandLineArgs());
            parameters = parameters.Substring(Environment.GetCommandLineArgs()[0].Length);
            return parameters.Trim();
        }

        /// <summary>
        /// Gets the icon of the calling assembly.
        /// </summary>
        /// <returns>The icon of the calling assembly.</returns>
        public static Icon GetIcon()
        {
            var assembly = Assembly.GetEntryAssembly();
            return Icon.ExtractAssociatedIcon(assembly.Location);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the application should automatically start with windows.
        /// </summary>
        public static bool StartWithWindows
        {
            get
            {
                // if there is a subkey in the start-up registry area matching the application name,
                // then the application is configured to launch on Windows startup.
                RegistryKey key = Registry.CurrentUser.OpenSubKey(_startUpPath, true);
                return (key.GetValue(GetProductName()) != null);
            }
            set
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(_startUpPath, true);
                if (value)
                {
                    // if StartWithWindows is set to true, put the application name and executable path
                    // in the list of applications to launch on Windows startup
                    key.SetValue(GetProductName(), GetExecutablePath().ToString());
                }
                else
                {
                    // otherwise delete the application from the list of applications to launch on startup
                    key.DeleteValue(GetProductName(), false);
                }
            }
        }

        #endregion
    }
}