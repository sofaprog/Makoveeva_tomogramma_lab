using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Makoveeva_tomogramma_lab
{
    public partial class Form1 : Form
    {
        private OpenTK.GLControl glControl1;
        enum Mode { Quads, Texture2D, QuadStrip };
        private Mode mode = Mode.Quads;
        private Bin bin;
        private View view;
        private bool loaded = false;
        private int currentLayer;

        private bool needReload = false;
        private DateTime NextFPSUpdate = DateTime.Now.AddSeconds(1);
        private int FrameCount;
        private bool NeedReload = false;

        private int min=0;
        private int width=255;
       
        public Form1()
        {
            InitializeComponent();
            InitializeGLControl();
            InitializeTrackBar();
            Application.Idle += Application_Idle;

        }
        
        private void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                if (loaded)
                {
    
                    FrameCount++;

                    if (DateTime.Now >= NextFPSUpdate)
                    {
                        this.Text = $"Томограмма (FPS: {FrameCount})";
                        NextFPSUpdate = DateTime.Now.AddSeconds(1);
                        FrameCount = 0;
                    }

               
                    glControl1.Invalidate();
                }
            }
        }
        private void InitializeTrackBar()
        {
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 10;
            trackBar1.Value = 0;
            trackBar1.TickFrequency = 1;
            trackBar1.Scroll += trackBar1_Scroll;
        }
        private void InitializeGLControl()
        {
            this.glControl1 = new OpenTK.GLControl();
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(10, 10);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(800, 600);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Load += GlControl1_Load;
            this.glControl1.Paint += GlControl1_Paint;
            this.Controls.Add(this.glControl1);
        }

        private void GlControl1_Load(object sender, EventArgs e)
        {

            GL.ClearColor(Color.Black);
        }

        private void GlControl1_Paint(object sender, PaintEventArgs e)
        {
            if (loaded)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                view.DrawQuads(currentLayer, min, width);

      
                glControl1.SwapBuffers();

                if (NeedReload)
                {
                    NeedReload = false;
                    GL.Finish();
                }
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Idle -= Application_Idle;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            bin = new Bin();
            view = new View();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string str = dialog.FileName;
                bin.readBIN(str);
                view.SetupView(glControl1.Width, glControl1.Height);
                loaded = true;

                trackBar1.Maximum = Bin.Z - 1;
                trackBar1.Value = Bin.Z / 2;
                currentLayer = trackBar1.Value;

                glControl1.Invalidate();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBar1.Value;
            if (loaded)
            {
                glControl1.Invalidate();
            }
        }
    }
}