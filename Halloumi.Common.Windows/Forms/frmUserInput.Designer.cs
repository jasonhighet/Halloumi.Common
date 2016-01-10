namespace Halloumi.Common.Windows.Forms
{
    partial class frmUserInput
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlMain = new Halloumi.Common.Windows.Controls.Panel();
            this.textBox = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.captionLabel = new Halloumi.Common.Windows.Controls.Label();
            this.beveledLine = new Halloumi.Common.Windows.Controls.BeveledLine();
            this.flpButtonsRight = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new Halloumi.Common.Windows.Controls.Button();
            this.btnOK = new Halloumi.Common.Windows.Controls.Button();
            this.pnlButtons = new Halloumi.Common.Windows.Controls.Panel();
            this.pnlMain.SuspendLayout();
            this.flpButtonsRight.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.Control;
            this.pnlMain.Controls.Add(this.textBox);
            this.pnlMain.Controls.Add(this.panel2);
            this.pnlMain.Controls.Add(this.captionLabel);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(10);
            this.pnlMain.Size = new System.Drawing.Size(292, 61);
            this.pnlMain.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlMain.TabIndex = 26;
            // 
            // textBox
            // 
            this.textBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox.Location = new System.Drawing.Point(10, 32);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(272, 20);
            this.textBox.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(10, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(272, 7);
            this.panel2.TabIndex = 13;
            // 
            // captionLabel
            // 
            this.captionLabel.AutoSize = true;
            this.captionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.captionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captionLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.captionLabel.Location = new System.Drawing.Point(10, 10);
            this.captionLabel.Name = "captionLabel";
            this.captionLabel.Size = new System.Drawing.Size(85, 15);
            this.captionLabel.TabIndex = 12;
            this.captionLabel.Text = "Enter Caption:";
            // 
            // beveledLine
            // 
            this.beveledLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.beveledLine.Location = new System.Drawing.Point(0, 61);
            this.beveledLine.Name = "beveledLine";
            this.beveledLine.Size = new System.Drawing.Size(292, 2);
            this.beveledLine.TabIndex = 25;
            // 
            // flpButtonsRight
            // 
            this.flpButtonsRight.BackColor = System.Drawing.Color.Transparent;
            this.flpButtonsRight.Controls.Add(this.btnCancel);
            this.flpButtonsRight.Controls.Add(this.btnOK);
            this.flpButtonsRight.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpButtonsRight.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpButtonsRight.Location = new System.Drawing.Point(1, 0);
            this.flpButtonsRight.Name = "flpButtonsRight";
            this.flpButtonsRight.Padding = new System.Windows.Forms.Padding(5);
            this.flpButtonsRight.Size = new System.Drawing.Size(290, 42);
            this.flpButtonsRight.TabIndex = 16;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(213, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Location = new System.Drawing.Point(137, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 25);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.flpButtonsRight);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 63);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(1);
            this.pnlButtons.Size = new System.Drawing.Size(292, 43);
            this.pnlButtons.Style = Halloumi.Common.Windows.Controls.PanelStyle.ButtonStrip;
            this.pnlButtons.TabIndex = 24;
            // 
            // frmUserInput
            // 
            this.AcceptButton = this.btnOK;
            this.AllowFormChrome = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(292, 106);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.beveledLine);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUserInput";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Input";
            this.UseApplicationIcon = true;
            this.Load += new System.EventHandler(this.frmUserInput_Load);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.flpButtonsRight.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.Panel pnlMain;
        private Halloumi.Common.Windows.Controls.BeveledLine beveledLine;
        private System.Windows.Forms.FlowLayoutPanel flpButtonsRight;
        private Halloumi.Common.Windows.Controls.Button btnCancel;
        private Halloumi.Common.Windows.Controls.Button btnOK;
        private Halloumi.Common.Windows.Controls.Panel pnlButtons;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Panel panel2;
        private Halloumi.Common.Windows.Controls.Label captionLabel;
    }
}

