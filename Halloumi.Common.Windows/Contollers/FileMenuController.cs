using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Common.Helpers;

namespace Halloumi.Common.Windows.Controllers
{
    /// <summary>
    /// Class for managing a file-based open/new/save/save-as style documents.
    /// </summary>
    public partial class FileMenuController : Component
    {
        #region Private Variables

        /// <summary>
        /// If true, all save functionality is enabled
        /// </summary>
        private bool _canSave = true;

        /// <summary>
        /// If true, reload functionality is enabled
        /// </summary>
        private bool _canReload = true;

        /// <summary>
        /// The filename of the current document
        /// </summary>
        private string _filename = string.Empty;

        /// <summary>
        /// The file filter for the open/save dialogs
        /// </summary>
        private string _fileFilter = "";

        /// <summary>
        /// Set to true when the document is new
        /// </summary>
        private bool _isNew = true;

        /// <summary>
        /// Set to true when the document is modified
        /// </summary>
        private bool _isModified = false;

        /// <summary>
        /// The title of the document. Is a truncated version of the filename, 
        /// plus a '*' if the document has been modified.
        /// </summary>
        private string _title = string.Empty;

        /// <summary>
        /// A list of recently used files
        /// </summary>
        private List<string> _recentFiles = new List<string>();

        /// <summary>
        /// The maximum number of files in the recent files list
        /// </summary>
        private const int _maxRecentFiles = 8;

        /// <summary>
        /// A file menu
        /// </summary>
        private ToolStripMenuItem _fileMenu = null;

        /// <summary>
        /// A collection of file types the document manager manages, based on the file filter
        /// </summary>
        private List<FileType> _fileTypes = null;

        /// <summary>
        /// Set to true if the current file is modified externally
        /// </summary>
        private bool _modifiedExternally = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FileMenuController class.
        /// </summary>
        public FileMenuController()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the FileMenuController class.
        /// </summary>
        /// <param name="container">The parent container.</param>
        public FileMenuController(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a list of recently used files
        /// </summary>
        /// <value>The recent files.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string RecentFiles
        {
            get { return String.Join(",", _recentFiles.ToArray()); }
            set
            {
                if (value != "")
                {
                    _recentFiles = new List<string>(value.Split(','));
                }

                // update list if not design mode
                if (!DesignMode)
                {
                    UpdateRecentFilesMenu();
                }
            }
        }

        /// <summary>
        /// Gets the filename for the document.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FileName
        {
            get { return _filename; }
        }

        /// <summary>
        /// Gets the title of the document.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// Gets a value indicating whether the document is new.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsNew
        {
            get { return _isNew; }
        }

        /// <summary>
        /// Gets a value indicating whether the document has been modified
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsModified
        {
            get { return _isModified; }
            set
            {
                if (_isModified != value)
                {
                    _isModified = value;
                    UpdateTitle();
                }
            }
        }

        /// <summary>
        /// Gets or sets a flag indicating if save functionality is enabled
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("Indicates whether save functionality is enabled.")]
        public bool CanSave
        {
            get { return _canSave; }
            set
            {
                _canSave = value;
                mnuSave.Visible = _canSave;
                mnuSaveAs.Visible = _canSave && (!_forceSaveAs);
                mnuSep2.Visible = _canSave || _canCreateNew;
            }
        }


        /// <summary>
        /// Gets or sets a flag indicating if new functionality is enabled
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("Indicates whether new functionality is enabled.")]
        public bool CanCreateNew
        {
            get { return _canCreateNew; }
            set
            {
                _canCreateNew = value;
                mnuNew.Visible = _canCreateNew;
                mnuSep2.Visible = _canSave || _canCreateNew;
            }
        }
        private bool _canCreateNew = true;

        /// <summary>
        /// Gets or sets a value indicating whether saving should always show 'save as dialog'
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicates whether saving should always show 'save as dialog'.")]
        public bool ForceSaveAs
        {
            get { return _forceSaveAs; }
            set
            {
                _forceSaveAs = value;
                mnuSaveAs.Visible = _canSave && (!_forceSaveAs);
            }
        }
        private bool _forceSaveAs = false;



