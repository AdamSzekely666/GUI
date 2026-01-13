using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zebra.ADA.OperatorAPI;
using Zebra.ADA.OperatorAPI.ExecutionMessagesPackage;
using Zebra.ADA.OperatorAPI.InputsPackage;
using Zebra.ADA.OperatorAPI.ControlsPackage;
using Zebra.ADA.OperatorAPI.OutputsPackage;
using OmniCheck_360.Properties;
using System.IO;
using System.Diagnostics;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.Drawing;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;
using OmniCheck_360;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using Zebra.ADA.OperatorAPI.Communication;
using OpenHardwareMonitor.Hardware;
using static MatroxLDS.MainForm;

namespace MatroxLDS
{
    public partial class Inspection5 : MatroxLDS.BaseForm
    {
        private OperatorViewPage CameraPage;
        private Host _host;
        public MainForm mainForm;
        public MainForm _mainForm;
        public const string FILLLEVELX = "FillLevelIntensityX";
        public const string FILLLEVELY = "FillLevelIntensityY";
        public const string FILLLEVELWIDTH = "FillLevelIntensityWidth";
        public const string FILLLEVELHEIGHT = "FillLevelIntensityHeight";
        public const string FILLLEVELANGLE = "FillLevelIntensityAngle";
        public const string FILLLEVELSETVALUE = "FillLevelIntensityThreshold";
        public const string FILLLEVELVALUE = "FillLevelIntensityValue";
        public const string DISPLAY = "FillLevelCopy";
        private System.Windows.Forms.Timer hideTimer;

        public Inspection5(MainForm mainForm, Inspection inspection)
        {
            InitializeComponent();
            this._mainForm = mainForm;
            ButtonAnimator.InitializeAnimation(DashboardBtn, "blue");
            // Timer setup
            hideTimer = new System.Windows.Forms.Timer();
            hideTimer.Interval = 3000; // 3 seconds
            hideTimer.Tick += HideTimer_Tick;


        }

        public Inspection5(Inspection inspection)
        {
        }

        private async void Inspection5_Load(object sender, EventArgs e)
        {

        }
        private void ConnectToAllTextboxes(SplashScreen splashScreen)
        {

        }
        private double GetValidData(string newValue)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                Console.WriteLine("Received null or empty value.");
                return 0; // Default value for null or empty input
            }

            try
            {
                return Convert.ToDouble(newValue);
            }
            catch (FormatException)
            {
                Console.WriteLine("Value conversion error. Received invalid format.");
                return 0; // Default value for invalid format
            }
        }
        private void DashboardBtn_Click(object sender, EventArgs e)
        {

            if (_mainForm != null)
            {
                Console.WriteLine("Returning to MainForm.");
                _mainForm.Show();
                this.Close(); // Or this.Dispose()
            }
            else
            {
                MessageBox.Show("MainForm is not initialized.");
            }

        }
        public void HandleTextBoxFocusLoss()
        {
        }
        private void HideTimer_Tick(object sender, EventArgs e)
        {
            _messagesListView.Visible = false;
            hideTimer.Stop();
        }
        void ExecutionMessagesReceived(object sender, NewExecutionMessagesReceivedEventArgs args)
        {
            if (args.NewMessages.Count <= 0) return;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action<List<ExecutionMessage>>(UpdateMessageList), args.NewMessages);
            }
            else
            {
                UpdateMessageList(args.NewMessages);
            }
        }
        private bool _flushMessages = false;
        private void UpdateMessageList(List<ExecutionMessage> msgs)
        {
            if (_flushMessages)
            {
                _messagesListView.Items.Clear();
                _flushMessages = false;
            }
            foreach (ExecutionMessage message in msgs)
            {
                int imageIndex;
                switch (message.Severity)
                {
                    case MessageSeverity.Info:
                        imageIndex = 0;
                        break;

                    case MessageSeverity.Warning:
                        imageIndex = 1;
                        break;

                    case MessageSeverity.Error:
                        imageIndex = 2;
                        break;

                    default:
                        imageIndex = 0;
                        break;
                }

                ListViewItem listViewItem = new ListViewItem(message.Content, imageIndex);
                _messagesListView.Items.Insert(0, listViewItem);
                _columnHeader.Width = -2;
            }
            // Show the ListView and start/reset the hide timer
            _messagesListView.Visible = true;
            hideTimer.Stop();
            hideTimer.Start();
        }


    }
}
