using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
   public class master
    {
        public static bool nextBE { get;private set; }
        public static bool backBE { get;private set; }
        public master()
        {
            backBE = true;
            nextBE = false;
        }
        public void nextButton(bool nextbe)
        {
            nextBE = nextbe;
        }
        public void backbutton(bool backbe)
        {
            backBE = backBE;
        }
    }
}
