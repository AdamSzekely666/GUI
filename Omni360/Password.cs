using Nini.Config;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace Omnicheck360
{
    public partial class PasswordForm : Form
    {
        private TextBox activeTextBox;
        private string buttonColor = "blue"; // Default color, change as needed
        private MainMenu mainMenuForm;
        private bool isShiftOn = false;
        private string _txtPasswd;
        private MainMenu _mainMenu;
        private AdminScreen adminScreen;
       // private string _documentsDirectory;
        private static IniConfigSource source;

        public string Password { get; private set; }
        public PasswordForm(MainMenu menuForm)
        {
            InitializeComponent();
            mainMenuForm = menuForm;
            this.ClientSize = new Size(1000, 370); // Adjust the form size
            this.BackColor = Color.Black;
            CreateTextBox();
            CreateKeyboard();
            CreateNumberPad();

        }
        private void CreateTextBox()
        {
            TextBox inputTextBox = new TextBox();
            inputTextBox.Location = new Point(300, 20);
            inputTextBox.Size = new Size(500, 50); // Adjust size
            inputTextBox.Font = new Font(inputTextBox.Font.FontFamily, 20); // Increase font size
            inputTextBox.ReadOnly = true; // Make it read-only to avoid manual edits
            this.Controls.Add(inputTextBox);
            activeTextBox = inputTextBox; // Use this TextBox for input display

        }
        private void CreateKeyboard()
        {
            // QWERTY layout
            string[] keyRows = new string[]
            {
        "1234567890",
        "qwertyuiop",
        "asdfghjkl",
            };

            int x = 10, y = 100; // Initial position, leaving space for the TextBox
            int buttonSize = 60; // Button size for usability
            int buttonSpacing = 5; // Space between buttons

            foreach (string row in keyRows)
            {
                foreach (char key in row)
                {
                    Button btn = new Button();
                    btn.Text = key.ToString();
                    btn.Tag = key;
                    btn.Click += KeyButton_Click;
                    btn.Size = new Size(buttonSize, buttonSize);
                    btn.Location = new Point(x, y);
                    btn.Font = new Font(btn.Font.FontFamily, 20);
                    ButtonAnimator.InitializeAnimation(btn, buttonColor);
                    this.Controls.Add(btn);

                    x += buttonSize + buttonSpacing; // Adjust spacing
                }

                x = 10; // Reset X position for the next row
                y += buttonSize + buttonSpacing; // Move to the next row
            }

            // Add Backspace next to 0 on the top row
            Button backspaceBtn = new Button();
            backspaceBtn.Text = "Backspace";
            backspaceBtn.Click += BackspaceButton_Click;
            backspaceBtn.Size = new Size(buttonSize * 2, buttonSize);
            backspaceBtn.Location = new Point(10 + 10 * (buttonSize + buttonSpacing), 100);
            backspaceBtn.Font = new Font(backspaceBtn.Font.FontFamily, 20);
            ButtonAnimator.InitializeAnimation(backspaceBtn, buttonColor);
            this.Controls.Add(backspaceBtn);

            // Add Enter next to L on the third row
            Button enterBtn = new Button();
            enterBtn.Text = "Enter";
            enterBtn.Click += EnterButton_Click;
            enterBtn.Size = new Size(buttonSize * 2, buttonSize);
            enterBtn.Location = new Point(10 + 9 * (buttonSize + buttonSpacing), 100 + 2 * (buttonSize + buttonSpacing));
            enterBtn.Font = new Font(enterBtn.Font.FontFamily, 20);
            ButtonAnimator.InitializeAnimation(enterBtn, buttonColor);
            this.Controls.Add(enterBtn);

            // Add Shift button and position it below the third row
            Button shiftBtn = new Button();
            shiftBtn.Text = "Shift";
            shiftBtn.Click += ShiftButton_Click; // Ensure ShiftButton_Click is used
            shiftBtn.Size = new Size(buttonSize * 2, buttonSize); // Adjust size as needed
            shiftBtn.Location = new Point(10, 100 + 3 * (buttonSize + buttonSpacing)); // Below the third row
            shiftBtn.Font = new Font (Font.FontFamily, 20);
            ButtonAnimator.InitializeAnimation(shiftBtn, buttonColor);
            this.Controls.Add(shiftBtn);

            // Adjust position of ZXCVBNM row to start to the right of Shift button
            x = shiftBtn.Location.X + shiftBtn.Width + buttonSpacing;
            y = shiftBtn.Location.Y;

            string lowerRow = "zxcvbnm";
            foreach (char key in lowerRow)
            {
                Button btn = new Button();
                btn.Text = key.ToString();
                btn.Tag = key;
                btn.Click += KeyButton_Click;
                btn.Size = new Size(buttonSize, buttonSize);
                btn.Location = new Point(x, y);
                btn.Font = new Font(Font.FontFamily, 20);
                ButtonAnimator.InitializeAnimation(btn, buttonColor);
                this.Controls.Add(btn);

                x += buttonSize + buttonSpacing; // Adjust spacing
            }
        }
        private void CreateNumberPad()
        {
            // Number pad layout
            string[] numberPadRows = new string[]
            {
        "123",
        "456",
        "789",
        "0"
            };

            int x = 800, y = 100; // Position it on the right side of the form
            int buttonSize = 60;
            int buttonSpacing = 5;

            foreach (string row in numberPadRows)
            {
                foreach (char key in row)
                {
                    Button btn = new Button();
                    btn.Text = key.ToString();
                    btn.Tag = key;
                    btn.Click += KeyButton_Click;
                    btn.Size = new Size(buttonSize, buttonSize);
                    btn.Location = new Point(x, y);
                    btn.Font = new Font(btn.Font.FontFamily,20);
                    ButtonAnimator.InitializeAnimation(btn, buttonColor);
                    this.Controls.Add(btn);

                    x += buttonSize + buttonSpacing; // Adjust horizontal spacing
                }

                x = 800; // Reset X position for the next row
                y += buttonSize + buttonSpacing; // Move to the next row
            }
        }
        private void BackspaceButton_Click(object sender, EventArgs e)
        {
            if (activeTextBox.Text.Length > 0)
            {
                activeTextBox.Text = activeTextBox.Text.Substring(0, activeTextBox.Text.Length - 1);
            }
        }
        private void EnterButton_Click(object sender, EventArgs e)
        {
            Password = activeTextBox.Text; // Capture password from TextBox
            DialogResult = DialogResult.OK; // Close form and set result
            this.Close();

            string sUser;
            bool lClose = true;
            string cPath;
            string IniFile;

            cPath = System.Environment.CurrentDirectory;
            IniFile = cPath + "\\app.ini";

            source = new IniConfigSource(IniFile);


            if (Password == source.Configs["User"].Get("Administrator"))
            {
                _txtPasswd = "0000";
                //txtDisplayText.Text = "0000";

                MainForm.log.Info("New User Logged In: ADMINISTRATOR");
                sUser = "Administrateur";
                MainForm.nUserAccess = 3;
                mainMenuForm.mainForm.DoActiveButtons(3);
                mainMenuForm.UserLoginBtn.Text = "LOGOUT";
            }

            else if (Password == source.Configs["User"].Get("Electrician"))
            {
                _txtPasswd = "0000";
                //txtDisplayText.Text = "0000";
                sUser = "Entretien";

                MainForm.log.Info("New User Logged In: TECHNICIAN");
                MainForm.nUserAccess = 2;
                mainMenuForm.mainForm.DoActiveButtons(2);
                mainMenuForm.UserLoginBtn.Text = "LOGOUT";
            }
            else if (Password == source.Configs["User"].Get("Operator"))
            {
                _txtPasswd = "0000";
                //txtDisplayText.Text = "0000";
                sUser = "Opérateur";

                MainForm.log.Info("New User Logged In: OPERATOR");
                MainForm.nUserAccess = 1;
                mainMenuForm.mainForm.DoActiveButtons(1);
                mainMenuForm.UserLoginBtn.Text = "LOGOUT";
            }
            else if (Password == source.Configs["User"].Get("Omnifission"))
            {
                _txtPasswd = "0000";
                //txtDisplayText.Text = "0000";
                sUser = "Omnifission";

                MainForm.log.Info("New User Logged In: OMNIFISSION");
                MainForm.nUserAccess = 4;
                mainMenuForm.mainForm.DoActiveButtons(4);
                mainMenuForm.UserLoginBtn.Text = "LOGOUT";
            }

            else
            {
                _txtPasswd = "";
                //txtDisplayText.Text = "";
                sUser = "Mode opérateur";

                MainForm.log.Info("Invalid Password Entered");
                MainForm.nUserAccess = 0;
                mainMenuForm.mainForm.DoActiveButtons(0);
                lClose = false;
            }

            //Update the other screens
            mainMenuForm.ShowUser(sUser);
            // Detach the CartesianChart and reattach it to MainForm
            if (MainForm.Instance != null)
            {
                //ChartPanelCamera1.Controls.Remove(MainForm.Instance.CartesianChart); // Remove from Camera1InspectionSetup
                //MainForm.Instance.ChartPanel.Controls.Add(MainForm.Instance.CartesianChart); // Add back to MainForm
                //MainForm.Instance.CartesianChart.Dock = DockStyle.Fill;
            }

            if (lClose)
            {
                mainMenuForm.TopMost = true;
                Close();
                mainMenuForm.Close();
            }

        }
        private void KeyButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                // Add text to activeTextBox based on the button's text
                activeTextBox.Text += btn.Text;

                // Optionally, reset shift state after a key press
                if (isShiftOn)
                {
                    ShiftButton_Click(sender, e);
                    isShiftOn = false;
                }
            }
        }
        private void ShiftButton_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn && btn.Tag != null)
                {
                    if (btn.Text.Length == 1 && char.IsLetter(btn.Text[0]))
                    {
                        btn.Text = isShiftOn ? btn.Text.ToLower() : btn.Text.ToUpper();
                    }
                    else if (btn.Text.Length == 1 && (char.IsDigit(btn.Text[0]) || "!@#$%^&*()".Contains(btn.Text[0])))
                    {
                        // Skip the right number pad by checking the location
                        if (btn.Location.X < 600) // Assuming the number pad starts at X=600
                        {
                            btn.Text = isShiftOn ? GetNumber(btn.Text[0]) : GetSymbol(btn.Text[0]);
                        }
                    }
                }
            }
            isShiftOn = !isShiftOn; // Toggle the state of isShiftOn
        }
        private string GetSymbol(char number)
        {
            switch (number)
            {
                case '1': return "!";
                case '2': return "@";
                case '3': return "#";
                case '4': return "$";
                case '5': return "%";
                case '6': return "^";
                case '7': return "&";
                case '8': return "*";
                case '9': return "(";
                case '0': return ")";
                default: return number.ToString();
            }
        }
        private string GetNumber(char symbol)
        {
            switch (symbol)
            {
                case '!': return "1";
                case '@': return "2";
                case '#': return "3";
                case '$': return "4";
                case '%': return "5";
                case '^': return "6";
                case '&': return "7";
                case '*': return "8";
                case '(': return "9";
                case ')': return "0";
                default: return symbol.ToString();
            }
        }

    }
}