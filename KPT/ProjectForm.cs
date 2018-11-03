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
    public partial class ProjectForm : Form
    {
        public ProjectForm()
        {
            InitializeComponent();
        }

        public bool CheckFolder(string fileName)
        {
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ProjectFolder.RebuildCPKs())
            {
                MessageBox.Show("CPKs rebuilt!");
            }
            else
            {
                MessageBox.Show("There was an error while rebuilding the CPKs.");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ProjectFolder.DumpStrings())
            {
                MessageBox.Show("Strings dumped!");
            }
            else
            {
                MessageBox.Show("There was an error while dumping strings.");
            }
        }
    }
}
