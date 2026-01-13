using System;
using System.Drawing;
using System.Windows.Forms;

namespace MatroxLDS
{
    public partial class ImageModalForm : Form
    {
        private readonly string[] images;
        private int currentIndex;
        private readonly string title;

        public ImageModalForm(string[] images, int startIndex, string title)
        {
            // Designer should have:
            // - lblImageTitle (Label, Dock=Top)
            // - pbImage (PictureBox, Dock=Fill, SizeMode=Zoom)
            // - btnPrev, btnNext, btnClose (Buttons, in panelButtons at bottom)
            InitializeComponent();
            ButtonAnimator.InitializeAnimation(btnClose, "blue");
            ButtonAnimator.InitializeAnimation(btnNext, "blue");
            ButtonAnimator.InitializeAnimation(btnPrev, "blue");

            this.images = images;
            this.currentIndex = startIndex;
            this.title = title;

            // Hide the Windows title bar and system buttons
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;

            // Show the first image
            ShowImage();

            btnPrev.Click += BtnPrev_Click;
            btnNext.Click += BtnNext_Click;
            btnClose.Click += BtnClose_Click;
        }

        private void ShowImage()
        {
            if (images == null || images.Length == 0) return;
            lblImageTitle.Text = $"{title} ({currentIndex + 1} / {images.Length})";
            pbImage.Image?.Dispose();
            pbImage.Image = Image.FromFile(images[currentIndex]);
            btnPrev.Enabled = currentIndex > 0;
            btnNext.Enabled = currentIndex < images.Length - 1;
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                ShowImage();
            }
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (currentIndex < images.Length - 1)
            {
                currentIndex++;
                ShowImage();
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                pbImage.Image?.Dispose();
            }
            base.Dispose(disposing);
        }

        // Optional: Make the window draggable by clicking the title label
        private Point dragOffset;
        private void lblImageTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                dragOffset = new Point(e.X, e.Y);
        }
        private void lblImageTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - dragOffset.X;
                this.Top += e.Y - dragOffset.Y;
            }
        }
    }
}