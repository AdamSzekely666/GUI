using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Omnicheck360.Properties;
using System.Diagnostics;
using Nini.Config;

namespace Omnicheck360
{
    public partial class Documents : BaseForm
    {

        private MainForm mainForm;
        private string _documentsDirectory;

        private static IniConfigSource source;
  
        public string OperatingDoc = "";
        public string TroubleShootingDoc = "";
        public string PartsListDoc = "";


        public Documents(MainForm _mainForm)
        {
            InitializeComponent();
            ButtonAnimator.InitializeAnimation(MainMenuFormBtn, "blue");
            ButtonAnimator.InitializeAnimation(OperatingManualBtn, "blue");
            ButtonAnimator.InitializeAnimation(PartsListBtn, "blue");
            ButtonAnimator.InitializeAnimation(TroubleshootingManualBtn, "blue");


            mainForm = _mainForm;
            _documentsDirectory = System.Environment.CurrentDirectory + "\\docs\\";
        }
        private void Documents_Load(object sender, EventArgs e)
        {
            string cPath;
            string AppFile;

            timer1.Start();
            CurrentUserTxt.Text = mainForm.CurrentUserTxt.Text;

            cPath = Environment.CurrentDirectory;
            AppFile = cPath + "\\app.ini";
            source = new IniConfigSource(AppFile);

            OperatingDoc = source.Configs["Documents"].Get("Operating");
            TroubleShootingDoc = source.Configs["Documents"].Get("TroubleShooting");
            PartsListDoc = source.Configs["Documents"].Get("PartsList");
        }
        private void OperatingManualBtn_Click(object sender, EventArgs e)
        {
            string cFilename;

            MainMenu.ActiveForm.TopMost = false;

            MainForm.log.Info("Documents Page: Operating Manual Button Pressed");
            cFilename = _documentsDirectory + OperatingDoc;
            Process.Start(cFilename);
        }
        private void PartsListBtn_Click(object sender, EventArgs e)
        {
            string cFilename;

            MainMenu.ActiveForm.TopMost = false;

            MainForm.log.Info("Documents Page: Parts List Button Pressed");
            cFilename = _documentsDirectory + PartsListDoc;
            Process.Start(cFilename);
        }
        private void TroubleshootingManualBtn_Click(object sender, EventArgs e)
        {
            string cFilename;

            MainMenu.ActiveForm.TopMost = false;

            MainForm.log.Info("Documents Page: Troubleshooting Manual Button Pressed");
            cFilename = _documentsDirectory + TroubleShootingDoc;
            Process.Start(cFilename);
        }
        private void MainMenuFormBtn_Click(object sender, EventArgs e)
        {
            MainForm.log.Info("Access Control Page: Dashboard Button Pressed");
            this.Close();
            mainForm.Show();

        }
    }
}
