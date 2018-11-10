using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace KPT
{
    public partial class Form1 : Form
    {
        class InitalizationData
        {
            public string lastOpenedProjectFile = "";
        }

        const string lastOpenedFileData = "last.yaml";
        InitalizationData initalizationData = new InitalizationData(); 

        public Form1()
        {
            InitializeComponent();
            this.Text = "KPT";

            if (LoadLastOpened())
            {
                button3.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";

            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName == "")
            {
                return;
            }

            if (ProjectFolder.ReadProjectFile(openFileDialog1.FileName))
            {
                this.Visible = false;

                var projectForm = new ProjectForm();

                ProjectFolder.rootDir = Path.GetDirectoryName(openFileDialog1.FileName);
                
                initalizationData.lastOpenedProjectFile = openFileDialog1.FileName;
                SaveLastOpened();
                LoadLastOpened();

                projectForm.ShowDialog();

                this.Visible = true;
            }

        }

        private void SaveLastOpened()
        {
            FileStream fs = new FileStream("last.yaml", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            var yamlSerialzer = new SharpYaml.Serialization.Serializer();

            var serialzedData = yamlSerialzer.Serialize(initalizationData);

            sw.Write(serialzedData);

            sw.Close();
            fs.Close();

        }

        private bool LoadLastOpened()
        {
            if (!File.Exists(lastOpenedFileData))
            {
                return false;
            }

            FileStream fs = new FileStream(lastOpenedFileData, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            var fileData = sr.ReadToEnd();

            sr.Close();
            fs.Close();

            var yamlSerialzer = new SharpYaml.Serialization.Serializer();
            initalizationData = yamlSerialzer.Deserialize<InitalizationData>(fileData);

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = "";

            folderBrowserDialog1.ShowDialog();

            if (folderBrowserDialog1.SelectedPath == "")
            {
                return;
            }

            string selectedDir = folderBrowserDialog1.SelectedPath;

            if (ProjectFolder.InitalizeNewProjectFolder(selectedDir))
            {
                MessageBox.Show(string.Format("Project file created at {0}!", folderBrowserDialog1.SelectedPath));
            }
            else
            {
                MessageBox.Show("There was an error while attempting to create the new project file.");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

            string projectFile = initalizationData.lastOpenedProjectFile;

            if (ProjectFolder.ReadProjectFile(projectFile))
            {
                this.Visible = false;

                var projectForm = new ProjectForm();

                ProjectFolder.rootDir = Path.GetDirectoryName(projectFile);

                projectForm.ShowDialog();

                this.Visible = true;
            }
        }
    }
}
