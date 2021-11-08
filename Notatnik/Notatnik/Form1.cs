using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notatnik
{
    public partial class Notatnik : Form
    {
        const string title = "Notatnik";
        string file = "nienazwany.txt";

        public Notatnik()
        {
            InitializeComponent();
        }

        public string Status
        {
            get { return statusStrip2.Text; }
            set { statusStrip2.Text = value; }
        }

        private void Notatnik_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Gotowy";
        }

        private void autorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Autor: Adrian Celczyński", DateTime.Now.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void czcionkaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult fontResult = fontDialog1.ShowDialog();
            if (fontResult == DialogResult.OK)
            {
                textBox1.Font = fontDialog1.Font;
                Status = "Zmieniono czcionke: " + fontDialog1.Font;
            }
        }

        private void kolorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.AllowFullOpen = false;
            DialogResult result = colorDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                textBox1.ForeColor = colorDialog1.Color;
                Status = "Zmieniono kolor tekstu: " + textBox1.ForeColor;
            }
        }

        private void updateInterface(string filename)
        {
            file = filename;
            this.Text = title + " - " + Path.GetFileName(filename);
        }

        private void writeFile (string filename)
        {
            StreamWriter sw = new StreamWriter(filename);
            sw.Write(textBox1.Text);
            sw.Close();
        }

        private void zapiszJakoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                writeFile(filename);
                updateInterface(filename);
                Status = "Zapisano nowy plik :" + filename;
            }
        }

        #region utils
        private string readFile(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            string txt = sr.ReadToEnd();
            sr.Close();
            return txt;
        }
        #endregion

        private void otwórzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                textBox1.Text = readFile(filename);
                updateInterface(filename);
                Status = "Otwarto plik: " + filename;
            }
        }

        private void zakończToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Status = "Znaki: " + textBox1.Text.Length;
        }

        private DialogResult saveOnCloseDialog()
        {
            return MessageBox.Show("Czy zapisac przed zakonczeniem?", "Zakończ", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
        }

        private void main_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = saveOnCloseDialog();
            switch (result)
            {
                case DialogResult.Yes: zapiszJakoToolStripMenuItem_Click(null, null); break;
                case DialogResult.Cancel: e.Cancel = true; break;
            }
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            DialogResult result = saveOnCloseDialog();
            if (result == DialogResult.Cancel) return;
            if (result == DialogResult.Yes) zapiszJakoToolStripMenuItem_Click(null, null);

            string[] drop = (string[])e.Data.GetData(DataFormats.FileDrop);
            string filename = drop.Last();
            textBox1.Text = readFile(filename);
            updateInterface(filename);
            Status = "Otworzono plik: " + filename;
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (file == "")
            {
                zapiszJakoToolStripMenuItem_Click(null, null);
            }
            else
            {
                writeFile(file);
                Status = "Zapisano plik: " + file;
            }
        }
    }
}
