using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data.Common;
using FOND.Properties;
using System.Data.SQLite;
using System.Windows.Forms;
using Common;
using System.Data;

namespace FOND.masterPages
{
    public partial class page2 : UserControl
    {
        private string[] titles = new string[] { "ответственных сотрудников", "направленностей материала", "тем материалов", "видов материала", "качества материала", "видов выступления", "категорий выступающих", "райнов и пригородов вашего города", "подразделений участников" };
        private string[] tnames = new string[] { "workers", "materialway", "material_theme", "material_presentation", "quality", "type", "speakerlvl", "regions", "parts" };
        private string[] result;
        public int этап;
        private master mstr = new master();
        private SQLiteCommand comm;
        public int lasindex;
        public page2()
        {
            InitializeComponent();
            этап = 0;
        }
        public void load()
        {
            if (этап == 9)
            {
                этап = 8;
            }
            if (bd_master.conn != null)
            {
                if (bd_master.conn.State == ConnectionState.Closed)
                {
                    bd_master.conn.Open();
                }
            }
            else
            {
                bd_master.conn = new SQLiteConnection(string.Format("Data Source = {0};", Properties.Settings.Default.db_file_dir));
                try { bd_master.conn.Open(); } catch (SQLiteException e) { lastlog lg = new lastlog(); lg.add(e.Message); }
            }
            if (этап < tnames.Length && этап >= 0)
            {
                label1.Text = "Список " + titles[этап];
                textBox1.Text = "";
                comm = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type = 'table' AND name  = '" + tnames[этап] + "'", bd_master.conn);
                SQLiteDataReader dr = comm.ExecuteReader();
                if (dr.StepCount > 0)
                {
                    comm = new SQLiteCommand("SELECT value FROM " + tnames[этап] + "", bd_master.conn);
                    SQLiteDataReader dr2 = comm.ExecuteReader();
                    foreach (DbDataRecord record in dr2)
                    {
                        textBox1.Text += record["value"] + "\r\n";
                    }
                }
            }
            if (textBox1.Text == "")
            {
                mstr.nextButton(false);
            }
        }

        public bool worker()
        {

            result = textBox1.Lines;
            comm = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type = 'table' AND name  = '" + tnames[этап] + "'", bd_master.conn);
            SQLiteDataReader dr = comm.ExecuteReader();
            if (dr.StepCount > 0)
            {

                comm = new SQLiteCommand("DELETE FROM " + tnames[этап], bd_master.conn);
                comm.ExecuteNonQuery();
            }
            else
            {
                comm = new SQLiteCommand("CREATE TABLE `" + tnames[этап] + "`  (`value` TEXT NOT NULL , `id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE ); ", bd_master.conn);
                comm.ExecuteNonQuery();
            }

            for (var i = 0; i < result.Length; i++)
            {
                if (result[i] != "")
                {
                    comm = new SQLiteCommand("INSERT INTO " + tnames[этап] + "  ( value ) VALUES ('" + result[i] + "')", bd_master.conn);
                    comm.ExecuteNonQuery();
                }
            }
            textBox1.Text = "";
            mstr.nextButton(false);
            этап++;
            if (этап == tnames.Length)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool workerBack()
        {
            if (этап == 0)
            {
                return true;
            }
            этап--;
            load();
            return false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != null && textBox1.Text != "")
            {
                mstr.nextButton(true);
            }
            else
            {
                mstr.nextButton(false);
            }
        }
    }
}
