using System;
using System.Windows.Forms;

public class BottleSizeSelector : Form
{
    public string SelectedBottleSize { get; private set; }
    private ComboBox comboBox;
    private Button okButton;
    private Button cancelButton;

    public BottleSizeSelector(string[] bottleSizes)
    {
        this.Text = "Select Bottle Size";
        this.Width = 300;
        this.Height = 130;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.ShowInTaskbar = false;

        comboBox = new ComboBox { DataSource = bottleSizes, Dock = DockStyle.Top, DropDownStyle = ComboBoxStyle.DropDownList };
        okButton = new Button { Text = "OK", DialogResult = DialogResult.OK, Dock = DockStyle.Left, Width = 120 };
        cancelButton = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Dock = DockStyle.Right, Width = 120 };

        okButton.Click += (s, e) =>
        {
            if (comboBox.SelectedItem != null)
            {
                SelectedBottleSize = comboBox.SelectedItem.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a bottle size.");
            }
        };

        cancelButton.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

        var btnPanel = new Panel { Dock = DockStyle.Bottom, Height = 35 };
        btnPanel.Controls.Add(okButton);
        btnPanel.Controls.Add(cancelButton);

        this.Controls.Add(comboBox);
        this.Controls.Add(btnPanel);
    }
}