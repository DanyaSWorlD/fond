using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOND.ExtraForms
{
    public partial class TextEdit : Form
    {
        //save without Asking
        private bool saveWA = false;
        private bool changed = false;
        public TextEdit()
        {
            InitializeComponent();
            richTextBox1.Font = new Font(FontFamily.GenericSansSerif, 12);
            try
            {
                richTextBox1.LoadFile(Properties.Settings.Default.footer_text, RichTextBoxStreamType.RichText);
            }
            catch(Exception e)
            {
                Common.lastlog lg = new Common.lastlog();
                lg.add("TextEdit:" + e.Message);
            }
        }

        private void bold_Click(object sender, EventArgs e)
        {
            Font oldFont = richTextBox1.SelectionFont;
            richTextBox1.SelectionFont = fontCh(oldFont, FontStyle.Bold, oldFont.Bold);
        }
        private void italic_Click(object sender, EventArgs e)
        {
            Font oldFont = richTextBox1.SelectionFont;
            richTextBox1.SelectionFont = fontCh(oldFont, FontStyle.Italic, oldFont.Italic);
        }
        private void underline_Click(object sender, EventArgs e)
        {
            Font oldFont = richTextBox1.SelectionFont;
            richTextBox1.SelectionFont = fontCh(oldFont, FontStyle.Underline, oldFont.Underline);
            richTextBox1.Focus();
        }
        private Font fontCh(Font oldFont, FontStyle fs, bool param)
        {
            if (param)
            {
                return new Font(oldFont, oldFont.Style & ~fs);
            }
            else
            {
                return new Font(oldFont, oldFont.Style | fs);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionAlignment == HorizontalAlignment.Center)
                richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
            else richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox1.Focus();

        }

        private void save_Click(object sender, EventArgs e)
        {
            saveWA = true;
            this.Close();
        }

        private void TextEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!saveWA)
            {
                var dr = MessageBox.Show("Вы хотите сохранить изменения?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    saveWA = true;
                }
            }
            if(saveWA)
            {
                string ffile = Directory.GetCurrentDirectory() + "/footer.rtf";
                richTextBox1.SaveFile(ffile,RichTextBoxStreamType.RichText);
                    Properties.Settings.Default.footer_text = ffile;
                    Properties.Settings.Default.Save();
            }
        }
    }
}
