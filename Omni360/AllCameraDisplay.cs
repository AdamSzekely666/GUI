using System;
using System.Windows.Forms;

namespace MatroxLDS
{
    public partial class AllCameraDisplay : BaseForm
    {
        public MainForm mainForm;

        public AllCameraDisplay(MainForm _mainform)
        {
            InitializeComponent();
            mainForm = _mainform;
            ButtonAnimator.InitializeAnimation(DashboardBtn, "blue");

            // Set initial DisplayControl properties
            displayControl1.PageName = MainForm.OPERATOR_VIEW_NAME;
            displayControl1.HostName = MainForm.HOST_NAME;
            displayControl1.ProjectName = MainForm.PROJECT_NAME1;
            displayControl1.DisplayName = "Finish"; // Initial/default

            displayControl2.PageName = MainForm.OPERATOR_VIEW_NAME;
            displayControl2.HostName = MainForm.HOST_NAME;
            displayControl2.ProjectName = MainForm.PROJECT_NAME2;
            displayControl2.DisplayName = "BaseInspection"; // Initial/default
        }

        private async void AllCameraDisplay_Load(object sender, EventArgs e)
        {
            await displayControl1.ConnectAsync();
            await displayControl2.ConnectAsync();

            CurrentUserTxt.Text = mainForm.CurrentUserTxt.Text;
            DateTimeLabel.Text = mainForm.DateTimeLabel.Text;

            // ** wire up PictureBox-as-button click events **
            picECI1Inspect.Click += async (s, ev) => await ShowEci1("Finish");
            picECI1Reject.Click += async (s, ev) => await ShowEci1("FinishLastReject");

            picECI2BaseInspect.Click += async (s, ev) => await ShowEci2("BaseInspection");
            picECI2ISWInspect.Click += async (s, ev) => await ShowEci2("ISWInspection");
            picECI2DentInspect.Click += async (s, ev) => await ShowEci2("DentInspection");

            picECI2BaseReject.Click += async (s, ev) => await ShowEci2("BaseLastReject");
            picECI2ISWReject.Click += async (s, ev) => await ShowEci2("ISWLastReject");
            picECI2DentReject.Click += async (s, ev) => await ShowEci2("DentLastReject");
        }

        // --- Zebra ECI1 Display navigation (use for Camera 1) ---
        private async System.Threading.Tasks.Task ShowEci1(string displayName)
        {
            displayControl1.Disconnect();                     // Disconnect first!
            displayControl1.DisplayName = displayName;        // Set display
            await displayControl1.ConnectAsync();             // Connect
        }

        // --- Zebra ECI2 Display navigation (use for Camera 2) ---
        private async System.Threading.Tasks.Task ShowEci2(string displayName)
        {
            displayControl2.Disconnect();                     // Disconnect first!
            displayControl2.DisplayName = displayName;        // Set display
            await displayControl2.ConnectAsync();             // Connect
        }

        private void DashboardBtn_Click(object sender, EventArgs e)
        {
            displayControl1.Disconnect();
            displayControl2.Disconnect();
            MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];
            if (mainForm != null)
            {
                mainForm.Show();
                this.Close();
            }
        }
    }
}