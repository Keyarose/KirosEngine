using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KirosEditor
{
    public static class Prompt
    {
        public static string ShowTextDialog(string text, string caption)
        {
            bool canceled = false;
            Form prompt = new Form();
            prompt.Width = 500;
            prompt.Height = 200;
            prompt.Text = caption;
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button okButton = new Button() { Text = "OK", Left = 50, Width = 100, Top = 70 };
            okButton.Click += (sender, e) => { prompt.Close(); };
            Button cancelButton = new Button() { Text = "Cancel", Left = 350, Width = 100, Top = 70 };
            cancelButton.Click += (sender, e) => { canceled = true; prompt.Close(); };

            prompt.Controls.Add(cancelButton);
            prompt.Controls.Add(okButton);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(textLabel);
            prompt.ShowDialog();

            if(canceled)
            {
                return null;
            }
            else
            {
                return textBox.Text;
            }
        }
    }
}
