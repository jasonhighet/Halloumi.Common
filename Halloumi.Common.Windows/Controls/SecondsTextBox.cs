using System;
using System.Windows.Forms;
using System.ComponentModel;
using ComponentFactory.Krypton.Toolkit;

namespace Halloumi.Common.Windows.Controls
{
    public class SecondsTextBox : KryptonTextBox
    {
        /// <summary>
        /// The value of the textbox, in seconds
        /// </summary>
        private double _seconds = 0;
        
        /// <summary>
        /// Gets or sets the value of the textbox in seconds
        /// </summary>
        [Category("Value")] 
        [DefaultValue(0)]
        [Description("Gets or sets the value of the textbox in seconds")]
        public double Seconds
        { 
            get 
            {
                return _seconds;
            }
            set 
            {
                _seconds = value;
                SetTextFromSeconds();
            }
        }

        /// <summary>
        /// Sets the text value based on the current seconds value
        /// </summary>
        private void SetTextFromSeconds()
        {
            var formattedValue = GetFormattedSeconds();
            if (this.Text != formattedValue)
            {
                this.Text = formattedValue;
            }
        }

        /// <summary>
        /// Sets the seconds value based on the current text
        /// </summary>
        private void SetSecondsFromText()
        {
            var value = GetSecondsFromText();
            if (value != this.Seconds)
            {
                this.Seconds = value;
            }
        }

        /// <summary>
        /// Gets the current seconds value as formatted text
        /// </summary>
        /// <returns></returns>
        private string GetFormattedSeconds()
        {
            try
            {
                var timeSpan = TimeSpan.FromSeconds(_seconds);
                return string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                                        timeSpan.Hours,
                                        timeSpan.Minutes,
                                        timeSpan.Seconds,
                                        timeSpan.Milliseconds);
            }
            catch { return "Error";  }
        }

        /// <summary>
        /// Converts the current text in a value in seconds
        /// </summary>
        /// <returns></returns>
        private double GetSecondsFromText()
        {
            try
            {
                var textArray = this.Text.Split(':');
                var hoursValue = "0";
                var minutesValue = "0";
                var secondsValue = "0";

                if (textArray.Length == 1)
                {
                    secondsValue = textArray[0];
                }
                else if (textArray.Length == 2)
                {
                    minutesValue = textArray[0];
                    secondsValue = textArray[1];
                }
                else if (textArray.Length == 3)
                {
                    hoursValue = textArray[0];
                    minutesValue = textArray[1];
                    secondsValue = textArray[2];
                }
                else 
                {
                    throw new Exception("Text not in hh:mm:ss.tttt format");
                }

                return double.Parse(secondsValue.Trim())
                    + (double.Parse(minutesValue.Trim()) * 60)
                    + (double.Parse(hoursValue.Trim()) * 60 * 60);
            }
            catch
            {
                return _seconds;
            }
        }

        /// <summary>
        /// Raises the Validating event.
        /// Ensures the Seconds value has the same value as the text, and also formats the text
        /// </summary>
        protected override void OnValidating(System.ComponentModel.CancelEventArgs e)
        {
            SetSecondsFromText();
            base.OnValidating(e);
        }

        /// <summary>
        /// Overrides the key press and filters out non-numeric keys
        /// </summary>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ':' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            base.OnKeyPress(e);
        }
    }
}
