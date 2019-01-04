using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace KPT
{
    public partial class ProgressBar : Form
    {

        BackgroundWorker attachedWorker;

        public ProgressBar(BackgroundWorker worker)
        {
            attachedWorker = worker;
            InitializeComponent();
            this.Text = "Progress Bar";
        }

        private void ProgressBar_Load(object sender, EventArgs e)
        {

        }

        public void UpdateProgressBar(int value)
        {
            progressBar1.Value = value;
        }

        public void UpdateProgressBarText(string text)
        {
            //label1.Text = text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            attachedWorker.CancelAsync();
            this.Close();
        }
    }

}
