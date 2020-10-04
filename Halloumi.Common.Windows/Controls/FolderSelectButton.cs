using System;
using ComponentFactory.Krypton.Toolkit;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace Halloumi.Common.Windows.Controls
{
    public class FolderSelectButton : KryptonButton
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderSelectButton"/> class.
        /// </summary>
        public FolderSelectButton()
        {
            this.Title = "";
            this.AssociatedControl = null;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the title displayed when the folder-select dialog is displayed
        /// </summary>
        [Category("Folder Select")]
        [Description("Specifies the title displayed when the folder-select dialog is displayed")]
        [DefaultValue("")]
        public string Title { get; set ; }

        /// <summary>
        /// Gets or sets the control to set the text of after a folder has been selected
        /// </summary>
        [Category("Folder Select")]
        [Description("Specifies the control to set the text of after a folder has been selected")]
        [DefaultValue(null)]
        public Control AssociatedControl { get; set; }

        /// <summary>
        /// Gets or sets the selected folder.
        /// </summary>
        [Category("Folder Select")]
        [Description("The selected folder")]
        [DefaultValue("")]
        public string SelectedFolder
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
        /// Shows the folder select dialog and puts the selected folder into the combobox
        /// </summary>
        private void SelectFolder()
        {
            var dialog = new FolderBrowserDialog();
            dialog.Description = this.Title;

            if (Directory.Exists(this.SelectedFolder))
            {
                dialog.SelectedPath = this.SelectedFolder;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.SelectedFolder = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Click"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnClick(EventArgs e)
        {
            SelectFolder();
            base.OnClick(e);
        }

        #endregion
    }
}
