using iText.IO.Util;
using Zebra.ADA.OperatorAPI;
using Zebra.ADA.OperatorAPI.Communication;
using Zebra.ADA.OperatorAPI.ControlsPackage;
using Zebra.ADA.OperatorAPI.ExecutionMessagesPackage;
using Zebra.ADA.OperatorAPI.HostServices;
using Zebra.ADA.OperatorAPI.InputsPackage;
using Zebra.ADA.OperatorAPI.OutputsPackage;
using OmniCheck_360;
using OmniCheck_360.Properties;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatroxLDS
{

    public partial class MainMenu : BaseForm
    {
        public MainForm mainForm;
        public Documents Documents;
        private Host _host;
        private PasswordForm _password;
        private Data _data;
        private UserManagerForm _userManager;
        public MainMenu(MainForm _mainform)
        {
            InitializeComponent();
            
            ButtonAnimator.InitializeAnimation(ChangeRecipeBtn, "blue");
            ButtonAnimator.InitializeAnimation(DocumentsBtn, "blue");
            ButtonAnimator.InitializeAnimation(TrackingSystemBtn, "blue");
            ButtonAnimator.InitializeAnimation(AdminSettingsBtn, "blue");
            ButtonAnimator.InitializeAnimation(UPSSettingBtn, "blue");
            ButtonAnimator.InitializeAnimation(UserLoginBtn, "blue");
            ButtonAnimator.InitializeAnimation(DashboardBtn, "blue");
            ButtonAnimator.InitializeAnimation(QuitApplicationBtn, "blue");
            ButtonAnimator.InitializeAnimation(Data, "blue");
            ButtonAnimator.InitializeAnimation(Inspection, "blue");
            ButtonAnimator.InitializeAnimation(FlavourManagerBtn, "blue");




            mainForm = _mainform;
            if (mainForm != null)
            {
                mainForm.UserStateChanged += MainForm_UserStateChanged;
            }
        }
        private void MainForm_UserStateChanged(object sender, EventArgs e)
        {
            // Ensure we run on UI thread
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => MainForm_UserStateChanged(sender, e)));
                return;
            }

            try
            {
                // Update top-level displays
                CurrentUserTxt.Text = mainForm?.currentUserName ?? "";
                DateTimeLabel.Text = mainForm?.DateTimeLabel?.Text ?? DateTime.Now.ToString("g");

                // Update login/logout icon
                UserLoginBtn.BackgroundImage = (mainForm != null && mainForm.IsUserLoggedIn)
                    ? Resources.Logout48x48
                    : Resources._48x48Login;

                // Refresh MainMenu buttons visibility using the same permission map as MainForm
                if (mainForm?.buttonPermissions != null && mainForm.buttonPermissions.ContainsKey("MainMenu"))
                {
                    foreach (var kv in mainForm.buttonPermissions["MainMenu"])
                    {
                        var buttonName = kv.Key;
                        var requiredLevel = kv.Value;
                        var button = this.Controls.Find(buttonName, true).FirstOrDefault() as Button;
                        if (button != null)
                        {
                            button.Visible = mainForm.nUserAccess >= requiredLevel;
                        }
                    }
                }

                // Force a UI refresh
                this.Refresh();
            }
            catch
            {
                // Ignore UI update errors from stale state
            }
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                if (mainForm != null)
                    mainForm.UserStateChanged -= MainForm_UserStateChanged;
            }
            catch { }
            base.OnFormClosed(e);
        }
        private void MainMenu_Load(object sender, EventArgs e)
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

            //Console.WriteLine($"Panels visibility set based on access level. User Access Level: {mainForm.nUserAccess}, Panels Visible: {showPanels}");

            // Set the background image of the UserLoginBtn based on login state
            if (mainForm.IsUserLoggedIn)
            {
                UserLoginBtn.BackgroundImage = Resources.Logout48x48; // Set to logout icon if logged in
            }
            else
            {
                UserLoginBtn.BackgroundImage = Resources._48x48Login; // Set to login icon if logged out
            }

           // Console.WriteLine($"Current User: {mainForm.currentUserName}");
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
        private void ChangeRecipeBtn_Click(object sender, EventArgs e)
        {
           // ChangeRecipe.DoClick();
            this.Close();
            RecipeChange recipeChange = new RecipeChange(mainForm);
            recipeChange.TopMost = true;
            recipeChange.Show();
        }
        private void DocumentsBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            Documents documents = new Documents(mainForm);
            documents.Show();
            documents.Focus();
            documents.TopMost = true;
        }
        private void AdminSettingsBtn_Click(object sender, EventArgs e)
        {
            this.Hide(); // Hide the main form to keep it in memory
            using (var userManagerForm = new UserManagerForm(this))
            {
                userManagerForm.ShowDialog(); // Show UserManagerForm as a modal dialog
            }
            this.Show(); // Show the main form again when UserManagerForm is closed
        }
        private void UPSSettingBtn_Click(object sender, EventArgs e)
        {
            this.Hide(); // Hide the main form to keep it in memory
            using (var RejectedImages = new RejectedImages())
            {
                RejectedImages.ShowDialog(); // Show UserManagerForm as a modal dialog
            }
            this.Show(); // Show the main form again when UserManagerForm is closed
        }
        private void UserLoginBtn_Click(object sender, EventArgs e)
        {
            if (mainForm.IsUserLoggedIn)
            {
                // Logout path
                mainForm.LogOutToOperatingUser();

                // Update icon and username text to reflect logout
                UserLoginBtn.BackgroundImage = Resources.login48x48; // login icon
                UpdateUsernameTextBox(mainForm.currentUserName);
            }
            else
            {
                using (var passwordForm = new PasswordForm())
                {
                    if (passwordForm.ShowDialog() == DialogResult.OK)
                    {
                        var password = passwordForm.Password;
                        var user = mainForm.users.FirstOrDefault(u => u.Password == password);
                        if (user != null)
                        {
                            mainForm.LogInUser(user); // Authenticate user and update MainForm

                            if (mainForm.currentUserName != "Operate")
                            {
                                UserLoginBtn.BackgroundImage = Resources.Logout48x48; // Set to logout icon
                                UpdateUsernameTextBox(mainForm.currentUserName); // Update the username in MainMenu
                                mainForm.SetButtonVisibilityForCurrentUser(); // Refresh buttons and panels
                               // Console.WriteLine($"User logged in as: {mainForm.currentUserName}, Access Level: {mainForm.nUserAccess}");
                            }


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

                            //Console.WriteLine($"Panels and buttons updated based on user access. User Access Level: {mainForm.nUserAccess}, Panels Visible: {showPanels}");
                        }
                        else
                        {
                            MessageBox.Show("Invalid password.");
                        }
                    }
                }
            }
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
        private void QuitApplicationBtn_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                // Log any exceptions if needed
            }
            finally
            {
                // Ensure process termination
                Environment.Exit(0);
                var process = Process.GetCurrentProcess();
                process.Kill();
            }
        }
        private void TrackingSystemBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            TrackingSystem trackingSystem = new TrackingSystem(mainForm);
            trackingSystem.Show();
            trackingSystem.Focus();
            trackingSystem.TopMost = true;
        }
        public void UpdateUsernameTextBox(string username)
        {
            CurrentUserTxt.Text = username;
        }
        private async void Data_Click(object sender, EventArgs e)
        {
            lblPleaseWait.Visible = true;
            lblPleaseWait.BringToFront();
            await Task.Delay(100); // Give the label time to render

            // Open the Data form asynchronously (non-blocking)
            await Task.Run(() =>
            {
                // You can do any heavy pre-loading here if needed.
            });

            Data dataForm = new Data(mainForm);
            dataForm.TopMost = true;
            dataForm.Show();

            lblPleaseWait.Visible = false;
            this.Close(); // Optional: close or Hide MainMenu if you want
        }
        private void Inspection_Click(object sender, EventArgs e)
        {
            Inspection InspectionForm = new Inspection(mainForm);
            InspectionForm.TopMost = true;
            
            InspectionForm.Show();
            this.Close(); // Close MainMenu if needed or use Hide() to keep it in memory

        }

        private void FlavourManagerBtn_Click(object sender, EventArgs e)
        {
            FlavourManagerForm flavourManager = new FlavourManagerForm(mainForm);
            flavourManager.TopMost = true;
            flavourManager.Show();
            this.Close();
        }
    }
}

