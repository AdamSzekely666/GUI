using System;
using System.Drawing;
using System.Windows.Forms;

namespace MatroxLDS
{
    public partial class PopUpNumberPad : Form
    {
        public string Number { get; private set; }
        private TextBox activeTextBox;


        public PopUpNumberPad()
        {
            InitializeComponent();
            this.ClientSize = new Size(270, 500); // Adjust the form size
            this.BackColor = Color.Black;
            CreateTextBox();
            CreateNumberPad();

        }

        private void CreateTextBox()
        {
            activeTextBox = new TextBox
            {
                Location = new Point(40, 20),
                Size = new Size(190, 200), // Adjust size
                Font = new Font("Microsoft Sans Serif", 20), // Increase font size
                ReadOnly = true // Make it read-only to avoid manual edits
            };
            this.Controls.Add(activeTextBox); // Add TextBox to the form
        }

        private void CreateNumberPad()
        {
            // Number pad layout
            string[] numberPadRows = new string[]
            {
                "123",
                "456",
                "789",
                "0.-"
            };

            int x = 40, y = 80; // Initial position, leaving space for the TextBox
            int buttonSize = 60; // Button size for usability
            int buttonSpacing = 5; // Space between buttons

            foreach (string row in numberPadRows)
            {
                foreach (char key in row)
                {
                    Button btn = new Button
                    {
                        Text = key.ToString(),
                        Tag = key,
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(x, y),
                        Font = new Font("Microsoft Sans Serif", 20)
                    };
                    btn.Click += KeyButton_Click;
                    ButtonAnimator.InitializeAnimation(btn, "blue"); // Ensure the animation
                    this.Controls.Add(btn);
                    x += buttonSize + buttonSpacing; // Adjust spacing
                }

                x = 40; // Reset X position for the next row
                y += buttonSize + buttonSpacing; // Move to the next row
            }

            // Add Enter button
            CreateButton("Enter", EnterButton_Click, 105, 365, 120, 60);

            // Add Cancel button
            CreateButton("Cancel", CancelButton_Click, 105, 430, 120, 60);

            // Add Up and Down Arrow buttons for increase/decrease
            CreateButton("↑", UpButton_Click, 40, 365, 60,60);
            CreateButton("↓", DownButton_Click, 40, 430, 60,60);
        }

        private void CreateButton(string text, EventHandler clickEvent, int x, int y, int sizex, int sizey)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(sizex, sizey),
                Location = new Point(x, y),
                Font = new Font("Microsoft Sans Serif", 20)
            };
            btn.Click += clickEvent;
            ButtonAnimator.InitializeAnimation(btn, "blue"); // Ensure the animation
            this.Controls.Add(btn);
        }

        private void KeyButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                activeTextBox.Text += btn.Text;
            }
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            Number = activeTextBox.Text; // Capture number from TextBox
            DialogResult = DialogResult.OK; // Close form and set result


            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            if (int.TryParse(activeTextBox.Text, out int number))
            {
                activeTextBox.Text = (number + 1).ToString();
            }
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            if (int.TryParse(activeTextBox.Text, out int number))
            {
                activeTextBox.Text = (number - 1).ToString();
            }
        }
    }
}
