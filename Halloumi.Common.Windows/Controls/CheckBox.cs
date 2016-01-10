using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Halloumi.Common.Windows.Controls
{
    public class CheckBox : ComponentFactory.Krypton.Toolkit.KryptonCheckBox
    {
        /// <summary>
        /// Initializes a new instance of the CheckBox class.
        /// </summary>
        public CheckBox()
        {
            this.CheckedValue = "True";
            this.UncheckedValue = "False";
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Category("Value")]
        [DefaultValue("False")]
        [Description("The current value of the checkbox")]
        public string Value
        {
            get
            {
                if (this.Checked) return this.CheckedValue;
                return this.UncheckedValue;
            }
            set
            {
                if (value.ToLower() == this.CheckedValue.ToLower())
                {
                    if (!this.Checked) this.Checked = true;
                }
                else
                {
                    if (this.Checked) this.Checked = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the value of the checkbox when checked
        /// </summary>
        [Category("Value")]
        [DefaultValue("True")]
        [Description("Gets or sets the value of the checkbox when checked")]
        public string CheckedValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value of the checkbox when unchecked
        /// </summary>
        [Category("Value")]
        [DefaultValue("False")]
        [Description("Gets or sets the value of the checkbox when unchecked")]
        public string UncheckedValue
        {
            get;
            set;
        }
    }
}
