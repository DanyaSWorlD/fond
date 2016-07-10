using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.SQLite;
using System.Resources;
using System.Configuration;
using Common;
using System.IO;
using System.Windows.Forms;
using FOND.Properties;

namespace FOND.masterPages
{
    public partial class page1 : UserControl
    {
        private lastlog lg;
        public Common.master mstr = new master();
        public page1()
        {
            InitializeComponent();
            lg = new lastlog();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox2.Text = folderBrowserDialog1.SelectedPath;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBox1.Text = openFileDialog1.FileName;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = !textBox1.Enabled;
            textBox2.Enabled = !textBox2.Enabled;
            button4.Enabled = !button4.Enabled;
            button5.Enabled = !button5.Enabled;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            necheck();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            necheck();
        }
        private void necheck()
        {

            switch (radioButton1.Checked)
            {
                case true:
                    if (textBox1.Text != null && textBox1.Text != "")
                    {
                        mstr.nextButton(File.Exists(textBox1.Text) ? true : false);
                    }
                    else
                    {
                        mstr.nextButton(false);
                    }
                    break;
                case false:
                    mstr.nextButton(textBox2.Text != "" && textBox2.Text != null ? true : false);
                    break;
            }
        }
        public bool worker()
        {
            if (radioButton1.Checked == true)
            {
                if (File.Exists(textBox1.Text))
                {
                    Settings.Default.db_file_dir = textBox1.Text;
                    Settings.Default.Save();
                    return true;
                }
                return false;
            }
            else
            {
                if (!File.Exists(textBox2.Text + @"\main.db"))
                {
                    SQLiteConnection.CreateFile(textBox2.Text + @"\main.db");
                    if (File.Exists(textBox2.Text + @"\main.db"))
                    {
                        lg.add(" бд мастер: создание базы прошло успешно");
                        Settings.Default.db_file_dir = textBox2.Text + @"\main.db";
                        Settings.Default.Save();
                        return true;
                    }
                    else
                    {
                        lg.add(" бд мастер: что то пошло не так...");
                        return false;
                    }
                }
                else
                {
                    Settings.Default.db_file_dir = textBox2.Text + @"\main.db";
                    Settings.Default.Save();
                    return true;
                }

            }



        }
        public void loaddbdir(string inp)
        {
            textBox1.Text = inp;
        }
    }
}
