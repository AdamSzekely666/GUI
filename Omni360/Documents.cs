using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OmniCheck_360.Properties;
using Nini.Config;
using System.Diagnostics;

namespace MatroxLDS
{
    public partial class Documents : BaseForm
    {

        private MainForm mainForm;
        private string _documentsDirectory;

        private static IniConfigSource source;

        public string ElectricalDoc;
        public string OperatingDoc;
        public string TroubleShootingDoc;
        public string PartsListDoc; 

        public Documents(MainForm _mainForm)
        {
            InitializeComponent();
            ButtonAnimator.InitializeAnimation(DashboardBtn, "blue");
            ButtonAnimator.InitializeAnimation(btnDoc1, "blue");
            ButtonAnimator.InitializeAnimation(btnDoc2, "blue");
            ButtonAnimator.InitializeAnimation(btnDoc3, "blue");
            ButtonAnimator.InitializeAnimation(btnDoc4, "blue");

            mainForm = _mainForm;
            _documentsDirectory = System.Environment.CurrentDirectory + "\\docs\\";
        }

        private void Documents_Load(object sender, EventArgs e)
        {
            string cPath;
            string IniFile;

            cPath = System.Environment.CurrentDirectory;
            IniFile = cPath + "\\app.ini";

            source = new IniConfigSource(IniFile);

            CurrentUserTxt.Text = mainForm.CurrentUserTxt.Text;
            DateTimeLabel.Text = mainForm.DateTimeLabel.Text;

            //ElectricalDoc = source.Configs["Documentation"].Get("ElectricalDoc");
            //OperatingDoc = source.Configs["Documentation"].Get("OperatingDoc");
            //TroubleShootingDoc = source.Configs["Documentation"].Get("TroubleShootingDoc");
            //PartsListDoc = source.Configs["Documentation"].Get("PartsListDoc");
        }

        //Settings for Electrical drawings Button
        //********************************************************
        private void btnDoc1_Click(object sender, EventArgs e)
        {
            string cFilename;
            int nVal;

           // nVal = MainForm.nUserAccess;

            MainMenu.ActiveForm.TopMost = false;

            cFilename = _documentsDirectory + ElectricalDoc;
            Process.Start(cFilename);
        }

        //********************************************************

        //Settings for Operating Manual Button
        //********************************************************
        private void btnDoc2_Click(object sender, EventArgs e)
        {
            string cFilename;
            int nVal;

          //  nVal = MainForm.nUserAccess;

            MainMenu.ActiveForm.TopMost = false;

            cFilename = _documentsDirectory + OperatingDoc;
            Process.Start(cFilename);
        }

        //********************************************************

        //Settings for Parts List Button
        //********************************************************
        private void btnDoc3_Click(object sender, EventArgs e)
        {
            string cFilename;
            int nVal;

//nVal = MainForm.nUserAccess;

            MainMenu.ActiveForm.TopMost = false;

            cFilename = _documentsDirectory + PartsListDoc;
            Process.Start(cFilename);
        }

        //********************************************************

        //Settings for Troubleshooting Manual Button
        //********************************************************
        private void btnDoc4_Click(object sender, EventArgs e)
        {
            string cFilename;
            int nVal;

          //  nVal = MainForm.nUserAccess;

            MainMenu.ActiveForm.TopMost = false;

            cFilename = _documentsDirectory + TroubleShootingDoc;
            Process.Start(cFilename);
        }

        //********************************************************

        //Settings for Dashboard Button
        //********************************************************
        private void DashboardBtn_Click(object sender, EventArgs e)
        {
            mainForm.Show();
            mainForm.Focus();
            this.Close();
        }

        //********************************************************
    }
}
