using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Halloumi.Common.Windows.Forms;
using System.Windows.Forms;

namespace Halloumi.Common.Windows.Helpers
{
    public static class UserInputHelper
    {
        public static string GetUserInput(string title, string defaultValue, Form owner)
        {
            var form = new frmUserInput();
            form.Value = defaultValue;
            form.Title = title;
            form.ShowDialog(owner);
            return form.Value;
        }
    }
}
