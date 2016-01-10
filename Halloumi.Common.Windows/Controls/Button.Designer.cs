namespace Halloumi.Common.Windows.Controls
{
    partial class Button
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnKrypton = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSystem = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnKrypton
            // 
            this.btnKrypton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnKrypton.Location = new System.Drawing.Point(0, 0);
            this.btnKrypton.Name = "btnKrypton";
            this.btnKrypton.Size = new System.Drawing.Size(75, 25);
            this.btnKrypton.TabIndex = 0;
            this.btnKrypton.Text = "&OK";
            this.btnKrypton.Values.ExtraText = "";
            this.btnKrypton.Values.Image = null;
            this.btnKrypton.Values.ImageStates.ImageCheckedNormal = null;
            this.btnKrypton.Values.ImageStates.ImageCheckedPressed = null;
            this.btnKrypton.Values.ImageStates.ImageCheckedTracking = null;
            this.btnKrypton.Values.Text = "&OK";
            this.btnKrypton.Click += new System.EventHandler(this.btnKrypton_Click);
            // 
            // btnSystem
            // 
            this.btnSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSystem.Location = new System.Drawing.Point(0, 0);
            this.btnSystem.Margin = new System.Windows.Forms.Padding(0);
            this.btnSystem.Name = "btnSystem";
            this.btnSystem.Size = new System.Drawing.Size(75, 25);
            this.btnSystem.TabIndex = 1;
            this.btnSystem.Text = "&OK";
            this.btnSystem.UseVisualStyleBackColor = true;
            this.btnSystem.Visible = false;
            this.btnSystem.Click += new System.EventHandler(this.btnSystem_Click);
            // 
            // Button
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSystem);
            this.Controls.Add(this.btnKrypton);
            this.Name = "Button";
            this.Size = new System.Drawing.Size(75, 25);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnKrypton;
        private System.Windows.Forms.Button btnSystem;
    }
}
