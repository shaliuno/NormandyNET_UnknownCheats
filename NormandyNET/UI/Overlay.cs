using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Factory = SharpDX.Direct2D1.Factory;
using FontFactory = SharpDX.DirectWrite.Factory;
using Numerics = System.Numerics;

namespace NormandyNET
{
    public partial class Overlay : Form
    {
        #region Public Fields

        public static bool ingame = false;
        public bool stopRequested = false;
        public bool overlayLoaded = false;

        #endregion Public Fields

        #region Internal Fields

        internal readonly float heightForCrouch = 1.2f;
        internal readonly float heightForProne = 0.3f;
        internal readonly float heightForStand = 1.6f;
        internal readonly float playerFeetVector = 0.0f;
        internal bool canRun = false;
        internal float fixedHeadCircleSize = 0.30f;
        internal int geometryPixelSize = 2;
        internal float heightToHead = 0;
        internal bool modulesDone = false;

        internal Thread renderThread;

        internal WindowStyleEnum windowStyle;

        #endregion Internal Fields

        #region Private Fields

        private const int
           HTLEFT = 10,
           HTRIGHT = 11,
           HTTOP = 12,
           HTTOPLEFT = 13,
           HTTOPRIGHT = 14,
           HTBOTTOM = 15,
           HTBOTTOMLEFT = 16,
           HTBOTTOMRIGHT = 17;

        private const int windowGripSizeBorder = 10;
        private static WindowRenderTarget device;
        private float aspectRatio = 1.777f;
        private float coeffResolutionTo = 1f;
        private bool drag = false;
        private Factory factory = new Factory();

        private FontFactory fontFactory = new FontFactory();

        private bool formResizeInProgress;
        private int geometryPixelSizeCircle = 2;
        private IntPtr handle;
        private Margins marg;
        private HwndRenderTargetProperties renderProperties;
        private Point startPoint = new Point(0, 0);
        private NativeMethods.Rectangle targetWindowRect;
        private bool updateOverlayRequested;
        private float[] viewMatrix = new float[16];
        private System.Timers.Timer windowFollower;
        private IntPtr windowsSourceHandle;

        #endregion Private Fields

        #region Public Constructors

