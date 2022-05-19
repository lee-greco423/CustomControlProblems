using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Drawing.Drawing2D;

namespace WindowsFormsControlLibrary1
{
    [DefaultEvent("Click")]
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            this.SetStyle(
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint |
              ControlStyles.DoubleBuffer, true);
        }

        protected ColorPair backColorPair = new ColorPair();
        [Browsable(true)]
        [Category("Appearance")]
        [Description("BackColor pair of the control")]
        [Editor(typeof(ColorPairEditor), typeof(UITypeEditor))]
        [DefaultValue(typeof(ColorPair), "Blue; White")]
        public ColorPair BackColorPair
        {
            get { return this.backColorPair; }
            set
            {
                this.backColorPair = value;
                Invalidate();
            }
        }

        protected float gradientAngle = 0F;
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Gradient angle for the BackColor of the control")]
        [DefaultValue(0F)]
        public float GradientAngle
        {
            get { return this.gradientAngle; }
            set
            {
                this.gradientAngle = value;
                Invalidate();
            }
        }

        // use DoubleBuffer
        private Bitmap backBuffer;
        protected override void OnPaint(PaintEventArgs e)
        {
            if(this.backBuffer == null)
            {
                backBuffer = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
            }
            GraphicsUnit gUnits = GraphicsUnit.Pixel;
            Rectangle r = Rectangle.Round(backBuffer.GetBounds(ref gUnits));
            using (Graphics g = Graphics.FromImage(backBuffer))
            {
                using (var b = new LinearGradientBrush(r, this.backColorPair.ColorA, this.backColorPair.ColorB, this.gradientAngle))
                {
                    g.FillRectangle(b, r);
                }
            }
            e.Graphics.DrawImageUnscaled(backBuffer, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // nope
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if(this.backBuffer != null)
            {
                this.backBuffer.Dispose();
                this.backBuffer = null;
            }
            base.OnSizeChanged(e);
        }
    }
}
