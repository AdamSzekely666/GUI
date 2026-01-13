using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MatroxLDS
{
    public partial class PopUpTimePicker : Form
    {
        private int hour = 12;
        private int minute = 0;
        private bool isPM = false;
        private enum EditPart { Hour, Minute, AmPm }
        private EditPart currentPart = EditPart.Hour;

        private Label lblTimeDisplay;
        private Button btnAmPm;
        private Button btnEnter;
        private Button btnCancel;
        private Button btnUp;
        private Button btnDown;
        private Button[] numPadButtons = new Button[12];
        private List<Button> timePartButtons = new List<Button>();

        // Result
        public DateTime? SelectedTime { get; private set; }

        public PopUpTimePicker()
        {
            InitializeComponent();
            this.ClientSize = new Size(290, 420); // Same as PopUpNumberPad
            this.BackColor = Color.Black;         // Same as PopUpNumberPad
            CreateTimeDisplay();
            CreateNumberPad();
            CreateAmPmButton();
            CreateControlButtons();
            UpdateTimeDisplay();
        }

        private void CreateTimeDisplay()
        {
            lblTimeDisplay = new Label
            {
                Location = new Point(20, 20), // Adjusted for new form size
                Size = new Size(250, 60),
                Font = new Font("Microsoft Sans Serif", 32, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.DimGray,
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };
            lblTimeDisplay.Click += (s, e) =>
            {
                // Cycle edited part: Hour → Minute → AM/PM
                currentPart = (EditPart)(((int)currentPart + 1) % 3);
                HighlightCurrentPart();
            };
            this.Controls.Add(lblTimeDisplay);
        }

        private void CreateNumberPad()
        {
            string[] rows = { "123", "456", "789", "0  " };
            int x0 = 20, y0 = 100, buttonSize = 60, spacing = 5;
            int idx = 0;
            for (int row = 0; row < rows.Length; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    char key = rows[row][col];
                    if (key == ' ') continue;
                    Button btn = new Button
                    {
                        Text = key.ToString(),
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(x0 + col * (buttonSize + spacing), y0 + row * (buttonSize + spacing)),
                        Font = new Font("Microsoft Sans Serif", 16, FontStyle.Bold),
                        ForeColor = Color.Black,
                        BackColor = Color.LightGray,
                        FlatStyle = FlatStyle.Standard
                    };
                    btn.Click += NumPadButton_Click;
                    this.Controls.Add(btn);
                    if (idx < numPadButtons.Length)
                        numPadButtons[idx++] = btn;
                    timePartButtons.Add(btn);
                }
            }

            btnUp = new Button
            {
                Text = "↑",
                Size = new Size(60, 60),
                Location = new Point(x0 + 3 * (buttonSize + spacing), y0 + 0 * (buttonSize + spacing)),
                Font = new Font("Microsoft Sans Serif", 16, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Standard
            };
            btnUp.Click += BtnUp_Click;
            this.Controls.Add(btnUp);
            timePartButtons.Add(btnUp);

            btnDown = new Button
            {
                Text = "↓",
                Size = new Size(60, 60),
                Location = new Point(x0 + 3 * (buttonSize + spacing), y0 + 1 * (buttonSize + spacing)),
                Font = new Font("Microsoft Sans Serif", 16, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Standard
            };
            btnDown.Click += BtnDown_Click;
            this.Controls.Add(btnDown);
            timePartButtons.Add(btnDown);
        }
        private void CreateAmPmButton()
        {
            int buttonSize = 60, spacing = 5, x0 = 20, y0 = 100;
            int col = 2, row = 3;
            int ampmX = x0 + col * (buttonSize + spacing);
            int ampmY = y0 + row * (buttonSize + spacing);

            btnAmPm = new Button
            {
                Text = isPM ? "PM" : "AM",
                Size = new Size(buttonSize, 40),
                Location = new Point(ampmX, ampmY),
                Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Standard
            };
            btnAmPm.Click += (s, e) =>
            {
                isPM = !isPM;
                btnAmPm.Text = isPM ? "PM" : "AM";
                UpdateTimeDisplay();
            };
            this.Controls.Add(btnAmPm);
        }

        private void CreateControlButtons()
        {
            CreateButton("Enter", EnterButton_Click, 20, 360, 90, 40, Color.LightGray, Color.Black, 16);
            CreateButton("Cancel", CancelButton_Click, 140, 360, 120, 40, Color.LightGray, Color.Black, 16);
        }
        private void CreateButton(string text, EventHandler clickEvent, int x, int y, int sizex, int sizey, Color backColor, Color foreColor, float fontSize)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(sizex, sizey),
                Location = new Point(x, y),
                Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Bold),
                BackColor = backColor,
                ForeColor = foreColor,
                FlatStyle = FlatStyle.Standard
            };
            btn.Click += clickEvent;
            this.Controls.Add(btn);
            timePartButtons.Add(btn); // <--- Add to shared list!
            if (text == "Enter") btnEnter = btn;
            if (text == "Cancel") btnCancel = btn;
        }
        private void NumPadButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            int value = int.Parse(btn.Text);
            switch (currentPart)
            {
                case EditPart.Hour:
                    hour = (hour < 10) ? value : (hour % 10) * 10 + value;
                    if (hour == 0) hour = 1;
                    if (hour > 12) hour = value; // Don't allow >12
                    break;
                case EditPart.Minute:
                    minute = (minute < 10) ? value : (minute % 10) * 10 + value;
                    if (minute > 59) minute = value;
                    break;
            }
            UpdateTimeDisplay();
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            switch (currentPart)
            {
                case EditPart.Hour:
                    hour = hour < 12 ? hour + 1 : 1;
                    break;
                case EditPart.Minute:
                    minute = minute < 59 ? minute + 1 : 0;
                    break;
            }
            UpdateTimeDisplay();
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            switch (currentPart)
            {
                case EditPart.Hour:
                    hour = hour > 1 ? hour - 1 : 12;
                    break;
                case EditPart.Minute:
                    minute = minute > 0 ? minute - 1 : 59;
                    break;
            }
            UpdateTimeDisplay();
        }

        private void UpdateTimeDisplay()
        {
            string hourStr = hour.ToString("D2");
            string minStr = minute.ToString("D2");
            string ampmStr = isPM ? "PM" : "AM";
            lblTimeDisplay.Text = $"{hourStr}:{minStr} {ampmStr}";
            HighlightCurrentPart();
        }

        private void HighlightCurrentPart()
        {
            Color partColor;
            switch (currentPart)
            {
                case EditPart.Hour:
                    partColor = Color.DeepSkyBlue;
                    lblTimeDisplay.BackColor = Color.DeepSkyBlue;
                    btnAmPm.BackColor = Color.LightGray;
                    break;
                case EditPart.Minute:
                    partColor = Color.MediumPurple;
                    lblTimeDisplay.BackColor = Color.MediumPurple;
                    btnAmPm.BackColor = Color.LightGray;
                    break;
                case EditPart.AmPm:
                default:
                    partColor = Color.DimGray;
                    lblTimeDisplay.BackColor = Color.DimGray;
                    btnAmPm.BackColor = Color.Orange;
                    break;
            }

            // Set all time part buttons to match current part color
            foreach (var btn in timePartButtons)
                btn.BackColor = partColor;

            lblTimeDisplay.ForeColor = Color.White;
        }
        /// <summary>
        /// Set the picker to a specific time (useful for pre-filling).
        /// </summary>
        public void SetTime(DateTime time)
        {
            hour = time.Hour % 12;
            if (hour == 0) hour = 12;
            minute = time.Minute;
            isPM = time.Hour >= 12;
            UpdateTimeDisplay();
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            int h = hour % 12;
            if (isPM) h += 12;
            SelectedTime = DateTime.Today.AddHours(h).AddMinutes(minute);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}