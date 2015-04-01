using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using KirosEngine;

namespace KirosEditor
{
    public partial class MainForm : Form
    {
        string _packageDirectory;
        string _workingDirectory;
        PackageData _gamePackage;

        public MainForm()
        {
            InitializeComponent();
            this.FormClosing += MainForm_FormClosing;

            XDocument prefDoc = XDocument.Load("preferences.xml");
            XElement pref = prefDoc.Root;

            _packageDirectory = pref.TryGetElementValue("packageDirectory");            
            if(_packageDirectory == null)
            {
                _packageDirectory = "packages";
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //save preferences
            XElement packageDirect = new XElement("packageDirectory", _packageDirectory);

            XElement pref = new XElement("preferences");
            pref.Add(packageDirect);

            XDocument prefDoc = new XDocument(new XDeclaration("1.0", "utf-8", null), pref);
            prefDoc.Save("preferences.xml");
        }

        private void solarSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(_gamePackage != null)
            {
                SolarSystemForm _solarForm = new SolarSystemForm(_gamePackage);
                _solarForm.Show();
            }
        }

        private void packageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //open a dialog
            string newPackageName = Prompt.ShowTextDialog("Package Name:", "Create a New Package");
        }
    }

    public class PackageData
    {
        string _filePath;
    }
}
