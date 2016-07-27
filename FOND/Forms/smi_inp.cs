using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.Common;
using System.Linq;
using Common;
using System.Windows.Forms;
using System.Data.SQLite;

namespace FOND.Forms
{
    public partial class smi_inp : Form
    {
        private bool isupdate;
        private long id;
        private string cisid;
        public smi_inp(long id)
        {
            InitializeComponent();
            this.id = id;
        }
        public smi_inp(long id, string cisid)
        {
            this.id = id;
            InitializeComponent();
            load_saved_params(cisid);
        }
        private void load_saved_params(string cisid)
        {
            this.cisid = cisid;
            isupdate = true;
            SQLiteConnection conn = new SQLiteConnection(string.Format("Data Source = {0};", Properties.Settings.Default.db_file_dir));
            conn.Open();
            SQLiteCommand comm = new SQLiteCommand("SELECT * FROM card_in_smi WHERE id = " + cisid, conn);
            SQLiteDataReader dr = comm.ExecuteReader();
            foreach (DbDataRecord ddr in dr)
            {
                var smi = ddr["smi"].ToString();
                    textBox1.Text = ddr["times"].ToString();
                    textBox2.Text = ddr["link"].ToString();
                if (ddr["date"].ToString() != "")
                    dateTimePicker1.Value = DateTime.Parse(ddr["date"].ToString());
                if (ddr["pu"].ToString() == "")
                    checkBox1.Checked = false;
                else
                    checkBox1.Checked = true;
                comm = new SQLiteCommand("SELECT * FROM smi WHERE id = " + smi, conn);
                SQLiteDataReader dr2 = comm.ExecuteReader();
                foreach (DbDataRecord ddr2 in dr2)
                {
                    switch (ddr2["type"].ToString())
                    {
                        case "internet":
                            comboBox1.SelectedIndex = 0;
                            break;
                        case "tv":
                            comboBox1.SelectedIndex = 1;
                            break;
                        case "rad":
                            comboBox1.SelectedIndex = 2;
                            break;
                        case "gaz":
                            comboBox1.SelectedIndex = 3;
                            break;
                    }
                    switch (ddr2["place"].ToString())
                    {

                        case "centr":
                            comboBox2.SelectedIndex = 0;
                            break;
                        case "reg":
                            comboBox2.SelectedIndex = 1;
                            break;
                        case "another":
                            comboBox2.SelectedIndex = 2;
                            break;

                    }
                    update();
                    for (var i = 0; i < comboBox3.Items.Count; i++)
                    {
                        if (ddr2["name"].ToString() == comboBox3.Items[i].ToString())
                            comboBox3.SelectedIndex = i;
                    }
                }

            }

            conn.Close();
        }

        private void smi_inp_Load(object sender, EventArgs e)
        {

        }

        private void line1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection(string.Format("Data Source = {0};", Properties.Settings.Default.db_file_dir));
                var itm = (comboBox3.SelectedItem as combovalue);
                if (itm != null)
                {
                    itm.value = SqliteCommon.realEscapeString(itm.value);
                    conn.Open();
                    if (isupdate)
                    {

                        SQLiteCommand comm = new SQLiteCommand("UPDATE `card_in_smi` SET `cards_num` = " + id + ",`smi` = " + itm.number + ",`date` = '" + SqliteCommon.realEscapeString(dateTimePicker1.Value.ToString("yyyy-MM-dd")) + "',`times` = '" + SqliteCommon.realEscapeString(textBox1.Text) + "',`link` = '" + SqliteCommon.realEscapeString(textBox2.Text) + "',`pu` = '" + (checkBox1.Checked == true ? "true" : "") + "' WHERE id = " + cisid + ";", conn);
                        comm.ExecuteNonQuery();
                        conn.Close();
                        this.Close();

                    }
                    else
                    {
                        var col = 0;
                        SQLiteCommand comm = new SQLiteCommand("SELECT * FROM `card_in_smi` WHERE cards_num = " + id + " AND smi = " + itm.number + " ;", conn);
                        SQLiteDataReader dr = comm.ExecuteReader();
                        foreach (DbDataRecord ddr in dr)
                        {
                            col++;
                        }
                        if (col == 0)
                        {
                            comm = new SQLiteCommand("INSERT INTO `card_in_smi`(`cards_num`,`smi`,`date`,`times`,`link`,`pu`) VALUES(" + id + ", " + itm.number + ", '" + SqliteCommon.realEscapeString(dateTimePicker1.Value.ToString("yyyy-MM-dd")) + "', '" + SqliteCommon.realEscapeString(textBox1.Text) + "','" + SqliteCommon.realEscapeString(textBox2.Text) + "','" + (checkBox1.Checked == true ? "true" : "") + "');", conn);
                            comm.ExecuteNonQuery();
                            conn.Close();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show(this, "Вы пытаетесь добавить сми которая уже есть в списках этой карточки. Вы можете увеличить количество повторов в предыдущей записи.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            conn.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Наименование - поле обязательное для заполнения!");
                    conn.Close();
                }
            }catch (Exception E)
            {
                MessageBox.Show("Ошибка!\n" + E.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        void update()
        {
            string type = null;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    type = "internet";
                    break;
                case 1:
                    type = "tv";
                    break;
                case 2:
                    type = "rad";
                    break;
                case 3:
                    type = "gaz";
                    break;
            }
            if (type != null && comboBox2.SelectedText == "")
            {
                if (comboBox1.SelectedIndex == 0)
                    comboBox2.SelectedIndex = 2;
            }
            string place = null;
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    place = "centr";
                    break;
                case 1:
                    place = "reg";
                    break;
                case 2:
                    place = "another";
                    break;
            }

            if (place == null)
            {

            }
            SQLiteConnection conn = new SQLiteConnection(string.Format("Data Source = {0};", Properties.Settings.Default.db_file_dir));
            conn.Open();
            comboBox3.Items.Clear();
            SQLiteCommand comm = new SQLiteCommand("SELECT name,id FROM smi WHERE type = '" + type + "' AND place = '" + place + "';", conn);
            SQLiteDataReader dr = comm.ExecuteReader();
            foreach (DbDataRecord dbdr in dr)
            {
                combovalue cmb = new combovalue();
                cmb.number = (int)(long)dr["id"];
                cmb.value = dr["name"].ToString();
                comboBox3.Items.Add(cmb);
            }
            conn.Close();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            update();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox2.Text = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start(textBox2.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
