using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOND.ExtraForms
{
    public partial class datePicker : Form,Eform
    {
        private DialogResult d = DialogResult.Abort;
        public datePicker()
        {
            InitializeComponent();
            dateTimePicker1.Value = DateTime.Now.AddYears(-1);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ((Forms.report_maker)this.Owner).continueWork(dateTimePicker1.Value, dateTimePicker2.Value, "");
            d = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void datePicker_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = d;
        }
    }
}
