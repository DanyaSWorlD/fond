using System;
using System.Collections.Generic;
using System.Media;
using System.Data.Common;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;

namespace FOND
{
    public partial class MainForm : Form
    {
        lastlog lg = new lastlog();
        public MainForm()
        {
            InitializeComponent();
            button9.Image = new Bitmap(Properties.Resources.s_ico1, new Size(button9.ClientRectangle.Width - 7, button9.ClientRectangle.Height - 7));
            lg.add(Application.CompanyName + " || FOND™ Copyright © 2015-2016 все права защищены || версия продукта: " + Application.ProductVersion);
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            datacheck();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            lg.add("нажата кнопка 1");
            MainForm.ActiveForm.Hide();
            RedForm datainsform = new RedForm(true);
            datainsform.ShowDialog();
            this.Show();
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox1, "Настройки");
        }

        private void userControl11_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(userControl11, "Текущая дата и время компьютера");
        }

        private void userControl11_Load(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Forms.Settings set = new Forms.Settings();
            MainForm.ActiveForm.Hide();
            set.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lg.add("нажата кнопка 2");
            MainForm.ActiveForm.Hide();
            RedForm datainsform = new RedForm(false);
            datainsform.ShowDialog();
            this.Show();
        }

        //проверка бд, ее структуры и значенй перед запуском приложения
        private void datacheck()
        {
            if (FOND.Properties.Settings.Default.db_file_dir == "" || !File.Exists(Properties.Settings.Default.db_file_dir))
            {
                bd_master Bd_master = new bd_master();
                Bd_master.Owner = this;
                Bd_master.ShowDialog();
                Bd_master.Dispose();
            }
            else
            {
                var tnames = new string[] { "workers", "materialway", "material_theme", "material_presentation", "quality", "type", "speakerlvl", "regions", "parts", "smi", "card_in_smi" };
                var connstr = "`name` = '" + tnames[0] + "'";
                SQLiteConnection conn = new SQLiteConnection(string.Format("Data Source = {0};", Properties.Settings.Default.db_file_dir));
                for (int i = 1; i < tnames.Length; i++)
                {
                    connstr += " OR `name` = '" + tnames[i] + "'";
                }
                SQLiteCommand comm = new SQLiteCommand("SELECT `name` FROM `sqlite_master` WHERE " + connstr, conn);
                conn.Open();
                try { comm.ExecuteNonQuery(); } catch (SQLiteException e) { MessageBox.Show(e.Message, "lol"); }
                SQLiteDataReader dr = comm.ExecuteReader();
                var p = 0;
                foreach (DbDataRecord record in dr)
                {
                    var name = record["name"] + "";
                    for (int i = 0; i < tnames.Length; i++)
                    {
                        if (name == tnames[i])
                        {
                            p++;
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
                if (p != tnames.Length)
                {
                    bd_master Bd_master = new bd_master();
                    Bd_master.Owner = this;
                    Bd_master.ShowDialog();
                    Bd_master.Dispose();
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void miBut1_Click(object sender, EventArgs e)
        {

        }
    }
}
       
