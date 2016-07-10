using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FOND.Settings_Pages
{
    public partial class MainSettings : UserControl
    {
        public MainSettings()
        {
            InitializeComponent();
            textBox1.Text = Properties.Settings.Default.db_file_dir;
        }

        private void label2_MouseHover(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Blue;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            bd_master bm = new bd_master();
            bm.ShowDialog();
        }
    }
}
