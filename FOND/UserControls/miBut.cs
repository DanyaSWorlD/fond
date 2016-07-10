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
                Image = new Bitmap(BackgroundImage, new Size(ClientRectangle.Width - Padding.Horizontal, ClientRectangle.Height - Padding.Vertical));
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            base.OnPaint(pevent);
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            FlatAppearance.BorderColor = Color.LightBlue;
            FlatAppearance.BorderSize +=1;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            
            FlatAppearance.BorderColor = Color.Gray;
            FlatAppearance.BorderSize -=1;
        }
        protected override void OnBackgroundImageChanged(EventArgs e)
        {
            base.OnBackgroundImageChanged(e);
            try
            {
                Image = new Bitmap(BackgroundImage, new Size(ClientRectangle.Width - Padding.Horizontal, ClientRectangle.Height - Padding.Vertical));
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
