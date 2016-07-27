using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FOND.Extra.importPages;

namespace FOND.Extra
{
    public partial class import : Form
    {
        public static string db_file;
        public delegate void buttonChangedHandler();
        public delegate void nextButtonAutoClickHandler();
        private int currItemId = 0;
        import_page[] c;
        public static SQLiteConnection conn;
        public static OleDbConnection OleDbconn;
        public import()
        {
            InitializeComponent();
            c = new import_page[] { new import_mp() ,new import_insert(), new finish()};
            panel1.Controls.Add((UserControl)c[currItemId]);
            buttons();
        }
        private void buttons()
        {
            if(currItemId == 0)
            {
                buttonBack.Enabled = false;
            }
            else
            {
                buttonBack.Enabled = c[currItemId].prevButtonIsEnabled(new buttonChangedHandler(buttons_update));
            }
            if(currItemId+1>=c.Length)
            {
                buttonNext.Enabled = false;
                buttonCancel.Text = "готово";
            }
            else
            {
                buttonCancel.Text = "отмена";
                buttonNext.Enabled = c[currItemId].nextButtonIsEnabled(new buttonChangedHandler(buttons_update));
            }
        }
        public void buttons_update()
        {
            this.Invoke(new buttonChangedHandler(delegate { buttons(); }));          
        }
        private void buttonBack_Click(object sender, EventArgs e)
        {
            bool coontinue = c[currItemId].workerBack();
            if (coontinue)
                controlChanger(-1);
            buttons();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            bool coontinue = c[currItemId].worker();
            if (coontinue)
                controlChanger(1);
            buttons();
        }
        private void controlChanger(int changeTo)
        {
            currItemId += changeTo;
            panel1.Controls.Clear();
            panel1.Controls.Add((UserControl)c[currItemId]);
            ((UserControl)c[currItemId]).Dock = DockStyle.Fill;
            c[currItemId].load(new nextButtonAutoClickHandler(nextButtonAutoClick));
        }
        private void import_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(buttonCancel.Text == "отмена")
            {
                var d = MessageBox.Show("Все не сохраненные данные будут удалены! Продолжить?", "внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                if(d == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            if(OleDbconn != null)
            {
                if (OleDbconn.State == System.Data.ConnectionState.Open)
                {
                    OleDbconn.Close();
                }
                OleDbconn.Dispose();
                OleDbconn = null;
            }
            if(conn != null)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
                conn = null;
            }
        }
        private void nextButtonAutoClick()
        {
            this.Invoke(new buttonChangedHandler(delegate { buttonNext_Click(null, null); }));           
        }
        public interface import_page
        {
            bool worker();
            bool workerBack();
            bool nextButtonIsEnabled(buttonChangedHandler deleg);
            bool prevButtonIsEnabled(buttonChangedHandler deleg);
            void load(nextButtonAutoClickHandler deleg);
        }
    }
}
