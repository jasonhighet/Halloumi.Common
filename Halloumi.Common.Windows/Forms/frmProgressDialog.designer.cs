namespace Halloumi.Common.Windows.Forms
{
    partial class frmProgressDialog
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
            this.components = new System.ComponentModel.Container();
            this.pnlButtons = new Halloumi.Common.Windows.Controls.Panel();
            this.flpButtonsRight = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new Halloumi.Common.Windows.Controls.Button();
            this.btnOK = new Halloumi.Common.Windows.Controls.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.beveledLine = new Halloumi.Common.Windows.Controls.BeveledLine();
            this.pnlMain = new Halloumi.Common.Windows.Controls.Panel();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new Halloumi.Common.Windows.Controls.Panel();
            this.txtDetails = new Halloumi.Common.Windows.Controls.RichTextBox();
            this.lblText = new Halloumi.Common.Windows.Controls.Label();
            this.prgProgessBar = new Halloumi.Common.Windows.Controls.ProgressBar();
            this.pnlButtons.SuspendLayout();
            this.flpButtonsRight.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.tblMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.flpButtonsRight);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 279);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(494, 42);
            this.pnlButtons.Style = Halloumi.Common.Windows.Controls.PanelStyle.ButtonStrip;
            this.pnlButtons.TabIndex = 10;
            // 
            // flpButtonsRight
            // 
            this.flpButtonsRight.BackColor = System.Drawing.Color.Transparent;
            this.flpButtonsRight.Controls.Add(this.btnCancel);
            this.flpButtonsRight.Controls.Add(this.btnOK);
            this.flpButtonsRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpButtonsRight.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpButtonsRight.Location = new System.Drawing.Point(0, 0);
            this.flpButtonsRight.Name = "flpButtonsRight";
            this.flpButtonsRight.Padding = new System.Windows.Forms.Padding(5);
            this.flpButtonsRight.Size = new System.Drawing.Size(494, 42);
            this.flpButtonsRight.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(411, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(335, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 25);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // beveledLine
            // 
            this.beveledLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.beveledLine.Location = new System.Drawing.Point(0, 277);
            this.beveledLine.Name = "beveledLine";
            this.beveledLine.Size = new System.Drawing.Size(494, 2);
            this.beveledLine.TabIndex = 26;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.Control;
            this.pnlMain.Controls.Add(this.tblMain);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(5);
            this.pnlMain.Size = new System.Drawing.Size(494, 277);
            this.pnlMain.Style = Halloumi.Common.Windows.Controls.PanelStyle.Content;
            this.pnlMain.TabIndex = 27;
            // 
            // tblMain
            // 
            this.tblMain.BackColor = System.Drawing.Color.Transparent;
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Controls.Add(this.panel1, 0, 3);
            this.tblMain.Controls.Add(this.lblText, 0, 0);
            this.tblMain.Controls.Add(this.prgProgessBar, 0, 2);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(5, 5);
            this.tblMain.Name = "tblMain";
            this.tblMain.Padding = new System.Windows.Forms.Padding(5);
            this.tblMain.RowCount = 4;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Size = new System.Drawing.Size(484, 267);
            this.tblMain.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtDetails);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(8, 73);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(468, 186);
            this.panel1.TabIndex = 3;
            // 
            // txtDetails
            // 
            this.txtDetails.BackColor = System.Drawing.SystemColors.Window;
            this.txtDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDetails.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetails.Location = new System.Drawing.Point(0, 0);
            this.txtDetails.Name = "txtDetails";
            this.txtDetails.ReadOnly = true;
            this.txtDetails.Size = new System.Drawing.Size(466, 184);
            this.txtDetails.TabIndex = 3;
            this.txtDetails.Text = "";
            this.txtDetails.WordWrap = false;
            // 
            // lblText
            // 
            this.lblText.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblText.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblText.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblText.Location = new System.Drawing.Point(8, 5);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(468, 20);
            this.lblText.Style = Halloumi.Common.Windows.Controls.LabelStyle.Caption;
            this.lblText.TabIndex = 0;
            this.lblText.Text = "Text";
            // 
            // prgProgessBar
            // 
            this.prgProgessBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prgProgessBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.prgProgessBar.Location = new System.Drawing.Point(8, 38);
            this.prgProgessBar.Max = 100;
            this.prgProgessBar.Min = 0;
            this.prgProgessBar.Name = "prgProgessBar";
            this.prgProgessBar.Size = new System.Drawing.Size(468, 23);
            this.prgProgessBar.TabIndex = 1;
            this.prgProgessBar.Value = 50;
            // 
            // frmProgressDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(494, 321);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.beveledLine);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProgressDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmProgressDialog_Load);
            this.Shown += new System.EventHandler(this.frmProgressDialog_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmProgressDialog_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmProgressDialog_FormClosing);
            this.pnlButtons.ResumeLayout(false);
            this.flpButtonsRight.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.tblMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Halloumi.Common.Windows.Controls.Panel pnlButtons;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.FlowLayoutPanel flpButtonsRight;
        private Halloumi.Common.Windows.Controls.Button btnCancel;
        private Halloumi.Common.Windows.Controls.Button btnOK;
        private Halloumi.Common.Windows.Controls.BeveledLine beveledLine;
        private Halloumi.Common.Windows.Controls.Panel pnlMain;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private Halloumi.Common.Windows.Controls.Label lblText;
        private Halloumi.Common.Windows.Controls.ProgressBar prgProgessBar;
        private Halloumi.Common.Windows.Controls.Panel panel1;
        private Halloumi.Common.Windows.Controls.RichTextBox txtDetails;
    }
}

