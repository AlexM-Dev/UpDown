namespace UpDown.DemoVisualAddon {
    partial class frmMain {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lvwData = new System.Windows.Forms.ListView();
            this.stuMain = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.SuspendLayout();

            //
            // lvwData
            //
            lvwData.Dock = System.Windows.Forms.DockStyle.Fill;
            lvwData.View = System.Windows.Forms.View.Details;
            lvwData.Columns.Add("Website").Width = 500;
            lvwData.Columns.Add("Is Down").Width = 300;
            lvwData.FullRowSelect = true;
            lvwData.MultiSelect = false;
            lvwData.GridLines = true;

            //
            // stuMain
            //
            stuMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            stuMain.Items.Add(lblStatus);

            //
            // lblStatus
            //
            lblStatus.Text = "Waiting.";

            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lvwData);
            this.Controls.Add(this.stuMain);
            this.Name = "DemoVisualAddon";
            this.Text = "DemoVisualAddon";
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListView lvwData;
        private System.Windows.Forms.StatusStrip stuMain;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}

