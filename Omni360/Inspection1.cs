using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WinForms;
using Zebra.ADA.OperatorAPI;
using Zebra.ADA.OperatorAPI.Communication;
using Zebra.ADA.OperatorAPI.ControlsPackage;
using Zebra.ADA.OperatorAPI.ExecutionMessagesPackage;
using Zebra.ADA.OperatorAPI.InputsPackage;
using Zebra.ADA.OperatorAPI.OutputsPackage;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using OmniCheck_360;
using OmniCheck_360.Properties;
using OpenHardwareMonitor.Hardware;
using Sharp7;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MatroxLDS.MainForm;
namespace MatroxLDS
{
    public partial class Inspection1 : MatroxLDS.BaseForm
    {
        private OperatorViewPage CameraPage;
        private Host _host;
        public MainForm mainForm;
        public MainForm _mainForm;
        private double _capWidthSetValue;
        private System.Windows.Forms.Timer hideTimer;
        private Inspection parentInspection;
        private S7Client s7Client;
        private string plcIp = "192.168.0.21";
        private int plcRack = 0; // Common default for S7-300/400 is 0
        private int plcSlot = 1; // Common default for S7-300/400 is 2; S7-1200/1500 is 1
        private System.Windows.Forms.Timer plcBoolPollTimer;
        private bool testTriggerBitState = false;
        public Inspection1(Inspection parentInspection)
        {

            InitializeComponent();
            this.parentInspection = parentInspection;
            ButtonAnimator.InitializeAnimation(DashboardBtn, "blue");
            SetupSharp7Connection();
            CurrentUserTxt.Text = MainForm.Instance.CurrentUserTxt.Text;
            DateTimeLabel.Text = MainForm.Instance.DateTimeLabel.Text;

            // Timer setup
            hideTimer = new System.Windows.Forms.Timer();
            hideTimer.Interval = 3000; // 3 seconds
            hideTimer.Tick += HideTimer_Tick;



        }
        private async void Inspection1_Load(object sender, EventArgs e)
        {
            await displayControl1.ConnectAsync();
            await displayControl2.ConnectAsync();
            await displayControl3.ConnectAsync();
            await displayControl4.ConnectAsync();
            await displayControl5.ConnectAsync();
            await FitErrorThreshold.ConnectAsync();
            await ScoreErrorThreshold.ConnectAsync();
            await RadiusErroThreshold.ConnectAsync();
            await ScaleErrorThreshold.ConnectAsync();
            await C1ExposureTxt.ConnectAsync();
            await FitErrorDropDown.ConnectAsync();
            await ScoreErrorDropDown.ConnectAsync ();
            await ScaleErrorDropDown.ConnectAsync ();
            await RadiusErrorDropDown.ConnectAsync ();
            await NumbeOfOccurance.ConnectAsync();
            await Smoothness.ConnectAsync();
            await DetailLevelDropDown.ConnectAsync();
            await Acceptance.ConnectAsync();
            await Certainity.ConnectAsync();
            await PolarityDropDown.ConnectAsync();
            await FitScoreMin.ConnectAsync();
            await SagittaTolerance.ConnectAsync();
            await CoverageMax.ConnectAsync();
            await CenterX.ConnectAsync();
            await CenterY.ConnectAsync();   
            await Height.ConnectAsync();
            await Width.ConnectAsync();
            await Angle.ConnectAsync();
            await Scale.ConnectAsync();
            await ScaleMax.ConnectAsync();
            await ScaleMin.ConnectAsync();  
            await FinishRejectionStatus.ConnectAsync();
            await FinishRejectionStatus1.ConnectAsync();
            await NumberOfCircelDropDown.ConnectAsync();
            await NumberOfCircleThreshold.ConnectAsync();
            await NumberOfCircleCenterX.ConnectAsync();
            await NumberOfCircleCenterY.ConnectAsync();
            await NumberOfCircleStartX.ConnectAsync();
            await NumberOfCircleStartY.ConnectAsync();
            await NumberOfCircleEndX.ConnectAsync();
            await NumberOfCircleEndY.ConnectAsync();
            await NumberOfCircleThresholding.ConnectAsync();
            await NumberOfCircelConditionDropDown.ConnectAsync();
            await NumberOfCircelOperationDropDown.ConnectAsync();
            await HostManager.GetHost(MainForm.HOST_NAME).Projects[MainForm.PROJECT_NAME1].ExecutionMessages.SetEnable();
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
                int C1TriggDelay = S7.GetUIntAt(delaysBuffer, 0);     // Offset 100
                int RejDelay = S7.GetUIntAt(delaysBuffer, 10);    // Offset 110
                int RejPulse = S7.GetUIntAt(delaysBuffer, 12);    // Offset 112
                int C1TriggPulse = S7.GetDIntAt(delaysBuffer, 40);    // Offset 140

                // Assign to controls
                C1TriggerDelay.Text = C1TriggDelay.ToString();
                C1TriggerPulse.Text = C1TriggPulse.ToString();
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
                int C1TriggDelay = S7.GetUIntAt(buffer, 0);   // Offset 100
                int C2TriggDelay = S7.GetUIntAt(buffer, 2);   // Offset 102
                int C3TriggDelay = S7.GetUIntAt(buffer, 4);   // Offset 104
                int RejDelay = S7.GetUIntAt(buffer, 10);
                int RejPulse = S7.GetUIntAt(buffer, 12);
                int C1TriggPulse = S7.GetDIntAt(buffer, 40);  // Offset 140
                int C2TriggPulse = S7.GetDIntAt(buffer, 44);  // Offset 144
                int C3TriggPulse = S7.GetDIntAt(buffer, 48);  // Offset 148

                // Assign to textboxes (convert to string)
                C1TriggerDelay.Text = C1TriggDelay.ToString();

                C1TriggerPulse.Text = C1TriggPulse.ToString();

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
            CameraPage = HostManager.GetHost(MainForm.HOST_NAME).Projects[MainForm.PROJECT_NAME1].OperatorViews[MainForm.OPERATOR_VIEW_NAME];
            CameraPage.ValueElements["FitErrorValue"].ValueChanged += FitErrorValue1;
            CameraPage.ValueElements["ScaleErrorValue"].ValueChanged += ScaleErrorValue1;
            CameraPage.ValueElements["RadiusErrorValue"].ValueChanged += RadiusErrorValue1;
            CameraPage.ValueElements["ScoreErrorValue"].ValueChanged += ScoreErrorValue1;
            CameraPage.ValueElements["NumberOfCircleValue"].ValueChanged += NumberOfCircleValue1;
            HostManager.GetHost(MainForm.HOST_NAME).Projects[MainForm.PROJECT_NAME1].ExecutionMessages.NewMessagesReceived += ExecutionMessagesReceived;
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

             displayControl1.Disconnect();
             displayControl2.Disconnect();
             displayControl3.Disconnect();
            displayControl4.Disconnect();
            displayControl5.Disconnect();
             FitErrorThreshold.Disconnect();
             ScoreErrorThreshold.Disconnect();
             RadiusErroThreshold.Disconnect();
             ScaleErrorThreshold.Disconnect();
             C1ExposureTxt.Disconnect();
             FitErrorDropDown.Disconnect();
             ScoreErrorDropDown.Disconnect();
             ScaleErrorDropDown.Disconnect();
             RadiusErrorDropDown.Disconnect();
             NumbeOfOccurance.Disconnect();
             Smoothness.Disconnect();
             DetailLevelDropDown.Disconnect();
             Acceptance.Disconnect();
             Certainity.Disconnect();
             PolarityDropDown.Disconnect();
             FitScoreMin.Disconnect();
             SagittaTolerance.Disconnect();
             CoverageMax.Disconnect();
             CenterX.Disconnect();
             CenterY.Disconnect();
             Height.Disconnect();
             Width.Disconnect();
             Angle.Disconnect();
             Scale.Disconnect();
             ScaleMax.Disconnect();
             ScaleMin.Disconnect();
            FinishRejectionStatus.Disconnect();
            FinishRejectionStatus1.Disconnect();
             NumberOfCircelDropDown.Disconnect();
             NumberOfCircleThreshold.Disconnect();
             NumberOfCircleCenterX.Disconnect();
             NumberOfCircleCenterY.Disconnect();
             NumberOfCircleStartX.Disconnect();
             NumberOfCircleStartY.Disconnect();
             NumberOfCircleEndX.Disconnect();
             NumberOfCircleEndY.Disconnect();
             NumberOfCircleThresholding.Disconnect();
             NumberOfCircelConditionDropDown.Disconnect();
             NumberOfCircelOperationDropDown.Disconnect();

            parentInspection.Show();
            this.Close();
        }
        public void HandleTextBoxFocusLoss()
        {
            this.hiddenButton.Focus(); // Ensure this sets the focus to the hidden button
        }
        private void FitErrorValue1(object sender, ValueChangedEventArgs args)
        { 
            try
            {
                string newValue = GetValidData(args.NewValue).ToString();
                if (FitErrorValue.InvokeRequired)
                {
                    FitErrorValue.Invoke(new Action(() => FitErrorValue.Text = newValue));
                }
                else
                {
                    FitErrorValue.Text = newValue;

                }
            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating CapWidthReadValue: {ex.Message}");
            }
        }
        private void ScaleErrorValue1(object sender, ValueChangedEventArgs args)
        {
            try
            {
                string newValue = GetValidData(args.NewValue).ToString();
                if (ScaleErrorValue.InvokeRequired)
                {
                    ScaleErrorValue.Invoke(new Action(() => ScaleErrorValue.Text = newValue));
                }
                else
                {
                    ScaleErrorValue.Text = newValue;

                }
            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating CapWidthReadValue: {ex.Message}");
            }
        }
        private void RadiusErrorValue1(object sender, ValueChangedEventArgs args)
        {
            try
            {
                string newValue = GetValidData(args.NewValue).ToString();
                if (RadiusErrorValue.InvokeRequired)
                {
                    RadiusErrorValue.Invoke(new Action(() => RadiusErrorValue.Text = newValue));
                }
                else
                {
                    RadiusErrorValue.Text = newValue;

                }
            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating CapWidthReadValue: {ex.Message}");
            }
        }
        private void ScoreErrorValue1(object sender, ValueChangedEventArgs args)
        {
            try
            {
                string newValue = GetValidData(args.NewValue).ToString();
                if (ScoreErrorValue.InvokeRequired)
                {
                    ScoreErrorValue.Invoke(new Action(() => ScoreErrorValue.Text = newValue));
                }
                else
                {
                    ScoreErrorValue.Text = newValue;

                }
            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating CapWidthReadValue: {ex.Message}");
            }
        }
        private void NumberOfCircleValue1(object sender, ValueChangedEventArgs args)
        {
            try
            {
                string newValue = GetValidData(args.NewValue).ToString();
                if (NumberOfCircelValue.InvokeRequired)
                {
                    NumberOfCircelValue.Invoke(new Action(() => NumberOfCircelValue.Text = newValue));
                }
                else
                {
                    NumberOfCircelValue.Text = newValue;

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
            //_messagesListView.Visible = true;
            hideTimer.Stop();
            hideTimer.Start();
        }
        private void C1TriggerDelay_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    C1TriggerDelay.Text = numberPadForm.Number;
                    if (ushort.TryParse(numberPadForm.Number, out ushort value))
                    {
                        byte[] buffer = new byte[2];
                        S7.SetUIntAt(buffer, 0, value);
                        int result = s7Client.DBWrite(4, 100, 2, buffer);
                        if (result != 0)
                            Debug.WriteLine($"Write error: {s7Client.ErrorText(result)}");
                    }
                }
            }
            HandleTextBoxFocusLoss();
        }
        private void C1TriggerPulse_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    C1TriggerPulse.Text = numberPadForm.Number;
                    if (int.TryParse(numberPadForm.Number, out int value))
                    {
                        byte[] buffer = new byte[4];
                        S7.SetDIntAt(buffer, 0, value);
                        int result = s7Client.DBWrite(4, 140, 4, buffer);
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
        private void C1Exposure_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    C1ExposureTxt.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void FitErrorThreshold_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FitErrorThreshold.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void RadiusErroThreshold_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    RadiusErroThreshold.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void ScoreErrorThreshold_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    ScoreErrorThreshold.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void ScaleErrorThreshold_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    ScaleErrorThreshold.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void NumbeOfOccurance_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    NumbeOfOccurance.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void Smoothness_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    Smoothness.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void Acceptance_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    Acceptance.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void Certainity_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    Certainity.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void FitScoreMin_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    FitScoreMin.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void SagittaTolerance_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    SagittaTolerance.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void CoverageMax_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    CoverageMax.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void CenterX_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    CenterX.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void CenterY_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    CenterY.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void Width_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    Width.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void Height_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    Height.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void Angle_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    Angle.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void ScaleMin_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    ScaleMin.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void Scale_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    Scale.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }
        private void ScaleMax_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    ScaleMax.Text = numberPadForm.Number; // Set the text box value
                }
            }
            HandleTextBoxFocusLoss();

        }

        private void NumberOfCircleThreshold_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())

                if (numberPadForm.ShowDialog() == DialogResult.OK)
            {
                NumberOfCircleThreshold.Text = numberPadForm.Number;
            }
            HandleTextBoxFocusLoss();

        }

        private void NumberOfCircleCenterX_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())

                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    NumberOfCircleCenterX.Text = numberPadForm.Number;
                }
            HandleTextBoxFocusLoss();

        }

        private void NumberOfCircleCenterY_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())

                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    NumberOfCircleCenterY.Text = numberPadForm.Number;
                }
            HandleTextBoxFocusLoss();

        }

        private void NumberOfCircleStartX_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())

                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    NumberOfCircleStartX.Text = numberPadForm.Number;
                }
            HandleTextBoxFocusLoss();

        }

        private void NumberOfCircleStartY_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())

                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    NumberOfCircleStartY.Text = numberPadForm.Number;
                }
            HandleTextBoxFocusLoss();

        }

        private void NumberOfCircleEndX_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())

                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    NumberOfCircleEndX.Text = numberPadForm.Number;
                }
            HandleTextBoxFocusLoss();

        }

        private void NumberOfCircleEndY_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())

                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    NumberOfCircleEndY.Text = numberPadForm.Number;
                }
            HandleTextBoxFocusLoss();

        }

        private void NumberOfCircleThresholding_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())

                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    NumberOfCircleThresholding.Text = numberPadForm.Number;
                }
            HandleTextBoxFocusLoss();

        }

        private void NumberOfCircleEndConditionThreshold_Enter(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())

                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    NumberOfCircleEndConditionThreshold.Text = numberPadForm.Number;
                }
            HandleTextBoxFocusLoss();

        }
    }
}
