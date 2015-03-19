using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.Direct3D11;
using KirosEngine;

namespace KirosEditor
{
    public partial class SolarSystemForm : Form
    {
        D3DCore _core;
        Device _device;
        DeviceContext _context;

        public SolarSystemForm()
        {
            InitializeComponent();
            Application.Idle += Run;
            _core = new D3DCore();

            this.Init();
        }

        private bool Init()
        {
            _core.Initialize(splitContainer2.Panel2.Handle, splitContainer2.Panel2.Width, splitContainer2.Panel2.Height, false, 0.1f, 1000.0f);
            _device = _core.GetDevice();
            _context = _device.ImmediateContext;

            return true;
        }

        void Run(object sender, EventArgs e)
        {
            while(IsApplicationIdle())
            {
                this.UpdateSolarView();
                this.Draw();
            }
        }

        public void UpdateSolarView()
        {

        }

        public void Draw()
        {
            _core.BeginScene(new Color4(0.5f, 0.5f, 1.0f));

            _core.EndScene();
        }

        //TODO: Unload

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
