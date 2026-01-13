using Zebra.ADA.OperatorAPI;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MatroxLDS
{
    public partial class RejectedImages : BaseForm
    {
        // Update this to your actual username!
        private readonly string imageRootFolder = @"C:\Users\ZUser\Desktop\Images";

        private string currentDefectType = "Finish";
        private FileSystemWatcher[] watchers = new FileSystemWatcher[4];

        public RejectedImages()
        {
            InitializeComponent();
            ButtonAnimator.InitializeAnimation(DashboardBtn, "blue");
            ButtonAnimator.InitializeAnimation(FinishBtn, "blue");
            ButtonAnimator.InitializeAnimation(BaseBtn, "blue");
            ButtonAnimator.InitializeAnimation(ISWBtn, "blue");
            ButtonAnimator.InitializeAnimation(Dentbtn, "blue");

            CurrentUserTxt.Text = MainForm.Instance.CurrentUserTxt.Text;
            DateTimeLabel.Text = MainForm.Instance.DateTimeLabel.Text;

            FinishBtn.Click += (s, e) => ShowFilmStrip("Finish");
            BaseBtn.Click += (s, e) => ShowFilmStrip("Base");
            ISWBtn.Click += (s, e) => ShowFilmStrip("ISW");
            Dentbtn.Click += (s, e) => ShowFilmStrip("Dent");

            ShowFilmStrip("Finish"); // Default view
        }

        private string GetImageFolderForDefect(string defectType)
        {
            switch (defectType)
            {
                case "Finish": return Path.Combine(imageRootFolder, "C1FinishFail");
                case "Base": return Path.Combine(imageRootFolder, "C2BaseFail");
                case "ISW": return Path.Combine(imageRootFolder, "C2ISWFail");
                case "Dent": return Path.Combine(imageRootFolder, "C2DentFail");
                default: return null;
            }
        }

        private void ShowFilmStrip(string defectType)
        {
            currentDefectType = defectType;
            DefectTypeTxt.Text = GetDefectDisplayName(defectType);

            // Dispose previous watchers
            foreach (var watcher in watchers)
            {
                watcher?.Dispose();
            }
            Array.Clear(watchers, 0, watchers.Length);

            // Clear all four panels
            flowCam1.Controls.Clear();
            flowCam2.Controls.Clear();
            flowCam3.Controls.Clear();
            flowCam4.Controls.Clear();

            string folder = GetImageFolderForDefect(defectType);
            if (folder != null && Directory.Exists(folder))
            {
                var images = Directory.GetFiles(folder, "*.jpg")
                    .Concat(Directory.GetFiles(folder, "*.png"))
                    .ToArray();

                foreach (var imgPath in images)
                {
                    var thumb = new PictureBox
                    {
                        Image = LoadThumbnail(imgPath, 180, 120),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Width = 180,
                        Height = 120,
                        Margin = GetCenteredMarginFor(
                            defectType == "Finish" ? flowCam1 :
                            defectType == "Base" ? flowCam2 :
                            defectType == "ISW" ? flowCam3 :
                                                     flowCam4
                        ),
                        Cursor = Cursors.Hand,
                        Tag = new ImageTag
                        {
                            Path = imgPath,
                            DefectType = defectType,
                            CamNumber = GetCamNumberForDefect(defectType),
                            AllImages = images
                        }
                    };
                    thumb.Click += Thumbnail_Click;

                    if (defectType == "Finish") flowCam1.Controls.Add(thumb);
                    else if (defectType == "Base") flowCam2.Controls.Add(thumb);
                    else if (defectType == "ISW") flowCam3.Controls.Add(thumb);
                    else if (defectType == "Dent") flowCam4.Controls.Add(thumb);
                }
            }
        }

        private int GetCamNumberForDefect(string defectType)
        {
            switch (defectType)
            {
                case "Finish": return 1;
                case "Base": return 2;
                case "ISW": return 3;
                case "Dent": return 4;
                default: return 0;
            }
        }

        private void ReloadFilmstrip(int cam, string defectType)
        {
            // NEW: Added support for cam 4
            FlowLayoutPanel flowPanel = cam == 1 ? flowCam1
                                        : cam == 2 ? flowCam2
                                        : cam == 3 ? flowCam3
                                                   : flowCam4;

            flowPanel.Controls.Clear();

            string[] camFolders = new string[] { Path.Combine(imageRootFolder, $"C{cam}{defectType}") };

            var images = camFolders
                .Where(Directory.Exists)
                .SelectMany(f => Directory.GetFiles(f, "*.jpg").Concat(Directory.GetFiles(f, "*.png")))
                .ToArray();

            foreach (var imgPath in images)
            {
                var thumb = new PictureBox
                {
                    Image = LoadThumbnail(imgPath, 180, 120),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 180,
                    Height = 120,
                    Margin = GetCenteredMarginFor(flowPanel),
                    Cursor = Cursors.Hand,
                    Tag = new ImageTag
                    {
                        Path = imgPath,
                        DefectType = defectType,
                        CamNumber = cam,
                        AllImages = images
                    }
                };
                thumb.Click += Thumbnail_Click;
                flowPanel.Controls.Add(thumb);
            }
        }

        private Padding GetCenteredMarginFor(FlowLayoutPanel panel)
        {
            int panelWidth = panel.Width > 0 ? panel.Width : 400; // Fallback width
            int leftRight = Math.Max(0, (panelWidth - 180) / 2);
            return new Padding(leftRight, 8, leftRight, 8);
        }

        private Image LoadThumbnail(string path, int width, int height)
        {
            using (var img = Image.FromFile(path))
            {
                return new Bitmap(img, new Size(width, height));
            }
        }

        private void Thumbnail_Click(object sender, EventArgs e)
        {
            var tag = (ImageTag)((PictureBox)sender).Tag;
            int index = Array.IndexOf(tag.AllImages, tag.Path);
            var modal = new ImageModalForm(tag.AllImages, index, $"Camera {tag.CamNumber} - {GetDefectDisplayName(tag.DefectType)}");
            modal.ShowDialog(this);
        }

        private class ImageTag
        {
            public string Path;
            public string DefectType;
            public int CamNumber;
            public string[] AllImages;
        }

        private void DashboardBtn_Click(object sender, EventArgs e)
        {
            MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];
            if (mainForm != null)
            {
                mainForm.Show();
                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            foreach (var watcher in watchers)
                watcher?.Dispose();
            base.OnFormClosing(e);
        }

        // Converts internal defect type to display text for the textbox
        private string GetDefectDisplayName(string defectType)
        {
            switch (defectType)
            {
                case "Finish": return "Finish";
                case "Base": return "Base";
                case "ISW": return "ISW";
                case "Dent": return "Dent";
                default: return defectType;
            }
        }
    }
}