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
using System.Data.SQLite;
using System.IO;
using System.Diagnostics;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.Drawing;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;
using static iText.IO.Util.IntHashtable;
using Sharp7;


namespace MatroxLDS
{
    public partial class Inspection2 : MatroxLDS.BaseForm
    {

        private OperatorViewPage CameraPage;
        private Host _host;
        public MainForm mainForm;
        public MainForm _mainForm;
        private Inspection parentInspection;
        private System.Windows.Forms.Timer hideTimer;
        private S7Client s7Client;
        private string plcIp = "192.168.0.21";
        private int plcRack = 0; // Common default for S7-300/400 is 0
        private int plcSlot = 1; // Common default for S7-300/400 is 2; S7-1200/1500 is 1
        private HashSet<string> nitrousRecipes;
        private string iniPath;
        private string currentRecipeName; // Set this based on your app logic
        private System.Windows.Forms.Timer plcBoolPollTimer;
        private bool testTriggerBitState = false;

        public Inspection2(Inspection parentInspection)
        {
            InitializeComponent();
            this.parentInspection = parentInspection;

            ButtonAnimator.InitializeAnimation(DashboardBtn, "blue");
            CurrentUserTxt.Text = MainForm.Instance.CurrentUserTxt.Text;
            DateTimeLabel.Text = MainForm.Instance.DateTimeLabel.Text;
            SetupSharp7Connection();
            hideTimer = new System.Windows.Forms.Timer();
            hideTimer.Interval = 3000; // 3 seconds
            hideTimer.Tick += HideTimer_Tick;


        }
        private async void Inspection2_Load(object sender, EventArgs e)
        {
            await Display.ConnectAsync();
            await Display2.ConnectAsync();
            await Display1.ConnectAsync();
            await Display5.ConnectAsync();
            await Display4.ConnectAsync();
            await Display3.ConnectAsync();
            await DentThroughputToggle.ConnectAsync();
            await ISWThroughputToggle.ConnectAsync();
            await BaseThroughputToggle.ConnectAsync();  

            await C2ExposureTxt.ConnectAsync();
            await AllImagesToStore.ConnectAsync();
            await StoreAllImages.ConnectAsync();

            await FindFinishCenterX.ConnectAsync();
            await FindFinishCenterY.ConnectAsync();
            await FindFinishCenterStartRadius.ConnectAsync();
            await FindFinishCenterEndRadius.ConnectAsync();
            await FindFinishCenterPolarity.ConnectAsync();
            await FindFinishCenterAccuracy.ConnectAsync();
            await FindFinishCenterNumber.ConnectAsync();
            await FindFinishCenterChordAngle.ConnectAsync();
            await FindFinishCenterFilterType.ConnectAsync();
            await FindFinishCenterSmoothness.ConnectAsync();
            await FindFinishCenterEdgeMinVariation.ConnectAsync();
            await FindFinishCenterEdgeThreshold.ConnectAsync();

            await FindBaseCenterX.ConnectAsync();
            await FindBaseCenterY.ConnectAsync();
            await FindBaseCenterStartRadius.ConnectAsync();
            await FindBaseCenterEndRadius.ConnectAsync();
            await FindBaseCenterPolarityDropDown.ConnectAsync();
            await FindBaseCenterAcuracyDropDown.ConnectAsync();
            await FindBaseCenterNumber.ConnectAsync();
            await FindBaseCenterChordAngle.ConnectAsync();
            await FindBaseCenterFilterType.ConnectAsync();
            await FindBaseCenterSmoothness.ConnectAsync();
            await FindBaseCenterMinVariation.ConnectAsync();
            await FindBaseCenterEdgeThreshold.ConnectAsync();

            await DentIntCenterX.ConnectAsync();
            await DentIntCenterY.ConnectAsync();
            await DentIntStartRadX.ConnectAsync();
            await DentIntStartRadY.ConnectAsync();
            await DentIntEndRadX.ConnectAsync();
            await DentIntEndRadY.ConnectAsync();
            await DentIntIncludeCondition.ConnectAsync();
            await DentIntLowValue.ConnectAsync();
            await DentIntHighValue.ConnectAsync();
            await DentNumberOfPixelsDropDown.ConnectAsync();
            await DentNumberofPixelsthreshold.ConnectAsync();
            await DentContrastDropDown.ConnectAsync();
            await DentContrastThreshold.ConnectAsync();
            await DentRejectionStatus.ConnectAsync();
            await DentNumberOfImagesToStore.ConnectAsync();
            await StoreDentImages.ConnectAsync();

            await ISWCenterX.ConnectAsync();
            await ISWCenterY.ConnectAsync();
            await ISWStartRadiusX.ConnectAsync();
            await ISWStartRadiusY.ConnectAsync();
            await ISWEndRadiusX.ConnectAsync();
            await ISWEndRadiusY.ConnectAsync();
            await ISWIncludeConditionDropDown.ConnectAsync();
            await ISWLowValue.ConnectAsync();
            await ISWHighValue.ConnectAsync();
            await ISWGreaterThenLessThen.ConnectAsync();
            await ISWThreshold.ConnectAsync();
            await ISWRejectionStatus.ConnectAsync();
            await ISWNumberOfImagesToStore.ConnectAsync();
            await StoreISWImages.ConnectAsync();

            await BaseCenterX.ConnectAsync();
            await BaseCenterY.ConnectAsync();
            await BaseWidth.ConnectAsync();
            await BaseHeight.ConnectAsync();
            await BaseIncludeCondition.ConnectAsync();
            await BaseHighValue.ConnectAsync();
            await BaseLowValue.ConnectAsync();
            await BaseIntesityGreaterLessThan.ConnectAsync();
            await BaseThreshold.ConnectAsync();
            await BaseRejectionStatus.ConnectAsync();
            await BaseNumberOfImagesToStore.ConnectAsync();
            await StoreBaseImages.ConnectAsync();


            await HostManager.GetHost(MainForm.HOST_NAME).Projects[MainForm.PROJECT_NAME2].ExecutionMessages.SetEnable();
            ReadSharp7TriggerDelay();
            ConnectToAllTextboxes(Program.splashForm);
            plcBoolPollTimer = new System.Windows.Forms.Timer();
            plcBoolPollTimer.Interval = 250; // 250 ms refresh
            plcBoolPollTimer.Tick += PlcBoolPollTimer_Tick;
            plcBoolPollTimer.Start();
            ReadStartupPLCValuesAndUpdateUI();

        }
        private void PlcBoolPollTimer_Tick(object sender, EventArgs e)
        {
            bool plcBit = ReadPlcTestTriggerBit();
            if (btnTestTrigger.InvokeRequired)
                btnTestTrigger.Invoke(new Action(() => UpdateTriggerButtonState(plcBit)));
            else
                UpdateTriggerButtonState(plcBit);
            testTriggerBitState = plcBit;
        }
        private void ReadStartupPLCValuesAndUpdateUI()
        {
            // 1. Read delay values (DB4.DB100–DB4.DB155)
            byte[] delaysBuffer = new byte[56];
            int delaysResult = s7Client.DBRead(4, 100, 56, delaysBuffer);

            if (delaysResult == 0)
            {
                int C2TriggDelay = S7.GetUIntAt(delaysBuffer, 2);     // Offset 100
                int RejDelay = S7.GetUIntAt(delaysBuffer, 10);    // Offset 110
                int RejPulse = S7.GetUIntAt(delaysBuffer, 12);    // Offset 112
                int C2TriggPulse = S7.GetDIntAt(delaysBuffer, 44);    // Offset 140

                // Assign to controls
                C2TriggerDelay.Text = C2TriggDelay.ToString();
                C2TriggerPulse.Text = C2TriggPulse.ToString();
                RejectDelay.Text = RejDelay.ToString();
                RejectPulse.Text = RejPulse.ToString();
            }
            else
            {
                Debug.WriteLine($"Delays DBRead error: {s7Client.ErrorText(delaysResult)}");
            }

            // 2. Read trigger bit value (DB4.DB200.5)
            byte[] triggerBuffer = new byte[1];
            int triggerResult = s7Client.DBRead(4, 200, 1, triggerBuffer);

            if (triggerResult == 0)
            {
                bool triggerBit = S7.GetBitAt(triggerBuffer, 0, 5); // DB4.DBX200.5
                UpdateTriggerButtonState(triggerBit);                // Update button color/text
            }
            else
            {
                Debug.WriteLine($"Trigger Bit DBRead error: {s7Client.ErrorText(triggerResult)}");
                btnTestTrigger.Text = "PLC Offline";
                btnTestTrigger.BackColor = Color.Gray;
            }
        }
        private bool ReadPlcTestTriggerBit()
        {
            if (s7Client == null || !s7Client.Connected) return false;
            byte[] buffer = new byte[1]; // Read byte at DB200
            int result = s7Client.DBRead(4, 200, 1, buffer);
            if (result == 0)
            {
                return S7.GetBitAt(buffer, 0, 5); // Extract bit 5, which is DBX200.5
            }
            else
            {
                //Debug.WriteLine($"Sharp7 DBRead error: {s7Client.ErrorText(result)}");
                return false;
            }
        }
        private void WritePlcTestTriggerBit(bool value)
        {
            if (s7Client == null || !s7Client.Connected) return;
            byte[] buffer = new byte[1];
            // First, read the current value (to preserve other bits in this byte!)
            int readResult = s7Client.DBRead(4, 200, 1, buffer);
            if (readResult == 0)
            {
                S7.SetBitAt(ref buffer, 0, 5, value); // Set DBX200.5
                int writeResult = s7Client.DBWrite(4, 200, 1, buffer);
                if (writeResult != 0)
                {
                    //Debug.WriteLine($"Sharp7 DBWrite error: {s7Client.ErrorText(writeResult)}");
                }
            }
            else
            {
                // Debug.WriteLine($"Sharp7 DBRead error: {s7Client.ErrorText(readResult)}");
            }
        }
        private void btnTestTrigger_Click(object sender, EventArgs e)
        {
            bool currentState = ReadPlcTestTriggerBit();           // Read current PLC state
            bool newState = !currentState;                         // Toggle it
            WritePlcTestTriggerBit(newState);                      // Write new state
            testTriggerBitState = ReadPlcTestTriggerBit();         // Read back to confirm (optional, but best for UI sync!)
            UpdateTriggerButtonState(testTriggerBitState);         // Update UI immediately
                                                                   // Next timer tick will reconcile again
        }
        private void UpdateTriggerButtonState(bool state)
        {
            // Update color if you want (optional)
            btnTestTrigger.BackColor = state ? Color.Red : Color.LimeGreen;
            // Update text
            btnTestTrigger.Text = state ? "Triggering" : "Not Triggering";
        }
        private void SetupSharp7Connection()
        {
            s7Client = new S7Client();
            int result = s7Client.ConnectTo(plcIp, plcRack, plcSlot);

            if (result == 0)
            {
                Debug.WriteLine("Connected to PLC via Sharp7.");
            }
            else
            {
                Debug.WriteLine($"Sharp7 connection error: {s7Client.ErrorText(result)}");
            }
        }
        private void ReadSharp7TriggerDelay()
        {
            if (s7Client == null || !s7Client.Connected) return;

            byte[] buffer = new byte[56];
            int result = s7Client.DBRead(4, 100, 56, buffer);

            if (result == 0)
            {
                int C2TriggDelay = S7.GetUIntAt(buffer, 2);   // Offset 102
                int RejDelay = S7.GetUIntAt(buffer, 10);
                int RejPulse = S7.GetUIntAt(buffer, 12);
                int C2TriggPulse = S7.GetDIntAt(buffer, 44);  // Offset 144

                // Assign to textboxes (convert to string)
                C2TriggerDelay.Text = C2TriggDelay.ToString();

                C2TriggerPulse.Text = C2TriggPulse.ToString();

                RejectDelay.Text = RejDelay.ToString();
                RejectPulse.Text = RejPulse.ToString();
            }
            else
            {
                Debug.WriteLine($"Sharp7 DBRead error: {s7Client.ErrorText(result)}");
            }
        }
        private void ConnectToAllTextboxes(SplashScreen splashScreen)
        {
            CameraPage = HostManager.GetHost(MainForm.HOST_NAME).Projects[MainForm.PROJECT_NAME2].OperatorViews[MainForm.OPERATOR_VIEW_NAME];
            CameraPage.ValueElements["BaseIntensityCenterValue"].ValueChanged += BaseIntensityCenterValue1;
            CameraPage.ValueElements["ISWIntensityValue"].ValueChanged += ISWIntensityValue1;
            CameraPage.ValueElements["DentIntensityNumberOfPixels"].ValueChanged += DentIntensityNumberOfPixels;
            CameraPage.ValueElements["DentIntensityContrast"].ValueChanged += DentIntensityContrast;

            HostManager.GetHost(MainForm.HOST_NAME).Projects[MainForm.PROJECT_NAME2].ExecutionMessages.NewMessagesReceived += ExecutionMessagesReceived;

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

             Display.Disconnect();
             Display2.Disconnect();
            Display1.Disconnect();
             Display5.Disconnect();
             Display4.Disconnect();
             Display3.Disconnect();
             DentThroughputToggle.Disconnect();
             ISWThroughputToggle.Disconnect();
             BaseThroughputToggle.Disconnect();

             C2ExposureTxt.Disconnect();
             AllImagesToStore.Disconnect();
             StoreAllImages.Disconnect();

             FindFinishCenterX.Disconnect();
             FindFinishCenterY.Disconnect();
             FindFinishCenterStartRadius.Disconnect();
             FindFinishCenterEndRadius.Disconnect();
             FindFinishCenterPolarity.Disconnect();
             FindFinishCenterAccuracy.Disconnect();
             FindFinishCenterNumber.Disconnect();
             FindFinishCenterChordAngle.Disconnect();
             FindFinishCenterFilterType.Disconnect();
             FindFinishCenterSmoothness.Disconnect();
             FindFinishCenterEdgeMinVariation.Disconnect();
             FindFinishCenterEdgeThreshold.Disconnect();

             FindBaseCenterX.Disconnect();
             FindBaseCenterY.Disconnect();
             FindBaseCenterStartRadius.Disconnect();
             FindBaseCenterEndRadius.Disconnect();
             FindBaseCenterPolarityDropDown.Disconnect();
             FindBaseCenterAcuracyDropDown.Disconnect();
             FindBaseCenterNumber.Disconnect();
             FindBaseCenterChordAngle.Disconnect();
             FindBaseCenterFilterType.Disconnect();
             FindBaseCenterSmoothness.Disconnect();
             FindBaseCenterMinVariation.Disconnect();
             FindBaseCenterEdgeThreshold.Disconnect();

             DentIntCenterX.Disconnect();
             DentIntCenterY.Disconnect();
             DentIntStartRadX.Disconnect();
             DentIntStartRadY.Disconnect();
             DentIntEndRadX.Disconnect();
             DentIntEndRadY.Disconnect();
             DentIntIncludeCondition.Disconnect();
             DentIntLowValue.Disconnect();
             DentIntHighValue.Disconnect();
             DentNumberOfPixelsDropDown.Disconnect();
             DentNumberofPixelsthreshold.Disconnect();
             DentContrastDropDown.Disconnect();
             DentContrastThreshold.Disconnect();
             DentRejectionStatus.Disconnect();
             DentNumberOfImagesToStore.Disconnect();
             StoreDentImages.Disconnect();

             ISWCenterX.Disconnect();
             ISWCenterY.Disconnect();
             ISWStartRadiusX.Disconnect();
             ISWStartRadiusY.Disconnect();
             ISWEndRadiusX.Disconnect();
             ISWEndRadiusY.Disconnect();
             ISWIncludeConditionDropDown.Disconnect();
             ISWLowValue.Disconnect();
             ISWHighValue.Disconnect();
             ISWGreaterThenLessThen.Disconnect();
             ISWThreshold.Disconnect();
             ISWRejectionStatus.Disconnect();
             ISWNumberOfImagesToStore.Disconnect();
             StoreISWImages.Disconnect();

             BaseCenterX.Disconnect();
             BaseCenterY.Disconnect();
             BaseWidth.Disconnect();
             BaseHeight.Disconnect();
             BaseIncludeCondition.Disconnect();
             BaseHighValue.Disconnect();
             BaseLowValue.Disconnect();
             BaseIntesityGreaterLessThan.Disconnect();
             BaseThreshold.Disconnect();
             BaseRejectionStatus.Disconnect();
             BaseNumberOfImagesToStore.Disconnect();
             StoreBaseImages.Disconnect();

            parentInspection.Show();
            this.Close();

        }
        public void HandleTextBoxFocusLoss()
        {
            this.hiddenButton.Focus(); // Ensure this sets the focus to the hidden button
        }
        private void C2Exposure_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    C2ExposureTxt.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void BaseIntensityCenterValue1(object sender, ValueChangedEventArgs args)
        {
            try
            {
                string newValue = GetValidData(args.NewValue).ToString();
                if (BaseNumberOfPixels.InvokeRequired)
                {
                    BaseNumberOfPixels.Invoke(new Action(() => BaseNumberOfPixels.Text = newValue));
                }
                else
                {
                    BaseNumberOfPixels.Text = newValue;

                }
            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating CapWidthReadValue: {ex.Message}");
            }
        }
        private void ISWIntensityValue1(object sender, ValueChangedEventArgs args)
        {
            try
            {
                string newValue = GetValidData(args.NewValue).ToString();
                if (ISWPixelValue.InvokeRequired)
                {
                    ISWPixelValue.Invoke(new Action(() => ISWPixelValue.Text = newValue));
                }
                else
                {
                    ISWPixelValue.Text = newValue;

                }
            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating CapWidthReadValue: {ex.Message}");
            }
        }

