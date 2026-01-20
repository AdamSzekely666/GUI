using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nini.Config;

namespace Omnicheck360
{
    public partial class SplashScreen : Form
    {
        public string OmnifissionPassword;

        private static IniConfigSource source;
        private string cPath;
        private string AppFile;

        public PopUpKeyPad popUpKeyPad;

        bool Startup;

        public SplashScreen()
        {
            InitializeComponent();

            cPath = Environment.CurrentDirectory;
            AppFile = cPath + "\\app.ini";

            source = new IniConfigSource(AppFile);

            OmnifissionPassword = source.Configs["Omnifission"].Get("Password");
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            SplashTimer1.Start();
            Startup = true;
            //SplashTimer2.Start();
        }

        private void SplashTimer1_Tick(object sender, EventArgs e)
        {
            SplashTimer1.Stop();

            while (OmnifissionPassword != "UAW8SU95ECQ4NASQ")
            {
                if (Startup == true)
                {
                    Startup = false;
                    OmniMsgOriginal message = new OmniMsgOriginal("First Startup Detected. For Access Code Please Call 1-905-405-9777 or Email orders@omnifission.com", 2);
                    message.ShowDialog();
                    message.TopMost = true;

                    popUpKeyPad = new PopUpKeyPad(this);
                    PopUpKeyPad.oPassedField = OmnifissionPassword;
                    popUpKeyPad.ShowDialog();
                }
                else
                {
                    OmniMsgOriginal message = new OmniMsgOriginal("Incorrect Password!", 2);
                    message.ShowDialog();
                    message.TopMost = true;

                    popUpKeyPad = new PopUpKeyPad(this);
                    PopUpKeyPad.oPassedField = OmnifissionPassword;
                    popUpKeyPad.ShowDialog();
                }
            }

            source.Configs["Omnifission"].Set("Password", OmnifissionPassword);
            source.Save();
            SplashTimer2.Start();
        }

        private void SplashTimer2_Tick(object sender, EventArgs e)
        {
            Pgr1.PerformStep();
            if (Pgr1.Value > 1400)
            {
                this.Close();
            }
        }
    }
}
