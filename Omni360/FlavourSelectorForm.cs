using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Omnicheck360
{
    public partial class FlavourSelectorForm : Form
    {
        public string SelectedSize { get; private set; }
        public string SelectedFlavour { get; private set; }
        public string EnteredBarcode { get; private set; }
        public BarcodeItem SelectedBarcodeItem { get; private set; }

        private TextBox txtBarcodeInput;
        private Button btnEnterBarcode;
        private Button exitButton;
        private Dictionary<string, List<BarcodeItem>> _flavoursBySizeWithItems;

        public FlavourSelectorForm(Dictionary<string, List<BarcodeItem>> flavoursBySizeWithItems)
        {
            InitializeComponent();
            _flavoursBySizeWithItems = flavoursBySizeWithItems;
            BuildDynamicUI(_flavoursBySizeWithItems);
            ConsoleCheckAllFlavours();
        }

        private void BuildDynamicUI(Dictionary<string, List<BarcodeItem>> flavoursBySizeWithItems)
        {
            this.Controls.Clear();
            this.Text = "Select Flavour";
            this.Size = new Size(900, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;

            // Outer panel
            var outerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            this.Controls.Add(outerPanel);

            // Top panel for barcode input
            var topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White
            };
            outerPanel.Controls.Add(topPanel);

            var lblBarcode = new Label
            {
                Text = "Scannez ou saisissez le code-barres :",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Location = new Point(24, 16),
                AutoSize = true
            };
            topPanel.Controls.Add(lblBarcode);

            txtBarcodeInput = new TextBox
            {
                Name = "txtBarcodeInput",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Location = new Point(360, 12),
                Width = 300
            };
            topPanel.Controls.Add(txtBarcodeInput);

            btnEnterBarcode = new Button
            {
                Text = "Entrer",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Location = new Point(680, 10),
                Width = 70,
                Height = 32
            };
            btnEnterBarcode.Click += BtnEnterBarcode_Click;
            topPanel.Controls.Add(btnEnterBarcode);

            // Main panel for columns
            var mainPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(20, 50, 20, 60)
            };
            outerPanel.Controls.Add(mainPanel);

            foreach (var kvp in flavoursBySizeWithItems)
            {
                string size = kvp.Key;
                List<BarcodeItem> flavourItemList = kvp.Value;
                if (flavourItemList.Count == 0) continue;

                var columnPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    Width = 220,
                    Height = 400,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(12),
                    Padding = new Padding(4, 12, 4, 12),
                    AutoScroll = true
                };


                foreach (var item in flavourItemList)
                {
                    var btn = new Button
                    {
                        Text = $"Code: {item.CodeDeProduit ?? ""}\nDescription: {item.Description}",
                        Width = 200,
                        Height = 70,
                        Margin = new Padding(8, 4, 8, 4),
                        Tag = item,
                        Font = new Font("Segoe UI", 10, FontStyle.Regular),
                        AutoSize = false,
                        UseCompatibleTextRendering = true,
                        TextAlign = ContentAlignment.MiddleLeft
                    };
                    btn.Click += FlavourButton_Click;
                    columnPanel.Controls.Add(btn);
                }

                mainPanel.Controls.Add(columnPanel);
            }

            // Exit button
            exitButton = new Button
            {
                Size = new Size(40, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                TabStop = false,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            exitButton.FlatAppearance.BorderSize = 0;
            string exitIconPath = @"C:\\Users\\Omnicheck_LDS\\Desktop\\OmniCheck\\Omni360\\Resources\\Exit48x48.png";
            if (File.Exists(exitIconPath))
            {
                exitButton.Image = Image.FromFile(exitIconPath);
            }
            else
            {
                exitButton.Text = "✖";
                exitButton.Font = new Font("Segoe UI", 16, FontStyle.Bold);
                exitButton.ForeColor = Color.DarkGray;
            }
            // Add to outer panel, but float above the main area
            outerPanel.Controls.Add(exitButton);
            PositionExitButton();

            outerPanel.Resize += (s, e) => PositionExitButton();

            // Enter key triggers barcode enter
            txtBarcodeInput.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    BtnEnterBarcode_Click(btnEnterBarcode, EventArgs.Empty);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            };

            exitButton.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };
        }

        private void PositionExitButton()
        {
            if (exitButton?.Parent is Control parent)
            {
                exitButton.Location = new Point(parent.Width - exitButton.Width - 8, parent.Height - exitButton.Height - 8);
                exitButton.BringToFront();
            }
        }

        private void BtnEnterBarcode_Click(object sender, EventArgs e)
        {
            EnteredBarcode = txtBarcodeInput.Text.Trim();
            if (!string.IsNullOrEmpty(EnteredBarcode))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a barcode.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FlavourButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is BarcodeItem item)
            {
                SelectedSize = item.CodeDeProduit?.ToString();
                SelectedFlavour = item.Description;
                SelectedBarcodeItem = item;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        private void ConsoleCheckAllFlavours()
        {
            if (_flavoursBySizeWithItems == null)
            {
                Console.WriteLine("No flavour data loaded.");
                return;
            }

            foreach (var kvp in _flavoursBySizeWithItems)
            {
                string size = kvp.Key;
                var items = kvp.Value;
                Console.WriteLine($"Size: '{size}' - {items.Count} item(s)");
                foreach (var item in items)
                {
                    Console.WriteLine(
                        $"  Code: {item.CodeDeProduit}, Desc: {item.Description}, UPC: {item.UPC ?? "N/A"}"
                    );
                }
            }
        }
    }
}