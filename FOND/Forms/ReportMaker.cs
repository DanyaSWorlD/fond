using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOND.Forms
{
    public partial class ReportMaker : Form
    {
        public ReportMaker()
        {
            InitializeComponent();
        }

        private void ReportMaker_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
    }
}
