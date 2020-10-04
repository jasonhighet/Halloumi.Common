using System;
using System.Windows.Forms;

namespace Halloumi.Common.Windows.Controls
{
    public partial class ProgressBar : UserControl
    {
        private int _min = 0;
        private int _max = 100;
        private int _value = 50;

        public ProgressBar()
        {
            InitializeComponent();
            ResizeLevel();
        }

        public int Min
        {
            get { return _min; }
            set
            {
                _min = value;
                ResizeLevel();
            }
        }

        public int Max
        {
            get { return _max; }
            set
            {
                _max = value;
                ResizeLevel();
            }
        }

        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                ResizeLevel();
            }
        }

        private void ResizeLevel()
        {
            var adjustedMax = _max - _min;
            var adjustedValue = _value - _min;
            var percent = adjustedValue / (decimal)adjustedMax;
            pnlLevel.Width = (int)(pnlBackground.Width * percent);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ResizeLevel();
        }
    }
}
