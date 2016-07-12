using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOND.UserControls
{
    public partial class miBut : Button
    {
        [Browsable(true)]
        public override Image BackgroundImage { get; set; }
        [Browsable(true)]
        public int ImagePaddingHorisontal { get; set; }
        [Browsable(true)]
        public int ImagePaddingVertical { get; set; }
        public miBut()
        {
            InitializeComponent();
            OnCreate();
        }

        public miBut(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            OnCreate();
        }
        protected void OnCreate()
        {
            FlatAppearance.MouseDownBackColor = Color.Gray;
            FlatAppearance.MouseOverBackColor = Color.LightBlue;
            FlatAppearance.BorderSize = 0;
            FlatStyle = FlatStyle.Flat;
            Text = "";
            Dock = DockStyle.Fill;
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            try
            {
                if(Image == null)
                Image = new Bitmap(BackgroundImage, new Size(ClientRectangle.Width - ImagePaddingHorisontal - FlatAppearance.BorderSize, ClientRectangle.Height - ImagePaddingVertical - FlatAppearance.BorderSize));
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            base.OnPaint(pevent);
        }

        protected override void OnBackgroundImageChanged(EventArgs e)
        {
            base.OnBackgroundImageChanged(e);
            try
            {
                Image = new Bitmap(BackgroundImage, new Size(ClientRectangle.Width - Padding.Horizontal - FlatAppearance.BorderSize, ClientRectangle.Height - Padding.Vertical - FlatAppearance.BorderSize));
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
