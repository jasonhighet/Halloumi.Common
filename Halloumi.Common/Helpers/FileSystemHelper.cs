using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Halloumi.Common.Helpers
{
    /// <summary>
    /// Helper functionality around the file system
    /// </summary>
    public static class FileSystemHelper
    {
        /// <summary>
        /// Deletes the specified files.
        /// </summary>
        /// <param name="folder">The folder to search</param>
        /// <param name="fileMasks">A semi-colon delimetered list of filemasks to match (eg '*.cs;t*.txt'). All files will be searched if empty</param>
        /// <param name="includeSubfolders">If true, all subfolders will also be searched</param>
        public static void DeleteFiles(string folder, string fileMasks, bool includeSubfolders)
        {
            var files = SearchFiles(folder, fileMasks, includeSubfolders);
            files.ForEach(f => File.Delete(f));
        }

        /// <summary>
        /// Deletes the specified files.
        /// </summary>
        /// <param name="folder">The folder to search</param>
        /// <param name="fileMasks">A semi-colon delimetered list of filemasks to match (eg '*.cs;t*.txt'). All files will be searched if empty</param>
        /// <param name="includeSubfolders">If true, all subfolders will also be searched</param>
        public static void DeleteFiles(string folder, string fileMasks)
        {
            DeleteFiles(folder, fileMasks, true);
        }

        /// <summary>
        /// Deletes the specified files.
        /// </summary>
        /// <param name="folder">The folder to search</param>
        /// <param name="includeSubfolders">If true, all subfolders will also be searched</param>
        public static void DeleteFiles(string folder)
        {
            DeleteFiles(folder, "*.*", true);
        }

        /// <summary>
        /// Returns a list of files matching a specific file mask from a folder (and its subfolders if required)
        /// </summary>
        /// <param name="folder">The folder to search</param>
        /// <param name="fileMasks">A semi-colon delimetered list of filemasks to match (eg '*.cs;t*.txt'). All files will be searched if empty</param>
        /// <param name="includeSubfolders">If true, all subfolders will also be searched</param>
        /// <returns>A list of matching file names, including the full path,</returns>
        public static List<string> SearchFiles(string folder, string fileMasks, bool includeSubfolders)
        {
            var matchingFiles = new List<string>();

            if (!Directory.Exists(folder))
                return matchingFiles;

            // set look in files to everything (*.*) if empty
            if (fileMasks.Trim() == string.Empty)
                fileMasks = "*.*";

            // convert to array of each file mask
            string[] fileMaskArray = fileMasks.Split(';');

            SearchFiles(matchingFiles, folder, fileMaskArray, includeSubfolders);

            return matchingFiles;
        }

        /// <summary>
        /// Searches a folder (and subfolders if required) and adds all matching files to the specified list.
        /// </summary>
        /// <param name="matchingFiles">Any matching files will be added to this list</param>
        /// <param name="folder">The folder to search</param>
        /// <param name="fileMasks">An array of file masks to search on</param>
        /// <param name="includeSubfolders">If true, all subfolders will also be searched</param>
        private static void SearchFiles(List<string> matchingFiles, string folder, string[] fileMasks, bool includeSubfolders)
        {
            if (!Directory.Exists(folder))
                return;

            // add all matching files in current folder to list
            foreach (string fileMask in fileMasks)
            {
                foreach (string fileName in Directory.GetFiles(folder, fileMask))
                {
                    matchingFiles.Add(fileName);
                }
            }

            // search all sub-folders if neccessary
            if (includeSubfolders)
            {
                foreach (string subFolder in Directory.GetDirectories(folder))
                {
                    SearchFiles(matchingFiles, subFolder, fileMasks, includeSubfolders);
                }
            }
        }

        /// <summary>
        /// Returns a list of folders matching a specific folder mask from a folder (and its subfolders if required)
        /// </summary>
        /// <param name="folder">The folder to search</param>
        /// <param name="folderMasks">A semi-colon delimetered list of foldermasks to match (eg 'j*;*t*'). All folders will be searched if empty</param>
        /// <param name="includeSubfolders">If true, all subfolders will also be searched</param>
        /// <returns>A list of matching folder names, including the full path</returns>
        public static List<string> SearchFolders(string folder, string folderMasks, bool includeSubfolders)
        {
            // set look in folders to everything (*.*) if empty
            if (folderMasks.Trim() == string.Empty)
            {
                folderMasks = "*.*";
            }

            // convert to array of each folder mask
            string[] folderMaskArray = folderMasks.Split(';');

            List<string> matchingFolders = new List<string>();

            SearchFolders(matchingFolders, folder, folderMaskArray, includeSubfolders);

            return matchingFolders;
        }

        /// <summary>
        /// Searches a folder (and subfolders if required) and adds all matching folders to the specified list.
        /// </summary>
        /// <param name="matchingFolders">Any matching folders will be added to this list</param>
        /// <param name="folder">The folder to search</param>
        /// <param name="folderMasks">An array of folder masks to search on</param>
        /// <param name="includeSubfolders">If true, all subfolders will also be searched</param>
        private static void SearchFolders(List<string> matchingFolders, string folder, string[] folderMasks, bool includeSubfolders)
        {
            // add all matching folders in current folder to list
            foreach (string folderMask in folderMasks)
            {
                foreach (string folderName in Directory.GetDirectories(folder, folderMask))
                {
                    matchingFolders.Add(folderName);
                }
            }

            // search all sub-folders if neccessary
            if (includeSubfolders)
            {
                foreach (string subFolder in Directory.GetDirectories(folder))
                {
                    SearchFolders(matchingFolders, subFolder, folderMasks, includeSubfolders);
                }
            }
        }

        /// <summary>
        /// Gets the checksum of a file
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>The checksum as a string</returns>
        public static string GetChecksum(string filename)
        {
            // open stream using 1 meg buffer
            using (var stream = new BufferedStream(File.OpenRead(filename), 1200000))
            {
                // calculate md5 hash
                var md5 = System.Security.Cryptography.MD5.Create();
                byte[] checksum = md5.ComputeHash(stream);
                return BitConverter.ToString(checksum).Replace("-", String.Empty);
            }
        }

        /// <summary>
        /// Truncate a path to fit within a certain number of characters by replacing path components with ellipses.
        /// </summary>
        /// <param name="filename">Long file name</param>
        /// <returns>Truncated file name</returns>
        public static string TruncateLongFilename(string filename)
        {
            return TruncateLongFilename(filename, 64);
        }

        public static bool AreFilesDifferent(string file1, string file2)
        {
            if (!File.Exists(file1) || !File.Exists(file2))
                throw new Exception("File(s) do not exist");

            var fileInfo1 = new FileInfo(file1);
            var fileInfo2 = new FileInfo(file2);

            return (fileInfo1.Length != fileInfo2.Length
                || fileInfo1.LastWriteTime != fileInfo2.LastWriteTime);
        }

        /// <summary>
        /// Truncate a path to fit within a certain number of characters by replacing path components with ellipses.
        /// </summary>
        /// <param name="filename">Long file name</param>
        /// <param name="maxLength">The maximum length of the truncated file name.</param>
        /// <returns>Truncated file name</returns>
        public static string TruncateLongFilename(string filename, int maxLength)
        {
            if (filename.Length > maxLength)
            {
                StringBuilder shortPath = new StringBuilder(maxLength + maxLength + 2);  // for safety
                if (PathCompactPathEx(shortPath, filename, maxLength, 0))
                {
                    return shortPath.ToString();
                }
            }
            return filename;
        }

        /// <summary>
        /// Truncates a path to fit within a certain number of characters by replacing path components with ellipses.
        /// </summary>
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern bool PathCompactPathEx(StringBuilder outPath, string inPath, int maxLength, int reserved);

        /// <summary>
        /// Strips invalid file name characters from a filename
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>A valid filename</returns>
        public static string StripInvalidFileNameChars(string filename)
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(invalidChar.ToString(), "");
            }
            return filename;
        }

        /// <summary>
        /// Deletes a folder, including all of the files in it and all subfolders
        /// </summary>
        /// <param name="folder">The folder to delete.</param>
        public static void DeleteFolder(string folder)
        {
            foreach (var file in FileSystemHelper.SearchFiles(folder, "*.*", true)) File.Delete(file);
            var folderInfo = new DirectoryInfo(folder);
            foreach (var subfolder in folderInfo.GetDirectories()) subfolder.Delete(true);
            Directory.Delete(folder);
        }

        /// <summary>
        /// Copies the specified source file to the specified destination file, overwriting if it exists.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void Copy(string source, string destination)
        {
            var bufferLength = 4 * 1024 * 32;
            var readBuffer = new Byte[bufferLength];
            var writeBuffer = new Byte[bufferLength];
            var readSize = -1;

            IAsyncResult writeResult;
            IAsyncResult readResult;

            using (var sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.None, 8, FileOptions.Asynchronous | FileOptions.SequentialScan))
            using (var destinationStream = new FileStream(destination, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8, FileOptions.Asynchronous | FileOptions.SequentialScan))
            {
                destinationStream.SetLength(sourceStream.Length);
                readSize = sourceStream.Read(readBuffer, 0, readBuffer.Length);
                readBuffer = Interlocked.Exchange(ref writeBuffer, readBuffer);

                while (readSize > 0)
                {
                    writeResult = destinationStream.BeginWrite(writeBuffer, 0, readSize, null, null);
                    readResult = sourceStream.BeginRead(readBuffer, 0, readBuffer.Length, null, null);
                    destinationStream.EndWrite(writeResult);
                    readSize = sourceStream.EndRead(readResult);
                    readBuffer = Interlocked.Exchange(ref writeBuffer, readBuffer);
                }

                sourceStream.Close();
                destinationStream.Close();
            }
        }
    }
}