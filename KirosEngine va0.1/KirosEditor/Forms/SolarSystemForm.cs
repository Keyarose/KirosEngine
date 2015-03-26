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
        SolarSystemPanel _dPanel;

        public SolarSystemForm(PackageData data)
        {
            InitializeComponent();
            _dPanel = new SolarSystemPanel(splitContainer2.Panel2.Handle, splitContainer2.Panel2.Width, splitContainer2.Panel2.Height, this);
        }
    }
}
