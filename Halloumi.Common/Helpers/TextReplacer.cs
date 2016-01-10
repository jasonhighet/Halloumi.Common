using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

namespace Halloumi.Common.Helpers
{
    /// <summary>
    /// Used to do search and replace of text in a specific file.  Also keeps 
    /// track of summary information of all search/replacments done so far.
    /// </summary>
    public class TextReplacer
    {
        #region Private Variables

        /// <summary>
        /// The total number of files that have been modified
        /// </summary>
        private int FileCount { get; set; } 

        /// <summary>
        /// The total number of items replaced
        /// </summary>
        private int ReplaceCount { get; set; }

        /// <summary>
        /// The number of errors that occured
        /// </summary>
        private int ErrorCount { get; set; } 

        /// <summary>
        /// The text to replace
        /// </summary>
        private string ReplaceText { get; set; }

        /// <summary>
        /// The regular expression used to find the required text
        /// </summary>
        private Regex MatchExpression { get; set; } 

        /// <summary>
        /// If true, any special codes in the replace text will be replaced with their values
        /// </summary>
        private bool UseReplaceCodes { get; set; } 

        #endregion

        #region Constructors

        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="findText">The text to find</param>
        /// <param name="replaceText">The text to replace</param>
        /// <param name="matchCase">If true, exact case should be matched</param>
        /// <param name="matchWholeWord">If true, the find text should match a whole word</param>
        /// <param name="useRegEx">If true, the find text contains regular expressions that should be matched</param>
        public TextReplacer(string findText,
            string replaceText,
            bool matchCase,
            bool matchWholeWord,
            bool useRegEx,
            bool useReplaceCodes)
        {

            this.FileCount = 0;
            this.ReplaceCount = 0;
            this.ErrorCount = 0;

            this.ReplaceText = replaceText;
            this.UseReplaceCodes = useReplaceCodes;
            this.MatchExpression = GetMatchExpression(findText, matchCase, matchWholeWord, useRegEx);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// eplaces all instances of the find text with the replace text in the specified folder
        /// </summary>
        /// <param name="folder">The folder to search</param>
        /// <param name="fileMasks">A semi-colon delimetered list of filemasks to match (eg '*.cs;t*.txt'). All files will be searched if empty</param>
        /// <param name="includeSubfolders">If true, all subfolders will also be searched</param>
        public void Replace(string folder, string fileMasks, bool includeSubFolders)
        {
            var files = FileSystemHelper.SearchFiles(folder, fileMasks, includeSubFolders);
            foreach (var file in files)
            {
                Replace(file);
            }
        }

        /// <summary>
        /// Replaces all instances of the find text with the replace text in a specific file
        /// </summary>
        /// <param name="fileName">The name of the file to do the find/replace on</param>
        public void Replace(string fileName)
        {
            // holds file content
            string content = string.Empty;

            // set to true if an error occurred
            bool errorOccured = false;

            // clear current status
            this.CurrentStatus = string.Empty;

            // if using replace codes, replace all codes 
            string replaceText = this.ReplaceText;
            if (this.UseReplaceCodes)
            {
                // get file name only
                string file = Path.GetFileName(fileName);

                // get folder name only
                string folder = string.Empty;
                string[] folders = Path.GetDirectoryName(fileName).Split(Path.DirectorySeparatorChar);
                if (folders.Length > 0)
                {
                    folder = folders[folders.Length - 1];
                }

                replaceText = replaceText.Replace("${Folder}", folder);
                replaceText = replaceText.Replace("${File}", file);
                replaceText = replaceText.Replace("${Date}", DateTime.Now.ToShortDateString());
                replaceText = replaceText.Replace("${Name}", Environment.UserName);
                replaceText = replaceText.Replace("${Upper(Folder)}", folder.ToUpper());
                replaceText = replaceText.Replace("${Lower(Folder)}", folder.ToLower());
                replaceText = replaceText.Replace("${Camel(Folder)}", StringHelper.CamelCase(folder));
                replaceText = replaceText.Replace("${Upper(File)}", file.ToUpper());
                replaceText = replaceText.Replace("${Lower(File)}", file.ToLower());
                replaceText = replaceText.Replace("${Camel(File)}", StringHelper.CamelCase(file));
            }

            // attempt to open and read the file
            if (!ReadTextFile(fileName, ref content))
            {
                // if cannot read fron file, set status to show a brief error message
                this.CurrentStatus = "Error: Could not open file '" + fileName + "'";
                errorOccured = true;
            }
            else
            {
                // find all matching instances of the find text
                MatchCollection matchList = this.MatchExpression.Matches(content);

                // at least one found, do the replace
                if (matchList.Count > 0)
                {
                    content = this.MatchExpression.Replace(content, this.ReplaceText);

                    // attempt to save the changes
                    if (!SaveTextFile(fileName, ref content))
                    {
                        // if cannot write to file, set status to show a brief error message
                        this.CurrentStatus = "Error: Could not save changes to file '" + fileName + "'";
                        errorOccured = true;
                    }
                    else
                    {
                        // otherwise update the counts and set the status 
                        this.FileCount++;
                        ReplaceCount += matchList.Count;
                        this.CurrentStatus = matchList.Count.ToString()
                            + " replacement(s) made in file '"
                            + fileName
                            + "'";
                    }
                }
            }

            // update the summary status
            this.SummaryStatus = ReplaceCount.ToString()
                + " replacement(s) made in "
                + this.FileCount.ToString()
                + " file(s).";

            // update the error count if neccessary
            if (errorOccured)
            {
                this.ErrorCount++;
            }

            // append error status if neccessary
            if (this.ErrorCount > 0)
            {
                this.SummaryStatus += "  " + this.ErrorCount.ToString() + " error(s) occured.";
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Saves a text file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="content">The content.</param>
        /// <returns>True if successful</returns>
        private bool SaveTextFile(string fileName, ref string content)
        {
            try
            {
                File.WriteAllText(fileName, content);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Reads a text file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="content">The content.</param>
        /// <returns>True if successful</returns>
        private bool ReadTextFile(string fileName, ref string content)
        {
            try
            {
                content = File.ReadAllText(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Generates the RegEx match object for the specified search criteria
        /// </summary>
        /// <param name="findText">The text to find</param>
        /// <param name="matchCase">If true, exact case should be matched</param>
        /// <param name="matchWholeWord">If true, the find text should match a whole word</param>
        /// <param name="useRegEx">If true, the find text contains regular expressions that should be matched</param>
        /// <returns>A RegEx match object for the specified search criteria</returns>
        private Regex GetMatchExpression(string findText, bool matchCase, bool matchWholeWord, bool useRegEx)
        {
            Regex matchExpression = null;

            // if not using regex, encode special regex characters
            if (!useRegEx)
            {
                findText = Regex.Escape(findText);
            }

            // if match whole word, suround with start-word and and end-word regex codes
            if (matchWholeWord)
            {
                findText = String.Format("{0}{1}{0}", @"\b", findText);
            }

            // create new regex match object based on ignore case options
            if (matchCase)
            {
                matchExpression = new Regex(findText);
            }
            else
            {
                matchExpression = new Regex(findText, RegexOptions.IgnoreCase);
            }

            return matchExpression;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns a description of the last find/replace done 
        /// </summary>
        public string CurrentStatus { get; set; }

        /// <summary>
        /// Returns a summary description of all find/replaces done so far
        /// </summary>
        public string SummaryStatus { get; set; }

        #endregion
    }
}
