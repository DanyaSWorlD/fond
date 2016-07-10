using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;

namespace FOND.Settings_Pages
{
    public partial class console : UserControl
    {
        public console()
        {
            InitializeComponent();
            lastlog lg = new lastlog();
            lg.WasUpdated += new lastlog.ConsoleUpdatedEventHandler(lg_WasUpdated);
            this.Dock = DockStyle.Fill;
            this.textBox1.Text = lastlog.Log;
        }
        private void lg_WasUpdated(Object sender, ConsoleUpdatedArgs e)
        {
            this.textBox1.Text = lastlog.Log;
        }
    }
}
