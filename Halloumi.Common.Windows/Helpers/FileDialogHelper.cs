using System;
using System.Windows.Forms;

namespace Halloumi.Common.Windows.Helpers
{
    public static class FileDialogHelper
    {
        public static string OpenFolder()
        {
            var folder = "";
            var dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = true;
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                folder = dialog.SelectedPath;
            }
            return folder;
        }

        /// <summary>
        /// Shows a file dialog and then returns the selected file
        /// </summary>
        /// <param name="fileFilter">The file filter.</param>
        /// <returns>
        /// The selected filename
        /// </returns>
        public static string OpenSingle(string fileFilter)
        {
            return OpenSingle(fileFilter, "");
        }

        /// <summary>
        /// Shows a file dialog and then returns the selected file
        /// </summary>
        /// <param name="fileFilter">The file filter.</param>
        /// <param name="intitialFolder">The intitial folder.</param>
        /// <returns>
        /// The selected filename
        /// </returns>
        public static string OpenSingle(string fileFilter, string initialFolder)
        {
            var filename = "";
            if (fileFilter.IndexOf("*.*") < 0) { fileFilter += "|All Files|*.*"; }

            var openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = fileFilter;
            openFileDialog.Multiselect = false;
            openFileDialog.RestoreDirectory = true;

            if (!string.IsNullOrEmpty(initialFolder))
                openFileDialog.InitialDirectory = initialFolder;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog.FileName;
            }

            return filename;
        }

        /// <summary>
        /// Shows a save-as file dialog and returns the file name selected
        /// </summary>
        /// <param name="fileFilter">The file filter for the save-as dialog</param>
        /// <param name="defaultFileName">The default filename</param>
        /// <returns>The selected filename</returns>
        public static string SaveAs(string fileFilter, string defaultFileName)
        {
            var filename = "";
            if (fileFilter.IndexOf("*.*") < 0) { fileFilter += "|All files|*.*"; }

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = fileFilter;
            saveFileDialog.FileName = defaultFileName;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                filename = saveFileDialog.FileName;
            }

            return filename;
        }
    }
}