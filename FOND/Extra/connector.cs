using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOND
{
    public class connector
    {
        public delegate void bd_formClosedEventHandler();
        public event bd_formClosedEventHandler bd_formClosed;
        private static connector eto;
        static object locker = new object();
        private bd_master bm;
        public bool appRunning { get; set; }
        private connector()
        {

        }
        private connector(bd_master b)
        {
            bm = b;
            b.bd_masterClose += new bd_master.bd_masterCloseHandler(Continue);
        }
        private static connector getInstance(bd_master b)
        {
            lock (locker)
            {
                if (eto == null)
                {
                    eto = new connector(b);
                }
                return eto;
            }
        }
        public static connector getInstance()
        {
            lock (locker)
            {
                if (eto == null)
                {
                    eto = new connector();
                }
                return eto;
            }
        }
        public static connector getInitializedWithBd_masterInstance(bd_master b)
        {
            connector bI = getInstance(b);
            if(bI.bm == null)
            {
                bI.bm = b;
                b.bd_masterClose += new bd_master.bd_masterCloseHandler(bI.Continue);
            }
            return bI;
        }
        public void Continue()
        {
            try
            {
                bd_formClosed();
            }
            catch(Exception e)
            {
                new Common.lastlog().add("connector: "+e.Message);
            }
        }
        
    }
}