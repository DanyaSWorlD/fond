using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOND.masterPages
{
    public abstract class MasterPage : UserControl
    {
        abstract public bool workerBack();
        abstract public bool worker();
        virtual public void load()
        {
            //dont need to do anything here, this method can be added to class, or not to be.
        }
    }
}
