using System;
using System.IO;
using System.ComponentModel;
using System.Data.SQLite;
using System.Threading;
using Common;
using FOND.Properties;
using FOND.masterPages;
using System.Windows.Forms;

namespace FOND
{
    public partial class bd_master : Form
    {
        public static SQLiteConnection conn;

        public master mstr = new master();
        private Control[] pages = new Control[] { new page1(), new page2(), new page3(), new pageend() };
        public bool endwconfim;
        public bd_master()
        {
            InitializeComponent();
            loadI();
            if (Settings.Default.db_file_dir != null && Settings.Default.db_file_dir != "") (pages[0] as page1).loaddbdir(Settings.Default.db_file_dir);
        }

        private void loadI()
        {
            if (panel1.Controls.Count == 0)
            {
                panel1.Controls.Add(pages[0]);
            }
            if (panel1.Controls[0] != pages[0] && panel1.Controls[0] != pages[pages.Length - 1])
            {
                nextB.Enabled = master.nextBE;
                bacbB.Enabled = master.backBE;
                cancB.Text = "отмена";
            }
            else
            {
                if (panel1.Controls[0] == pages[0])
                {
                    bacbB.Enabled = false;
                    nextB.Enabled = master.nextBE;
                    cancB.Text = "отмена";
                }
                else
                {
                    nextB.Enabled = false;
                    bacbB.Enabled = master.backBE;
                    cancB.Text = "готово";
                }
            }
            
        }
        private void button3_Click(object sender, EventArgs e)
        {
            var pInd = Array.IndexOf(pages, panel1.Controls[0]);
            var continue_ok = false;
            switch (pInd)
            {
                case 0:
                    continue_ok = (pages[pInd] as page1).worker();
                    break;
                case 1:
                    continue_ok = (pages[pInd] as page2).worker();
                    break;
                case 2:
                    continue_ok = (pages[pInd] as page3).worker();
                    break;
            }
            if (continue_ok == true)
            {
                if (pInd == 1)
                {
                    (pages[2] as page3).load();
                }
                panel1.Controls.Clear();
                panel1.Controls.Add(pages[pInd + 1]);
            }
            bm_load();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            loadI();
        }

        private void bacbB_Click(object sender, EventArgs e)
        {
            var pInd = Array.IndexOf(pages, panel1.Controls[0]);
            var continue_ok = false;
            switch (pInd)
            {
                case 0:
                    continue_ok = (pages[pInd] as page1).worker();
                    break;
                case 1:
                    continue_ok = (pages[pInd] as page2).workerBack();
                    break;
                case 2:
                    continue_ok = (pages[pInd] as page3).workerback();
                    bm_load();
                    break;
                case 3:
                    continue_ok = (pages[pInd] as pageend).workerback();
                    break;

            }

            if (continue_ok == true)
            {
                panel1.Controls.Clear();
                panel1.Controls.Add(pages[pInd - 1]);

            }
        }


        private void cancB_Click(object sender, EventArgs e)
        {
            var pind = Array.IndexOf(pages, panel1.Controls[0]);
            if (pind == 3)
            {
               endwconfim = (pages[pind] as pageend).enderer();
                this.Close();
            }
            else
            {

                this.Close();
            }


        }

        private void bd_master_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (endwconfim == false)
            {
                var rez = MessageBox.Show("Вы точно хотите покинуть мастер настройки баз данных? Все не сохраненные данные будут утеряны", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                e.Cancel = rez == DialogResult.No;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void bm_load()
        {
            (pages[1] as page2).load();

        }

        private void bd_master_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (conn != null)
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    try
                    { conn.Close(); }
                    catch (SQLiteException e2) { lastlog lg = new lastlog(); lg.add(e2.Message); }
                }
            }
        }
    }
}