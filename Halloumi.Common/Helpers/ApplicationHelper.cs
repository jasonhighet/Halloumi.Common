using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace Halloumi.Common.Helpers
{
    /// <summary>
    ///     Helper methods around the executing application details
    /// </summary>
    public static class ApplicationHelper
    {
        /// <summary>
        ///     The path to the area in the registry that contains applications to launch on Windows startup
        /// </summary>
        private const string StartUpPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        /// <summary>
        ///     Gets or sets a value indicating whether the application should automatically start with windows.
        /// </summary>
        public static bool StartWithWindows
        {
            get
            {
                // if there is a sub-key in the start-up registry area matching the application name,
                // then the application is configured to launch on Windows startup.
                var key = Registry.CurrentUser.OpenSubKey(StartUpPath, true);
                return key?.GetValue(GetProductName()) != null;
            }
            set
            {
                var key = Registry.CurrentUser.OpenSubKey(StartUpPath, true);
                if (value)
                {
                    // if StartWithWindows is set to true, put the application name and executable path
                    // in the list of applications to launch on Windows startup
                    key?.SetValue(GetProductName(), GetExecutablePath());
                }
                else
                {
                    // otherwise delete the application from the list of applications to launch on startup
                    key?.DeleteValue(GetProductName(), false);
                }
            }
        }

        /// <summary>
        ///     Gets the application title.
        /// </summary>
        /// <returns>The application title</returns>
        public static string GetTitle()
        {
            // get executing assembly
            var assembly = Assembly.GetEntryAssembly();

            // get all title attributes on this assembly
            var attributes = assembly.GetCustomAttributes(typeof (AssemblyTitleAttribute), false);

            // if there is at least one title attribute
            if (attributes.Length <= 0) return Path.GetFileNameWithoutExtension(assembly.CodeBase);

            // select the first one
            var titleAttribute = (AssemblyTitleAttribute) attributes[0];

            // if it is not an empty string, return it
            // if there was no title attribute, or if the title attribute was the empty string, return the .exe name

            return titleAttribute.Title != "" ? titleAttribute.Title : Path.GetFileNameWithoutExtension(assembly.CodeBase);
        }

        /// <summary>
        ///     Gets the name of the product.
        /// </summary>
        /// <returns>The name of the product</returns>
        public static string GetProductName()
        {
            // get executing assembly
            var assembly = Assembly.GetEntryAssembly();

            // get all product attributes on this assembly
            var attributes = assembly.GetCustomAttributes(typeof (AssemblyProductAttribute), false);

            // if there aren't any product attributes, return an empty string
            // if there is a product attribute, return its value
            return attributes.Length == 0 ? "" : ((AssemblyProductAttribute) attributes[0]).Product;
        }

        /// <summary>
        ///     Gets the application version number.
        /// </summary>
        /// <returns>The application version number</returns>
        public static string GetVersionNumber()
        {
            return Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        /// <summary>
        ///     Gets the application executable path.
        /// </summary>
        /// <returns>The application executable path</returns>
        public static string GetExecutablePath()
        {
            return Assembly.GetEntryAssembly().CodeBase.Replace("file:///", "").Replace("/", "\\");
        }

        /// <summary>
        ///     Gets the folder of the application executable.
        /// </summary>
        /// <returns>The folder of the application executable</returns>
        public static string GetExecutableFolder()
        {
            return Path.GetDirectoryName(GetExecutablePath());
        }

        public static string GetUserDataPath()
        {
            var applicationName = GetTitle();
            if (applicationName == "") applicationName = "Halloumi";

            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folder = Path.Combine(folder, applicationName);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            return folder;
        }

        /// <summary>
        ///     Gets the application compilation date.
        /// </summary>
        /// <returns>The application compilation date</returns>
        public static DateTime GetCompilationDate()
        {
            return File.GetLastWriteTime(GetExecutablePath());
        }

        /// <summary>
        ///     Gets the application copyright message.
        /// </summary>
        /// <returns>The application copyright message</returns>
        public static string GetCopyrightMessage()
        {
            // get executing assembly
            var assembly = Assembly.GetEntryAssembly();

            // get all copyright attributes on this assembly
            var attributes = assembly.GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);

            // if there aren't any copyright attributes, return an empty string
            // if there is a copyright attribute, return its value
            return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute) attributes[0]).Copyright;
        }

        /// <summary>
        ///     Gets the command line parameters.
        /// </summary>
        /// <returns>The command line parameters</returns>
        public static string GetCommandLineParameters()
        {
            var parameters = string.Join(" ", Environment.GetCommandLineArgs());
            parameters = parameters.Substring(Environment.GetCommandLineArgs()[0].Length);
            return parameters.Trim();
        }

        /// <summary>
        ///     Gets the icon of the calling assembly.
        /// </summary>
        /// <returns>The icon of the calling assembly.</returns>
        public static Icon GetIcon()
        {
            var assembly = Assembly.GetEntryAssembly();
            return Icon.ExtractAssociatedIcon(assembly.Location);
        }

        /// <summary>
        ///     Determines whether the calling assembly is in debug mode.
        /// </summary>
        /// <returns>True if the calling assembly is in debug mode</returns>
        public static bool IsDebugMode()
        {
            var assembly = Assembly.GetEntryAssembly();
            var attributes = assembly.GetCustomAttributes(typeof (DebuggableAttribute), false);

            // If the 'DebuggableAttribute' is not found then it is definitely an OPTIMIZED build
            if (attributes.Length <= 0) return false;

            // Just because the 'DebuggableAttribute' is found doesn't necessarily mean
            // it's a DEBUG build; we have to check the JIT Optimization flag
            // i.e. it could have the "generate PDB" checked but have JIT Optimization enabled
            var debuggableAttribute = attributes[0] as DebuggableAttribute;
            return debuggableAttribute != null && debuggableAttribute.IsJITOptimizerDisabled;
        }
    }
}