        /// <summary>
        /// Gets or sets a flag indicating if reload functionality is enabled
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("Indicates whether reload functionality is enabled.")]
        public bool CanReload
        {
            get { return _canReload; }
            set
            {
                _canReload = value;
                mnuReload.Visible = _canReload;
                mnuSep3.Visible = _canReload;
            }
        }

        /// <summary>
        /// Gets or sets the file filter for the open/save dialogs.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("Gets or sets the file filter for the open/save dialogs.")]
        public string FileFilter
        {
            get { return _fileFilter; }
            set
            {
                _fileFilter = value;
                _fileTypes = GetFileTypes(_fileFilter);
                CreateNewDocumentMenuItems();
            }
        }

        /// <summary>
        /// Gets or sets the file menu.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(null)]
        [Description("Gets or sets the menu the document menu options should appear in.")]
        public ToolStripMenuItem FileMenu
        {
            get { return _fileMenu; }
            set
            {
                _fileMenu = value;
                if (_fileMenu != null)
                {
                    var itemCount = contextMenu.Items.Count;
                    for (var i = 0; i < itemCount; i++)
                    {
                        _fileMenu.DropDownItems.Insert(i, contextMenu.Items[0]);
                    }
                }

                // create sub-menu items
                CreateRecentFilesMenuItems();

                // update list if not design mode
                if (!DesignMode)
                {
                    UpdateRecentFilesMenu();
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Opens a file if there is a command line value, or creates a new document.
        /// </summary>
        public void OpenFromCommandLine()
        {
            // get file name from command line parameters
            var filename = ApplicationHelper.GetCommandLineParameters();

            // if no command line, create new document, otherwise open file
            if (filename == "")
            {
                New();
            }
            else
            {
                Open(filename);
            }
        }

        /// <summary>
        /// Raises the 'New' event for the default document type
        /// </summary>
        /// <returns>True if successful</returns>
        public bool New()
        {
            // get new file name
            var filename = "Untitled" + GetDefaultExtension();
            return New(filename);
        }

        /// <summary>
        /// Raises the 'New' event for the default document type
        /// </summary>
        /// <returns>True if successful</returns>
        public bool New(string filename)
        {
            // abort if changes made and the user doesn't want to continue
            if (!ConfirmDocumentClose())
            {
                return false;
            }

            // raise 'new' event
            var e = new FileMenuControllerEventArgs(filename);
            if (NewDocument != null)
            {
                NewDocument(this, e);
            }

            // abort if new canceled
            if (e.Cancel)
            {
                return false;
            }

            // update status
            _filename = filename;
            _isNew = true;
            _isModified = false;
            _modifiedExternally = false;

            UpdateTitle();
            fileSystemWatcher.EnableRaisingEvents = false;

            return true;
        }

        /// <summary>
        /// Raises the Save event for the current file.  If is a new document, prompts for a file name
        /// </summary>
        /// <returns>True if successful</returns>
        public bool Save()
        {
            if (_isNew)
            {
                return SaveAs();
            }
            else
            {
                return Save(_filename);
            }
        }

        /// <summary>
        /// Shows the save file dialog, and then raises the Save event for the specified file.
        /// </summary>
        /// <returns>True if successful</returns>
        public bool SaveAs()
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = GetFullFileFilter();
            dialog.Title = "Save As";
            dialog.FileName = _filename;
            dialog.OverwritePrompt = true;

            if (_filename != "")
            {
                dialog.DefaultExt = Path.GetExtension(_filename);
                dialog.InitialDirectory = Path.GetDirectoryName(_filename);
                dialog.FilterIndex = GetFilterIndex(Path.GetExtension(_filename));
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return Save(dialog.FileName);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Raises the Save event for the specified file
        /// </summary>
        /// <param name="filename">The file to save to.</param>
        /// <returns>True if successful</returns>
        public bool Save(string filename)
        {
            // disable watching for external file modification
            fileSystemWatcher.EnableRaisingEvents = false;

            // raise 'save' event
            var e = new FileMenuControllerEventArgs(filename);
            if (SaveDocument != null)
            {
                SaveDocument(this, e);
            }

            // abort if save canceled
            if (e.Cancel)
            {
                // if not new, resume watching file
                if (!_isNew)
                {
                    fileSystemWatcher.EnableRaisingEvents = true;
                }
                return false;
            }

            // update status
            _filename = filename;

            if (_isNew)
            {
                // if new, start watching newly saved file
                StartFileWatcher();
            }
            else
            {
                // otherwise resume watching file for external changes
                fileSystemWatcher.EnableRaisingEvents = true;
            }

            _isNew = false;
            _isModified = false;
            UpdateTitle();
            AddToRecentFiles();

            return true;
        }

        /// <summary>
        /// Shows the open file dialog, and then raises the Load event for the specified file.
        /// </summary>
        /// <returns>True if successful</returns>
        public bool Open()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = GetFullFileFilter();
            dialog.Title = "Open";
            dialog.DefaultExt = "";

            if (_filename != "")
            {
                dialog.InitialDirectory = Path.GetDirectoryName(_filename);
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return Open(dialog.FileName);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Raises the Load event for the specified file
        /// </summary>
        /// <param name="filename">The filename to open.</param>
        /// <returns>True if successful</returns>
        public bool Open(string filename)
        {
            return Open(filename, true);
        }

        /// <summary>
        /// Raises the Load event for the specified file
        /// </summary>
        /// <param name="filename">The filename to open.</param>
        /// <param name="saveExistingChanges">
        /// If set to true, prompts to save any existing changes 
        /// in current document before opening new one.
        /// </param>
        /// <returns>True if successful</returns>
        public bool Open(string filename, bool saveExistingChanges)
        {
            // abort if changes made and the user doesn't want to continue
            if (saveExistingChanges && !ConfirmDocumentClose())
            {
                return false;
            }

            // raise 'load' event
            var e = new FileMenuControllerEventArgs(filename);
            if (LoadDocument != null)
            {
                LoadDocument(this, e);
            }

            // abort if load canceled
            if (e.Cancel)
            {
                RemoveFromRecentFiles(filename);
                return false;
            }

            // update status
            _filename = filename;
            _isNew = false;
            _isModified = false;
            UpdateTitle();
            AddToRecentFiles();
            StartFileWatcher();

            return true;
        }

        /// <summary>
        /// Reloads the current document - prompts the user to save changes or cancel
        /// </summary>
        public void Reload()
        {
            // ignore if new or no changes made
            if (!_isModified || _isNew) return;

            // otherwise, confirm changes and reload
            var message = "'"
                + FileSystemHelper.TruncateLongFilename(_filename)
                + "' has changed."
                + Environment.NewLine
                + Environment.NewLine
                + "Do you want to save the changes?";

            var confirm = MessageBoxHelper.ConfirmOrCancel(message, "Save Changes?");

            var filename = _filename;
            if (confirm != DialogResult.Cancel)
            {
                if (confirm == DialogResult.Yes)
                {
                    if (SaveAs())
                    {
                        Open(filename, false);
                    }
                }
                else
                {
                    Open(filename, false);
                }
            }
        }

        /// <summary>
        /// Confirms if the current document can be closed if changes have been made. 
        /// If no changes have been made, returns true
        /// If changes have been made, the user has the option of saving the current document.
        /// If they click yes, the current document is saved, and the function returns true
        /// If they click no, the current document is not saved, and the function returns true
        /// If they click cancel, the current document is not saved, and the function returns false
        /// </summary>
        /// <returns>True if the current document can be closed, or false if not.</returns>
        public bool ConfirmDocumentClose()
        {
            var confirmClose = true;
            if (_isModified)
            {
                var message = "'"
                    + FileSystemHelper.TruncateLongFilename(_filename)
                    + "' has changed."
                    + Environment.NewLine
                    + Environment.NewLine
                    + "Do you want to save the changes?";

                var confirm = MessageBoxHelper.ConfirmOrCancel(message, "Save Changes?");

                if (confirm == DialogResult.Cancel)
                {
                    confirmClose = false;
                }
                else if (confirm == DialogResult.Yes)
                {
                    if (_isNew)
                    {
                        confirmClose = SaveAs();
                    }
                    else
                    {
                        confirmClose = Save();
                    }
                }
            }
            return confirmClose;
        }

        /// <summary>
        /// If the currently opened file has been externally modified, 
        /// prompts the user if they want to reload the file.
        /// </summary>
        public void CheckForExternalUpdate()
        {
            // if the file has been modified externally,
            // prompt the user if they want to reload
            if (_modifiedExternally && !_isNew)
            {
                _modifiedExternally = false;

                var message = "'"
                    + FileSystemHelper.TruncateLongFilename(_filename)
                    + "' has been modified externally."
                    + Environment.NewLine
                    + Environment.NewLine
                    + "Do you want to reload it?";

                if (MessageBoxHelper.Confirm(message))
                {
                    Reload();
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Given a file filter string (in the format "Text Files (*.txt)|*.txt|All Files (*.*)|*.*" etc,
        /// returns a FileType object for each file type that isn't '*.*. 
        /// </summary>
        /// <param name="fileFilter">The file filter.</param>
        /// <returns>A list of file types</returns>
        private List<FileType> GetFileTypes(string fileFilter)
        {
            var fileTypes = new List<FileType>();

            FileType fileType = null;
            var filterEntries = fileFilter.Split('|');
            for (var i = 0; i < filterEntries.Length; i++)
            {
                var entry = filterEntries[i];
                if (i % 2 == 0)
                {
                    fileType = new FileType();

                    // set the filter name to the whole entry (eg 'Text Files (*.txt)
                    fileType.FilterName = entry;

                    // set the name to the description of the file filter (eg 'Text Files')
                    if (entry.IndexOf("(") > 1)
                    {
                        fileType.Name = entry.Substring(0, entry.IndexOf("(") - 1).Trim();
                    }
                    else
                    {
                        fileTypes.Add(fileType);
                    }

                    // convert to the singular (eg 'Text Files' -> 'Text File')
                    if (fileType.Name.ToLower().EndsWith("files"))
                    {
                        fileType.Name = fileType.Name.Substring(0, fileType.Name.Length - 1);
                    }
                }
                else
                {
                    // set the filter to the wildcard filter, and the extension to the extension of the filter
                    fileType.Filter = entry;
                    fileType.Extension = Path.GetExtension(entry);

                    // add the file type if it is not the 'all files' one
                    if (fileType.Filter != "*.*")
                    {
                        fileTypes.Add(fileType);
                    }
                }
            }
            return fileTypes;
        }

        /// <summary>
        /// Gets the full file filter, including All Files and All File Types filters
        /// </summary>
        /// <returns>The full file filter, including All Files and All File Types filters</returns>
        private string GetFullFileFilter()
        {
            var fullFileFilter = string.Empty;

            // if more than one file type, add 'All files types' filter
            if (_fileTypes != null && _fileTypes.Count > 1)
            {
                fullFileFilter = "All File Types (";
                foreach (var fileType in _fileTypes)
                {
                    fullFileFilter += "*" + fileType.Extension + ",";
                }
                fullFileFilter = fullFileFilter.Substring(0, fullFileFilter.Length - 1);
                fullFileFilter += ")|";
                foreach (var fileType in _fileTypes)
                {
                    fullFileFilter += "*" + fileType.Extension + ";";
                }
                fullFileFilter = fullFileFilter.Substring(0, fullFileFilter.Length - 1);
                fullFileFilter += "|";
            }

            // add normal file filters
            fullFileFilter += _fileFilter;

            if (fullFileFilter == "")
            {
                // add All Files filter if no filters specified
                fullFileFilter = "All Files (*.*)|*.*";
            }
            else if (!fullFileFilter.EndsWith("All Files (*.*)|*.*"))
            {
                // otherwise add All Files filter if not already at end
                fullFileFilter += "|All Files (*.*)|*.*";
            }
            return fullFileFilter;
        }

        /// <summary>
        /// Gets the default extension, based on the file filter - it is the extension of the first filter
        /// eg If the filter is "Text Files (*.txt)|*.txt|All Files (*.*)|*.*" it will return ".txt"
        /// If the file filter is empty or "*.*", will return an empty string
        /// </summary>
        /// <returns></returns>
        private string GetDefaultExtension()
        {
            if (_fileTypes.Count > 0)
            {
                return _fileTypes[0].Extension;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Given an extension, returns the index of the filter associated with it in the FileFilter property
        /// </summary>
        /// <param name="extension">The extension to find the index of.</param>
        /// <returns>The index of the filter, or -1 if none found</returns>
        private int GetFilterIndex(string extension)
        {
            if (_fileTypes.Count > 1)
            {
                for (var i = 0; i < _fileTypes.Count; i++)
                {
                    if (_fileTypes[i].Extension == extension)
                    {
                        return i + 2;
                    }
                }
            }
            return 1;
        }

        /// <summary>
        /// Updates the title and raises the TitleChanged event if it has changed
        /// </summary>
        private void UpdateTitle()
        {
            var title = FileSystemHelper.TruncateLongFilename(Path.GetFileName(_filename));
            if (_isModified)
            {
                title += "*";
            }

            if (_title != title)
            {
                _title = title;
                if (TitleChanged != null)
                {
                    TitleChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Adds the current file to the recent files list.
        /// </summary>
        private void AddToRecentFiles()
        {
            // remove from list if file is already there
            RemoveFromRecentFiles(_filename);

            // insert file at top of list
            _recentFiles.Insert(0, _filename);

            // ensure there are no more than maxRecentFiles files in list
            while (_recentFiles.Count > _maxRecentFiles)
            {
                _recentFiles.RemoveAt(_recentFiles.Count - 1);
            }

            UpdateRecentFilesMenu();
        }

        /// <summary>
        /// Removes a file from the recent files list.
        /// </summary>
        private void RemoveFromRecentFiles(string filename)
        {
            // remove from list if file is already there
            for (var i = 0; i < _recentFiles.Count; i++)
            {
                if (_recentFiles[i] == filename)
                {
                    _recentFiles.RemoveAt(i);
                    break;
                }
            }
            UpdateRecentFilesMenu();
        }

        /// <summary>
        /// Updates the recent file menu
        /// </summary>
        private void UpdateRecentFilesMenu()
        {
            for (var i = 0; i < _maxRecentFiles; i++)
            {
                if (i < _recentFiles.Count)
                {
                    var text = "&"
                        + (i + 1).ToString()
                        + ". "
                        + FileSystemHelper.TruncateLongFilename(_recentFiles[i]);
                    mnuRecentItems.DropDownItems[i].Text = text;
                    mnuRecentItems.DropDownItems[i].Visible = true;
                }
                else
                {
                    mnuRecentItems.DropDownItems[i].Visible = false;
                }

                mnuRecentItems.Enabled = (_recentFiles.Count > 0);
            }
        }

        /// <summary>
        /// Creates recent file menu items
        /// </summary>
        private void CreateRecentFilesMenuItems()
        {
            mnuRecentItems.DropDownItems.Clear();
            for (var i = 1; i <= _maxRecentFiles; i++)
            {
                var menuItem = new ToolStripMenuItem();
                menuItem.Text = "&" + i.ToString() + ". File" + i.ToString() + ".ext";
                menuItem.Click += new System.EventHandler(RecentItemClick);
                mnuRecentItems.DropDownItems.Add(menuItem);
            }
        }

        /// <summary>
        /// Creates new file menu items
        /// </summary>
        private void CreateNewDocumentMenuItems()
        {
            mnuNew.DropDownItems.Clear();

            var first = true;
            foreach (var fileType in _fileTypes)
            {
                var menuItem = new ToolStripMenuItem();
                menuItem.Text = "&" + fileType.Name;
                if (first)
                {
                    menuItem.ShortcutKeys = Keys.Control | Keys.N;
                    first = false;
                }
                menuItem.Click += new System.EventHandler(NewDocumentTypeClick);
                mnuNew.DropDownItems.Add(menuItem);
            }
        }

        /// <summary>
        /// Starts the file-watcher object monitoring the current file
        /// </summary>
        private void StartFileWatcher()
        {
            _modifiedExternally = false;
            fileSystemWatcher.Filter = Path.GetFileName(_filename);
            fileSystemWatcher.Path = Path.GetDirectoryName(_filename);
            fileSystemWatcher.IncludeSubdirectories = false;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the mnuNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void mnuNew_Click(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            Application.DoEvents();

            // call new from here only if there aren't multiple document types to choose from
            if (mnuNew.DropDownItems.Count == 0)
            {
                New();
            }
        }

        /// <summary>
        /// Handles the Click event of the mnuOpen control.
        /// </summary>
        private void mnuOpen_Click(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            Application.DoEvents();
            Open();
        }

        /// <summary>
        /// Handles the Click event of the mnuSave control.
        /// </summary>
        private void mnuSave_Click(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            Application.DoEvents();
            if (_forceSaveAs) SaveAs();
            else Save();
        }

        /// <summary>
        /// Handles the Click event of the mnuSaveAs control.
        /// </summary>
        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            Application.DoEvents();
            SaveAs();
        }

        /// <summary>
        /// Handles the Click event of the mnuReload control.
        /// </summary>
        private void mnuReload_Click(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            Application.DoEvents();
            Reload();
        }

        /// <summary>
        /// Called when a recent menu item is clicked
        /// </summary>
        private void RecentItemClick(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            Application.DoEvents();

            var menuItem = (ToolStripMenuItem)sender;
            var index = int.Parse(menuItem.Text.Substring(1, 1)) - 1;
            var recentFile = _recentFiles[index];

            if (!File.Exists(recentFile))
            {
                var message = "'"
                    + FileSystemHelper.TruncateLongFilename(recentFile)
                    + "' cannot be opened or does not exist."
                    + Environment.NewLine
                    + Environment.NewLine
                    + "Do you want to remove it from the recent files list?";

                if (MessageBoxHelper.Confirm(message))
                {
                    RemoveFromRecentFiles(recentFile);
                }
            }
            else
            {
                Open(recentFile);
            }
        }

        /// <summary>
        /// Called when a new document type menu item is clicked
        /// </summary>
        private void NewDocumentTypeClick(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            Application.DoEvents();

            var menuItem = (ToolStripMenuItem)sender;
            foreach (var fileType in _fileTypes)
            {
                if (menuItem.Text.Replace("&", "") == fileType.Name)
                {
                    Application.DoEvents();
                    New("Untitled" + fileType.Extension);
                    break;
                }
            }
        }

        /// <summary>
        /// Called when the current file is modified externally
        /// </summary>
        private void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            _modifiedExternally = true;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the document needs to be saved
        /// </summary>
        [Category("Behavior")]
        public event FileMenuControllerEventHandler SaveDocument;

        /// <summary>
        /// Occurs when the document needs to be loaded
        /// </summary>
        [Category("Behavior")]
        public event FileMenuControllerEventHandler LoadDocument;

        /// <summary>
        /// Occurs when a new document should be created
        /// </summary>
        [Category("Behavior")]
        public event FileMenuControllerEventHandler NewDocument;

        /// <summary>
        /// Occurs when the document title is changed
        /// </summary>
        [Category("Behavior")]
        public event EventHandler TitleChanged;

        #endregion

        #region Private Classes

        private class FileType
        {
            public string Name = string.Empty;
            public string FilterName = string.Empty;
            public string Filter = string.Empty;
            public string Extension = string.Empty;
        }

        #endregion
    }

    /// <summary>
    /// Delegate definition for FileMenuControllerEvents
    /// </summary>
    public delegate void FileMenuControllerEventHandler(object sender, FileMenuControllerEventArgs e);

    /// <summary>
    /// Event arguments for FileMenuControllerEvents
    /// </summary>
    public class FileMenuControllerEventArgs : EventArgs
    {
        #region Private Variables

        /// <summary>
        /// If set to true, the event has been cancelled
        /// </summary>
        private bool _cancel = false;
        
        /// <summary>
        /// The filename associated with the event
        /// </summary>
        private string _filename = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileMenuControllerEventArgs"/> class.
        /// </summary>
        public FileMenuControllerEventArgs()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the FileMenuControllerEventArgs class.
        /// </summary>
        public FileMenuControllerEventArgs(string filename)
            : base()
        {
            _filename = filename;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this event has been cancelled.
        /// </summary>
        public bool Cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        public string FileName
        {
            get { return _filename; }
            set { _filename = value; }
        }

        #endregion
    }
}
