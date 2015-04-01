using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlimDX;

namespace KirosEditor
{
    class SolarSystemPanel : DirectXPanel
    {
        public SolarSystemPanel(IntPtr handle, int width, int height, Form parent) : base(handle, width, height, parent)
        {

        }

        public override void Draw()
        {
            _core.BeginScene(new Color4(0.5f, 0.5f, 1.0f));

            _core.EndScene();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
