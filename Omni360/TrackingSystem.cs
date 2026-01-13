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
using Sharp7;

namespace MatroxLDS
{
    public partial class TrackingSystem : MatroxLDS.BaseForm
    {
        private MainForm mainForm;
        private S7Client s7Client;
        private string plcIp = "192.168.0.20";
        private int plcRack = 0;
        private int plcSlot = 1;

        private int[] plcToGuiDInts = new int[6];

        // GUI->PLC arrays
        private int[] guiToPlcDInts = new int[6];
        private bool[] guiToPlcBools = new bool[7]; // Only the first 7 used (0-6)

        // PLC->GUI BOOLs
        private bool[] plcToGuiBools = new bool[18];

        // DB15 fields
        private bool[] db15Bools = new bool[6];
        private byte[] db15USInts = new byte[25];
        private ushort[] db15UInts = new ushort[4];
        private uint db15UDInt;
        // Add more arrays/fields if you later want to parse structs/arrays

        private Timer errorUpdateTimer;

        public TrackingSystem(MainForm _mainForm)
        {
            InitializeComponent();
            ButtonAnimator.InitializeAnimation(DashboardBtn, "blue");
            SetupSharp7Connection();

            mainForm = _mainForm;

            errorUpdateTimer = new Timer();
            errorUpdateTimer.Interval = 1000;
            errorUpdateTimer.Tick += ErrorUpdateTimer_Tick;
            errorUpdateTimer.Start();

        }