        public Overlay()
        {
            InitializeComponent();
            Width = 711;
            Height = 400;
            handle = Handle;

            this.MouseDown += new MouseEventHandler(Title_MouseDown);
            this.MouseUp += new MouseEventHandler(Title_MouseUp);
            this.MouseMove += new MouseEventHandler(Title_MouseMove);
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler OnPrepareOverlayRenderObjectsEvent;

        #endregion Public Events

        #region Private Properties

        private Rectangle Bottom
        { get { return new Rectangle(0, this.ClientSize.Height - windowGripSizeBorder, this.ClientSize.Width, windowGripSizeBorder); } }

        private Rectangle BottomLeft
        { get { return new Rectangle(0, this.ClientSize.Height - windowGripSizeBorder, windowGripSizeBorder, windowGripSizeBorder); } }

        private Rectangle BottomRight
        { get { return new Rectangle(this.ClientSize.Width - windowGripSizeBorder, this.ClientSize.Height - windowGripSizeBorder, windowGripSizeBorder, windowGripSizeBorder); } }

        private Rectangle Left
        { get { return new Rectangle(0, 0, windowGripSizeBorder, this.ClientSize.Height); } }

        private Rectangle Right
        { get { return new Rectangle(this.ClientSize.Width - windowGripSizeBorder, 0, windowGripSizeBorder, this.ClientSize.Height); } }

        private Rectangle Top
        { get { return new Rectangle(0, 0, this.ClientSize.Width, windowGripSizeBorder); } }

        private Rectangle TopLeft
        { get { return new Rectangle(0, 0, windowGripSizeBorder, windowGripSizeBorder); } }

        private Rectangle TopRight
        { get { return new Rectangle(this.ClientSize.Width - windowGripSizeBorder, 0, windowGripSizeBorder, windowGripSizeBorder); } }

        #endregion Private Properties

        #region Public Methods

        public void DirectXThread()
        {
            do
            {
                DoRender();
            } while (!stopRequested);
        }

        private void DoRender()
        {
            if (device == null)
            {
                Thread.Sleep(125);
            }

            if (!canRun)
            {
                Console.WriteLine("nothing to do sleeping");
                Thread.Sleep(1000);
            }

            if (canRun && !formResizeInProgress && device != null)
            {
                try
                {
                    device.BeginDraw();
                    device.Clear(SharpDX.Color.Transparent);
                    device.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Aliased;

                    OnPrepareOverlayRenderObjectsEvent?.Invoke(this, null);
                    do
                    {
                        CommonHelpers.NOP(1000);
                    } while (modulesDone != true);

                    modulesDone = false;

                    RenderObjects();

                    device.Flush();
                    device.EndDraw();
                }
                catch
                {
                    Console.WriteLine($"smth wrong");
                    Thread.Sleep(50);

                    try
                    {
                        device.Flush();
                        device.EndDraw();
                    }
                    catch
                    {
                        Thread.Sleep(50);
                    }
                }
            }
            else
            {
                Thread.Sleep(50);
            }
        }

        public void Init()
        {
            Width = 711;
            Height = 400;
            canRun = true;
            LoadOverlay();
            stopRequested = false;

            renderThread = new Thread(DirectXThread);
            renderThread.Start();
        }

        #endregion Public Methods

        #region Internal Methods

        internal void UpdateAspectRatio(int Width, int Height)
        {
            var newAspectRatio = ((float)Width / (float)Height);
            aspectRatio = newAspectRatio;

            if (windowStyle == WindowStyleEnum.FullScreen)
            {
                DoFullScreen();
            }
            else
            {
                Height = (int)Math.Round(Width / aspectRatio, 0);
            }
        }

        internal bool WorldToScreen(OpenTK.Vector3 enemy, out OpenTK.Vector3 screen, System.Numerics.Matrix4x4? matrix, int width, int height)
        {
            screen = new OpenTK.Vector3(0, 0, 0);
            Numerics.Matrix4x4 temp = Numerics.Matrix4x4.Transpose((Numerics.Matrix4x4)matrix);

            OpenTK.Vector3 translationVector = new OpenTK.Vector3(temp.M41, temp.M42, temp.M43);
            OpenTK.Vector3 up = new OpenTK.Vector3(temp.M21, temp.M22, temp.M23);
            OpenTK.Vector3 right = new OpenTK.Vector3(temp.M11, temp.M12, temp.M13);

            float w = D3DXVec3Dot(translationVector, enemy) + temp.M44;

            if (w < 0.098f)
            {
                return false;
            }

            float y = D3DXVec3Dot(up, enemy) + temp.M24;
            float x = D3DXVec3Dot(right, enemy) + temp.M14;

            screen.X = (((Width / 2) * (1f + (x / w))) - (Width / 2)) * coeffResolutionTo;
            screen.Y = (((Height / 2) * (1f - (y / w))) - (Height / 2)) * coeffResolutionTo;
            screen.Z = w;
            return true;
        }

        internal bool WorldToScreenRUST(OpenTK.Vector3 enemy, out OpenTK.Vector3 screen, System.Numerics.Matrix4x4? matrix, int width, int height)
        {
            screen = new OpenTK.Vector3(0, 0, 0);
            Numerics.Matrix4x4 pViewMatrix = (Numerics.Matrix4x4)matrix;
  
            OpenTK.Vector3 TransVec = new OpenTK.Vector3(pViewMatrix.M14, pViewMatrix.M24, pViewMatrix.M34);
            OpenTK.Vector3 UpVec = new OpenTK.Vector3(pViewMatrix.M12, pViewMatrix.M22, pViewMatrix.M32);
            OpenTK.Vector3 RightVec = new OpenTK.Vector3(pViewMatrix.M11, pViewMatrix.M21, pViewMatrix.M31);
            float w = D3DXVec3Dot(TransVec, enemy) + pViewMatrix.M44;

            if (w < 0.098f)
            {
                return false;
            }

            float y = D3DXVec3Dot(UpVec, enemy) + pViewMatrix.M42;
            float x = D3DXVec3Dot(RightVec, enemy) + pViewMatrix.M41;

            screen.X = (((Width / 2) * (1f + (x / w))) - (Width / 2)) * coeffResolutionTo;
            screen.Y = (((Height / 2) * (1f - (y / w))) - (Height / 2)) * coeffResolutionTo;
            screen.Z = w;

            return true;
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            canRun = false;
            stopRequested = true;
            while (renderThread.IsAlive)
            {
                Console.WriteLine("Overlay: waiting thread stop");
                CommonHelpers.NOP(10000);
            }
            this.Hide();
            e.Cancel = true;
        }

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);
            if (message.Msg == 0x84)
            {
                var cursor = this.PointToClient(Cursor.Position);

                if (TopLeft.Contains(cursor)) message.Result = (IntPtr)HTTOPLEFT;
                else if (TopRight.Contains(cursor)) message.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeft.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRight.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;
                else if (Top.Contains(cursor)) message.Result = (IntPtr)HTTOP;
                else if (Left.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                else if (Right.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                else if (Bottom.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        [DllImport("dwmapi.dll")]
        private static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Margins margins);

        private void ClosedOverlay(object sender, FormClosingEventArgs e)
        {
            try
            {
                device.Flush();
                device.EndDraw();
                device.Dispose();
                device = null;
                windowFollower.Stop();
                windowFollower.Dispose();
            }
            catch
            {
            }
        }

        private float D3DXVec3Dot(OpenTK.Vector3 a, OpenTK.Vector3 b)
        {
            return (a.X * b.X) +
                   (a.Y * b.Y) +
                   (a.Z * b.Z);
        }

        private void DoFullScreen()
        {
            var screenWidth = Screen.PrimaryScreen.Bounds.Width;
            var screenHeight = Screen.PrimaryScreen.Bounds.Height;
            Location = new Point(0, 0);

            int w, h;

            h = (int)Math.Round(screenWidth / aspectRatio, 0);
            w = (int)Math.Round(screenHeight * aspectRatio, 0);

            if (h > screenHeight)
            {
                h = screenHeight;
                w = (int)Math.Round(screenHeight * aspectRatio, 0);
            }

            if (w > screenWidth)
            {
                w = screenWidth;
                h = (int)Math.Round(screenWidth / aspectRatio, 0);
            }

            Width = w;
            Height = h;

            BringToFront();
        }

        private NativeMethods.Rectangle GetTargetProcessWindowData()
        {
            if (windowsSourceHandle == IntPtr.Zero)
            {
                if (windowStyle == WindowStyleEnum.OBS)
                {
                    windowsSourceHandle = NativeMethods.FindWindowA(null, "Windowed Projector (Preview)");
                }

                if (windowStyle == WindowStyleEnum.Moonlight)
                {
                    windowsSourceHandle = NativeMethods.FindWindowA(null, "Moonlight");
                }
            }

            NativeMethods.GetWindowRect(windowsSourceHandle, out NativeMethods.Rectangle captureAreaTemp);

            NativeMethods.ClientToScreen(windowsSourceHandle, out NativeMethods.Point pnt);

            var windowMetricsCorrectionX = (pnt.x - captureAreaTemp.left) * 2;

            var windowMetricsCorrectionY = (pnt.y - captureAreaTemp.top) + (pnt.x - captureAreaTemp.left);

            var rect = new NativeMethods.Rectangle
            {
                left = captureAreaTemp.left + (pnt.x - captureAreaTemp.left),
                right = (captureAreaTemp.right - captureAreaTemp.left) - windowMetricsCorrectionX,
                top = captureAreaTemp.top + (pnt.y - captureAreaTemp.top),
                bottom = (captureAreaTemp.bottom - captureAreaTemp.top) - windowMetricsCorrectionY
            };

            return rect;
        }

        private IntPtr GetWindowHandle(string windowName)
        {
            IntPtr hwnd = IntPtr.Zero;
            foreach (Process process in Process.GetProcesses())
            {
                Console.WriteLine(process.ProcessName);

                if (process.ProcessName.ToLower().Contains(windowName))
                {
                    hwnd = process.MainWindowHandle;
                }
            }

            return hwnd;
        }

        private void KeepWindowInPlace(object source, ElapsedEventArgs e)
        {
            if (windowStyle == WindowStyleEnum.OBS || windowStyle == WindowStyleEnum.Moonlight)
            {
                try
                {
                    targetWindowRect = GetTargetProcessWindowData();
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        Location = new Point(targetWindowRect.left, targetWindowRect.top);
                    });
                    if (Width != targetWindowRect.right || Height != targetWindowRect.bottom)
                    {
                        updateOverlayRequested = true;
                    }
                }
                catch
                {
                }
            }

            if (formResizeInProgress == false && updateOverlayRequested)
            {
                try
                {
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        LoadOverlay();
                    });
                }
                catch
                {
                }
            }
            windowFollower.Start();
        }

