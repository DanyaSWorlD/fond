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
    public partial class smiStats : UserControl
    {
        int result = 0;
        int tv = 0;
        int rad = 0;
        int prt = 0;
        smiStatsDisplayngModel sdm = smiStatsDisplayngModel.full;

        public smiStats()
        {
            InitializeComponent();
        }

        public void setResultCount(int value)
        {
            result = value;
            update();
        }
        public void setTvCount(int value)
        {
            tv = value;
            update();
        }
        public void setRadCount(int value)
        {
            rad = value;
            update();
        }
        public void setPrtCount(int value)
        {
            prt = value;
            update();
        }
        public enum smiStatsDisplayngModel
        {
            full,
            onlyResult
        }
        private void update()
        {
            if(sdm == smiStatsDisplayngModel.onlyResult)
            {
                label5.Text = result.ToString();
            }
            else
            {
                int[] p = new int[] { result, tv, rad, prt };
                int i = 0;
                foreach(Label l in new Label[] {label5,label6,label7,label8})
                {
                    l.Text = p[i].ToString();
                    i++;
                }
            }
        }
        public void addResultCount(int value)
        {
            result += value;
            update();
        }
        public void addTvCount(int value)
        {
            tv += value;
            update();
        }
        public void addRadCount(int value)
        {
            rad += value;
            update();
        }
        public void addPrtCount(int value)
        {
            prt += value;
            update();
        }
        public void setDisplayMode(smiStatsDisplayngModel _sdm)
        {
            sdm = _sdm;
            if (sdm == smiStatsDisplayngModel.onlyResult)
            {
                Label[] l = new Label[] { label2, label3, label4, label6, label7, label8 };
                for (int i = 0; i < l.Length; i++)
                {
                    l[i].Text = "";
                    l[i] = null;
                }
            }
        }
    }
}
