using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOND.UserControls
{
    public partial class ReportViewer : UserControl
    {
        public ReportViewer()
        {
            InitializeComponent();
        }
        public void setHtml(string HTML)
        {
            webBrowser1.DocumentText = HTML;
        }

        private void miBut1_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintPreviewDialog();
        }
    }
}
