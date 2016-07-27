using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOND.Extra.importPages
{
    public partial class finish : UserControl,import.import_page
    {
        public finish()
        {
            InitializeComponent();
        }

        public void load(import.nextButtonAutoClickHandler deleg)
        {
            Properties.Settings.Default.db_file_dir = import.db_file; 
        }

        public bool nextButtonIsEnabled(import.buttonChangedHandler deleg)
        {
            return false;
        }

        public bool prevButtonIsEnabled(import.buttonChangedHandler deleg)
        {
            return false;
        }

        public bool worker()
        {
            return false;
        }

        public bool workerBack()
        {
            return false;
        }
    }
}
