using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Windows.Forms;
using Common;
using FOND.Properties;
using System.Collections;

namespace FOND.masterPages
{
    public partial class page3 : MasterPage
    {
        private TextBox[] tm;
        private string[] type;
        private string[] place;
        private SQLiteCommand comm;
        private master mstr = new master();
        public page3()
        {
            InitializeComponent();
            mstr.nextButton(false);
            tm = new TextBox[] { TvCentr, TvReg, RadCentr, RadReg, GazCentr, GazReg, internet };
        }
        override public bool worker()
        {
            mstr.nextButton(false);
            comm = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type = 'table' AND name  = 'smi'", bd_master.conn);
            SQLiteDataReader dr = comm.ExecuteReader();
            if (dr.StepCount == 0)
            {
                comm = new SQLiteCommand("CREATE TABLE `smi` (`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,`name`	TEXT,`type`	TEXT NOT NULL,`place`	TEXT NOT NULL); ", bd_master.conn);
                comm.ExecuteNonQuery();
            }
            tm = new TextBox[] { TvCentr, TvReg, RadCentr, RadReg, GazCentr, GazReg, internet };
            type = new string[] { "tv", "tv", "rad", "rad", "gaz", "gaz", "internet" };
            place = new string[] { "centr", "reg", "centr", "reg", "centr", "reg", "another" };

            ArrayList toDelete = new ArrayList();
            comm = new SQLiteCommand("SELECT name, type, place FROM smi;", bd_master.conn);
            dr = comm.ExecuteReader();
            foreach (DbDataRecord ddr in dr)
            {
                toDelete.Add(new dbm_item(ddr["name"].ToString(), ddr["type"].ToString(), ddr["place"].ToString()));
            }

            for (var i = 0; i < tm.Length; i++)
            {
                for (var j = 0; j < tm[i].Lines.Length; j++)
                {
                    if (tm[i].Lines[j] != "")
                    {
                        dbm_item p = new dbm_item(tm[i].Lines[j], type[i], place[i]);
                        if(toDelete.Contains(p))
                        {
                            toDelete.Remove(p);
                        }
                        else
                        {
                            comm = new SQLiteCommand("INSERT INTO smi(name, type, place) VALUES('" + SqliteCommon.realEscapeString(p.getName()) + "', '" + SqliteCommon.realEscapeString(p.getType()) + "','" + SqliteCommon.realEscapeString(p.getPlace()) + "')", bd_master.conn);
                            comm.ExecuteNonQuery();
                        }
                    }
                }
            }
            if(toDelete.Count>0)
            {
                DialogResult rez = MessageBox.Show("Вы собираетесь удалить несколько строк из этой таблицы. Если есть карточки, в которых задействованы удаляемые данные, то они будут отображаться некорректно. Удаление необратимо.", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (rez == DialogResult.OK)
                {
                    foreach(dbm_item o in toDelete)
                    {
                        comm = new SQLiteCommand("DELETE FROM smi WHERE name = '" + o.getName() + "' and type = '" + o.getType() + "' and place = '" + o.getPlace() + "';", bd_master.conn);
                        comm.ExecuteNonQuery();
                    }
                }
                else
                {
                    return false;
                }
            }
            foreach (TextBox tb in tm)
            {
                tb.Text = "";
            }
            return true;

        }
        override public bool workerBack()
        {
            foreach (TextBox tb in tm)
            {
                tb.Text = "";
            }
            return true;
        }
        override public void load()
        {
            if (Settings.Default.db_file_dir != "" && Settings.Default.db_file_dir != null)
            {
                comm = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type = 'table' AND name  = 'smi'", bd_master.conn);
                SQLiteDataReader dr = comm.ExecuteReader();
                if (dr.StepCount > 0)
                {
                    comm = new SQLiteCommand("SELECT * FROM smi", bd_master.conn);
                    SQLiteDataReader dr2 = comm.ExecuteReader();
                    foreach (DbDataRecord record in dr2)
                    {
                        switch (record["type"].ToString())
                        {
                            case "tv":
                                if (record["place"] + "" == "reg")
                                {
                                    TvReg.Text += record["name"] + "\r\n";
                                }
                                else
                                {
                                    TvCentr.Text += record["name"] + "\r\n";
                                }
                                break;
                            case "rad":
                                if (record["place"] + "" == "reg")
                                {
                                    RadReg.Text += record["name"] + "\r\n";
                                }
                                else
                                {
                                    RadCentr.Text += record["name"] + "\r\n";
                                }
                                break;
                            case "gaz":
                                if (record["place"] + "" == "reg")
                                {
                                    GazReg.Text += record["name"] + "\r\n";
                                }
                                else
                                {
                                    GazCentr.Text += record["name"] + "\r\n";
                                }
                                break;
                            case "internet":
                                internet.Text += record["name"] + "\r\n";
                                break;
                        }
                    }
                }
            }
        }

        private void TvCentr_TextChanged(object sender, EventArgs e)
        {
            if (TvCentr.Text != "" && TvReg.Text != "" && GazCentr.Text != "" && GazReg.Text != "" && RadCentr.Text != "" && RadReg.Text != "" && internet.Text != "")
            {
                mstr.nextButton(true);
            }
            else
            {
                mstr.nextButton(false);
            }
        }
        private class dbm_item
        {
            private string name;
            private string type;
            private string place;
            public dbm_item(string _name, string _type, string _place)
            {
                name = _name;
                type = _type;
                place = _place;
            }
            public string getName()
            {
                return name;
            }
            public string getType()
            {
                return type;
            }
            public string getPlace()
            {
                return place;
            }
            public override bool Equals(object obj)
            {
                if (obj.GetType() != typeof(dbm_item) || obj == null)
                {
                    return false;
                }
                else
                {
                    dbm_item eq = obj as dbm_item;
                    if (name == eq.getName() && place == eq.getPlace() && type == getType())
                    {
                        return true;
                    }
                    else return false;
                }
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}
