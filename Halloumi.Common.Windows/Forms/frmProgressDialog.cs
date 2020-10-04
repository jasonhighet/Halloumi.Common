using System;
using System.Windows.Forms;
using Halloumi.Common.Windows.Components;

namespace Halloumi.Common.Windows.Forms
{
    /// <summary>
    /// A message-box style form that auto-resizes itself depending on its text
    /// </summary>
    internal partial class frmProgressDialog : BaseForm
    {
        #region Private Variables

        /// <summary>
        /// The progress dialog component that launched the form
        /// </summary>
        ProgressDialog _progressDialog = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the frmProgressDialog class.
        /// </summary>
        /// <param name="progressDialog">The progress dialog component that launched the form.</param>
        public frmProgressDialog(ProgressDialog progressDialog)
        {
            InitializeComponent();
            _progressDialog = progressDialog;
            btnCancel.DialogResult = DialogResult.None;
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///  Sets the state of the controls that should be displayed.
        /// </summary>
        private void SetControls()
        {
            // set window title if different from current
            if (this.Text != _progressDialog.Title)
            {
                this.Text = _progressDialog.Title;
            }

            // set message label text if different from current
            if (lblText.Text != _progressDialog.Text)
            {
                lblText.Text = _progressDialog.Text;
            }

            // set detail text if different current
            if (txtDetails.Text != _progressDialog.Details.Replace(Environment.NewLine, "\n"))
            {
                txtDetails.DisablePaint();
                txtDetails.Text = _progressDialog.Details;
                txtDetails.ScrollToEnd();
                txtDetails.EnablePaint();
            }

            // enable/disable ok button based on completion/cancelled state.
            var okVisible = (!_progressDialog.Cancelled && !_progressDialog.Processing);
            if (btnOK.Visible != okVisible)
            {
                btnOK.Visible = okVisible;
            }

            // enable/disable cancel button based on completion/cancelled state.
            var cancelVisible = !okVisible;
            if (btnCancel.Visible != cancelVisible)
            {
                btnCancel.Visible = cancelVisible;
            }
            
            // set progress bar value if different from current
            if(prgProgessBar.Value != _progressDialog.PercentComplete)
            {
                prgProgessBar.Value = _progressDialog.PercentComplete;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the FormClosed event of the frmProgressDialog control.
        /// </summary>
        private void frmProgressDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            _progressDialog = null;
        }

        /// <summary>
        /// Handles the Shown event of the frmProgressDialog control.
        /// </summary>
        private void frmProgressDialog_Shown(object sender, EventArgs e)
        {
            _progressDialog.BackgroundWorker.RunWorkerAsync();
            SetControls();
            timer.Enabled = true;
        }

        /// <summary>
        /// Handles the Tick event of the timer control.
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            SetControls();

            // close form if close-on-complete is true
            // and finished processing and not cancelled
            if (_progressDialog.CloseOnComplete && 
                !_progressDialog.Processing 
                && !_progressDialog.Cancelled)
            {
                this.Close();
            }

            // close form if close-on-cancel is true and has been cancelled
            if (_progressDialog.CloseOnCancel && _progressDialog.Cancelled)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Handles the Load event of the frmProgressDialog control.
        /// </summary>
        private void frmProgressDialog_Load(object sender, EventArgs e)
        {
            SetControls();
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // flag operation as cancelled
            _progressDialog.Cancelled = true;
            
            // if not close on cancel, and button says 'cancel',
            // set text to 'close' and don't close the form
            if (!_progressDialog.CloseOnCancel && btnCancel.Text == "&Cancel")
            {
                btnCancel.Text = "&Close";
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// Handles the FormClosing event of the frmProgressDialog control.
        /// </summary>
        private void frmProgressDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            // cancel processing if it is still going
            if (_progressDialog.Processing)
            {
                _progressDialog.Cancelled = true;
            }
            
            // set dialog result to cancelled if necessary
            if (_progressDialog.Cancelled)
            {
                this.DialogResult = DialogResult.Cancel;
            }

            // disable the timer
            timer.Enabled = false;
        }

        #endregion
    }
}