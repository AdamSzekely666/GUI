using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OmniCheck_360.Properties;
using System.Diagnostics;
using Zebra.ADA.OperatorAPI;
using Zebra.ADA.OperatorAPI.ControlsPackage;
using Zebra.ADA.OperatorAPI.Communication;
using Zebra.ADA.OperatorAPI.InputsPackage;
using Zebra.ADA.OperatorAPI.OutputsPackage;
using Zebra.ADA.OperatorAPI.ExecutionMessagesPackage;
using Zebra.ADA.OperatorAPI.HostServices;
using OmniCheck_360;
using System.Linq;
using iText.IO.Util;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace MatroxLDS
{

    public partial class Inspection : BaseForm
    {
        public MainForm mainForm;

        private Inspection1 inspection1Form;
        private Inspection2 inspection2Form;
        public Inspection(MainForm _mainform)
        {
            InitializeComponent();
            
            ButtonAnimator.InitializeAnimation(MissingCapBtn, "blue");
            ButtonAnimator.InitializeAnimation(HighCapBtn, "blue");
            ButtonAnimator.InitializeAnimation(DashboardBtn, "blue");
            mainForm = _mainform;

        }
        private void Inspection_Load(object sender, EventArgs e)
        {
            CurrentUserTxt.Text = mainForm.CurrentUserTxt.Text;
            DateTimeLabel.Text = mainForm.DateTimeLabel.Text;
           // await ChangeRecipe.ConnectAsync();

            mainForm.SetButtonVisibilityForCurrentUser(); // Ensure visibility logic applies

            foreach (var btn in mainForm.buttonPermissions["MainMenu"])
            {
                var buttonName = btn.Key;
                var requiredLevel = btn.Value;
                var button = this.Controls.Find(buttonName, true).FirstOrDefault() as Button;
                if (button != null)
                {
                    bool isVisible = mainForm.nUserAccess >= requiredLevel;
                    button.Visible = isVisible;
                    //Console.WriteLine($"MainMenu Button: {buttonName}, Required Level: {requiredLevel}, Current Level: {mainForm.nUserAccess}, Visible: {isVisible}");
                }
            }

        }
        private void LogButtonVisibility()
        {
            // Log visibility for MainForm buttons
            foreach (var formButtons in mainForm.buttonPermissions)
            {
                foreach (var btn in formButtons.Value)
                {
                    var buttonName = btn.Key;
                    var requiredLevel = btn.Value;
                    var button = mainForm.Controls.Find(buttonName, true).FirstOrDefault() as Button;
                    if (button != null)
                    {
                        bool isVisible = mainForm.nUserAccess >= requiredLevel;
                       // Console.WriteLine($"MainMenu Button: {buttonName}, Required Level: {requiredLevel}, Current Level: {mainForm.nUserAccess}, Visible: {button.Visible}");
                    }
                }
            }

            // Log visibility for MainMenu buttons
            foreach (var btn in mainForm.buttonPermissions["MainMenu"])
            {
                var buttonName = btn.Key;
                var requiredLevel = btn.Value;
                var button = Controls.Find(buttonName, true).FirstOrDefault() as Button;
                if (button != null)
                {
                    bool isVisible = mainForm.nUserAccess >= requiredLevel;
                   // Console.WriteLine($"MainMenu Button: {buttonName}, Required Level: {requiredLevel}, Current Level: {mainForm.nUserAccess}, Visible: {button.Visible}");
                }
            }
        }
        public void ShowUser(string user)
        {
            mainForm.CurrentUserTxt.Text = user;
            mainForm.SetButtonVisibilityForCurrentUser(); // Update button visibility
        }
        private void DashboardBtn_Click(object sender, EventArgs e)
        {

            MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];
            if (mainForm != null)
            {
                mainForm.Show();
                this.Close();
            }
        }

        private void MissingCapBtn_Click(object sender, EventArgs e)
        {
            Inspection1 inspection1Form = new Inspection1(this);
            inspection1Form.Show();
            this.Hide();
        }
        private void HighCapBtn_Click(object sender, EventArgs e)
        {
            // Pass the current MainForm instance to the Data form
            Inspection2 inspection2Form = new Inspection2(this);
            inspection2Form.Show();
            this.Hide(); // Optional: Hide the current form if needed

        }

    }
}

