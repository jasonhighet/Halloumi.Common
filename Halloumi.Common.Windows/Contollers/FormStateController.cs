using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Helpers;

namespace Halloumi.Common.Windows.Contollers
{
    public partial class FormStateController : Component
    {
        #region Private Variables

        /// <summary>
        /// The current form state 
        /// </summary>
        private FormState _formState = null;

        /// <summary>
        /// The form to manage the state of
        /// </summary>
        private Form _form = null;

        /// <summary>
        /// If set to true, the form is being resized
        /// </summary>
        private bool _isBeingResized = false;

        /// <summary>
        /// Set to true once the form has been loaded
        /// </summary>
        private bool _formShown = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FormStateController class.
        /// </summary>
        public FormStateController()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the FormStateController class.
        /// </summary>
        /// <param name="container">The parent container object.</param>
        public FormStateController(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the form to manage the size of
        /// </summary>
        [Category("Behavior")]
        [Description("The form to manage the size of")]
        public Form Form
        {
            get { return _form; }
            set
            {
                _form = value;
                if (_form != null && !DesignMode)
                {
                    // if form is not null, hook up event handlers
                    _form.Resize += new EventHandler(Form_Resize);
                    _form.LocationChanged += new EventHandler(Form_LocationChanged);
                    _form.Shown += new EventHandler(Form_Shown);
                    _form.Load += new EventHandler(Form_Load);
                }
            }
        }

        /// <summary>
        /// Gets or sets the current form state as an xml string
        /// </summary>
        [Browsable(false)]
        public string FormStateSettings
        {
            get
            {
                // ignore if in design more
                if (this.DesignMode || _formState == null) return "";

                // convert to xml and return
                return SerializationHelper<FormState>.ToXmlString(_formState);
            }
            set
            {
                // ignore in design more
                if (this.DesignMode || value == "") return;

                // load from xml and return
                _formState = SerializationHelper<FormState>.FromXmlString(value, true);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Resizes the form based on the current form state object
        /// </summary>
        public void RestoreForm()
        {
            // abort if no form
            if (_form == null) return;
            if (_formState == null) return;

            // save current visible state and hide
            _isBeingResized = true;
            var oldVisible = _form.Visible;
            if (_formShown)
            {
                _form.Visible = false;
            }

            try
            {
                // if outside of desktop bounds, set to 100, 100
                if (!DesktopHelper.IsLocationInDesktop(_formState.Location))
                {
                    _formState.Location = new Point(100, 100);
                }

                // if minimized, set to normal
                if (_formState.WindowState == FormWindowState.Minimized)
                {
                    _formState.WindowState = FormWindowState.Normal;
                }

                // set form state to normal for resizing
                if (_form.WindowState != FormWindowState.Normal)
                {
                    _form.WindowState = FormWindowState.Normal;
                }

                // set size and location
                _form.Size = _formState.Size;
                _form.Location = _formState.Location;
            }
            catch
            { }

            // restore visibility
            _isBeingResized = false;
            if (_formShown)
            {
                _form.Visible = oldVisible;
            }

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the form-state object based on the state of the current form.
        /// </summary>
        private void UpdateFormState()
        {
            // abort if no form
            if (_form == null) return;

            // create form state object if one doesn't exits
            if (_formState == null)
            {
                _formState = new FormState();
            }

            // save the form state (ignore size/location if window min/maxed)
            _formState.WindowState = _form.WindowState;
            if (_formState.WindowState == FormWindowState.Normal)
            {
                _formState.Size = _form.Size;
                _formState.Location = _form.Location;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Resize event of the Form control - saves the form state to the form-state object
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Form_Resize(object sender, EventArgs e)
        {
            // if the form hasn't been loaded yet, or the form is in the process of being resized, abort
            if (_isBeingResized || !_formShown) return;
            UpdateFormState();
        }

        /// <summary>
        /// Handles the LocationChanged event of the Form control - saves the form state to the form-state object
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Form_LocationChanged(object sender, EventArgs e)
        {
            // if the form hasn't been loaded yet, or the form is in the process of being resized, abort
            if (_isBeingResized || !_formShown) return;
            UpdateFormState();
        }

        /// <summary>
        /// Handles the Load event of the Form control
        /// </summary>
        private void Form_Shown(object sender, EventArgs e)
        {
            // set to form-state window state
            if (_formState != null && _form.WindowState != _formState.WindowState)
            {
                _form.WindowState = _formState.WindowState;
            }

            _formShown = true;
            UpdateFormState();
        }

        /// <summary>
        /// Handles the Load event of the Form control
        /// </summary>
        private void Form_Load(object sender, EventArgs e)
        {
            if (_formState != null)
            {
                RestoreForm();
            }
        }

        #endregion

        /// <summary>
        /// Represents the state of a form (size, position, window-state etc)
        /// </summary>
        public class FormState
        {
            #region Properties

            /// <summary>
            /// The size of a form
            /// </summary>
            public Size Size;

            /// <summary>
            /// The location of a form
            /// </summary>
            public Point Location;

            /// <summary>
            /// The state of a form (normal, minimized, maximized etc)
            /// </summary>
            public FormWindowState WindowState;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the FormState class.
            /// </summary>
            public FormState()
            {
                this.Size = new Size(0, 0);
                this.Location = new Point(0, 0);
                this.WindowState = FormWindowState.Normal;
            }

            #endregion
        }
    }
}