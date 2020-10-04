using System;
using ComponentFactory.Krypton.Toolkit;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace Halloumi.Common.Windows.Controls
{
    public class FileSelectButton : KryptonButton
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSelectButton"/> class.
        /// </summary>
        public FileSelectButton()
        {
            this.Title = "";
            this.Filter = "All files (*.*)|*.*";
            this.Mode = DialogMode.Open;
            this.AssociatedControl = null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the title displayed when the file dialog is displayed
        /// </summary>
        [Category("File Select")]
        [Description("Specifies the title displayed when the file dialog is displayed")]
        [DefaultValue("")]
        public string Title { get; set; }

        /// <summary>
        /// Gets/sets the file filter used when the file-select dialog is displayed
        /// </summary>
        [Category("File Select")]
        [DefaultValue("All files (*.*)|*.*")]
        [Description("Specifies the file filter used when the file-select dialog is displayed")]
        public string Filter  { get; set; }

        /// <summary>
        /// Gets/sets whether an Open or Save file dialog is displayed
        /// </summary>
        [Category("File Select")]
        [DefaultValue(DialogMode.Open)]
        [Description("Specifies whether an Open or Save file dialog is displayed")]
        public DialogMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the control to set the text of after a file has been selected
        /// </summary>
        [Category("File Select")]
        [Description("Specifies the control to set the text of after a file has been selected")]
        [DefaultValue(null)]
        public Control AssociatedControl { get; set; }

        /// <summary>
        /// Gets or sets the selected file.
        /// </summary>
        [Category("File Select")]
        [Description("The selected file")]
        [DefaultValue("")]
        public string SelectedFile
        {
            get
            {
                if (this.AssociatedControl != null)
                {
                    return this.AssociatedControl.Text;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                if (this.AssociatedControl != null)
                {
                    this.AssociatedControl.Text = value;
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Shows the file select dialog and puts the selected file into the combobox
        /// </summary>
        private void SelectFile()
        {
            FileDialog dialog;

            if (this.Mode == DialogMode.Open)
            {
                dialog = new OpenFileDialog();
                dialog.CheckFileExists = true;
            }
            else
            {
                dialog = new SaveFileDialog();
                dialog.CheckFileExists = false;
                dialog.CheckPathExists = true;
            }

            if (this.Title != "")
            {
                dialog.Title = this.Title;
            }

            dialog.Filter = this.Filter;

            if (File.Exists(this.SelectedFile))
            {
                dialog.FileName = this.SelectedFile;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.SelectedFile = dialog.FileName;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Click"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnClick(EventArgs e)
        {
            SelectFile();
            base.OnClick(e);
        }

        #endregion

        #region Enums

        /// <summary>
        /// 
        /// </summary>
        public enum DialogMode
        {
            /// <summary>
            /// An 'Open' file dialog will be displayed button is clicked
            /// </summary>
            Open,

            /// <summary>
            /// A 'Save' file dialog will be displayed button is clicked
            /// </summary>
            Save
        }

        #endregion
    }
}
