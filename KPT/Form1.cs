using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KPT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "KPT";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";

            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName == "")
            {
                return;
            }

            if (ProjectFolder.CheckProjectFile(openFileDialog1.FileName))
            {
                this.Visible = false;

                var projectForm = new ProjectForm();

                projectForm.ShowDialog();

                this.Visible = true;
            }

        }
    }
}
