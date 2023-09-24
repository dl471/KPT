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
using KPT.Parser.Headers;

namespace KPT.DisassemblyView
{
    public partial class DisassemblyView : Form
    {
        public DisassemblyView()
        {
            InitializeComponent();
        }

        private void DisassemblyView_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string rawInput = rawHex.Text;
            string[] hexBytes = rawInput.Split(' ');

            MemoryStream ms = new MemoryStream(hexBytes.Length);
            BinaryWriter bw = new BinaryWriter(ms);

            foreach (string hexByte in hexBytes)
            {
                byte parsedByte;
                bool success = byte.TryParse(hexByte, System.Globalization.NumberStyles.HexNumber, null, out parsedByte);
                if(!success)
                {
                    string message = string.Format("Failed to convert {0} to byte. Input must be list of hex bytes without 0x prefix seperated with spaces.", hexByte);
                    MessageBox.Show(message, "Disassembly failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                bw.Write(parsedByte);
            }

            ms.Seek(0, SeekOrigin.Begin);

            // create a fake header and footer to emulate an St file for the parser
            if (!headerFooterCheckbox.Checked)
            {
                int headerSize = St_Header.HEADER_SIZE; 
                int footerSize = 0x1E; // footer does not have its own element?

                MemoryStream newMemoryStream = new MemoryStream(hexBytes.Length + headerSize + footerSize);
                BinaryWriter newMSWriter = new BinaryWriter(newMemoryStream);
                BinaryReader oldMSReader = new BinaryReader(ms);

                for (int i = 0; i < headerSize; i++)
                {
                    newMSWriter.Write((byte)0x00);
                }

                byte[] oldMSContents = oldMSReader.ReadBytes(hexBytes.Length);
                newMSWriter.Write(oldMSContents);

                for (int i = 0; i < footerSize; i++)
                {
                    newMSWriter.Write((byte)0x88);
                }

                ms = newMemoryStream;
                ms.Seek(0, SeekOrigin.Begin);
            }

            BinaryReader br = new BinaryReader(ms);

            var parser = new KPT.Parser.FileParser();
            var file = parser.ParseFile(br, "[DISASSEMBLY INPUT]");

            br.Close();

            MemoryStream tempBuffer = new MemoryStream(4096); // why 4096?
            BinaryWriter tbWriter = new BinaryWriter(tempBuffer);
            BinaryReader tbReader = new BinaryReader(tempBuffer);

            StringBuilder sb = new StringBuilder();

            // IInstruction doesn't have any functionality to disassemble itself or convert itself to hex string, do we just do a quick conversion here for now
            foreach (var instruction in file.instructions)
            {
                instruction.Write(tbWriter);
                long bytesWritten = tbWriter.BaseStream.Position;
                tbWriter.BaseStream.Seek(0, SeekOrigin.Begin);
                
                for (int i = 0; i < bytesWritten; i++)
                {
                    byte currentByte = tbReader.ReadByte();
                    string currentHex = currentByte.ToString("X");
                    if (currentHex.Length == 1)
                    {
                        currentHex = "0" + currentHex;
                    }
                    sb.Append(currentHex);
                    sb.Append(" ");
                }

                sb.AppendLine();
                tbWriter.BaseStream.Seek(0, SeekOrigin.Begin);
                
            }

            string splitHex = sb.ToString();
            disassembled.Text = splitHex;
        }
    }
}
