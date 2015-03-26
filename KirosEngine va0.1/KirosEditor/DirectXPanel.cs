using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.Direct3D11;
using KirosEngine;

namespace KirosEditor
{
    class DirectXPanel
    {
        protected D3DCore _core;
        protected Device _device;
        protected DeviceContext _context;

        public DirectXPanel(IntPtr handel, int width, int height, Form parent)
        {
            Application.Idle += Run;
            parent.FormClosing += Unload;

            _core = new D3DCore();
            _core.Initialize(handel, width, height, false, 0.1f, 1000.0f);
            _device = _core.GetDevice();
            _context = _device.ImmediateContext;
        }

        public virtual void Run(object sender, EventArgs e)
        {
            while(IsApplicationIdle())
            {
                this.Update();
                this.Draw();
            }
        }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {
            _core.BeginScene(new Color4(0.5f, 0.5f, 1.0f));

            _core.EndScene();
        }

        public virtual void Unload(object sender, FormClosingEventArgs e)
        {
            Application.Idle -= Run;
            ((Form)sender).FormClosing -= Unload;
            _core.Dispose();
        }

        //message run stuff
        [StructLayout(LayoutKind.Sequential)]
        public struct NativeMessage
        {
            public IntPtr Handle;
            public uint Message;
            public IntPtr WParameter;
            public IntPtr LParameter;
            public uint Time;
            public Point Location;
        }

        [DllImport("user32.dll")]
        public static extern int PeekMessage(out NativeMessage message, IntPtr window, uint filterMin, uint filterMax, uint remove);

        bool IsApplicationIdle()
        {
            NativeMessage result;
            return PeekMessage(out result, IntPtr.Zero, (uint)0, (uint)0, (uint)0) == 0;
        }
    }
}
