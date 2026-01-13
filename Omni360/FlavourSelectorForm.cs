using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MatroxLDS
{
    public partial class FlavourSelectorForm : Form
    {
        public string SelectedSize { get; private set; }
        public string SelectedFlavour { get; private set; }

        public FlavourSelectorForm(Dictionary<string, List<string>> flavoursBySize)
        {
            InitializeComponent();
            BuildDynamicUI(flavoursBySize);
        }

        private void BuildDynamicUI(Dictionary<string, List<string>> flavoursBySize)
        {
            this.Controls.Clear();

            this.Text = "Select Flavour";
            this.Size = new Size(900, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None; // Remove window border and title bar

            // Outer panel to hold everything including the exit button
            var outerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            this.Controls.Add(outerPanel);

            // Exit (close) button with PNG icon
            var exitButton = new Button
            {
                Size = new Size(40, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                TabStop = false,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            exitButton.FlatAppearance.BorderSize = 0;

            // Load the image from the file path
            string exitIconPath = @"C:\Users\ZUser\Desktop\OmniCheck 5x\Omni360\Resources\Exit48x48.png";
            if (File.Exists(exitIconPath))
            {
                exitButton.Image = Image.FromFile(exitIconPath);
            }
            else
            {
                // Fallback to Unicode icon if image not found
                exitButton.Text = "✖";
                exitButton.Font = new Font("Segoe UI", 16, FontStyle.Bold);
                exitButton.ForeColor = Color.DarkGray;
            }

            // Initially position exit button in bottom-right corner
            exitButton.Location = new Point(this.Width - exitButton.Width - 8, this.Height - exitButton.Height - 8);
            exitButton.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            // Make sure button stays at bottom-right on resize
            outerPanel.Resize += (s, e) => {
                exitButton.Location = new Point(outerPanel.Width - exitButton.Width - 8, outerPanel.Height - exitButton.Height - 8);
            };
            outerPanel.Controls.Add(exitButton);

            // Main flow panel for columns (sizes)
            var mainPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(20, 20, 20, 60) // leave bottom space for exit button
            };
            outerPanel.Controls.Add(mainPanel);

            foreach (var kvp in flavoursBySize)
            {
                string size = kvp.Key;
                List<string> flavourList = kvp.Value;
                if (flavourList.Count == 0) continue;

                // Use a vertical FlowLayoutPanel for each column
                var columnPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    Width = 180,
                    Height = 400,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(12),
                    Padding = new Padding(4, 12, 4, 12),
                    AutoScroll = true
                };

                // Size label at the top
                var lbl = new Label
                {
                    Text = size,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Width = 160,
                    Height = 40,
                    Margin = new Padding(8, 4, 8, 8)
                };
                columnPanel.Controls.Add(lbl);

                foreach (string flavour in flavourList)
                {
                    var btn = new Button
                    {
                        Text = flavour,
                        Width = 160,
                        Height = 36,
                        Margin = new Padding(8, 4, 8, 4),
                        Tag = new Tuple<string, string>(size, flavour),
                        Font = new Font("Segoe UI", 10, FontStyle.Regular)
                    };
                    btn.Click += FlavourButton_Click;
                    columnPanel.Controls.Add(btn);
                }

                mainPanel.Controls.Add(columnPanel);
            }
        }

        private void FlavourButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Tuple<string, string> tag)
            {
                SelectedSize = tag.Item1;
                SelectedFlavour = tag.Item2;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}