        private void KeepWindowInPlace()
        {
            if (windowStyle == WindowStyleEnum.OBS || windowStyle == WindowStyleEnum.Moonlight)
            {
                targetWindowRect = GetTargetProcessWindowData();

                Location = new Point(targetWindowRect.left, targetWindowRect.top);
                Width = targetWindowRect.right;
                Height = targetWindowRect.bottom;
            }
        }

        private void LoadOverlay()
        {
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TransparencyKey = Color.Black;

            updateOverlayRequested = false;

            DoubleBuffered = true;
            SetStyle(
              ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
              ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor,
              true);

            KeepWindowInPlace();

            if (windowStyle == WindowStyleEnum.FullScreen)
            {
                DoFullScreen();
            }

            if (windowStyle == WindowStyleEnum.Standalone)
            {
                this.ShowInTaskbar = true;

                this.TransparencyKey = Color.Beige;
            }

            factory = new Factory();
            renderProperties = new HwndRenderTargetProperties
            {
                Hwnd = Handle,
                PixelSize = new SharpDX.Size2(Size.Width, Size.Height),
                PresentOptions = PresentOptions.None
            };

            marg.Left = 0;
            marg.Top = 0;
            marg.Right = Width;
            marg.Bottom = Height;

            if (windowStyle != WindowStyleEnum.Standalone)
            {
                DwmExtendFrameIntoClientArea(Handle, ref marg);
            }

            device = new WindowRenderTarget(factory, new RenderTargetProperties(new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)), renderProperties)
            {
                AntialiasMode = AntialiasMode.Aliased,
                TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Default
            };

