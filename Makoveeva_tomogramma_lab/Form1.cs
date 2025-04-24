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
        private DateTime NextFPSUpdate = DateTime.Now.AddSeconds(1);
        private int FrameCount;
        private bool needReload = false;


        private int min;
        private int width;

        public Form1()
        {
            InitializeComponent();
            InitializeGLControl();
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
            // Инициализация OpenGL
            GL.ClearColor(Color.Black);
        }

        private void GlControl1_Paint(object sender, PaintEventArgs e)
        {
            if (loaded)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                view.DrawQuads(currentLayer,0,255);
                glControl1.SwapBuffers();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bin = new Bin();
            view = new View();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // Обработчик клика по кнопке на ToolStrip
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
                glControl1.Invalidate();
            }
        }
    }
}