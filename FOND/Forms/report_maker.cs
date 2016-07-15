﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.Common;

namespace FOND.Forms
{
    public partial class report_maker : Form
    {
        ReportType thisType;
        public report_maker(ReportType rt)
        {
            InitializeComponent();
            thisType = rt;
        }

        private void report_maker_Load(object sender, EventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(string.Format("Data Source = {0};", Properties.Settings.Default.db_file_dir));
            conn.Open();
            string HTMLPage = "<head><style>body{                padding: 25px;                width: 900px;            } #row >             {"+
                "display.table-cell;         } #row{            display: table;            width: 100%;        }.trya{float:left;"+
"width:33%;font-size:small;text-align:center;} .bya{border-left:1px solid black;float:left;width:25%;text-align: center;height: 20px;"+
"} div > .bya:first-child {width:24%;border-left:none;}div.bya:last-of-type > .trya:last-child{width:50%;border-left: 1px solid black;"+
"font-size:medium;}</style></head><body>";
            if (thisType == ReportType.way)
            {
                this.Text = "Отчет о направленности материала";
                try
                {
                    
                    HTMLPage += "<p style=" + '"' + "font-size:big; padding-left:50px;" + '"' + ">Отчет по направленности материала</p><br />";
                    HTMLPage += string.Format("<div><div class={0}bya{0}></div><div class={0}bya{0}>ЦЕНТРАЛЬНЫЕ СМИ</div><div class={0}bya{0}>РЕГИОНАЛЬНЫЕ СМИ</div><div class={0}bya{0}></div></div><div style={0}border-top:1px solid black; border-bottom:2px solid lightblue; {0} id={0}row{0}><div class={0}bya{0}>Направленность материала</div><div class={0}bya{0}><div class={0}trya{0}>телевиденье</div><div class={0}trya{0}>радио</div><div class={0}trya{0}>печать</div></div><div class={0}bya{0}><div class={0}trya{0}>телевиденье</div><div class={0}trya{0}>радио</div><div class={0}trya{0}>печать</div></div><div class={0}bya{0}><div class={0}trya{0}style={0}width: 49% {0}>интернет</div><div class={0}trya{0} >итого</div></div></div>", '"');
                    SQLiteCommand comm = new SQLiteCommand("SELECT id,value FROM materialway;", conn);
                    SQLiteDataReader dr = comm.ExecuteReader();
                    dataSet ds = new dataSet();
                    ds.newRow("");
                    SQLiteCommand comm2 = new SQLiteCommand("SELECT smi.place, smi.type from smi inner join card_in_smi, cards  ON card_in_smi.cards_num = cards.id AND smi.id = card_in_smi.smi AND cards.materialway like '';",conn);
                    SQLiteDataReader dr2 = comm2.ExecuteReader();
                    foreach(DbDataRecord ddr in dr2)
                    {
                        ds.set(ddr);
                    }
                    foreach (DbDataRecord ddr in dr)
                    {
                        ds.newRow(ddr["value"].ToString());
                        comm2 = new SQLiteCommand("SELECT smi.place, smi.type from smi inner join card_in_smi, cards  ON card_in_smi.cards_num = cards.id AND smi.id = card_in_smi.smi AND cards.materialway like '%" + ddr["id"] + "%';",conn);
                        dr2 = comm2.ExecuteReader();
                        foreach(DbDataRecord ddr2 in dr2)
                        {
                            ds.set(ddr2);
                        }
                    }
                    for(int i =0; i < ds.getCount(); i++)
                    {
                        dataSet.row r = ds.get(i);
                        HTMLPage += string.Format("<div style={0}border-top:1px solid black;{0} id={0}row{0}><div class={0}bya{0}>{1}</div><div class={0}bya{0}><div class={0}trya{0}>{2}</div><div class={0}trya{0}>{3}</div><div class={0}trya{0}>{4}</div></div><div class={0}bya{0}><div class={0}trya{0}>{5}</div><div class={0}trya{0}>{6}</div><div class={0}trya{0}>{7}</div></div><div class={0}bya{0}><div class={0}trya{0}style={0}width: 49% {0}>{8}</div><div class={0}trya{0} >{9}</div></div></div>",'"',r.name,r.count[0], r.count[1], r.count[2],r.count[3], r.count[4], r.count[5], r.count[6], r.count[7]);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show( "Ошибка во время создания отчета!\n" + ex.Message,"Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            conn.Close();
            conn.Dispose();
            HTMLPage += "<br />Тестовый пример";
            reportViewer1.setHtml(HTMLPage);
        }
    }
    public enum ReportType
    {
        way,
        executor,
        theme,
        smi,
        concreteness,
        publicPresentation
    }
    public class dataSet
    {
        bool counted;
        ArrayList ar;
        int count,number;
        public dataSet()
        {
            count = -1;
            number = 0;
            ar = new ArrayList();
            counted = false;
        }
        public void newRow(String name)
        {
            if(number>0)
            {
                for(int i = 0; i < 7; i++)
                {
                    ((row)ar[count]).count[7] += ((row)ar[count]).count[i];
                }
            }
            ar.Add(new row(name));
            count++;
            number++;
        }
        public row get(int i)
        {
            if(!counted)
            {
                newRow("Итого");
                for(int p = 0; p < ar.Count - 1; p++)
                {
                    for(int j = 0; j < 8; j ++)
                    {
                        ((row)ar[ar.Count-1]).count[j]+=((row)ar[p]).count[j];
                    }
                }
                counted = true;
            }
            return ((row)ar[i]);
        }
        public int getCount()
        {
            if (!counted)
                return ar.Count + 1;
            return ar.Count;
        }
        public void set(DbDataRecord ddr)
        {
            if (ddr["type"].ToString() != "internet")
            {
                switch (ddr["type"].ToString())
                {
                    case "tv":
                        switch (ddr["place"].ToString())
                        {
                            case "centr":
                                ((row)ar[count]).plus(row.column.centr_tv);
                                break;
                            case "reg":
                                ((row)ar[count]).plus(row.column.reg_tv);
                                break;
                        }
                        break;
                    case "rad":
                        switch (ddr["place"].ToString())
                        {
                            case "centr":
                                ((row)ar[count]).plus(row.column.centr_rad);
                                break;
                            case "reg":
                                ((row)ar[count]).plus(row.column.reg_rad);
                                break;
                        }
                        break;
                    case "gaz":
                        switch (ddr["place"].ToString())
                        {

                            case "centr":
                                ((row)ar[count]).plus(row.column.centr_mag);
                                break;
                            case "reg":
                                ((row)ar[count]).plus(row.column.reg_mag);
                                break;
                        }
                        break;
                }

            }
            else
            {
                ((row)ar[count]).plus(row.column.internet);
            }
        }
        public class row
        {
            public string name;
            public int[] count = new int[8];
            public row(string _name)
            {
                name = _name;
            }
            public void plus(column c)
            {
                count[getInt(c)]++;
            }
            public enum column{
                centr_tv,
                centr_rad,
                centr_mag,
                reg_tv,
                reg_rad,
                reg_mag,
                internet,
            }
            int getInt(column c)
            {
                switch(c)
                {
                    case column.centr_tv: return 0;
                    case column.centr_rad: return 1;
                    case column.centr_mag: return 2;
                    case column.reg_tv: return 3;
                    case column.reg_rad: return 4;
                    case column.reg_mag: return 5;
                    case column.internet: return 6;
                    default: throw new Exception("Not valid input exeption");
                }
            }
        }
    }
}