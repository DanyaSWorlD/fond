using System;
using System.Collections.Generic;
using Common;
using System.Drawing;
using System.Data.SQLite;
using System.Data.Common;
using System.Text;
using System.Windows.Forms;

namespace FOND.masterPages
{
    
    public partial class pageend : UserControl
    {
        private SQLiteCommand comm;
        private lastlog lg = new lastlog();
        public pageend()
        {
            InitializeComponent();
        }
       public bool workerback()
    {
        return true;
    }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }
        public bool enderer()
        {
            comm = new SQLiteCommand("SELECT * FROM sqlite_master WHERE name = 'cards' and type = 'table'",bd_master.conn);
            DbDataReader dr = comm.ExecuteReader();
            var i = 0;
            foreach(DbDataRecord rec in dr)
            {
                i++;
            }
            if(i == 0)
            {
                comm = new SQLiteCommand("CREATE TABLE `cards` (`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,`workers`	TEXT,`materialway`	TEXT,`material_theme`	TEXT,`material_presentation`	TEXT,`quality`	TEXT,`regions`	TEXT,`parts`	TEXT,`discription`	TEXT,`type`	TEXT,`speakerlvl`	TEXT,`curspeaker`	TEXT,`fio`	TEXT,`link`	TEXT,`date`	TEXT)", bd_master.conn);
                comm.ExecuteNonQuery();
                return true;
            }
            comm = new SQLiteCommand("SELECT * FROM sqlite_master WHERE name = 'card_in_smi' and type = 'table'", bd_master.conn);
            dr = comm.ExecuteReader();
            i = 0;
            foreach (DbDataRecord rec in dr)
            {
                i++;
            }
            if (i == 0)
            {
                comm = new SQLiteCommand("CREATE TABLE `card_in_smi` (`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,`cards_num`	INTEGER,`smi`	TEXT,`date`	TEXT,`times`	TEXT,`link`	TEXT,`pu` TEXT); ",bd_master.conn);
                comm.ExecuteNonQuery();
                return true;
            }
            return true;
        }
    }
    
}
