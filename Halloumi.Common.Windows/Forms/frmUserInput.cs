using System;
using Halloumi.Common.Helpers;

namespace Halloumi.Common.Windows.Forms
{
    public partial class frmUserInput : BaseForm
    {
        public frmUserInput()
        {
            InitializeComponent();
        }

        public string Title { get; set; }
        public string Value { get; set; }

        private void frmUserInput_Load(object sender, EventArgs e)
        {
            this.captionLabel.Text = this.Title;
            this.textBox.Text = this.Value;

            // set window title
            this.Text = ApplicationHelper.GetTitle();

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Value = textBox.Text;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}