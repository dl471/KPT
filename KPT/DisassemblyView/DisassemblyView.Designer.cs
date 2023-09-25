namespace KPT.DisassemblyView
{
    partial class DisassemblyView
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
            this.rawHex = new System.Windows.Forms.RichTextBox();
            this.disassembled = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.headerFooterCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // rawHex
            // 
            this.rawHex.Location = new System.Drawing.Point(31, 47);
            this.rawHex.Name = "rawHex";
            this.rawHex.Size = new System.Drawing.Size(253, 312);
            this.rawHex.TabIndex = 0;
            this.rawHex.Text = "";
            // 
            // disassembled
            // 
            this.disassembled.Location = new System.Drawing.Point(322, 47);
            this.disassembled.Name = "disassembled";
            this.disassembled.Size = new System.Drawing.Size(399, 312);
            this.disassembled.TabIndex = 1;
            this.disassembled.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Raw Hex";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(322, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Disassembled";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(31, 388);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Disassemble";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // headerFooterCheckbox
            // 
            this.headerFooterCheckbox.AutoSize = true;
            this.headerFooterCheckbox.Location = new System.Drawing.Point(31, 365);
            this.headerFooterCheckbox.Name = "headerFooterCheckbox";
            this.headerFooterCheckbox.Size = new System.Drawing.Size(180, 17);
            this.headerFooterCheckbox.TabIndex = 5;
            this.headerFooterCheckbox.Text = "Contains StCp header and footer";
            this.headerFooterCheckbox.UseVisualStyleBackColor = true;
            // 
            // DisassemblyView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.headerFooterCheckbox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.disassembled);
            this.Controls.Add(this.rawHex);
            this.Name = "DisassemblyView";
            this.Text = "Disassembly View";
            this.Load += new System.EventHandler(this.DisassemblyView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rawHex;
        private System.Windows.Forms.RichTextBox disassembled;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox headerFooterCheckbox;
    }
}