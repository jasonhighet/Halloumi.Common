using System;
using System.Windows.Forms;

namespace Halloumi.Common.Windows.Forms
{
    public class BaseMinimizeToTrayForm : BaseForm
    {
        /// <summary>
        /// Initializes a new instance of the BaseMinimizeToTrayForm class.
        /// </summary>
        public BaseMinimizeToTrayForm()
            : base()
        {
            this.NotifyIcon = new NotifyIcon();
            this.NotifyIcon.MouseDoubleClick += new MouseEventHandler(NotifyIcon_MouseDoubleClick);

            this.LastWindowState = FormWindowState.Normal;

            this.Timer = new Timer();
            this.Timer.Interval = 50;
            this.Timer.Tick += new EventHandler(Timer_Tick);

            if (!DesignMode) this.Timer.Start();
        }

        /// <summary>
        /// Restores the form.
        /// </summary>
        private void RestoreForm()
        {
            if (_showing || _hiding) return;
            _showing = true;

            this.NotifyIcon.Visible = false;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Show();
            this.WindowState = this.LastWindowState;

            _showing = false;

            this.Timer.Start();


        }
        private bool _showing = false;


        /// <summary>
        /// Hides the form.
        /// </summary>
        private void HideForm()
        {
            if (_showing || _hiding) return;

            _hiding = true;

            this.Timer.Stop();
            this.NotifyIcon.Icon = this.Icon;
            this.NotifyIcon.Visible = true;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.Hide();

            _hiding = false;

        }
        private bool _hiding = false;

        /// <summary>
        /// Handles the MouseDoubleClick event of the NotifyIcon control.
        /// </summary>
        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RestoreForm();
        }

        /// <summary>
        /// Handles the Tick event of the Timer control.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) HideForm();
            else if (this.LastWindowState != this.WindowState) this.LastWindowState = this.WindowState;
        }

        /// <summary>
        /// Gets the notify icon component
        /// </summary>
        public NotifyIcon NotifyIcon { get; internal set; }

        /// <summary>
        /// Gets or sets the last state of the window before minimizing
        /// </summary>
        private FormWindowState LastWindowState { get; set; }

        /// <summary>
        /// Gets or sets the timer.
        /// </summary>
        private Timer Timer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether minimize to tray is enabled.
        /// </summary>
        public bool MinimizeToTrayEnabled
        {
            get { return _minimizeToTrayEnabled;  }
            set 
            {
                _minimizeToTrayEnabled = value;
                if(!DesignMode) Timer.Enabled = value;
            }
        }
        private bool _minimizeToTrayEnabled = true;
    }
}
