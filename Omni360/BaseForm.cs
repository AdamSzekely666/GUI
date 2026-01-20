using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Omnicheck360
{
    public partial class BaseForm : Form
    {
        public static bool isfrench =true;
        public static bool transversed_pages = false;
        public BaseForm()
        {
            InitializeComponent();
        }

        private void OmnifissionLogo_Click(object sender, EventArgs e)
        {
            string progFiles = @"C:\Windows\System32";
            string keyboardPath = Path.Combine(progFiles, "osk.exe");

            Process.Start(keyboardPath);
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            string time = DateTime.Now.ToString("f", new System.Globalization.CultureInfo("en-US"));
            DateTimeLabel.Text = time;
        }
    }
}
