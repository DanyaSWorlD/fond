using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOND.Extra.importPages
{
    public partial class import_mp : UserControl,import.import_page
    {
        private string[] connection = new string[] {"Microsoft.Jet.OLEDB.4.0","Microsoft.ACE.OLEDB.12.0" };
        delegate void container();
        container c;
        private bool nextButton = false;
        private bool prevButton = true;
        public import_mp()
        {
            InitializeComponent();
        }
        public bool worker()
        {
            string file1 = textBox2.Text + "\\main0.db";
            int p = 0;
            while(File.Exists(file1))
            {
                file1 = file1.Substring(0, file1.Length - 4) + p + ".db";
                p++;
            }
            System.Data.SQLite.SQLiteConnection.CreateFile(file1);
            if (!File.Exists(file1))
            {
                MessageBox.Show("ошибка при создании файла", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!File.Exists(textBox1.Text))
            {
                MessageBox.Show("Файл бд access не  существует", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            try
            {
                import.OleDbconn = new System.Data.OleDb.OleDbConnection(string.Format("Provider={0};Data Source=\"{1}\";", textBox1.Text));
                import.conn = new System.Data.SQLite.SQLiteConnection(string.Format("Data Source = {0};", file1));
                {
                    import.OleDbconn.Open();
                    import.conn.Open();
                    import.db_file = file1;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error!");
                new Common.lastlog().add("import: " + e.Message);
                if (import.OleDbconn.State == ConnectionState.Open)
                     import.OleDbconn.Close();
                if (import.conn.State == ConnectionState.Open)
                    import.conn.Close();
                return false;
            }
            return true;
        }
        public bool workerBack()
        {
            return false;
        }
        public bool nextButtonIsEnabled(import.buttonChangedHandler deleg)
        {
            c = new container(deleg);
            return nextButton;
        }

        public bool prevButtonIsEnabled(import.buttonChangedHandler deleg)       
        {
            c = new container(deleg);
            return prevButton;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox2.Text = folderBrowserDialog1.SelectedPath;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBox1.Text = openFileDialog1.FileName;
        }
        private void datacheck()
        {
            if(File.Exists(textBox1.Text) && textBox2.Text != "")
            {
                nextButton = true;
                c?.Invoke();
            }
            else
            {
                nextButton = false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            datacheck();
        }
        public void load(import.nextButtonAutoClickHandler deleg)
        {

        }
    }
}
