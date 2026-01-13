using System;
using System.Windows.Forms;
using System.Drawing;

public class WaitForm : Form
{
    private Label lblTitle;
    private Label lblCam1;
    private Label lblCam2;
    private Label lblCam3;

    public WaitForm()
    {
        this.Text = "Uploading";
        this.ControlBox = false; // No close button
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Width = 620;
        this.Height = 180;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // Layout: title + 3 camera rows
        var table = new TableLayoutPanel
        {
            RowCount = 4,
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            Padding = new Padding(8),
            AutoSize = false
        };
        this.Controls.Add(table);

        lblTitle = new Label()
        {
            Text = "Please wait, uploading recipes...",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            Height = 32
        };
        table.Controls.Add(lblTitle, 0, 0);

        lblCam1 = CreateCameraLabel("Camera 1: waiting...");
        lblCam2 = CreateCameraLabel("Camera 2: waiting...");
        lblCam3 = CreateCameraLabel("Camera 3: waiting...");

        table.Controls.Add(lblCam1, 0, 1);
        table.Controls.Add(lblCam2, 0, 2);
        table.Controls.Add(lblCam3, 0, 3);
    }

    private Label CreateCameraLabel(string text)
    {
        return new Label
        {
            Text = text,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI", 10),
            AutoEllipsis = true
        };
    }

    // Set the main message/title on the wait form.
    public void SetMessage(string message)
    {
        if (this.IsDisposed || this.Disposing) return;

        if (this.InvokeRequired)
        {
            this.Invoke(new Action(() => SetMessage(message)));
            return;
        }

        lblTitle.Text = message;
        lblTitle.Refresh();
        Application.DoEvents();
    }

    // Set a camera-specific line. camIndex is 1..3
    public void SetCameraMessage(int camIndex, string message)
    {
        if (this.IsDisposed || this.Disposing) return;

        if (this.InvokeRequired)
        {
            this.Invoke(new Action(() => SetCameraMessage(camIndex, message)));
            return;
        }

        switch (camIndex)
        {
            case 1:
                lblCam1.Text = $"Camera 1: {message}";
                lblCam1.Refresh();
                break;
            case 2:
                lblCam2.Text = $"Camera 2: {message}";
                lblCam2.Refresh();
                break;
            case 3:
                lblCam3.Text = $"Camera 3: {message}";
                lblCam3.Refresh();
                break;
            default:
                // fall back to title if invalid index
                lblTitle.Text = message;
                lblTitle.Refresh();
                break;
        }

        Application.DoEvents();
    }
}