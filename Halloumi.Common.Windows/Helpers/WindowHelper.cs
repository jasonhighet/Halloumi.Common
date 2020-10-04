using System.Drawing;
using System.Windows.Forms;

namespace Halloumi.Common.Windows.Helpers
{
    public static class WindowHelper
    {
        public static void ShowDialog(Form parentForm, Form dialogForm)
        {
            if (parentForm == null || parentForm.IsDisposed) return;
            if (dialogForm == null || dialogForm.IsDisposed) return;
            if (!parentForm.Visible) return;
            if (dialogForm.Visible) return;

            var x = parentForm.Location.X + (parentForm.Width - dialogForm.Width) / 2;
            var y = parentForm.Location.Y + (parentForm.Height - dialogForm.Height) / 2;

            dialogForm.StartPosition = FormStartPosition.Manual;
            dialogForm.Location = new Point(x, y);
            dialogForm.Show(parentForm);
        }

        public static DialogResult ShowModalDialog(Form parentForm, Form dialogForm)
        {
            if (parentForm == null || parentForm.IsDisposed) return DialogResult.Cancel;
            if (dialogForm == null || dialogForm.IsDisposed) return DialogResult.Cancel;
            if (!parentForm.Visible) return DialogResult.Cancel;
            if (dialogForm.Visible) return DialogResult.Cancel;

            var x = parentForm.Location.X + (parentForm.Width - dialogForm.Width) / 2;
            var y = parentForm.Location.Y + (parentForm.Height - dialogForm.Height) / 2;

            dialogForm.StartPosition = FormStartPosition.Manual;
            dialogForm.Location = new Point(x, y);
            return dialogForm.ShowDialog(parentForm);
        }
    }
}