using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
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