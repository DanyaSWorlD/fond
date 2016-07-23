using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOND
{
    public class bd_master_lissener
    {
        public delegate void bd_formClosedEventHandler();
        public event bd_formClosedEventHandler bd_formClosed;
        private static bd_master_lissener eto;
        static object locker = new object();
        private bd_master bm;
        private bd_master_lissener(bd_master b)
        {
            bm = b;
            b.bd_masterClose += new bd_master.bd_masterCloseHandler(Continue);
        }
        public static bd_master_lissener getInstance(bd_master b)
        {
            lock (locker)
            {
                if (eto == null)
                {
                    eto = new bd_master_lissener(b);
                }
                return eto;
            }
        }
        public static bd_master_lissener getInstance()
        {
            if (eto != null)
                return eto;
            return null;
        }
        public void Continue()
        {
            bd_formClosed();
        }
    }
}