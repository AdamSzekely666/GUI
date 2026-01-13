using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zebra.ADA.OperatorAPI;
using Zebra.ADA.OperatorAPI.ExecutionMessagesPackage;
using Zebra.ADA.OperatorAPI.InputsPackage;
using Zebra.ADA.OperatorAPI.ControlsPackage;
using Zebra.ADA.OperatorAPI.OutputsPackage;
using OmniCheck_360.Properties;
using System.Data.SQLite;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.Drawing;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;
using static iText.IO.Util.IntHashtable;
using OmniCheck_360;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using Zebra.ADA.OperatorAPI.Communication;
using OpenHardwareMonitor.Hardware;
using static MatroxLDS.MainForm;



namespace MatroxLDS
{
    public partial class RecipeChange : MatroxLDS.BaseForm
    {
        private OperatorViewPage CameraPage;
        private Host _host;
        public MainForm mainForm;
        public MainForm _mainForm;
       // private PasswordForm _password;

        public const string C1SELECTED_RECIPE_LISTBOX = "RecipeListBox";
        public const string C1DELETERECIPE = "DeleteRecipe";
        public const string C1ADDRECIPE = "AddRecipe";
        public const string C1CURRENTRECIPENAME = "CurrentRecipeName";
        public const string C1CURRENTRECIPEID = "CurrentRecipeIDValue";
        public const string C1NEWRECIPENAME = "NewRecipeName";

        public const string C2SELECTED_RECIPE_LISTBOX = "RecipeListBox";
        public const string C2DELETERECIPE = "DeleteRecipe";
        public const string C2ADDRECIPE = "AddRecipe";
        public const string C2CURRENTRECIPENAME = "CurrentRecipeName";
        public const string C2CURRENTRECIPEID = "CurrentRecipeIDValue";
        public const string C2NEWRECIPENAME = "NewRecipeName";

        public const string C3SELECTED_RECIPE_LISTBOX = "RecipeListBox";
        public const string C3DELETERECIPE = "DeleteRecipe";
        public const string C3ADDRECIPE = "AddRecipe";
        public const string C3CURRENTRECIPENAME = "CurrentRecipeName";
        public const string C3CURRENTRECIPEID = "CurrentRecipeIDValue";
        public const string C3NEWRECIPENAME = "NewRecipeName";

        private System.Windows.Forms.Timer hideTimer;


