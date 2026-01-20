using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Omnicheck360.Properties;
using Nini.Config;
using System.Diagnostics;

namespace Omnicheck360
{
    public partial class AdminScreen : BaseForm
    {
        public MainMenu MainMenu;
        public MainForm mainForm;
        public PopUpNumberPad popUpNumberPad;
        private string _documentsDirectory;
        private static IniConfigSource source;
        public AdminScreen(MainForm _mainForm)
        {
            InitializeComponent();
            ButtonAnimator.InitializeAnimation(MainMenuFormBtn, "blue");
            ButtonAnimator.InitializeAnimation(SecuritySettingsBtn, "blue");

            mainForm = _mainForm;
            LoadConfigs();
            _documentsDirectory = System.Environment.CurrentDirectory;
        }
        private void AdminScreen_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            timer1.Start();
            CurrentUserTxt.Text = mainForm.CurrentUserTxt.Text;
        }
        public void LoadConfigs()
        {
            string cPath;
            string IniFile;

            cPath = System.Environment.CurrentDirectory;
            IniFile = cPath + "\\app.ini";

            source = new IniConfigSource(IniFile);

            txtOperatorPassword.Text = source.Configs["User"].Get("Operator");
            txtMechanicPassword.Text = source.Configs["User"].Get("Electrician");
            txtAdministratorPassword.Text = source.Configs["User"].Get("Administrator");
            
        }
        private void txtOperatorPassword_TextChanged()
        {
            MainForm.log.Info("Access Control Page: Operator Password changed to" + txtOperatorPassword.Text);
            source.Configs["User"].Set("Operator", txtOperatorPassword.Text);
            source.Save();
        }
        private void txtOperatorPassword_Click(object sender, EventArgs e)
        {

            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    txtOperatorPassword.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            txtOperatorPassword_TextChanged();
            HandleTextBoxFocusLoss();

        }
        private void txtMechanicPassword_TextChanged()
        {
            MainForm.log.Info("Access Control Page: Technician Password changed to" + txtMechanicPassword.Text);
            source.Configs["User"].Set("Electrician", txtMechanicPassword.Text);
            source.Save();
        }
        private void txtMechanicPassword_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    txtMechanicPassword.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();
            txtMechanicPassword_TextChanged();
        }
        private void txtAdministratorPassword_TextChanged()
        {
            MainForm.log.Info("Access Control Page: Administrator Password changed to" + txtAdministratorPassword.Text);
            source.Configs["User"].Set("Administrator", txtAdministratorPassword.Text);
            source.Save();
        }
        private void txtAdministratorPassword_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    txtAdministratorPassword.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();
            txtAdministratorPassword_TextChanged();
        }
        private void SecuritySettingsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Path to the SQLite program (change this to the correct program path)
                string sqliteProgramPath = @"C:\Program Files\DB Browser for SQLite\DB Browser for SQLite.exe";

                // Path to the SQLite database file (located in the debug folder)
                string cFilename = Path.Combine(Environment.CurrentDirectory, "omnisec.db");

                // Check if the database file exists
                if (!File.Exists(cFilename))
                {
                    MessageBox.Show($"Database file not found: {cFilename}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Open the SQLite database file using the specified program
                Process.Start(sqliteProgramPath, cFilename);

                MainForm.log.Info("Access Control Page: Security Settings Button Pressed");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while opening the SQLite database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.log.Error($"Error opening SQLite database: {ex.Message}");
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainForm.log.Info("Access Control Page: Change to French Box is Checked");
            // InitializeComponent();
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                    isfrench = false;
                    //this.Controls.Clear();
                    InitializeComponent();
                    break;

                case 1:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-CA");
                    isfrench = true;
                    //this.Controls.Clear();
                    InitializeComponent();
                    break;

            }

            

        }
        public void HandleTextBoxFocusLoss()
        {
            this.hiddenButton.Focus(); // Ensure this sets the focus to the hidden button
        }
        private void MainMenuFormBtn_Click(object sender, EventArgs e)
        {
            MainForm.log.Info("Access Control Page: Dashboard Button Pressed");
            this.Close();
            mainForm.Show();

        }
    }
}
