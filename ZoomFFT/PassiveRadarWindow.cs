using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Windows.Forms;


namespace SDRSharp.Average
{

    public unsafe partial class IFAverageWindow : Form
    {
        public int BufferFrameSize;
        public double* Data;
        public double frequency;
        public int cumulation, cumulation_max;
        public float Gain;
        public float Level;
        public double rate;
        public bool background_recording;
        public bool background_corrected;
        //Recording multiple
        public bool file_recording;
        public int FileNumber;
        public long delay, delayMAX;
        public long recordingTime=0;

        protected int LocationX;
        /////////////////////////////////////////////////////////////////////////////////////

        protected BasicEffect mSimpleEffect;
        private SpriteBatch spriteBatch = null;
        protected SpriteFont spriteFont;
        private Texture2D texture = null;
        private bool resizing = false;
        private Color mBackColor = Color.AliceBlue;

        //fonts
        private ContentManager content;
        private GraphicsDeviceService service;
        private ServiceContainer services;


        //AV window close event
        public  delegate void MyEventHandler(int value);
        public static event MyEventHandler SomethingHappened;

        public IFAverageWindow()
        {

            InitializeComponent();
            
            panelViewport.Width = (int)(this.ClientRectangle.Width);
            panelViewport.Height = (int)(this.ClientRectangle.Height);

            service = new GraphicsDeviceService(this.panelViewport.Handle, this.panelViewport.Width, this.panelViewport.Height);
            service.DeviceResetting += mWinForm_DeviceResetting;
            service.DeviceReset += mWinForm_DeviceReset;

            services = new ServiceContainer();
            services.AddService<IGraphicsDeviceService>(service);
            content = new ContentManager(services, "Content");

        }

        ~IFAverageWindow()
        {
                service.ResetingDevice();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            content.RootDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            spriteBatch = new SpriteBatch(service.GraphicsDevice);
 
            spriteFont = content.Load<SpriteFont>("ft2");
            service.ResetDevice(this.Width, this.Height);
            ScaleXPrepare();
            ScaleYPrepare();
        }

        private void IFAverageWindow_SizeChanged(object sender, EventArgs e)
        {
            panelViewport.Width = (int)(this.ClientRectangle.Width);
            panelViewport.Height = (int)(this.ClientRectangle.Height);

            //if (service.GraphicsDevice != null )
            //{
            //   // if (resizing == false)
            //    {
            //        service.GraphicsDevice.Dispose();
            //        service.CreateDevice(this.panelViewport.Handle, this.panelViewport.Width, this.panelViewport.Height);
            //        service.ResetDevice(this.panelViewport.Width, this.panelViewport.Height);
            //        ScaleXPrepare();
            //    }
            //}
            //resizing = false;
        }

        public void ScaleUpdate()
        {
            ScaleXPrepare();
            ScaleYPrepare();
        }

        private void panelViewport_Paint(object sender, PaintEventArgs e)
        {
            if(!resizing)
            this.Render();
        }

        //Start rander the scene
        public void Render()
        {
             service.GraphicsDevice.Clear(this.mBackColor);

            if (this.service.GraphicsDevice != null)
                this.OnFrameRender();

            service.GraphicsDevice.Present();
        }

        void mWinForm_DeviceReset(Object sender, EventArgs e)
        {
            //Re-create content. Content depends on the graphics device if is ne the content has to be recreated too.

            content = new ContentManager(services, "Content");
            //Set the content rod directory
            content.RootDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            // Re-Create effect
            mSimpleEffect = new BasicEffect(service.GraphicsDevice);
            mSimpleEffect.VertexColorEnabled = true;
            mSimpleEffect.Projection = Matrix.CreateOrthographicOffCenter(0, service.GraphicsDevice.Viewport.Width,     // left, right
            service.GraphicsDevice.Viewport.Height, 0,    // bottom, top
            0, 1); 
            // Configure device
            //service.GraphicsDevice..CullMode = CullMode.None;

            //Recreate bath
            spriteBatch = new SpriteBatch(service.GraphicsDevice);

            //Recreate texture
            if (texture != null) texture.Dispose();
            texture = new Texture2D(service.GraphicsDevice, 1, 1, true, SurfaceFormat.Color);
            texture.SetData<Color>(new Color[] { Color.White });// fill the texture with white
            if (texture == null) MessageBox.Show("texture null.", "Important Message");
            //Recreate spritefont
            spriteFont = content.Load<SpriteFont>("ft2");

        }

        private void mWinForm_DeviceResetting(Object sender, EventArgs e)
        {
            // Dispose all

            if (content != null)
                content.Dispose();

            if (mSimpleEffect != null)
                mSimpleEffect.Dispose();

            if (spriteBatch != null)
                spriteBatch.Dispose();

            if (texture != null)
                texture.Dispose();
        }

        private void PassiveRadarWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("gello");
            SomethingHappened(0);
            service.ResetingDevice();
            
        }

        private void PassiveRadarWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            service.ResetingDevice();
        }

        private void panelViewport_MouseMove(object sender, MouseEventArgs e)
        {
            LocationX = e.Location.X;
        }

        private void panelViewport_Move(object sender, EventArgs e)
        {
            resizing = true;
        }

        private void panelViewport_Resize(object sender, EventArgs e)
        {
            resizing = true;
        }

        private void IFAverageWindow_ClientSizeChanged(object sender, EventArgs e)
        {
            resizing = false;
        }

        private void IFAverageWindow_LocationChanged(object sender, EventArgs e)
        {
            resizing = false;
        }

        private void IFAverageWindow_ResizeBegin(object sender, EventArgs e)
        {
            resizing = true;
        }

        private void IFAverageWindow_ResizeEnd(object sender, EventArgs e)
        {
            PanelSizechanged();
        }


        public void PanelSizechanged()
        {
            panelViewport.Width = (int)(this.ClientRectangle.Width);
            panelViewport.Height = (int)(this.ClientRectangle.Height);

            if (service.GraphicsDevice != null)
            {
                // if (resizing == false)
                {
                    service.GraphicsDevice.Dispose();
                    service.CreateDevice(this.panelViewport.Handle, this.panelViewport.Width, this.panelViewport.Height);
                    service.ResetDevice(this.panelViewport.Width, this.panelViewport.Height);
                    ScaleXPrepare();
                    ScaleYPrepare();
                }
            }
            resizing = false;
        }


        private void panelViewport_MouseLeave(object sender, EventArgs e)
        {
            LocationX = 0;
        }

    }
}
