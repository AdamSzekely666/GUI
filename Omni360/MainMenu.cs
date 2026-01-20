using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Omnicheck360.Properties;
using System.Diagnostics;
using Microsoft.VisualBasic.ApplicationServices;

namespace Omnicheck360
{

    public partial class MainMenu : BaseForm
    {
        public MainForm mainForm;
        public Documents Documents;
        public AdminScreen adminScreen;
        public RecipeChange recipeChange;
        private PasswordForm _password;
        public MainMenu mainMenuForm;
        public MainMenu(MainForm _mainform)
        {
            InitializeComponent();
            ButtonAnimator.InitializeAnimation(RecipeChangeBtn, "blue");
            ButtonAnimator.InitializeAnimation(DocumentsBtn, "blue");
            ButtonAnimator.InitializeAnimation(RecipeSetupBtn, "blue");
            ButtonAnimator.InitializeAnimation(AdminSettingsBtn, "blue");
            ButtonAnimator.InitializeAnimation(UPSSettingBtn, "blue");
            ButtonAnimator.InitializeAnimation(GlobalTrackingBtn, "blue");
            ButtonAnimator.InitializeAnimation(DashboardBtn, "blue");
            ButtonAnimator.InitializeAnimation(QuitApplicationBtn, "blue");
            ButtonAnimator.InitializeAnimation(UserLoginBtn, "blue");
            ButtonAnimator.InitializeAnimation(Data, "blue");

            mainForm = _mainform;
            adminScreen = new AdminScreen(mainForm);
        }
        public void MainMenu_Load(object sender, EventArgs e)
        {
            CurrentUserTxt.Text = mainForm.CurrentUserTxt.Text;
            DateTimeLabel.Text = mainForm.DateTimeLabel.Text;
            if (MainForm.nUserAccess == 0)
            {
                UserLoginBtn.BackgroundImage = Resources.userlogin48x48;
            }
            else
            {
                UserLoginBtn.BackgroundImage = Resources.Logout48x48;
            }

        }
        public void ShowUser(string user)
        {
            mainForm.CurrentUserTxt.Text = user;
        }
        private void DocumentsBtn_Click(object sender, EventArgs e)
        {
            MainForm.log.Info("Main Menu Page: Documents Button Pressed");

            Documents documents = new Documents(mainForm);
            documents.Show();
            documents.Focus();
            this.TopMost = false;
            documents.TopMost = false;
        }
        private void AdminSettingsBtn_Click(object sender, EventArgs e)
        {
            AdminScreen adminScreen = new AdminScreen(mainForm);
            MainForm.log.Info("Main Menu Page: Access Control Button Pressed");
            adminScreen.Show();
            adminScreen.Focus();
            this.TopMost = false;
            adminScreen.TopMost = false;
        }
        private void UPSSettingBtn_Click(object sender, EventArgs e)
        {
            Process proc;
            ProcessStartInfo StartInfo = new ProcessStartInfo();

            MainForm.log.Info("Main Menu Page: UPS Settings Button Pressed");
            this.Close();

            StartInfo.FileName = @"C:\Program Files (x86)\Phoenix Contact\UPS-CONF 2.6\UpsConf.exe";
            StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            StartInfo.Arguments = string.Empty;
            proc = Process.Start(StartInfo);
        }
        private void UserLoginBtn_Click(object sender, EventArgs e)
        {
            _password = new PasswordForm(this);

            if ((string)UserLoginBtn.Tag == "logout")
            {
                MainForm.log.Info("Main Menu Page: Logout Button Pressed");
                mainForm.DoActiveButtons(0);
                mainForm.CurrentUserTxt.Text = "Operate Mode";
                mainMenuForm.Close();
            }
            if ((string)UserLoginBtn.Tag == "login")
            {
                MainForm.log.Info("Main Menu Page: Login Button Pressed");
                _password.Show();
                _password.Focus();
                _password.TopMost = true;
            }
        }
        private void DashboardBtn_Click(object sender, EventArgs e)
        {
            MainForm.log.Info("Main Menu Page: Dashboard Button Pressed");
            this.Close();
        }
        private void QuitApplicationBtn_Click(object sender, EventArgs e)
        {
            MainForm.log.Info("Main Menu Page: Quit Application Button Pressed");
            MainForm.Client.Disconnect();
            Application.Exit();
        }
        private void RecipeChangeBtn_Click(object sender, EventArgs e)
        {
            RecipeChange recipeChange = new RecipeChange(mainForm);

            MainForm.log.Info("Main Menu Page: Recipe Change Button Pressed");
            recipeChange.Show();
            recipeChange.Focus();
            this.TopMost = false;
            recipeChange.TopMost = false;
                
        }
        private void Data_Click(object sender, EventArgs e)
        {
            // Pass the current MainForm instance to the Data form
            Data dataForm = new Data(mainForm);
            dataForm.TopMost = true;
            dataForm.Show();
            this.Close(); // Close MainMenu if needed or use Hide() to keep it in memory
        }

        private void RecipeSetupBtn_Click(object sender, EventArgs e)
        {
            Camera1InspectionSetup SetupPage = new Camera1InspectionSetup(mainForm);
            MainForm.log.Info("Camera Setup Page: Camera 1 Setup Button Pressed");
            SetupPage.Show();
            SetupPage.Focus();
            this.TopMost = false;
            SetupPage.TopMost = false;
            //SetupPage.TopMost = true;
        }
        private void GlobalTrackingBtn_Click(object sender, EventArgs e)
        {
            MainForm.log.Info("Main Menu Page: Global Tracking System Button Pressed");
            GlobalTracking globalTracking = new GlobalTracking(mainForm);
            globalTracking.Show();
            globalTracking.Focus();
            this.TopMost = false;
            globalTracking.TopMost = false;
        }
        private void InterfaceControlBtn_Click(object sender, EventArgs e)
        {
            MainForm.log.Info("Main Menu Page: Interface Control Button Pressed");
            InterfaceControl interfaceControl = new InterfaceControl(mainForm);
                interfaceControl.Show();
                interfaceControl.Focus();
                interfaceControl.TopMost = true;
                this.Close();
        }

    }
}

