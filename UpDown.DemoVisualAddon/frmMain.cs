using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UpDown.Core;

namespace UpDown.DemoVisualAddon {
    public partial class frmMain : Form {
        public frmMain() {
            InitializeComponent();
        }

        internal void ThreadClose() {
            try {
                this.Invoke((MethodInvoker)delegate {
                    this.Close();
                });
            } catch { }
        }

        internal void SetData(string website, bool down) {
            this.Invoke((MethodInvoker)delegate {
                ListViewItem lItem = lvwData.FindItemWithText(website);
                if (lItem == null) {
                    var item = lvwData.Items.Add(website);
                    item.SubItems.Add(down.ToString());
                } else {
                    lItem.SubItems[1].Text = down.ToString();
                }

                lblStatus.Text = "Waiting.";
            });
        }

        internal void SetStatus(string status) {
            this.Invoke((MethodInvoker)delegate {
                lblStatus.Text = status;
            });
        }

        internal void UpdateData(Dictionary<string, bool> data) {
            foreach (var website in data) {
                SetData(website.Key, website.Value);
            }
        }

        private void buttonExit_Click(object sender, EventArgs e) {
            Application.Exit();
        }
    }
}
