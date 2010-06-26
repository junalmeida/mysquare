using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MySquare.UI
{
    public partial class AnimatedGif : Control
    {
        public AnimatedGif()
        {
            InitializeComponent();
        }

        public Image Image
        { get; set; }

        public int FrameCount
        { get; set; }

        public int LoopCount
        { get; set; }


        public int Interval
        {
            get { return timer1.Interval; }
            set { timer1.Interval = value; }
        }
            
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            timer1.Enabled = this.Enabled;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Visible)
                timer1.Enabled = false;
            if (LoopCount == -1) //loop continuously
            {
                DrawFrame();
            }
            else
            {
                if (LoopCount == loopCounter) //stop the animation
                    this.Enabled = false;
                else
                    DrawFrame();
            }
        }

        int frameWidth;
        int currentFrame = 0;
        int loopCounter = 0;
        private void DrawFrame()
        {
            if (currentFrame < FrameCount - 1)
            {
                //move to the next frame
                currentFrame++;
            }
            else
            {
                //increment the loopCounter
                loopCounter++;
                currentFrame = 0;
            }
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (Image != null)
            {
                int frameWidth = Image.Width / FrameCount;
                //Calculate the left location of the drawing frame
                int xLocation = currentFrame * frameWidth;

                Rectangle rect = new Rectangle(xLocation, 0, frameWidth,
                  Image.Height);

                //Draw image
                pe.Graphics.DrawImage(Image, 0, 0, rect, GraphicsUnit.Pixel);

                if(!timer1.Enabled)
                    timer1.Enabled = this.Enabled;
            }
        }

    }
}
