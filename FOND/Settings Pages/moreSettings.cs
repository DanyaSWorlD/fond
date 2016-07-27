using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOND.Settings_Pages
{
    public partial class moreSettings : UserControl
    {
        public moreSettings()
        {
            InitializeComponent();
        }
        private void label1_MouseEnter(object sender, EventArgs e)
        {
            (sender as Label).ForeColor = Color.Blue;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            (sender as Label).ForeColor = Color.Black;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            ExtraForms.TextEdit te = new ExtraForms.TextEdit();
            te.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Extra.import i = new Extra.import();
           i.ShowDialog();
        }
    }
}
