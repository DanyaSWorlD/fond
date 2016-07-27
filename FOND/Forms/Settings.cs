using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FOND.Forms
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            tableLayoutPanel2.Controls.Add(new Settings_Pages.MainSettings());
            (tableLayoutPanel2.Controls[0] as UserControl).Dock = DockStyle.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tableLayoutPanel2.Controls.Clear();
            tableLayoutPanel2.Controls.Add(new Settings_Pages.MainSettings());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tableLayoutPanel2.Controls.Clear();
            tableLayoutPanel2.Controls.Add(new Settings_Pages.moreSettings());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tableLayoutPanel2.Controls.Clear();
            tableLayoutPanel2.Controls.Add(new Settings_Pages.console());
        }
    }
}
