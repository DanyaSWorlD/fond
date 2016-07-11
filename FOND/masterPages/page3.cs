using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data.Common;
using System.Linq;
using System.Data.SQLite;
using System.Windows.Forms;
using Common;
using FOND.Properties;

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
            if (dr.StepCount > 0)
            {
                comm = new SQLiteCommand("DELETE FROM smi", bd_master.conn);
                comm.ExecuteNonQuery();
            }
            else
            {
                comm = new SQLiteCommand("CREATE TABLE `smi` (`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,`name`	TEXT,`type`	TEXT NOT NULL,`place`	TEXT NOT NULL); ", bd_master.conn);
                comm.ExecuteNonQuery();
            }
            tm = new TextBox[] { TvCentr, TvReg, RadCentr, RadReg, GazCentr, GazReg, internet };
            type = new string[] { "tv", "tv", "rad", "rad", "gaz", "gaz", "internet" };
            place = new string[] { "centr", "reg", "centr", "reg", "centr", "reg", "another" };
            for (var i = 0; i < tm.Length; i++)
            {
                for (var j = 0; j < tm[i].Lines.Length; j++)
                {
                    if (tm[i].Lines[j] != "")
                    {
                        comm = new SQLiteCommand("INSERT INTO smi(name, type, place) VALUES('" + tm[i].Lines[j] + "', '" + type[i] + "','" + place[i] + "')", bd_master.conn);
                        comm.ExecuteNonQuery();
                    }
                }
            }
            return true;

                    }
        override public bool workerBack()
        {
            foreach(TextBox tb in tm)
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
    }
}