        public RecipeChange(MainForm _mainForm)
        {
            InitializeComponent();
            ButtonAnimator.InitializeAnimation(DashboardBtn, "blue");
            // Timer setup
            hideTimer = new System.Windows.Forms.Timer();
            hideTimer.Interval = 3000; // 3 seconds
            hideTimer.Tick += HideTimer_Tick;

            mainForm = _mainForm;

            C1RecipeListBox.PageName = MainForm.OPERATOR_VIEW_NAME;
            C1RecipeListBox.HostName = MainForm.HOST_NAME;
            C1RecipeListBox.ControlName = C1SELECTED_RECIPE_LISTBOX;
            C1RecipeListBox.ProjectName = MainForm.PROJECT_NAME1;
            C1AddRecipeBtn1.PageName = MainForm.OPERATOR_VIEW_NAME;
            C1AddRecipeBtn1.HostName = MainForm.HOST_NAME;
            C1AddRecipeBtn1.ControlName = C1ADDRECIPE;
            C1AddRecipeBtn1.ProjectName = MainForm.PROJECT_NAME1;
            C1DeleteRecipeBtn1.PageName = MainForm.OPERATOR_VIEW_NAME;
            C1DeleteRecipeBtn1.HostName = MainForm.HOST_NAME;
            C1DeleteRecipeBtn1.ControlName = C1DELETERECIPE;
            C1DeleteRecipeBtn1.ProjectName = MainForm.PROJECT_NAME1;
            C1NewRecipeNameTextBox.PageName = MainForm.OPERATOR_VIEW_NAME;
            C1NewRecipeNameTextBox.HostName = MainForm.HOST_NAME;
            C1NewRecipeNameTextBox.ControlName = C1NEWRECIPENAME;
            C1NewRecipeNameTextBox.ProjectName = MainForm.PROJECT_NAME1;
            C1NewRecipeNameTextBox.Click += C1NewRecipeNameTextBox_Click;

            C2RecipeListBox.PageName = MainForm.OPERATOR_VIEW_NAME;
            C2RecipeListBox.HostName = MainForm.HOST_NAME;
            C2RecipeListBox.ControlName = C2SELECTED_RECIPE_LISTBOX;
            C2RecipeListBox.ProjectName = MainForm.PROJECT_NAME2;
            C2AddRecipeBtn1.PageName = MainForm.OPERATOR_VIEW_NAME;
            C2AddRecipeBtn1.HostName = MainForm.HOST_NAME;
            C2AddRecipeBtn1.ControlName = C2ADDRECIPE;
            C2AddRecipeBtn1.ProjectName = MainForm.PROJECT_NAME2;
            C2DeleteRecipeBtn1.PageName = MainForm.OPERATOR_VIEW_NAME;
            C2DeleteRecipeBtn1.HostName = MainForm.HOST_NAME;
            C2DeleteRecipeBtn1.ControlName = C2DELETERECIPE;
            C2DeleteRecipeBtn1.ProjectName = MainForm.PROJECT_NAME2;
            C2NewRecipeNameTextBox.PageName = MainForm.OPERATOR_VIEW_NAME;
            C2NewRecipeNameTextBox.HostName = MainForm.HOST_NAME;
            C2NewRecipeNameTextBox.ControlName = C2NEWRECIPENAME;
            C2NewRecipeNameTextBox.ProjectName = MainForm.PROJECT_NAME2;
            C2NewRecipeNameTextBox.Click += C2NewRecipeNameTextBox_Click;


        }
        private async void RecipeChange_Load(object sender, EventArgs e)
        {
            await C1RecipeListBox.ConnectAsync();
            await C1AddRecipeBtn1.ConnectAsync();
            await C1DeleteRecipeBtn1.ConnectAsync();
            await C1NewRecipeNameTextBox.ConnectAsync();
            await HostManager.GetHost(MainForm.HOST_NAME).Projects[MainForm.PROJECT_NAME1].ExecutionMessages.SetEnable();

            await C2RecipeListBox.ConnectAsync();
            await C2AddRecipeBtn1.ConnectAsync();
            await C2DeleteRecipeBtn1.ConnectAsync();
            await C2NewRecipeNameTextBox.ConnectAsync();
            await HostManager.GetHost(MainForm.HOST_NAME).Projects[MainForm.PROJECT_NAME2].ExecutionMessages.SetEnable();


            ConnectToAllTextboxes(Program.splashForm);
            C1RecipeNameLb.Text = mainForm.C1RecipeNameLb.Text;
            C2RecipeNameLb.Text = mainForm.C2RecipeNameLb.Text;

        }
        private void ConnectToAllTextboxes(SplashScreen splashScreen)
        {
            CameraPage = HostManager.GetHost(MainForm.HOST_NAME).Projects[MainForm.PROJECT_NAME1].OperatorViews[MainForm.OPERATOR_VIEW_NAME];
            CameraPage.ValueElements[C1CURRENTRECIPEID].ValueChanged += C1CurrentRecipeID;
            CameraPage.TextBoxElements[C1NEWRECIPENAME].ValueChanged += C1NewRecipeName;
            CameraPage.ValueElements[C1CURRENTRECIPENAME].ValueChanged += C1CurrentRecipeName;
            HostManager.GetHost(MainForm.HOST_NAME).Projects[MainForm.PROJECT_NAME1].ExecutionMessages.NewMessagesReceived += ExecutionMessagesReceived;
            
            CameraPage = HostManager.GetHost(MainForm.HOST_NAME).Projects[MainForm.PROJECT_NAME2].OperatorViews[MainForm.OPERATOR_VIEW_NAME];
            CameraPage.ValueElements[C2CURRENTRECIPEID].ValueChanged += C2CurrentRecipeID;
            CameraPage.TextBoxElements[C2NEWRECIPENAME].ValueChanged += C2NewRecipeName;
            CameraPage.ValueElements[C2CURRENTRECIPENAME].ValueChanged += C2CurrentRecipeName;
            HostManager.GetHost(MainForm.HOST_NAME).Projects[MainForm.PROJECT_NAME2].ExecutionMessages.NewMessagesReceived += ExecutionMessagesReceived;


        }

