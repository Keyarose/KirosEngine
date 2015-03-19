using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KirosEditor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void solarSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SolarSystemForm _solarForm = new SolarSystemForm();
            _solarForm.Show();
        }
    }
}
