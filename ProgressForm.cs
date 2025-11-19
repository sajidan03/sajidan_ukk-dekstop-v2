using System;
using System.Windows.Forms;

namespace sajidan_ukk_dekstop
{
    public partial class ProgressForm : Form
    {
        public ProgressForm(string message)
        {
            InitializeComponent();
            labelMessage.Text = message;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ControlBox = false;
        }

        public ProgressForm() : this("Processing...")
        {
        }

        // Method untuk update pesan
        public void UpdateMessage(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateMessage), message);
            }
            else
            {
                labelMessage.Text = message;
                Application.DoEvents();
            }
        }

        // Method untuk mengubah style progress bar
        public void SetProgressStyle(ProgressBarStyle style)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<ProgressBarStyle>(SetProgressStyle), style);
            }
            else
            {
                progressBar.Style = style;
                Application.DoEvents();
            }
        }

        // Method untuk set nilai progress (jika menggunakan ProgressBarStyle.Continuous)
        public void SetProgressValue(int value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int>(SetProgressValue), value);
            }
            else
            {
                if (value >= progressBar.Minimum && value <= progressBar.Maximum)
                {
                    progressBar.Value = value;
                    Application.DoEvents();
                }
            }
        }

        private void labelMessage_Click(object sender, EventArgs e)
        {

        }
    }
}