        private void TrackingSystem_Load(object sender, EventArgs e)
        {
            CurrentUserTxt.Text = mainForm.CurrentUserTxt.Text;
            DateTimeLabel.Text = mainForm.DateTimeLabel.Text;
            ReadAllPLCData();
            UpdateGuiToPlcTextBoxes();
            UpdateStatusLabels();
            ReadDb15PLCDataAndUpdateUI();    // <--- ADDED
            UpdateDb15PLCFields();           // <--- ADDED
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

        private void ErrorUpdateTimer_Tick(object sender, EventArgs e)
        {
            ReadAllPLCData();
            ReadDb15PLCDataAndUpdateUI(); // <--- ADDED
            UpdateDb15PLCFields();        // <--- ADDED
        }

        // --- DB100 LOGIC UNCHANGED ---
        private void ReadAllPLCData()
        {
            if (s7Client == null || !s7Client.Connected) return;

            int dbNumber = 100;
            int dbOffset = 0;
            int dbLength = 53;

            byte[] buffer = new byte[dbLength];
            int result = s7Client.DBRead(dbNumber, dbOffset, dbLength, buffer);

            if (result == 0)
            {
                // --- GUI->PLC DINTs ---
                guiToPlcDInts[0] = S7.GetDIntAt(buffer, 0);   // Slider 1 Position SP (Offset 0)
                guiToPlcDInts[1] = S7.GetDIntAt(buffer, 4);   // Slider 2 Position SP (Offset 4)
                guiToPlcDInts[2] = S7.GetDIntAt(buffer, 8);   // Slider 3 Position SP (Offset 8)
                guiToPlcDInts[3] = S7.GetDIntAt(buffer, 12);  // Slider 1 Speed (Offset 12)
                guiToPlcDInts[4] = S7.GetDIntAt(buffer, 16);  // Slider 2 Speed (Offset 16)
                guiToPlcDInts[5] = S7.GetDIntAt(buffer, 20);  // Slider 3 Speed (Offset 20)

                // --- GUI->PLC BOOLs ---
                guiToPlcBools[0] = S7.GetBitAt(buffer, 24, 0); // Homing Slider 1
                guiToPlcBools[1] = S7.GetBitAt(buffer, 24, 1); // Homing Slider 2
                guiToPlcBools[2] = S7.GetBitAt(buffer, 24, 2); // Homing Slider 3
                guiToPlcBools[3] = S7.GetBitAt(buffer, 24, 3); // Reset Slider 1
                guiToPlcBools[4] = S7.GetBitAt(buffer, 24, 4); // Reset Slider 2
                guiToPlcBools[5] = S7.GetBitAt(buffer, 24, 5); // Reset Slider 3
                guiToPlcBools[6] = S7.GetBitAt(buffer, 24, 6); // Execute Sliders SP change

                // --- PLC->GUI DINTs (errors) ---
                plcToGuiDInts[0] = S7.GetDIntAt(buffer, 26); // DINT[0] 26.0 Slider 1 Error Messages
                plcToGuiDInts[1] = S7.GetDIntAt(buffer, 30); // DINT[1] 30.0 Slider 2 Error Messages
                plcToGuiDInts[2] = S7.GetDIntAt(buffer, 34); // DINT[2] 34.0 Slider 3 Error Messages
                plcToGuiDInts[3] = S7.GetDIntAt(buffer, 38); // DINT[3] 38.0 Unused/Spare
                plcToGuiDInts[4] = S7.GetDIntAt(buffer, 42); // DINT[4] 42.0 Unused/Spare
                plcToGuiDInts[5] = S7.GetDIntAt(buffer, 46); // DINT[5] 46.0 Unused/Spare

                // --- PLC->GUI BOOLs ---
                plcToGuiBools[0] = S7.GetBitAt(buffer, 50, 0); // BOOL[0] 50.0 Slider 1 Error
                plcToGuiBools[1] = S7.GetBitAt(buffer, 50, 1); // BOOL[1] 50.1 Slider 2 Error
                plcToGuiBools[2] = S7.GetBitAt(buffer, 50, 2); // BOOL[2] 50.2 Slider 3 Error
                plcToGuiBools[3] = S7.GetBitAt(buffer, 50, 3); // BOOL[3] 50.3 Slider 1 Homed
                plcToGuiBools[4] = S7.GetBitAt(buffer, 50, 4); // BOOL[4] 50.4 Slider 2 Homed
                plcToGuiBools[5] = S7.GetBitAt(buffer, 50, 5); // BOOL[5] 50.5 Slider 3 Homed
                plcToGuiBools[6] = S7.GetBitAt(buffer, 50, 6); // BOOL[6] 50.6 Slider 1 Job Finished
                plcToGuiBools[7] = S7.GetBitAt(buffer, 50, 7); // BOOL[7] 50.7 Slider 2 Job Finished
                plcToGuiBools[8] = S7.GetBitAt(buffer, 51, 0); // BOOL[8] 51.0 Slider 3 Job Finished
                plcToGuiBools[9] = S7.GetBitAt(buffer, 51, 1); // BOOL[9] 51.1
                plcToGuiBools[10] = S7.GetBitAt(buffer, 51, 2); // BOOL[10] 51.2
                plcToGuiBools[11] = S7.GetBitAt(buffer, 51, 3); // BOOL[11] 51.3 Sliders_Execute_Rising_Edge
                plcToGuiBools[12] = S7.GetBitAt(buffer, 51, 4); // BOOL[12] 51.4 Slider_1_Execution_in_Progress
                plcToGuiBools[13] = S7.GetBitAt(buffer, 51, 5); // BOOL[13] 51.5 Slider_2_Execution_in_Progress
                plcToGuiBools[14] = S7.GetBitAt(buffer, 51, 6); // BOOL[14] 51.6 Slider_3_Execution_in_Progress
                plcToGuiBools[15] = S7.GetBitAt(buffer, 51, 7); // BOOL[15] 51.7
                plcToGuiBools[16] = S7.GetBitAt(buffer, 52, 0); // BOOL[16] 52.0
                plcToGuiBools[17] = S7.GetBitAt(buffer, 52, 1); // BOOL[17] 52.1

                UpdateErrorLabels();
                UpdateGuiToPlcCheckBoxes();
                UpdateStatusLabels();
            }
            else
            {
                Debug.WriteLine($"Sharp7 DBRead error: {s7Client.ErrorText(result)}");
            }
        }

        // ------ DB15 LOGIC ------
        private void ReadDb15PLCDataAndUpdateUI()
        {
            if (s7Client == null || !s7Client.Connected) return;

            int dbNumber = 15;
            int dbOffset = 0;
            int dbLength = 40; // Enough for all static fields up to Next_sample

            byte[] buffer = new byte[dbLength];
            int result = s7Client.DBRead(dbNumber, dbOffset, dbLength, buffer);

            if (result != 0)
            {
                Debug.WriteLine($"Sharp7 DBRead error for DB15: {s7Client.ErrorText(result)}");
                return;
            }

            // ---- BOOLs ----
            db15Bools[0] = S7.GetBitAt(buffer, 0, 0); // Reset
            db15Bools[1] = S7.GetBitAt(buffer, 0, 1); // Pitch_sensor_error
            db15Bools[2] = S7.GetBitAt(buffer, 0, 2); // Valve_sensor_error
            db15Bools[3] = S7.GetBitAt(buffer, 0, 3); // Outfeed_sensor_error
            db15Bools[4] = S7.GetBitAt(buffer, 0, 4); // Cap_sensor_error
            db15Bools[5] = S7.GetBitAt(buffer, 0, 5); // Sampling

            // ---- USInts ----
            for (int i = 0; i < 25 && (4 + i) < buffer.Length; i++)
            {
                db15USInts[i] = buffer[4 + i]; // Valve_QTY at 4.0, Cap_QTY at 5.0, etc.
            }

            // ---- UInts ----
            db15UInts[0] = S7.GetUIntAt(buffer, 24); // Outfeed_pocket_min
            db15UInts[1] = S7.GetUIntAt(buffer, 26); // Outfeed_pocket_max

            // ---- UDInt ----
            db15UDInt = S7.GetUDIntAt(buffer, 28); // Outfeed_to_Trigger
        }

        private void UpdateDb15PLCFields()
        {
            // --- Map DB15 fields to UI ---
            NumberOfFillerValves.Text = db15USInts[0].ToString(); // Valve_QTY
            NumberOfCapperHeads.Text = db15USInts[1].ToString(); // Cap_QTY
            ValveOffset.Text = db15USInts[2].ToString(); // Valve_Offset
            HeadOffset.Text = db15USInts[3].ToString(); // Cap_Offset
            // Next samples: db15USInts[4] = First_sample, [5] = Last_sample, [6] = Rotations_Samples,
            // [7] = Outfeed_index_in, [8]=Outfeed_index_out, etc.

            ValveNumberatTrigger.Text = db15USInts[9].ToString();  // Valve_at_Trigger (offset 13)
            HeadNumberatTrigger.Text = db15USInts[10].ToString(); // Cap_at_Trigger (offset 14)
            ValveNumberatOutfeed.Text = db15USInts[12].ToString(); // Valve_at_outfeed_head (offset 16)
            HeadNumberatOutfeed.Text = db15USInts[14].ToString(); // Cap_at_outfeed_head (offset 18)

            PocketsOutfeedtoTrigger.Text = db15USInts[15].ToString(); // Pockets_outfeed_to_trigger (offset 15)
            OutfeedtoTriggerPulses.Text = db15UDInt.ToString();      // Outfeed_to_Trigger (offset 28)
            // Outfeed_pocket_min = db15UInts[0], Outfeed_pocket_max = db15UInts[1]
            // Use these if you want to display somewhere else
        }
        // Helper for writing USInt to DB15
        private void WriteUSIntToPLC_DB15(int offset, string valueText)
        {
            byte value;
            if (byte.TryParse(valueText, out value))
            {
                byte[] buffer = new byte[1];
                buffer[0] = value;
                int result = s7Client.DBWrite(15, offset, 1, buffer);
                if (result != 0)
                {
                    MessageBox.Show($"Failed to write value to DB15: {s7Client.ErrorText(result)}");
                }
            }
            else
            {
                MessageBox.Show("Invalid number entered!");
            }
        }



        private void UpdateErrorLabels()
        {
            labelSlider1Error.Text = GetErrorDescription(plcToGuiDInts[0]);
            labelSlider2Error.Text = GetErrorDescription(plcToGuiDInts[1]);
            labelSlider3Error.Text = GetErrorDescription(plcToGuiDInts[2]);
        }

        // Update TextBoxes for positions and speeds
        private void UpdateGuiToPlcTextBoxes()
        {
            textBoxSlider1Position.Text = guiToPlcDInts[0].ToString();
            textBoxSlider2Position.Text = guiToPlcDInts[1].ToString();
            textBoxSlider3Position.Text = guiToPlcDInts[2].ToString();

            textBoxSlider1Speed.Text = guiToPlcDInts[3].ToString();
            textBoxSlider2Speed.Text = guiToPlcDInts[4].ToString();
            textBoxSlider3Speed.Text = guiToPlcDInts[5].ToString();
        }

        // Update CheckBoxes for booleans
        private void UpdateGuiToPlcCheckBoxes()
        {
            checkBoxHoming1.Checked = guiToPlcBools[0];
            checkBoxHoming2.Checked = guiToPlcBools[1];
            checkBoxHoming3.Checked = guiToPlcBools[2];
            checkBoxReset1.Checked = guiToPlcBools[3];
            checkBoxReset2.Checked = guiToPlcBools[4];
            checkBoxReset3.Checked = guiToPlcBools[5];
            checkBoxExecuteSPChange.Checked = guiToPlcBools[6];
        }

        // --- ADDED: Update Status Labels for PLC->GUI BOOLs ---
        private void UpdateStatusLabels()
        {
            labelSlider1Status.Text = GetSliderStatusText(1);
            labelSlider2Status.Text = GetSliderStatusText(2);
            labelSlider3Status.Text = GetSliderStatusText(3);
        }

        private string GetSliderStatusText(int slider)
        {
            int errIdx = slider - 1;       // 0,1,2
            int homedIdx = slider + 2;     // 3,4,5
            int jobIdx = slider + 5;       // 6,7,8
            int execIdx = slider + 11;     // 12,13,14

            string status = $"Error: {(plcToGuiBools[errIdx] ? "Yes" : "No")}, " +
                            $"Homed: {(plcToGuiBools[homedIdx] ? "Yes" : "No")}, " +
                            $"Job Finished: {(plcToGuiBools[jobIdx] ? "Yes" : "No")}, " +
                            $"In Progress: {(plcToGuiBools[execIdx] ? "Yes" : "No")}";

            // Add newline after each comma for label display
            return status.Replace(", ", Environment.NewLine);
        }

        private void DashboardBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string GetErrorDescription(int errorCode)
        {
            switch (errorCode)
            {
                case 1: return "Direction Fault";
                case 2: return "Hardware Limit";
                case 3: return "Position Fault";
                case 4: return "System Fault";
                case 5: return "Config Fault";
                case 6: return "Software Limit";
                case 0: return "No Error Messages";
                default: return $"Unknown Error ({errorCode})";
            }
        }

        public void HandleTextBoxFocusLoss()
        {
            this.hiddenButton.Focus(); // Ensure this sets the focus to the hidden button
        }

        // Click events for DINT value textboxes - now updated to write changes to PLC on user entry
        private void textBoxSlider1Position_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxSlider1Position.Text = numberPadForm.Number;
                    WriteDIntToPLC(0, numberPadForm.Number); // Offset 0 for Slider 1 Position SP
                }
            }
            HandleTextBoxFocusLoss();
        }
        private void textBoxSlider2Position_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxSlider2Position.Text = numberPadForm.Number;
                    WriteDIntToPLC(4, numberPadForm.Number); // Offset 4 for Slider 2 Position SP
                }
            }
            HandleTextBoxFocusLoss();
        }
        private void textBoxSlider3Position_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxSlider3Position.Text = numberPadForm.Number;
                    WriteDIntToPLC(8, numberPadForm.Number); // Offset 8 for Slider 3 Position SP
                }
            }
            HandleTextBoxFocusLoss();
        }
        private void textBoxSlider1Speed_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxSlider1Speed.Text = numberPadForm.Number;
                    WriteDIntToPLC(12, numberPadForm.Number); // Offset 12 for Slider 1 Speed
                }
            }
            HandleTextBoxFocusLoss();
        }
        private void textBoxSlider2Speed_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxSlider2Speed.Text = numberPadForm.Number;
                    WriteDIntToPLC(16, numberPadForm.Number); // Offset 16 for Slider 2 Speed
                }
            }
            HandleTextBoxFocusLoss();
        }
        private void textBoxSlider3Speed_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxSlider3Speed.Text = numberPadForm.Number;
                    WriteDIntToPLC(20, numberPadForm.Number); // Offset 20 for Slider 3 Speed
                }
            }
            HandleTextBoxFocusLoss();
        }

        // Helper for writing DINT to PLC DB100 at correct offset
        private void WriteDIntToPLC(int offset, string valueText)
        {
            int value;
            if (int.TryParse(valueText, out value))
            {
                byte[] buffer = new byte[4];
                S7.SetDIntAt(buffer, 0, value);
                int result = s7Client.DBWrite(100, offset, 4, buffer);
                if (result != 0)
                {
                    MessageBox.Show($"Failed to write value to PLC: {s7Client.ErrorText(result)}");
                }
            }
            else
            {
                MessageBox.Show("Invalid number entered!");
            }
        }
        private void NumberOfFillerValves_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog(this) == DialogResult.OK)
                {
                    NumberOfCapperHeads.Text = numberPadForm.Number;
                    WriteUSIntToPLC_DB15(4, numberPadForm.Number);
                }
            }
            HandleTextBoxFocusLoss();

        }

        private void NumberOfCapperHeads_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog(this) == DialogResult.OK)
                {
                    NumberOfCapperHeads.Text = numberPadForm.Number;
                    WriteUSIntToPLC_DB15(5, numberPadForm.Number); // 5 is offset for Cap_QTY
                }
            }
            HandleTextBoxFocusLoss();
        }

        private void ValveOffset_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog(this) == DialogResult.OK)
                {
                    ValveOffset.Text = numberPadForm.Number;
                    WriteUSIntToPLC_DB15(6, numberPadForm.Number); // 6 for Valve_Offset
                }
            }
            HandleTextBoxFocusLoss();
        }

        private void HeadOffset_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog(this) == DialogResult.OK)
                {
                    HeadOffset.Text = numberPadForm.Number;
                    WriteUSIntToPLC_DB15(7, numberPadForm.Number); // 7 for Cap_Offset
                }
            }
            HandleTextBoxFocusLoss();
        }

        private void PocketsOutfeedtoTrigger_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog(this) == DialogResult.OK)
                {
                    NumberOfCapperHeads.Text = numberPadForm.Number;
                    WriteUSIntToPLC_DB15(15, numberPadForm.Number);
                }
            }
            HandleTextBoxFocusLoss();

        }

        private void OutfeedtoTriggerPulses_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog(this) == DialogResult.OK)
                {
                    NumberOfCapperHeads.Text = numberPadForm.Number;
                    WriteUSIntToPLC_DB15(28, numberPadForm.Number); 
                }
            }
            HandleTextBoxFocusLoss();

        }
    }
}