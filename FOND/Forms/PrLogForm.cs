using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Windows.Forms;
using Common;
using System.Configuration;
using System.IO;

namespace FOND
{
    public partial class PrLogForm : Form
    {
        public lastlog lg;
        ConnectionStringSettings vstr;
        public PrLogForm()
        {
            InitializeComponent();
            vstr = ConfigurationManager.ConnectionStrings["version"];
            this.Text = vstr.ConnectionString;
            timer1.Enabled = true;
            lg = new lastlog();
           

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            string cn = "Console :  ";
            lg.add(cn + vstr.ConnectionString);
            lg.add(cn + "Console starting....");
            lg.add(cn + "nearly done....");
            lg.add(cn + "done!");

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           if (lastlog.Nu == true)
            {
                textBox1.Text = lastlog.Log;
                textBox1.SelectionStart = textBox1.Text.Length;
                lastlog.Nu = false;
       }
    }
    }
}