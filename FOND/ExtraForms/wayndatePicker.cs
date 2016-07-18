using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;

namespace FOND.ExtraForms
{
    public partial class wayndatePicker : Form
    {
        private DialogResult D = DialogResult.Abort;
        public wayndatePicker()
        {
            InitializeComponent();
            try
            {
                dateTimePicker1.Value = DateTime.Today.AddYears(-1);
                SQLiteConnection conn = new SQLiteConnection(string.Format("Data Source = {0};", Properties.Settings.Default.db_file_dir));
                conn.Open();
                SQLiteCommand comm = new SQLiteCommand("SELECT * FROM material_theme;",conn);
                SQLiteDataReader dr = comm.ExecuteReader();
                foreach (DbDataRecord ddr in dr)
                {
                    combovalue cv = new combovalue();
                    cv.number = (int)(long)ddr["id"];
                    cv.value = (string)ddr["value"];
                    checkBoxComboBox1.Items.Add(cv);
                }
                conn.Close();
                conn.Dispose();
            }
            catch(Exception e) { MessageBox.Show("Ошибка при загрузке данных!\n" + e.Message, "Ошбка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            if (checkBoxComboBox1.Text != "")
            {
                string request = " where";
                string[] str = checkBoxComboBox1.Text.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                bool p = true;
                foreach (string st in str)
                {
                    int id = (checkBoxComboBox1.Items[checkBoxComboBox1.Items.IndexOf(st)] as combovalue).number;
                    if (p)
                    {
                        p = false;
                        request += " id = '" + id + "'";
                    }
                    else
                    {
                        request += " or id = '" + id + "'";
                    }
                }
                (Owner as Forms.report_maker).continueWork(dateTimePicker1.Value,dateTimePicker2.Value,request);
                D = DialogResult.OK;
                Close();
            }
            else
            {
                Close();
            }
        }

        private void wayndatePicker_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = D;
        }
    }
}
