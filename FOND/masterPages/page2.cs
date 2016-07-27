using System;
using System.Data.Common;
using System.Data.SQLite;
using Common;
using System.Data;
using System.Collections;
using System.Windows.Forms;

namespace FOND.masterPages
{
    public partial class page2 : MasterPage
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
        override public void load()
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
        override public bool worker()
        {

            result = textBox1.Lines;
            comm = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type = 'table' AND name  = '" + tnames[этап] + "'", bd_master.conn);
            SQLiteDataReader dr = comm.ExecuteReader();
            if (dr.StepCount == 0)
            {
                comm = new SQLiteCommand("CREATE TABLE `" + tnames[этап] + "`  (`value` TEXT NOT NULL , `id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE ); ", bd_master.conn);
                comm.ExecuteNonQuery();
            }

            ArrayList toDelete = new ArrayList();
            comm = new SQLiteCommand("SELECT * FROM " + tnames[этап] + " ;", bd_master.conn);
            dr = comm.ExecuteReader();
            foreach (DbDataRecord ddr in dr)
            {
                toDelete.Add(ddr["value"]);
            }
            ArrayList check = new ArrayList();
            for(int i = 0; i < result.Length; i ++)
            {
                check.Add(new stringnid(i, result[i]));
            }
            while(check.Count>0)
            {
                stringnid p = (stringnid)check[0];
                check.Remove(p);
                while (true)
                {
                    if (check.Contains(p))
                    {
                        p = (stringnid)check[check.IndexOf(p)];
                        result[p.id] = "";
                        check.Remove(p);
                    }
                    else break;
                }
            }
            foreach (string st in result)
            {
                if (st != "")
                    if (toDelete.Contains(st))
                    {
                        toDelete.Remove(st);
                    }
                    else
                    {
                        comm = new SQLiteCommand("INSERT INTO " + tnames[этап] + "  ( value ) VALUES ('" + SqliteCommon.realEscapeString(st) + "')", bd_master.conn);
                        comm.ExecuteNonQuery();
                    }
            }
            if (toDelete.Count > 0)
            {
                DialogResult rez = MessageBox.Show("Вы собираетесь удалить несколько строк из этой таблицы. Если есть карточки, в которых задействованы удаляемые данные, то они будут отображаться некорректно. Удаление необратимо.", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (rez == DialogResult.OK)
                {
                    string request = "";
                    int num = 0;
                    foreach (string st in toDelete)
                    {
                        if (num == 0)
                        {
                            request += "'" + st + "'";
                        }
                        else
                        {
                            request += " or value = '" + st + "'";

                        }
                        num++;
                    }
                    comm = new SQLiteCommand("DELETE FROM " + tnames[этап] + " where value = " + request + ";", bd_master.conn);
                    comm.ExecuteNonQuery();
                }
                else
                {
                    textBox1.Text = "";
                    return false;
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
        override public bool workerBack()
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
        private  struct stringnid
        {
            public int id;
            public string value;
            public stringnid(int _id, string _value)
            {
                id = _id;
                value = _value;
            }
            public override bool Equals(object obj)
            {
                if (obj != null && obj.GetType() == typeof(stringnid))
                {
                    if (((stringnid)obj).value == value)
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}