        private void C1CurrentRecipeName(object sender, ValueChangedEventArgs args)
        {
            try
            {
                string newValue = args.NewValue.ToString();
                string oldRecipeName = C1RecipeNameLb.Text;

                // Update the RecipeNameLb with the new recipe name
                if (C1RecipeNameLb.InvokeRequired)
                {
                    C1RecipeNameLb.Invoke(new Action(() => C1RecipeNameLb.Text = newValue));
                }
                else
                {
                    C1RecipeNameLb.Text = newValue;
                }

            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating RecipeNameLb: {ex.Message}");
            }
        }
        private void C1CurrentRecipeID(object sender, ValueChangedEventArgs args)
        {
            try
            {
                string newValue = GetValidData(args.NewValue).ToString();
                if (C1CurrentIDTextBox.InvokeRequired)
                {
                    C1CurrentIDTextBox.Invoke(new Action(() => C1CurrentIDTextBox.Text = newValue));
                }
                else
                {
                    C1CurrentIDTextBox.Text = newValue;
                }
            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating CurrentIDTextBox: {ex.Message}");
            }
        }
        private void C1NewRecipeName(object sender, TextBoxValueChangedEventArgs args)
        {
            try
            {
                string newValue = args.NewValue.ToString();
                if (C1NewRecipeNameTextBox.InvokeRequired)
                {
                    C1NewRecipeNameTextBox.Invoke(new Action(() => C1NewRecipeNameTextBox.Text = newValue));
                }
                else
                {
                    C1NewRecipeNameTextBox.Text = newValue;
                }
            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating NewRecipeNameTextBox: {ex.Message}");
            }
        }
        private void C1NewRecipeNameTextBox_Click(object sender, EventArgs e)
        {
            using (PasswordForm passwordForm = new PasswordForm())
            {
                if (passwordForm.ShowDialog(this) == DialogResult.OK)
                {
                    C1NewRecipeNameTextBox.Text = passwordForm.Password;
                }
            }
        }
        private async void C1AddRecipeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                await C1AddRecipeBtn1.DoClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("An error occured while clicking on button {0}: {1}", this.Name, ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private async void C1DeleteRecipeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                await C1DeleteRecipeBtn1.DoClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("An error occured while clicking on button {0}: {1}", this.Name, ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void C2CurrentRecipeName(object sender, ValueChangedEventArgs args)
        {
            try
            {
                string newValue = args.NewValue.ToString();
                string oldRecipeName = C2RecipeNameLb.Text;

                // Update the RecipeNameLb with the new recipe name
                if (C2RecipeNameLb.InvokeRequired)
                {
                    C2RecipeNameLb.Invoke(new Action(() => C2RecipeNameLb.Text = newValue));
                }
                else
                {
                    C2RecipeNameLb.Text = newValue;
                }

            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating RecipeNameLb: {ex.Message}");
            }
        }
        private void C2CurrentRecipeID(object sender, ValueChangedEventArgs args)
        {
            try
            {
                string newValue = GetValidData(args.NewValue).ToString();
                if (C2CurrentIDTextBox.InvokeRequired)
                {
                    C2CurrentIDTextBox.Invoke(new Action(() => C2CurrentIDTextBox.Text = newValue));
                }
                else
                {
                    C2CurrentIDTextBox.Text = newValue;
                }
            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating CurrentIDTextBox: {ex.Message}");
            }
        }
        private void C2NewRecipeName(object sender, TextBoxValueChangedEventArgs args)
        {
            try
            {
                string newValue = args.NewValue.ToString();
                if (C2NewRecipeNameTextBox.InvokeRequired)
                {
                    C2NewRecipeNameTextBox.Invoke(new Action(() => C2NewRecipeNameTextBox.Text = newValue));
                }
                else
                {
                    C2NewRecipeNameTextBox.Text = newValue;
                }
            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating NewRecipeNameTextBox: {ex.Message}");
            }
        }
        private void C2NewRecipeNameTextBox_Click(object sender, EventArgs e)
        {
            using (PasswordForm passwordForm = new PasswordForm())
            {
                if (passwordForm.ShowDialog(this) == DialogResult.OK)
                {
                    C2NewRecipeNameTextBox.Text = passwordForm.Password;
                }
            }
        }
        private async void C2AddRecipeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                await C2AddRecipeBtn1.DoClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("An error occured while clicking on button {0}: {1}", this.Name, ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private async void C2DeleteRecipeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                await C2DeleteRecipeBtn1.DoClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("An error occured while clicking on button {0}: {1}", this.Name, ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void C3CurrentRecipeName(object sender, ValueChangedEventArgs args)
        {
            try
            {
                string newValue = args.NewValue.ToString();
                string oldRecipeName = C3RecipeNameLb.Text;

                // Update the RecipeNameLb with the new recipe name
                if (C3RecipeNameLb.InvokeRequired)
                {
                    C3RecipeNameLb.Invoke(new Action(() => C3RecipeNameLb.Text = newValue));
                }
                else
                {
                    C3RecipeNameLb.Text = newValue;
                }

            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating RecipeNameLb: {ex.Message}");
            }
        }
        private void C3CurrentRecipeID(object sender, ValueChangedEventArgs args)
        {
            try
            {
                string newValue = GetValidData(args.NewValue).ToString();
                if (C3CurrentIDTextBox.InvokeRequired)
                {
                    C3CurrentIDTextBox.Invoke(new Action(() => C3CurrentIDTextBox.Text = newValue));
                }
                else
                {
                    C3CurrentIDTextBox.Text = newValue;
                }
            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating CurrentIDTextBox: {ex.Message}");
            }
        }
        private void C3NewRecipeName(object sender, TextBoxValueChangedEventArgs args)
        {
            try
            {
                string newValue = args.NewValue.ToString();
                if (C3NewRecipeNameTextBox.InvokeRequired)
                {
                    C3NewRecipeNameTextBox.Invoke(new Action(() => C3NewRecipeNameTextBox.Text = newValue));
                }
                else
                {
                    C3NewRecipeNameTextBox.Text = newValue;
                }
            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating NewRecipeNameTextBox: {ex.Message}");
            }
        }
        private void C3NewRecipeNameTextBox_Click(object sender, EventArgs e)
        {
            using (PasswordForm passwordForm = new PasswordForm())
            {
                if (passwordForm.ShowDialog(this) == DialogResult.OK)
                {
                    C3NewRecipeNameTextBox.Text = passwordForm.Password;
                }
            }
        }
        private async void C3AddRecipeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                await C3AddRecipeBtn1.DoClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("An error occured while clicking on button {0}: {1}", this.Name, ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private async void C3DeleteRecipeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                await C3DeleteRecipeBtn1.DoClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("An error occured while clicking on button {0}: {1}", this.Name, ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void DashboardBtn_Click(object sender, EventArgs e)
        {
            C1RecipeListBox.Disconnect();
            C1AddRecipeBtn1.Disconnect();
            C1DeleteRecipeBtn1.Disconnect();
            C1NewRecipeNameTextBox.Disconnect();
            mainForm.Show();
            this.Close();
        }
        private double GetValidData(string newValue)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                Console.WriteLine("Received null or empty value.");
                return 0; // Default value for null or empty input
            }

            try
            {
                return Convert.ToDouble(newValue);
            }
            catch (FormatException)
            {
                Console.WriteLine("Value conversion error. Received invalid format.");
                return 0; // Default value for invalid format
            }
        }

        void ExecutionMessagesReceived(object sender, NewExecutionMessagesReceivedEventArgs args)
        {
            if (args.NewMessages.Count <= 0) return;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action<List<ExecutionMessage>>(UpdateMessageList), args.NewMessages);
            }
            else
            {
                UpdateMessageList(args.NewMessages);
            }
        }
        private bool _flushMessages = false;
        private void UpdateMessageList(List<ExecutionMessage> msgs)
        {
            if (_flushMessages)
            {
                _messagesListView.Items.Clear();
                _flushMessages = false;
            }
            foreach (ExecutionMessage message in msgs)
            {
                int imageIndex;
                switch (message.Severity)
                {
                    case MessageSeverity.Info:
                        imageIndex = 0;
                        break;

                    case MessageSeverity.Warning:
                        imageIndex = 1;
                        break;

                    case MessageSeverity.Error:
                        imageIndex = 2;
                        break;

                    default:
                        imageIndex = 0;
                        break;
                }

                ListViewItem listViewItem = new ListViewItem(message.Content, imageIndex);
                _messagesListView.Items.Insert(0, listViewItem);
                _columnHeader.Width = -2;
            }
            // Show the ListView and start/reset the hide timer
            _messagesListView.Visible = true;
            hideTimer.Stop();
            hideTimer.Start();

        }
        private void HideTimer_Tick(object sender, EventArgs e)
        {
            _messagesListView.Visible = false;
            hideTimer.Stop();
        }
        // Pops up your PasswordForm and sets all three NewRecipeNameTextBox.Text values
        private void MasterNewRecipeName_Click(object sender, EventArgs e)
        {
            string newRecipeName = "";
            using (var passwordForm = new PasswordForm())
            {
                if (passwordForm.ShowDialog(this) == DialogResult.OK)
                {
                    newRecipeName = passwordForm.Password;
                }
                else
                {
                    return; // User cancelled, do nothing
                }
            }
            // Set the new recipe name in all three project's textboxes
            C1NewRecipeNameTextBox.Text = newRecipeName;
            C2NewRecipeNameTextBox.Text = newRecipeName;
            C3NewRecipeNameTextBox.Text = newRecipeName;
        }

        // Adds the recipe (must have already set the name using MasterNewRecipeName)
        private async void MasterAddRecipe_Click(object sender, EventArgs e)
        {
            await C1AddRecipeBtn1.DoClick();
            await C2AddRecipeBtn1.DoClick();
            await C3AddRecipeBtn1.DoClick();
        }

        // Deletes the selected recipe in all three projects
        private async void MasterDeleteRecipe_Click(object sender, EventArgs e)
        {
            await C1DeleteRecipeBtn1.DoClick();
            await C2DeleteRecipeBtn1.DoClick();
            await C3DeleteRecipeBtn1.DoClick();
        }
    }

}