            overlayLoaded = true;

            windowFollower = new System.Timers.Timer
            {
                Interval = 3000
            };

            windowFollower.Elapsed += KeepWindowInPlace;
            windowFollower.AutoReset = false;
            windowFollower.Enabled = true;
            windowFollower.Stop();
            windowFollower.Start();
        }

        private void Overlay_Resize(object sender, EventArgs e)
        {
            if (windowStyle == WindowStyleEnum.Standalone)
            {
                Height = (int)Math.Round(Width / aspectRatio, 0);
            }
        }

        private void Overlay_ResizeBegin(object sender, EventArgs e)
        {
            formResizeInProgress = true;
        }

        private void Overlay_ResizeEnd(object sender, EventArgs e)
        {
            formResizeInProgress = false;
            updateOverlayRequested = true;
        }

        private void OverlayLoaded(object sender, EventArgs e)
        {
            LoadOverlay();
        }

        private void RenderObjects()
        {
            var screenWidthOffset = Width / 2;
            var screenHeightOffset = Height / 2;

            OpenGL.OverlayGeometry.ForEach(u =>
            {
                switch (u.Text)
                {
                    case "line":
                        device.DrawLine(new RawVector2(u.MapPosX + screenWidthOffset, u.MapPosZ + screenHeightOffset), new RawVector2(u.MapPosXend + screenWidthOffset, u.MapPosZend + screenHeightOffset), new SolidColorBrush(device, new RawColor4((float)u.DrawColor.R / 256, (float)u.DrawColor.G / 256, (float)u.DrawColor.B / 256, (float)u.DrawColor.A / 256)), u.Size);
                        break;

                    case "point":

                        break;

                    case "circle":
                        device.DrawEllipse(new Ellipse(new RawVector2(u.MapPosX + screenWidthOffset, u.MapPosZ + screenHeightOffset), u.Size, u.Size), new SolidColorBrush(device, new RawColor4((float)u.DrawColor.R / 256, (float)u.DrawColor.G / 256, (float)u.DrawColor.B / 256, (float)u.DrawColor.A / 256)), geometryPixelSizeCircle);
                        break;

                    case "rectangle":
                        device.DrawRectangle(new RawRectangleF(u.MapPosX + screenWidthOffset, u.MapPosZ + screenHeightOffset, u.MapPosXend + screenWidthOffset, u.MapPosZend + screenHeightOffset), new SolidColorBrush(device, new RawColor4((float)u.DrawColor.R / 256, (float)u.DrawColor.G / 256, (float)u.DrawColor.B / 256, (float)u.DrawColor.A / 256)), u.Size);
                        break;

                    default:
                        break;
                }
            });

            float fontSize = 13;
            string fontFamily = "Arial Unicode MS";
            OpenGL.OverlayText.ForEach(u =>
            {
                Size measure = System.Windows.Forms.TextRenderer.MeasureText(u.Text, new System.Drawing.Font(fontFamily, fontSize - 3));
                if (u.TextOverlayOutline)
                {
                    var darkerColor = System.Windows.Forms.ControlPaint.Dark(u.DrawColor);
                    device.DrawText(u.Text, new TextFormat(fontFactory, fontFamily, FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, fontSize), new RawRectangleF(u.MapPosX + screenWidthOffset - 1, u.MapPosZ + screenHeightOffset - 1, u.MapPosX + measure.Width + screenWidthOffset, u.MapPosZ + measure.Height + screenHeightOffset), new SolidColorBrush(device, new RawColor4((float)darkerColor.R / 256, (float)darkerColor.G / 256, (float)darkerColor.B / 256, (float)darkerColor.A / 256)));
                    device.DrawText(u.Text, new TextFormat(fontFactory, fontFamily, FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, fontSize), new RawRectangleF(u.MapPosX + screenWidthOffset + 1, u.MapPosZ + screenHeightOffset + 1, u.MapPosX + measure.Width + screenWidthOffset, u.MapPosZ + measure.Height + screenHeightOffset), new SolidColorBrush(device, new RawColor4((float)darkerColor.R / 256, (float)darkerColor.G / 256, (float)darkerColor.B / 256, (float)darkerColor.A / 256)));
                    device.DrawText(u.Text, new TextFormat(fontFactory, fontFamily, FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, fontSize), new RawRectangleF(u.MapPosX + screenWidthOffset + 1, u.MapPosZ + screenHeightOffset - 1, u.MapPosX + measure.Width + screenWidthOffset, u.MapPosZ + measure.Height + screenHeightOffset), new SolidColorBrush(device, new RawColor4((float)darkerColor.R / 256, (float)darkerColor.G / 256, (float)darkerColor.B / 256, (float)darkerColor.A / 256)));
                    device.DrawText(u.Text, new TextFormat(fontFactory, fontFamily, FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, fontSize), new RawRectangleF(u.MapPosX + screenWidthOffset - 1, u.MapPosZ + screenHeightOffset + 1, u.MapPosX + measure.Width + screenWidthOffset, u.MapPosZ + measure.Height + screenHeightOffset), new SolidColorBrush(device, new RawColor4((float)darkerColor.R / 256, (float)darkerColor.G / 256, (float)darkerColor.B / 256, (float)darkerColor.A / 256)));
                }

                device.DrawText(u.Text, new TextFormat(fontFactory, fontFamily, FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, fontSize), new RawRectangleF(u.MapPosX + screenWidthOffset, u.MapPosZ + screenHeightOffset, u.MapPosX + measure.Width + screenWidthOffset, u.MapPosZ + measure.Height + screenHeightOffset), new SolidColorBrush(device, new RawColor4((float)u.DrawColor.R / 256, (float)u.DrawColor.G / 256, (float)u.DrawColor.B / 256, (float)u.DrawColor.A / 256)));
            });

            OpenGL.OverlayGeometry.Clear();
            OpenGL.OpenglOverlayIcons.Clear();
            OpenGL.OverlayText.Clear();
        }

        private void Title_MouseDown(object sender, MouseEventArgs e)
        {
            this.startPoint = e.Location;
            this.drag = true;
        }

        private void Title_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.drag)
            {
                Point p1 = new Point(e.X, e.Y);
                Point p2 = this.PointToScreen(p1);
                Point p3 = new Point(p2.X - this.startPoint.X,
                                     p2.Y - this.startPoint.Y);
                this.Location = p3;
            }
        }

        private void Title_MouseUp(object sender, MouseEventArgs e)
        {
            this.drag = false;
        }

        #endregion Private Methods

        #region Internal Structs

        internal struct Margins
        {
            #region Public Fields

            public int Left, Right, Top, Bottom;

            #endregion Public Fields
        }

        #endregion Internal Structs
    }
}