        private void DentIntensityNumberOfPixels(object sender, ValueChangedEventArgs args)
        {
            try
            {
                string newValue = GetValidData(args.NewValue).ToString();
                if (DentIntensityNumberOfPixelsValue.InvokeRequired)
                {
                    DentIntensityNumberOfPixelsValue.Invoke(new Action(() => DentIntensityNumberOfPixelsValue.Text = newValue));
                }
                else
                {
                    DentIntensityNumberOfPixelsValue.Text = newValue;

                }
            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating CapWidthReadValue: {ex.Message}");
            }
        }
        private void DentIntensityContrast(object sender, ValueChangedEventArgs args)
        {
            try
            {
                string newValue = GetValidData(args.NewValue).ToString();
                if (DentIntensityContrastValue.InvokeRequired)
                {
                    DentIntensityContrastValue.Invoke(new Action(() => DentIntensityContrastValue.Text = newValue));
                }
                else
                {
                    DentIntensityContrastValue.Text = newValue;

                }
            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating CapWidthReadValue: {ex.Message}");
            }
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
        private void C2TriggerDelay_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    C2TriggerDelay.Text = numberPadForm.Number;
                    if (ushort.TryParse(numberPadForm.Number, out ushort value))
                    {
                        byte[] buffer = new byte[2];
                        S7.SetUIntAt(buffer, 0, value);
                        int result = s7Client.DBWrite(4, 102, 2, buffer);
                        if (result != 0)
                            Debug.WriteLine($"Write error: {s7Client.ErrorText(result)}");
                    }
                }
            }
            HandleTextBoxFocusLoss();
        }
        private void C2TriggerPulse_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    C2TriggerPulse.Text = numberPadForm.Number;
                    if (int.TryParse(numberPadForm.Number, out int value))
                    {
                        byte[] buffer = new byte[4];
                        S7.SetDIntAt(buffer, 0, value);
                        int result = s7Client.DBWrite(4, 144, 4, buffer);
                        if (result != 0)
                            Debug.WriteLine($"Write error: {s7Client.ErrorText(result)}");
                    }
                }
            }
            HandleTextBoxFocusLoss();
        }
        private void RejectDelay_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    RejectDelay.Text = numberPadForm.Number;
                    if (ushort.TryParse(numberPadForm.Number, out ushort value))
                    {
                        byte[] buffer = new byte[2];
                        S7.SetUIntAt(buffer, 0, value);
                        int result = s7Client.DBWrite(4, 110, 2, buffer);
                        if (result != 0)
                            Debug.WriteLine($"Write error: {s7Client.ErrorText(result)}");
                    }
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void RejectPulse_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    RejectPulse.Text = numberPadForm.Number;
                    if (ushort.TryParse(numberPadForm.Number, out ushort value))
                    {
                        byte[] buffer = new byte[2];
                        S7.SetUIntAt(buffer, 0, value);
                        int result = s7Client.DBWrite(4, 112, 2, buffer);
                        if (result != 0)
                            Debug.WriteLine($"Write error: {s7Client.ErrorText(result)}");
                    }
                }
            }
            HandleTextBoxFocusLoss();
        }

        private void FindCenterX_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindBaseCenterX.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void FindCenterY_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindBaseCenterY.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void FindCenterStartRadius_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindBaseCenterStartRadius.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void FindCenterEndRadius_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindBaseCenterEndRadius.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void FindCenterNumber_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindBaseCenterNumber.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void FindCneterChordAngle_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindBaseCenterChordAngle.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void FindCenterSmoothness_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindBaseCenterSmoothness.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void FindCenterEdgeThreshold_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindBaseCenterEdgeThreshold.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void FindCenterMinVariation_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindBaseCenterMinVariation.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void BaseCenterX_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    BaseCenterX.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void BaseCenterY_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    BaseCenterY.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void BaseWidth_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    BaseWidth.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void BaseHeight_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    BaseHeight.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void BaseLowValue_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    BaseLowValue.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void BaseHighValue_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    BaseHighValue.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void BaseThreshold_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    BaseThreshold.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void ISWCenterX_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    ISWCenterX.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void ISWCenterY_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    ISWCenterY.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void ISWStartRadiusX_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    ISWStartRadiusX.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void ISWStartRadiusY_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    ISWStartRadiusY.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void ISWEndRadiusX_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    ISWEndRadiusX.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void ISWEndRadiusY_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    ISWEndRadiusY.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void ISWLowValue_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    ISWLowValue.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void ISWHighValue_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    ISWHighValue.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void ISWThreshold_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    ISWThreshold.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void NumberOfImagesToStore_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    AllImagesToStore.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void DentIntCenterX_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    DentIntCenterX.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void DentIntCenterY_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    DentIntCenterY.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void DentIntStartRadX_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    DentIntStartRadX.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void DentIntStartRadY_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    DentIntStartRadY.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void DentIntEndRadX_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    DentIntEndRadX.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();
        }
        private void DentIntEndRadY_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    DentIntEndRadY.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void DentIntLowValue_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    DentIntLowValue.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void DentIntHighValue_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    DentIntHighValue.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void DentNumberofPixelsthreshold_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    DentNumberofPixelsthreshold.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void DentContrastThreshold_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    DentContrastThreshold.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }

        private void FindFinishCenterX_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindFinishCenterX.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();


        }

        private void FindFinishCenterY_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindFinishCenterY.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();


        }

        private void FindFinishCenterStartRadius_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindFinishCenterStartRadius.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();


        }

        private void FindFinishCenterEndRadius_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindFinishCenterEndRadius.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();


        }

        private void FindFinishCenterNumber_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindFinishCenterNumber.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();


        }

        private void FindFinishCenterChordAngle_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindFinishCenterChordAngle.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();


        }

        private void FindFinishCenterSmoothness_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindFinishCenterSmoothness.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();


        }

        private void FindFinishCenterEdgeThreshold_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindFinishCenterEdgeThreshold.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();


        }

        private void FindFinishCenterEdgeMinVariation_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FindFinishCenterEdgeMinVariation.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();


        }

        private void DentNumberOfImagesToStore_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    DentNumberOfImagesToStore.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();


        }

        private void ISWNumberOfImagesToStore_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    ISWNumberOfImagesToStore.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();


        }

        private void BaseNumberOfImagesToStore_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    BaseNumberOfImagesToStore.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();


        }
    }
}
