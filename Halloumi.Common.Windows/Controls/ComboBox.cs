using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Halloumi.Common.Windows.Helpers;

namespace Halloumi.Common.Windows.Controls
{
    public class ComboBox : ComponentFactory.Krypton.Toolkit.KryptonComboBox
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBox"/> class.
        /// </summary>
        public ComboBox()
            : base()
        {
            this.TextChanged += new EventHandler(ComboBox_TextChanged);
            this.Validating += new CancelEventHandler(ComboBox_Validating);

            // default property values
            this.MaximumValue = int.MaxValue;
            this.MinimumValue = int.MinValue;
            this.MinLength = 0;
            this.IsRequired = false;
            this.ErrorMessage = "";
            this.EntryType = TextEntryType.Text;
            this.ValidateOnChange = true;
            this.RegularExpression = "";
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the control is validating itself
        /// </summary>
        [Browsable(true)]
        [Category("Validation")]
        [Description("Raised when the control is validating itself.")]
        public event CancelEventHandler CustomValidate;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether text is required.
        [Description("If true, a value is required for the text box")]
        [Category("Validation")]
        [DefaultValue(false)]
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [Description("The error message displayed in the error provider")]
        [Category("Validation")]
        [DefaultValue("")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the error provider.
        /// </summary>
        [Description("An error provider to display an error")]
        [Category("Validation")]
        public ErrorProvider ErrorProvider { get; set; }

        /// <summary>
        /// Gets or sets a value indicating what kind of text the combobox should accept.
        /// </summary>
        [Category("Validation")]
        [Description("Indicates what kind of text the combobox should accept.")]
        [DefaultValue(TextEntryType.Text)]
        public TextEntryType EntryType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the minimum integer value the combobox can accept.
        /// </summary>
        [Category("Validation")]
        [Description("The minimum integer value the combobox can accept.")]
        [DefaultValue(int.MinValue)]
        public int MinimumValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the maximum integer value the combobox can accept.
        /// </summary>
        [Category("Validation")]
        [Description("The maximum integer value the combobox can accept.")]
        [DefaultValue(int.MaxValue)]
        public int MaximumValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the minimum length the value of the combobox should be.
        /// </summary>
        [Category("Validation")]
        [Description("The minimum length the value of the combobox should be.")]
        [DefaultValue(0)]
        public int MinLength { get; set; }

        /// <summary>
        /// Gets or sets a value if validation should occur automatically when the text is changed
        /// </summary>
        [Category("Validation")]
        [Description("If true, validation will occur when the text is changed, otherwise only when IsValid is called")]
        [DefaultValue(true)]
        public bool ValidateOnChange { get; set; }

        /// <summary>
        /// Gets or sets regular expression to validate the contents of the combobox against.
        /// </summary>
        [Category("Validation")]
        [Description("A regular expression to validate the contents of the combobox against")]
        [DefaultValue("")]
        public string RegularExpression { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the selected index (thread safe).
        /// </summary>
        /// <returns></returns>
        public int GetSelectedIndexThreadSafe()
        {
            int index = -1;
            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate() { index = this.SelectedIndex; });
            else
                index = this.SelectedIndex;

            return index;
        }

        /// <summary>
        /// Gets the text (thread safe).
        /// </summary>
        /// <returns>The text</returns>
        public string GetTextThreadSafe()
        {
            var text = "";
            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate() { text = this.Text; });
            else
                text = this.Text;

            return text;
        }

        public T ParseEnum<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");
            return (T)Enum.Parse(typeof(T), this.GetTextThreadSafe().Replace(" ", ""));
        }

        /// <summary>
        /// Determines whether this combobox is valid, based on the IsRequired field and the Text value
        /// </summary>
        /// <returns>Returns true if the combobox is valid</returns>
        public bool IsValid()
        {
            bool isValid = true;

            if (this.IsRequired && this.Text == "")
            {
                // if required and no value, is not valid
                isValid = false;
            }
            else if (this.MinLength > 0 && this.Text.Length < this.MinLength)
            {
                // text is not long enough
                isValid = false;
            }
            else if (this.EntryType == TextEntryType.Integer)
            {
                // if not valid int, or less than minvalue,
                // or more than maxvalue, is not valid
                int intValue = 0;
                bool validInt = int.TryParse(this.Text, out intValue);
                if (!validInt
                    || intValue < this.MinimumValue
                    || intValue > this.MaximumValue)
                {
                    isValid = false;
                    if (this.Text == "" && !this.IsRequired) isValid = true;
                }
            }

            // validate the contents of the combobox using the regular expression provided
            if (this.RegularExpression != string.Empty)
            {
                Regex regex = new Regex(this.RegularExpression);
                isValid = regex.IsMatch(this.Text);
            }

            // if control is valid so far, raise custom validate event
            if (CustomValidate != null && isValid)
            {
                CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
                CustomValidate(this, cancelEventArgs);
                isValid = !cancelEventArgs.Cancel;
            }

            if (isValid)
            {
                if (this.ErrorProvider != null)
                    this.ErrorProvider.SetError(this, "");
                this.StateCommon.ComboBox.Back.Color1 = this.StateActive.ComboBox.Back.Color1;
            }
            else
            {
                SetError(this.ErrorMessage);
                this.StateCommon.ComboBox.Back.Color1 = KryptonHelper.GetErrorBackgroundColor();
            }

            return isValid;
        }

        /// <summary>
        /// Sets the text box in an error state
        /// </summary>
        /// <param name="message">The error message.</param>
        public void SetError(string message)
        {
            if (this.ErrorProvider != null)
                this.ErrorProvider.SetError(this, message);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Raises the TextChanged event.
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
            if (this.EntryType == TextEntryType.Integer)
            {
                // if entry mode is integer, filter out non-numeric chars
                string filteredText = Regex.Replace(this.Text, "[^0-9]", "");
                if (filteredText != this.Text)
                {
                    this.Text = filteredText;
                }
            }
            base.OnTextChanged(e);
        }

        /// <summary>
        /// Overrides the key press and filters out non-numeric keys if neccesary
        /// </summary>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (this.EntryType == TextEntryType.Integer)
            {
                // if integer mode, ignore non-numeric key presses
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            base.OnKeyPress(e);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the TextChanged event of the ComboBox control.
        /// </summary>
        private void ComboBox_TextChanged(object sender, EventArgs e)
        {
            if (this.ValidateOnChange)
            {
                this.IsValid();
            }
        }

        /// <summary>
        /// Handles the Validating event of the ComboBox control.
        /// </summary>
        private void ComboBox_Validating(object sender, CancelEventArgs e)
        {
            if (this.EntryType == TextEntryType.Integer && this.Text == "")
            {
                this.Text = "0";
            }
        }

        #endregion

        #region Enums

        /// <summary>
        /// The type of text the text box accepts
        /// </summary>
        public enum TextEntryType
        {
            Text,
            Integer
        }

        #endregion
    }
}