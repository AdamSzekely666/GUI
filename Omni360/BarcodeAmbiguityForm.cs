using Omnicheck360;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Omnicheck360
{
    public class BarcodeAmbiguityForm : Form
    {
        // Now uses the shared BarcodeItem class
        public BarcodeItem SelectedItem { get; private set; }

        public BarcodeAmbiguityForm(List<BarcodeItem> matches)
        {
            this.Text = "Select Product";
            this.Size = new Size(1100, 360);
            this.MinimumSize = new Size(350, 250);
            this.StartPosition = FormStartPosition.CenterParent;

            var listBox = new ListBox
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 16),
                Height = 200,
                IntegralHeight = false
            };

            foreach (var item in matches)
            {
                listBox.Items.Add(
                    $"Index: {item.Index ?? 0} | Product: {item.CodeDeProduit ?? ""} | Description: {item.Description} | Recipes: {item.Recipes ?? 0} | UPC: {item.UPC}"
                );
            }

            // Double-click to select
            listBox.DoubleClick += (s, e) =>
            {
                if (listBox.SelectedIndex >= 0)
                {
                    SelectedItem = matches[listBox.SelectedIndex];
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            };

            this.Controls.Add(listBox);

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Height = 50,
                Padding = new Padding(10),
                AutoSize = true
            };

            var btnOk = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Width = 80
            };
            btnOk.Click += (s, e) =>
            {
                if (listBox.SelectedIndex >= 0)
                {
                    SelectedItem = matches[listBox.SelectedIndex];
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please select an option.");
                }
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Width = 80
            };
            btnCancel.Click += (s, e) =>
            {
                SelectedItem = null;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            buttonPanel.Controls.Add(btnOk);
            buttonPanel.Controls.Add(btnCancel);

            this.Controls.Add(buttonPanel);
        }
    }
}