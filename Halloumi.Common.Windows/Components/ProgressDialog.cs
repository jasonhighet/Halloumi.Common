using System;
using System.ComponentModel;
using Halloumi.Common.Windows.Forms;

namespace Halloumi.Common.Windows.Components
{
    /// <summary>
    /// Displays a progress dialog form
    /// </summary>
    public partial class ProgressDialog : Component
    {
        #region Private Variables

        /// <summary>
        /// The number of increments needed for the progress dialog to be completed.
        /// </summary>
        private int _maximum = 100;

        /// <summary>
        /// The current value of the progress bar.
        /// </summary>
        private int _value = 0;

        /// <summary>
        /// The text displayed on the progress dialog above the progress bar.
        /// </summary>
        private string _text = "Processing...";

        /// <summary>
        /// The text displayed on the progress dialog above the progress bar.
        /// </summary>
        private string _details = "";

        /// <summary>
        /// The progress dialog window title.
        /// </summary>
        private string _title = "Processing...";

        /// <summary>
        /// A flag indicating whether the progress dialog should close automatically when it finishes processing
        /// </summary>
        private bool _closeOnComplete = false;

        /// <summary>
        /// A flag indicating whether the progress dialog should close automatically when it is cancelled
        /// </summary>
        private bool _closeOnCancel = true;

        /// <summary>
        /// Set to true when the user cancels processing
        /// </summary>
        private bool _cancelled = false;

        /// <summary>
        /// Set to true when the progress dialog is processing
        /// </summary>
        private bool _processing = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ProgressDialog class.
        /// </summary>
        public ProgressDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the ProgressDialog class, given the parent container.
        /// </summary>
        /// <param name="container">The parent container.</param>
        public ProgressDialog(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the number of increments needed for the progress dialog to be completed.
        /// </summary>
        [Category("Behavior")]
        [Description("The number of increments needed for the progress dialog to be completed.")]
        [DefaultValue(100)]
        public int Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }
        
        /// <summary>
        /// Gets or sets the current value of the progress bar.
        /// </summary>
        [Category("Behavior")]
        [Description("The current value of the progress bar.")]
        [DefaultValue(0)]
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Gets or sets the text displayed on the progress dialog above the progress bar
        /// </summary>
        [Category("Appearance")]
        [Description("The text displayed on the progress dialog above the progress bar.")]
        [DefaultValue("Processing...")]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Gets or sets the detailed text displayed on the progress dialog below the progress bar.
        /// </summary>
        [Category("Appearance")]
        [Description("The detailed text displayed on the progress dialog below the progress bar.")]
        [DefaultValue("")]
        public string Details
        {
            get { return _details; }
            set { _details = value; }
        }

        /// <summary>
        /// Gets or sets the progress dialog window title.
        /// </summary>
        [Category("Appearance")]
        [Description("The progress dialog window title.")]
        [DefaultValue("Processing...")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// Gets or sets a flag indicating if the current processing has been cancelled
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Cancelled
        {
            get { return _cancelled; }
            set { _cancelled = value; }
        }

        /// <summary>
        /// Gets a flag indicating if component is currently processing
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Processing
        {
            get  { return (_processing); }
        }

        /// <summary>
        /// Gets a flag indicating the current completion as a percentage
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PercentComplete
        {
            get
            {
                // avoid divide-by-zero errors
                if (_maximum == 0) return 0;

                var current = Convert.ToDecimal(_value);
                var max = Convert.ToDecimal(_maximum);
                var percentage = Convert.ToInt32(current / max * 100);
                if (percentage > 100) percentage = 100;
                return percentage;
            }
        }

        /// <summary>
        /// Gets or sets a flag indicating if the progress dialog should close automatically when it finishes processing
        /// </summary>
        [Category("Behavior")]
        [Description("A flag indicating if the progress dialog should close automatically when it finishes processing.")]
        [DefaultValue(false)]
        public bool CloseOnComplete
        {
            get { return _closeOnComplete; }
            set { _closeOnComplete = value; }
        }

        /// <summary>
        /// Gets or sets a flag indicating if the progress dialog should close automatically if it is cancelled.
        /// </summary>
        [Category("Behavior")]
        [Description("A flag indicating if the progress dialog should close automatically if it is cancelled.")]
        [DefaultValue(true)]
        public bool CloseOnCancel
        {
            get { return _closeOnCancel; }
            set { _closeOnCancel = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Shows the progress dialog form.
        /// </summary>
        public void Start()
        {
            // reset values
            _details = "";
            _value = 0;

            // show form - this will start the background worker when it is shown
            using (var progressForm = new frmProgressDialog(this))
            {
                progressForm.ShowDialog();
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the DoWork event of the BackgroundWorker control.
        /// </summary>
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // set state flags
            _processing = true;
            _cancelled = false;
            
            // raise 'process step' event
            if (PerformProcessing != null)
            {
                PerformProcessing(this, EventArgs.Empty);
            }

            // flag processing as finished
            _processing = false;

            // raise 'processing completed' event
            if (ProcessingCompleted != null)
            {
                ProcessingCompleted(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the current step needs to be processed
        /// </summary>
        [Category("Behavior")]
        public event EventHandler PerformProcessing;

        /// <summary>
        /// Occurs when the processing is completed
        /// </summary>
        [Category("Behavior")]
        public event EventHandler ProcessingCompleted;

        #endregion
    }
}
