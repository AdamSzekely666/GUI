using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Omnicheck360.Properties;
using Nini.Config;
using System.Diagnostics;
using Sharp7;

namespace Omnicheck360
{
    public partial class GlobalTracking : BaseForm
    {
        public MainMenu MainMenu;
        public MainForm mainForm;
        public PopUpNumberPad popUpNumberPad;

        private static IniConfigSource source;
        private string cPath;
        private string AppFile;

        public static byte[] DB_Byte_Write_Buffer = new byte[1];
        public static byte[] DB_MeasureProductWidth_Write_Buffer = new byte[1];
        public static byte[] DB_MeasureReject_Write_Buffer = new byte[1];
        public static byte[] DB_DINT_Write_Buffer = new byte[4];
        public static byte[] DB_ResetServo1Error_Write_Buffer = new byte[1];
        public static byte[] DB_ResetServo2Error_Write_Buffer = new byte[1];
        public static byte[] DB_ResetServo3Error_Write_Buffer = new byte[1];
        public static byte[] DB_ServoPositionChange_Write_Buffer = new byte[1];
        public static byte[] DB_RejectPulse_Write_Buffer = new byte[2];
        public static byte[] DB_RejectDelay_Write_Buffer = new byte[2];
        public static byte[] DB_ReTriggerDelay_Write_Buffer = new byte[2];
        public static byte[] DB_CameraTimeout_Write_Buffer = new byte[4];
        public static byte[] DB_ConsecutiveReject_Write_Buffer = new byte[4];
        public static byte[] DB_ProductWidth_Write_Buffer = new byte[4];
        public static byte[] DB_RejectWidth_Write_Buffer = new byte[4];
        public static byte[] DB_ZeroSystem_Bool = new byte[1];
        public static byte[] DB_Inspect_Conveyor_Write_Manual = new byte[1];
        public static byte[] DB_Inspect_Conveyor_Write_Run = new byte[1];
        public static byte[] DB_Reject_Conveyor_Write_Manual = new byte[1];
        public static byte[] DB_Reject_Conveyor_Write_Run = new byte[1];
        public static byte[] DB_Master_PPS_Max_Write_Buffer = new byte[2];
        public static byte[] DB_Master_PPS_Min_Write_Buffer = new byte[2];
        public static byte[] DB_VFD_Max_Write_Buffer = new byte[2];
        public static byte[] DB_Inspection_Set_Point_Write_Buffer = new byte[2];
        public static byte[] DB_Rejection_Set_Point_Write_Buffer = new byte[2];


        public static byte[] DB_RejectorDelay_Read_Buffer = new byte[4];
        public static byte[] DB_Timeout_Read_Buffer = new byte[4];
        public static byte[] DB_MaxConsecutiveRejects_Read_Buffer = new byte[4];
        public static byte[] DB_RejectorPulseWidth_Read_Buffer = new byte[4];
        public static byte[] DB_EncoderPPS_Read_Buffer = new byte[4];
        public static byte[] DB_UpstreamEncoderPPS_Read_Buffer = new byte[4];
        public static byte[] DB_ProductWidth_Read_Buffer = new byte[4];
        public static byte[] DB_RejectWidth_Read_Buffer = new byte[4];
        public static byte[] DB_CalculatedProductWidth_Read_Buffer = new byte[4];
        public static byte[] DB_CalculatedRejectWidth_Read_Buffer = new byte[4];
        public static byte[] DB_ServoError_Read_Buffer = new byte[1];
        public static byte[] DB_Servo1Position_Read_Buffer = new byte[4];
        public static byte[] DB_Servo2Position_Read_Buffer = new byte[4];
        public static byte[] DB_Servo3Position_Read_Buffer = new byte[4];
        public static byte[] DB_Servo1Done_Read_Buffer = new byte[1];
        public static byte[] DB_Servo2Done_Read_Buffer = new byte[1];
        public static byte[] DB_Servo3Done_Read_Buffer = new byte[1];
        public static byte[] DB_Variables_Read_Buffer = new byte[40];
        public static byte[] DB_CUCD_Read_Buffer = new byte[2];
        public static byte[] DB_Master_PPS_Max_Read_Buffer = new byte[2];
        public static byte[] DB_Master_PPS_Min_Read_Buffer = new byte[2];
        public static byte[] DB_VFD_Max_Read_Buffer = new byte[2];

        public static byte[] DB_Inspect_Conveyor_Read_Frequency = new byte[2];
        public static byte[] DB_Inspect_Conveyor_Read_Voltage = new byte[2];
        public static byte[] DB_Inspect_Conveyor_Read_Current = new byte[2];
        public static byte[] DB_Inspect_Conveyor_Read_Pole = new byte[1];
        public static byte[] DB_Inspect_Conveyor_Read_Mode = new byte[1];
        public static byte[] DB_Inspect_Conveyor_Read_DCBus = new byte[2];
        public static byte[] DB_Inspect_Conveyor_Read_Ratio = new byte[1];
        public static byte[] DB_Inspect_Conveyor_Read_Status = new byte[1];
        public static byte[] DB_Inspect_Conveyor_Read_Setpoint = new byte[2];
        public static byte[] DB_Inspect_Conveyor_Read_Manual = new byte[2];
        public static byte[] DB_Inspect_Conveyor_Read_Run = new byte[2];

        public static byte[] DB_Reject_Conveyor_Read_Frequency = new byte[2];
        public static byte[] DB_Reject_Conveyor_Read_Voltage = new byte[2];
        public static byte[] DB_Reject_Conveyor_Read_Current = new byte[2];
        public static byte[] DB_Reject_Conveyor_Read_Pole = new byte[1];
        public static byte[] DB_Reject_Conveyor_Read_Mode = new byte[1];
        public static byte[] DB_Reject_Conveyor_Read_DCBus = new byte[2];
        public static byte[] DB_Reject_Conveyor_Read_Ratio = new byte[1];
        public static byte[] DB_Reject_Conveyor_Read_Status = new byte[1];
        public static byte[] DB_Reject_Conveyor_Read_Setpoint = new byte[2];
        public static byte[] DB_Reject_Conveyor_Read_Manual = new byte[2];
        public static byte[] DB_Reject_Conveyor_Read_Run = new byte[2];


        public static byte[] MAX_PPS_TextBox = new byte[1];

        public GlobalTracking(MainForm _mainForm)
        {
            InitializeComponent();
            ButtonAnimator.InitializeAnimation(MainMenuFormBtn, "blue");

            mainForm = _mainForm;

            cPath = Environment.CurrentDirectory;
            AppFile = cPath + "\\RecipeTracking.ini";

            source = new IniConfigSource(AppFile);
        }

        private void GlobalTracking_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            timer1.Start();
            CurrentUserTxt.Text = mainForm.CurrentUserTxt.Text;

            SetinitialValues();

            ResetError1Btn.Visible = false;
            ResetError2Btn.Visible = false;
            ResetError3Btn.Visible = false;

            EncoderPPS.Start();

            MainForm.log.Info("*********************************************************************************");
            MainForm.log.Info("Global Tracking Page: Rejection Type Value is ---------------------" + CUCD_Text_Box);
            MainForm.log.Info("Global Tracking Page: Reject Pulsewidth Value is ------------------" + RejPulseWidthTextBox);
            MainForm.log.Info("Global Tracking Page: Cam1 to Reject Value is ---------------------" + TrigToRejDelayTextBox);
            MainForm.log.Info("Global Tracking Page: Reject Sensor to Reject Value is ------------" + ReTrigger_Reject);
            MainForm.log.Info("Global Tracking Page: Master PPS Max Value is ---------------------" + Master_PPS_Max);
            MainForm.log.Info("Global Tracking Page: Master PPS Min Value is ---------------------" + Master_PPS_Min);
            MainForm.log.Info("Global Tracking Page: VFD Max Value is ----------------------------" + VFD_Max);
            MainForm.log.Info("Global Tracking Page: Servo 1 Position Value is -------------------" + Servo1PositionTextBox);
            MainForm.log.Info("Global Tracking Page: Servo 2 Position Value is -------------------" + Servo2PositionTextBox);
            MainForm.log.Info("Global Tracking Page: Servo 3 Position Value is -------------------" + Servo3PositionTextBox);
            MainForm.log.Info("Global Tracking Page: Camera Timeout Value is ---------------------" + CameraTimeoutTextBox);
            MainForm.log.Info("Global Tracking Page: Max Consecutive Value is --------------------" + MaxConsecutiveTextBox);
            MainForm.log.Info("Global Tracking Page: Product Width Value is ----------------------" + ProductWidthTextBox);
            MainForm.log.Info("Global Tracking Page: Product Width Value is ----------------------" + RejectWidthTextBox);
            MainForm.log.Info("*********************************************************************************");
        }

        private void SetinitialValues()
        {
            int DB_Read_Result;

            DB_Read_Result = MainForm.Client.DBRead(4, 100, 40, DB_Variables_Read_Buffer);
            if (DB_Read_Result == 0)
            {
                TrigToRejDelayTextBox.Text = S7.GetUIntAt(DB_Variables_Read_Buffer, 10).ToString();
                RejPulseWidthTextBox.Text = S7.GetUIntAt(DB_Variables_Read_Buffer, 12).ToString();
                ProductWidthTextBox.Text = S7.GetUIntAt(DB_Variables_Read_Buffer, 14).ToString();
                RejectWidthTextBox.Text = S7.GetUIntAt(DB_Variables_Read_Buffer, 38).ToString();
                MaxConsecutiveTextBox.Text = S7.GetUIntAt(DB_Variables_Read_Buffer, 16).ToString();
                ReTrigger_Reject.Text = S7.GetUIntAt(DB_Variables_Read_Buffer, 30).ToString();
            }
            else
            {
            }

            DB_Read_Result = MainForm.Client.DBRead(4, 160, 4, DB_Timeout_Read_Buffer);
            if (DB_Read_Result == 0)
            {
                CameraTimeoutTextBox.Text = S7.GetDIntAt(DB_Timeout_Read_Buffer, 0).ToString();
            }
            else
            {
            }

            if (MainForm.Servo1Enable == 1)
            {
                DB_Read_Result = MainForm.Client.DBRead(MainForm.DataBlock, 88, 4, DB_Servo1Position_Read_Buffer);
                if (DB_Read_Result == 0)
                {
                    Servo1PositionTextBox.Text = S7.GetDIntAt(DB_Servo1Position_Read_Buffer, 0).ToString();
                }
                else
                {
                }
            }
            else
            {
                alphaBlendTextBox7.Text = "";
                Servo1PositionTextBox.Visible = false;
            }

            if (MainForm.Servo2Enable == 1)
            {
                DB_Read_Result = MainForm.Client.DBRead(MainForm.DataBlock, 92, 4, DB_Servo2Position_Read_Buffer);
                if (DB_Read_Result == 0)
                {
                    Servo2PositionTextBox.Text = S7.GetDIntAt(DB_Servo2Position_Read_Buffer, 0).ToString();
                }
                else
                {
                }
            }
            else
            {
                alphaBlendTextBox9.Text = "";
                Servo2PositionTextBox.Visible = false;
            }

            if (MainForm.Servo3Enable == 1)
            {
                DB_Read_Result = MainForm.Client.DBRead(MainForm.DataBlock, 96, 4, DB_Servo3Position_Read_Buffer);
                if (DB_Read_Result == 0)
                {
                    Servo3PositionTextBox.Text = S7.GetDIntAt(DB_Servo3Position_Read_Buffer, 0).ToString();
                }
                else
                {
                }
            }
            else
            {
                alphaBlendTextBox10.Text = "";
                Servo3PositionTextBox.Visible = false;
            }


        }

        private void TrigToRejDelayTextBox_Click(object sender, EventArgs e)
        {
            popUpNumberPad = new PopUpNumberPad();
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    TrigToRejDelayTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source.Configs[MainForm.Current_Recipe_Num.ToString()].Set("RejectorDelay", TrigToRejDelayTextBox.Text);
                source.Save();

                S7.SetUIntAt(DB_RejectDelay_Write_Buffer, 0, (ushort)Convert.ToInt32(TrigToRejDelayTextBox.Text));
                MainForm.Client.DBWrite(4, 110, 2, DB_RejectDelay_Write_Buffer);

                MainForm.log.Info("Global Tracking Page: Trigger to Rejector Delay Value Changed To" + TrigToRejDelayTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }
        }

        private void RejPulseWidthTextBox_Click(object sender, EventArgs e)
        {
            popUpNumberPad = new PopUpNumberPad();
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    RejPulseWidthTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source.Configs[MainForm.Current_Recipe_Num.ToString()].Set("RejectorPulseWidth", RejPulseWidthTextBox.Text);
                source.Save();

                S7.SetUIntAt(DB_RejectPulse_Write_Buffer, 0, (ushort)Convert.ToInt32(RejPulseWidthTextBox.Text));
                MainForm.Client.DBWrite(4, 112, 2, DB_RejectPulse_Write_Buffer);

                MainForm.log.Info("Global Tracking Page: Reject Pulse Width Value Changed To" + RejPulseWidthTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }
        }
        private void MeasureProductWidthBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Buffer.SetByte(DB_MeasureProductWidth_Write_Buffer, 0, 1);
                MainForm.Client.WriteArea(S7Consts.S7AreaDB, 4, 200, 0, S7Consts.S7WLBit, DB_MeasureProductWidth_Write_Buffer);
                MainForm.log.Info("Global Tracking Page: Measure Product Width Button Pressed");


                ProductWidthTimer.Start();
            }
            catch
            {
            }
        }

        private void EncoderPPS_Tick(object sender, EventArgs e)
        {
            int DB_Read_Result;

            DB_Read_Result = MainForm.Client.DBRead(4, 44, 4, DB_EncoderPPS_Read_Buffer);
            DB_Read_Result = MainForm.Client.DBRead(4, 60, 4, DB_UpstreamEncoderPPS_Read_Buffer);
            DB_Read_Result = MainForm.Client.DBRead(4, 56, 4, DB_ProductWidth_Read_Buffer);
            DB_Read_Result = MainForm.Client.DBRead(4, 64, 4, DB_RejectWidth_Read_Buffer);


            _ = MainForm.Client.DBRead(101, 0, 2, DB_Inspect_Conveyor_Read_Frequency);
            _ = MainForm.Client.DBRead(101, 2, 2, DB_Inspect_Conveyor_Read_Voltage);
            _ = MainForm.Client.DBRead(101, 4, 2, DB_Inspect_Conveyor_Read_Current);
            _ = MainForm.Client.DBRead(101, 6, 1, DB_Inspect_Conveyor_Read_Pole);
            _ = MainForm.Client.DBRead(101, 7, 1, DB_Inspect_Conveyor_Read_Mode);
            _ = MainForm.Client.DBRead(101, 8, 2, DB_Inspect_Conveyor_Read_DCBus);
            _ = MainForm.Client.DBRead(101, 10, 1, DB_Inspect_Conveyor_Read_Ratio);
            _ = MainForm.Client.DBRead(101, 11, 1, DB_Inspect_Conveyor_Read_Status);
            _ = MainForm.Client.DBRead(101, 12, 2, DB_Inspect_Conveyor_Read_Setpoint);

            
            _ = MainForm.Client.DBRead(101, 14, 2, DB_Inspect_Conveyor_Read_Manual);
            if (S7.GetBitAt(DB_Inspect_Conveyor_Read_Manual, 0, 0) == false)
            {
                Insp_Conv_Manual.Text = "AUTOMATIC";
                Insp_Conv_Setpoint.Visible = false;
                Insp_Conv_Run.Visible = false;
            }
            if (S7.GetBitAt(DB_Inspect_Conveyor_Read_Manual, 0, 0) == true)
            { 
                Insp_Conv_Manual.Text = "MANUAL";
                Insp_Conv_Setpoint.Visible = true;
                Insp_Conv_Run.Visible = true;
            }

            _ = MainForm.Client.DBRead(101, 14, 2, DB_Inspect_Conveyor_Read_Run);
            if (S7.GetBitAt(DB_Inspect_Conveyor_Read_Run, 0, 1) == false)
            { Insp_Conv_Run.Text = "FALSE"; }
            if (S7.GetBitAt(DB_Inspect_Conveyor_Read_Run, 0, 1) == true)
            { Insp_Conv_Run.Text = "TRUE"; }




            _ = MainForm.Client.DBRead(101, 16, 2, DB_Reject_Conveyor_Read_Frequency);
            _ = MainForm.Client.DBRead(101, 18, 2, DB_Reject_Conveyor_Read_Voltage);
            _ = MainForm.Client.DBRead(101, 20, 2, DB_Reject_Conveyor_Read_Current);
            _ = MainForm.Client.DBRead(101, 22, 1, DB_Reject_Conveyor_Read_Pole);
            _ = MainForm.Client.DBRead(101, 23, 1, DB_Reject_Conveyor_Read_Mode);
            _ = MainForm.Client.DBRead(101, 24, 2, DB_Reject_Conveyor_Read_DCBus);
            _ = MainForm.Client.DBRead(101, 26, 1, DB_Reject_Conveyor_Read_Ratio);
            _ = MainForm.Client.DBRead(101, 27, 1, DB_Reject_Conveyor_Read_Status);
            _ = MainForm.Client.DBRead(101, 28, 2, DB_Reject_Conveyor_Read_Setpoint);



            _ = MainForm.Client.DBRead(101, 30, 2, DB_Reject_Conveyor_Read_Manual);
            if (S7.GetBitAt(DB_Reject_Conveyor_Read_Manual, 0, 0) == false)
            { 
                Rej_Conv_Manual.Text = "AUTOMATIC";
                Rej_Conv_Setpoint.Visible = false;
                Rej_Conv_Run.Visible = false;
            }
            if (S7.GetBitAt(DB_Reject_Conveyor_Read_Manual, 0, 0) == true)
            { 
                Rej_Conv_Manual.Text = "MANUAL";
                Rej_Conv_Setpoint.Visible = true;
                Rej_Conv_Run.Visible = true;
            }
                
            
            
            _ = MainForm.Client.DBRead(101, 30, 2, DB_Reject_Conveyor_Read_Run);
            if (S7.GetBitAt(DB_Reject_Conveyor_Read_Manual, 0, 1) == false)
            { Rej_Conv_Run.Text = "FALSE"; }
            if (S7.GetBitAt(DB_Reject_Conveyor_Read_Manual, 0, 1) == true)
            { Rej_Conv_Run.Text = "TRUE"; }

            _ = MainForm.Client.DBRead(4, 200, 2, DB_CUCD_Read_Buffer);
            if (S7.GetBitAt(DB_CUCD_Read_Buffer, 0, 5) == false)
            { 
                CUCD_Text_Box.Text = "ENCODER";
                Cam1toRejectDelayLabel.Visible = true;
                TrigToRejDelayTextBox.Visible = true;
                RejSensortoRejLabel.Visible = false;
                ReTrigger_Reject.Visible = false;
                MeasuredRejectProductLabel.Visible = false;
                RejProductWidthLabel.Visible = false;
                RejectWidthTextBox.Visible = false;
                RejectProductWidthValue.Visible = false;
            }
            if (S7.GetBitAt(DB_CUCD_Read_Buffer, 0, 5) == true)
            { 
                CUCD_Text_Box.Text = "CUCD";
                Cam1toRejectDelayLabel.Visible = false;
                TrigToRejDelayTextBox.Visible = false;
                RejSensortoRejLabel.Visible = true;
                ReTrigger_Reject.Visible = true;
                MeasuredRejectProductLabel.Visible = true;
                RejProductWidthLabel.Visible = true;
                RejectWidthTextBox.Visible = true;
                RejectProductWidthValue.Visible = true;

            }

            _ = MainForm.Client.DBRead(4, 132, 2, DB_Master_PPS_Max_Read_Buffer);
            _ = MainForm.Client.DBRead(4, 134, 2, DB_Master_PPS_Min_Read_Buffer);
            _ = MainForm.Client.DBRead(4, 136, 2, DB_VFD_Max_Read_Buffer);



            if (DB_Read_Result == 0)
            {
                EncoderPPSValue.Text = S7.GetDIntAt(DB_EncoderPPS_Read_Buffer, 0).ToString();
                UpstreamEncoderPPSValue.Text = S7.GetDIntAt(DB_UpstreamEncoderPPS_Read_Buffer, 0).ToString();
                MeasuredProductWidthValue.Text = S7.GetDIntAt(DB_ProductWidth_Read_Buffer, 0).ToString();
                RejectProductWidthValue.Text = S7.GetDIntAt(DB_RejectWidth_Read_Buffer, 0).ToString();

                Insp_Conv_Freq.Text = S7.GetIntAt(DB_Inspect_Conveyor_Read_Frequency, 0).ToString();
                Insp_Conv_Volt.Text = S7.GetIntAt(DB_Inspect_Conveyor_Read_Voltage, 0).ToString();
                Insp_Conv_Current.Text = S7.GetIntAt(DB_Inspect_Conveyor_Read_Current, 0).ToString();
                Insp_Conv_Mode.Text = S7.GetUSIntAt(DB_Inspect_Conveyor_Read_Mode, 0).ToString();
                Insp_Conv_Pole.Text = S7.GetUSIntAt(DB_Inspect_Conveyor_Read_Pole, 0).ToString();
                Insp_Conv_DCBus.Text = S7.GetIntAt(DB_Inspect_Conveyor_Read_DCBus, 0).ToString();
                Insp_Conv_Ratio.Text = S7.GetUSIntAt(DB_Inspect_Conveyor_Read_Ratio, 0).ToString();
                Insp_Conv_Status.Text = S7.GetByteAt(DB_Inspect_Conveyor_Read_Status, 0).ToString();
                Insp_Conv_Setpoint.Text = S7.GetUIntAt(DB_Inspect_Conveyor_Read_Setpoint, 0).ToString();

                Rej_Conv_Freq.Text = S7.GetIntAt(DB_Reject_Conveyor_Read_Frequency, 0).ToString();
                Rej_Conv_Volt.Text = S7.GetIntAt(DB_Reject_Conveyor_Read_Voltage, 0).ToString();
                Rej_Conv_Current.Text = S7.GetIntAt(DB_Reject_Conveyor_Read_Current, 0).ToString();
                Rej_Conv_Mode.Text = S7.GetUSIntAt(DB_Reject_Conveyor_Read_Mode, 0).ToString();
                Rej_Conv_Pole.Text = S7.GetUSIntAt(DB_Reject_Conveyor_Read_Pole, 0).ToString();
                Rej_Conv_DCBus.Text = S7.GetIntAt(DB_Reject_Conveyor_Read_DCBus, 0).ToString();
                Rej_Conv_Ratio.Text = S7.GetUSIntAt(DB_Reject_Conveyor_Read_Ratio, 0).ToString();
                Rej_Conv_Status.Text = S7.GetByteAt(DB_Reject_Conveyor_Read_Status, 0).ToString();
                Rej_Conv_Setpoint.Text = S7.GetUIntAt(DB_Reject_Conveyor_Read_Setpoint, 0).ToString();

                Master_PPS_Max.Text = S7.GetUIntAt(DB_Master_PPS_Max_Read_Buffer, 0).ToString();
                Master_PPS_Min.Text = S7.GetUIntAt(DB_Master_PPS_Min_Read_Buffer, 0).ToString();
                VFD_Max.Text = S7.GetUIntAt(DB_VFD_Max_Read_Buffer, 0).ToString();


            }


            else
            {
            }

            DB_Read_Result = MainForm.Client.DBRead(MainForm.DataBlock, 988, 1, DB_ServoError_Read_Buffer);
            if (DB_Read_Result == 0)
            {
                if (MainForm.Servo1Enable == 1)
                {
                    if (S7.GetBitAt(DB_ServoError_Read_Buffer, 0, 0) == true)
                    {
                        ResetError1Btn.Visible = true;
                    }
                    else
                    {
                        ResetError1Btn.Visible = false;
                    }
                }

                if (MainForm.Servo2Enable == 1)
                {
                    if (S7.GetBitAt(DB_ServoError_Read_Buffer, 0, 1) == true)
                    {
                        ResetError2Btn.Visible = true;
                    }
                    else
                    {
                        ResetError2Btn.Visible = false;
                    }
                }

                if (MainForm.Servo3Enable == 1)
                {
                    if (S7.GetBitAt(DB_ServoError_Read_Buffer, 0, 2) == true)
                    {
                        ResetError3Btn.Visible = true;
                    }
                    else
                    {
                        ResetError3Btn.Visible = false;
                    }
                }  
            }
            else
            {
            }
        }

        private void CameraTimeoutTextBox_Click(object sender, EventArgs e)
        {
            popUpNumberPad = new PopUpNumberPad();
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    CameraTimeoutTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source.Configs[MainForm.Current_Recipe_Num.ToString()].Set("InspectionTimeout", CameraTimeoutTextBox.Text);
                source.Save();

                S7.SetDIntAt(DB_CameraTimeout_Write_Buffer, 0, Convert.ToInt32(CameraTimeoutTextBox.Text));
                MainForm.Client.DBWrite(4, 160, 4, DB_CameraTimeout_Write_Buffer);

                MainForm.log.Info("Global Tracking Page: Camera Timeout Value Changed To" + CameraTimeoutTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }
        }

        private void MaxConsecutiveTextBox_Click(object sender, EventArgs e)
        {
            popUpNumberPad = new PopUpNumberPad();
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    MaxConsecutiveTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source.Configs[MainForm.Current_Recipe_Num.ToString()].Set("MaxConsecutiveRejects", MaxConsecutiveTextBox.Text);
                source.Save();

                S7.SetUIntAt(DB_ConsecutiveReject_Write_Buffer, 0, (ushort)Convert.ToInt32(MaxConsecutiveTextBox.Text));
                MainForm.Client.DBWrite(4, 116, 4, DB_ConsecutiveReject_Write_Buffer);

                MainForm.log.Info("Global Tracking Page: Max Consecutive Reject Value Changed To" + MaxConsecutiveTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }
        }

        private void ProductWidthTextBox_Click(object sender, EventArgs e)
        {
            popUpNumberPad = new PopUpNumberPad();
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    ProductWidthTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source.Configs[MainForm.Current_Recipe_Num.ToString()].Set("ProductWidth", ProductWidthTextBox.Text);
                source.Save();

                S7.SetUIntAt(DB_ProductWidth_Write_Buffer, 0, (ushort)Convert.ToInt32(ProductWidthTextBox.Text));
                MainForm.Client.DBWrite(4, 114, 4, DB_ProductWidth_Write_Buffer);

                MainForm.log.Info("Global Tracking Page: Product Width Value Changed To" + ProductWidthTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }
        }

        private void RejectWidthTextBox_Click(object sender, EventArgs e)
        {
            popUpNumberPad = new PopUpNumberPad();
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    RejectWidthTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source.Configs[MainForm.Current_Recipe_Num.ToString()].Set("RejectWidth", RejectWidthTextBox.Text);
                source.Save();

                S7.SetUIntAt(DB_RejectWidth_Write_Buffer, 0, (ushort)Convert.ToInt32(RejectWidthTextBox.Text));
                MainForm.Client.DBWrite(4, 138, 4, DB_RejectWidth_Write_Buffer);

                MainForm.log.Info("Global Tracking Page: Product Width Value Changed To" + RejectWidthTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }
        }

        private void ResetError1Btn_Click(object sender, EventArgs e)
        {
            try
            {
                Buffer.SetByte(DB_ResetServo1Error_Write_Buffer, 0, 1);
                MainForm.Client.WriteArea(S7Consts.S7AreaDB, MainForm.DataBlock, 5236, 1, S7Consts.S7WLBit, DB_ResetServo1Error_Write_Buffer);
                MainForm.log.Info("Global Tracking Page: Reset Servo 1 Error Button Pressed");

                ResetError1Btn.Visible = false;
                MainForm.SendErrorMsgServo1 = true;
            }
            catch
            {
            }
        }

        private void ResetError1Btn_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void ResetError1Btn_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void ResetError2Btn_Click(object sender, EventArgs e)
        {
            try
            {
                Buffer.SetByte(DB_ResetServo2Error_Write_Buffer, 0, 1);
                MainForm.Client.WriteArea(S7Consts.S7AreaDB, MainForm.DataBlock, 5237, 1, S7Consts.S7WLBit, DB_ResetServo2Error_Write_Buffer);
                MainForm.log.Info("Global Tracking Page: Reset Servo 2 Error Button Pressed");

                ResetError2Btn.Visible = false;
                MainForm.SendErrorMsgServo2 = true;
            }
            catch
            {
            }
        }

        private void ResetError2Btn_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void ResetError2Btn_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void ResetError3Btn_Click(object sender, EventArgs e)
        {
            try
            {
                Buffer.SetByte(DB_ResetServo3Error_Write_Buffer, 0, 1);
                MainForm.Client.WriteArea(S7Consts.S7AreaDB, MainForm.DataBlock, 5238, 1, S7Consts.S7WLBit, DB_ResetServo3Error_Write_Buffer);
                MainForm.log.Info("Global Tracking Page: Reset Servo 3 Error Button Pressed");


                ResetError3Btn.Visible = false;
                MainForm.SendErrorMsgServo3 = true;
            }
            catch
            {
            }
        }

        private void ResetError3Btn_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void ResetError3Btn_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void Servo1PositionTextBox_Click(object sender, EventArgs e)
        {
            string original_text = Servo1PositionTextBox.Text;

            popUpNumberPad = new PopUpNumberPad();
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Servo1PositionTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            if (original_text != Servo1PositionTextBox.Text)
            {
                try
                {
                    source.Configs[MainForm.Current_Recipe_Num.ToString()].Set("Servo1Position", Servo1PositionTextBox.Text);
                    source.Save();

                    S7.SetDIntAt(DB_DINT_Write_Buffer, 0, Convert.ToInt32(Servo1PositionTextBox.Text));
                    MainForm.Client.DBWrite(MainForm.DataBlock, 88, 4, DB_DINT_Write_Buffer);

                    MainForm.log.Info("Global Tracking Page: Servo 1 Position Value Changed To" + Servo1PositionTextBox.Text);

                    Buffer.SetByte(DB_ServoPositionChange_Write_Buffer, 0, 1);
                    MainForm.Client.WriteArea(S7Consts.S7AreaDB, MainForm.DataBlock, 5239, 1, S7Consts.S7WLBit, DB_ServoPositionChange_Write_Buffer);

                    OmniMsg message = new OmniMsg("Please Wait...", 0, DB_Servo1Done_Read_Buffer, 988, 6);
                    message.ShowDialog();
                    message.TopMost = true;
                }
                catch
                {
                    MessageBox.Show("Failed To Change The Value");
                }
            }

        }

        private void Servo2PositionTextBox_Click(object sender, EventArgs e)
        {
            string original_text = Servo2PositionTextBox.Text;
            popUpNumberPad = new PopUpNumberPad();
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Servo2PositionTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            if (original_text != Servo2PositionTextBox.Text)
            {
                try
                {
                    source.Configs[MainForm.Current_Recipe_Num.ToString()].Set("Servo2Position", Servo2PositionTextBox.Text);
                    source.Save();

                    S7.SetDIntAt(DB_DINT_Write_Buffer, 0, Convert.ToInt32(Servo2PositionTextBox.Text));
                    MainForm.Client.DBWrite(MainForm.DataBlock, 92, 4, DB_DINT_Write_Buffer);

                    MainForm.log.Info("Global Tracking Page: Servo 2 Position Value Changed To" + Servo2PositionTextBox.Text);

                    Buffer.SetByte(DB_ServoPositionChange_Write_Buffer, 0, 1);
                    MainForm.Client.WriteArea(S7Consts.S7AreaDB, MainForm.DataBlock, 5239, 1, S7Consts.S7WLBit, DB_ServoPositionChange_Write_Buffer);

                    OmniMsg message = new OmniMsg("Please Wait...", 0, DB_Servo2Done_Read_Buffer, 988, 7);
                    message.ShowDialog();
                    message.TopMost = true;
                }
                catch
                {
                    MessageBox.Show("Failed To Change The Value");
                }
            }
        }

        private void Servo3PositionTextBox_Click(object sender, EventArgs e)
        {
            string original_text = Servo3PositionTextBox.Text;

            popUpNumberPad = new PopUpNumberPad();
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Servo3PositionTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            if (original_text != Servo3PositionTextBox.Text)
            {
                try
                {
                    source.Configs[MainForm.Current_Recipe_Num.ToString()].Set("Servo3Position", Servo3PositionTextBox.Text);
                    source.Save();

                    S7.SetDIntAt(DB_DINT_Write_Buffer, 0, Convert.ToInt32(Servo3PositionTextBox.Text));
                    MainForm.Client.DBWrite(MainForm.DataBlock, 96, 4, DB_DINT_Write_Buffer);

                    MainForm.log.Info("Global Tracking Page: Servo 3 Position Value Changed To" + Servo3PositionTextBox.Text);

                    Buffer.SetByte(DB_ServoPositionChange_Write_Buffer, 0, 1);
                    MainForm.Client.WriteArea(S7Consts.S7AreaDB, MainForm.DataBlock, 5239, 1, S7Consts.S7WLBit, DB_ServoPositionChange_Write_Buffer);

                    OmniMsg message = new OmniMsg("Please Wait...", 0, DB_Servo3Done_Read_Buffer, 989, 0);
                    message.ShowDialog();
                    message.TopMost = true;
                }
                catch
                {
                    MessageBox.Show("Failed To Change The Value");
                }
            }
        }

        private void ZeroSystem_Click(object sender, EventArgs e)
        {
            MainForm.Client.WriteArea(S7Consts.S7AreaDB, 4, 1601, 1, S7Consts.S7WLBit, DB_ZeroSystem_Bool);//User Offset 200.1 convert Byte to Bit = 1600.1
            MainForm.log.Info("Global Tracking Page: Zero System Button Pressed" );

        }

        private void ZeroSystem_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void ZeroSystem_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void ReTrigger_Reject_Click(object sender, EventArgs e)
        {
            popUpNumberPad = new PopUpNumberPad();
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    ReTrigger_Reject.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source.Configs[MainForm.Current_Recipe_Num.ToString()].Set("ReTriggerDelay", ReTrigger_Reject.Text);
                source.Save();

                S7.SetUIntAt(DB_ReTriggerDelay_Write_Buffer, 0, (ushort)Convert.ToInt32(ReTrigger_Reject.Text));
                MainForm.Client.DBWrite(4, 130, 2, DB_ReTriggerDelay_Write_Buffer);

                MainForm.log.Info("Global Tracking Page: ReTrigger Value Changed To" + ReTrigger_Reject.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }

        }
        private void Rej_Conv_Manual_Click(object sender, EventArgs e)
        {
            _ = MainForm.Client.DBRead(101, 30, 2, DB_Reject_Conveyor_Read_Manual);
            if (S7.GetBitAt(DB_Reject_Conveyor_Read_Manual, 0, 0) == false)
            {
                Buffer.SetByte(DB_Reject_Conveyor_Write_Manual, 0, 1);
                MainForm.Client.WriteArea(S7Consts.S7AreaDB, 101, 240, 1, S7Consts.S7WLBit, DB_Reject_Conveyor_Write_Manual);//User Offset 14 convert Byte to Bit = 112
                Rej_Conv_Manual.Text = "AUTOMATIC";
                MainForm.log.Info("Global Tracking Page:Reject Conveyor Manual Changed to AUTOMATIC");


            }
            if (S7.GetBitAt(DB_Reject_Conveyor_Read_Manual, 0, 0) == true)
            {
                Buffer.SetByte(DB_Reject_Conveyor_Write_Manual, 0, 0);
                MainForm.Client.WriteArea(S7Consts.S7AreaDB, 101, 240, 1, S7Consts.S7WLBit, DB_Reject_Conveyor_Write_Manual);//User Offset 14 convert Byte to Bit = 112
                Rej_Conv_Manual.Text = "MANUAL";
                MainForm.log.Info("Global Tracking Page:Reject Conveyor Manual Changed to MANUAL");

            }

        }

        private void Rej_Conv_Run_Click(object sender, EventArgs e)
        {
            _ = MainForm.Client.DBRead(101, 30, 2, DB_Reject_Conveyor_Read_Run);
            if (S7.GetBitAt(DB_Reject_Conveyor_Read_Run, 0, 1) == false)
            {
                Buffer.SetByte(DB_Reject_Conveyor_Write_Run, 0, 1);
                MainForm.Client.WriteArea(S7Consts.S7AreaDB, 101, 241, 1, S7Consts.S7WLBit, DB_Reject_Conveyor_Write_Run);//User Offset 14 convert Byte to Bit = 112
                Rej_Conv_Run.Text = "FALSE";
                MainForm.log.Info("Global Tracking Page:Reject Conveyor Run Changed to FALSE");

            }
            if (S7.GetBitAt(DB_Reject_Conveyor_Read_Run, 0, 1) == true)
            {
                Buffer.SetByte(DB_Reject_Conveyor_Write_Run, 0, 0);
                MainForm.Client.WriteArea(S7Consts.S7AreaDB, 101, 241, 1, S7Consts.S7WLBit, DB_Reject_Conveyor_Write_Run);//User Offset 14 convert Byte to Bit = 112
                Rej_Conv_Run.Text = "TRUE";
                MainForm.log.Info("Global Tracking Page:Reject Conveyor Run Changed to TRUE");

            }

        }

        private void Insp_Conv_Manual_Click(object sender, EventArgs e)
        {
            _ = MainForm.Client.DBRead(101, 14, 2, DB_Inspect_Conveyor_Read_Manual);
            if (S7.GetBitAt(DB_Inspect_Conveyor_Read_Manual, 0, 0) == false)
            {
                Buffer.SetByte(DB_Inspect_Conveyor_Write_Manual, 0, 1);
                MainForm.Client.WriteArea(S7Consts.S7AreaDB, 101, 112, 1, S7Consts.S7WLBit, DB_Inspect_Conveyor_Write_Manual);//User Offset 14 convert Byte to Bit = 112
                Insp_Conv_Manual.Text = "AUTOMATIC";
                MainForm.log.Info("Global Tracking Page:Inspection Conveyor Manual Changed to AUTOMATIC");


            }
            if (S7.GetBitAt(DB_Inspect_Conveyor_Read_Manual, 0, 0) == true)
            {
                Buffer.SetByte(DB_Inspect_Conveyor_Write_Manual, 0, 0);
                MainForm.Client.WriteArea(S7Consts.S7AreaDB, 101, 112, 1, S7Consts.S7WLBit, DB_Inspect_Conveyor_Write_Manual);//User Offset 14 convert Byte to Bit = 112
                Insp_Conv_Manual.Text = "MANUAL";
                MainForm.log.Info("Global Tracking Page:Inspection Conveyor Manual Changed to MANUAL");

            }

        }

        private void Insp_Conv_Run_Click(object sender, EventArgs e)
        {
            _ = MainForm.Client.DBRead(101, 14, 2, DB_Inspect_Conveyor_Read_Run);
            if (S7.GetBitAt(DB_Inspect_Conveyor_Read_Run, 0, 1) == false)
            {
                Buffer.SetByte(DB_Inspect_Conveyor_Write_Run, 0, 1);
                MainForm.Client.WriteArea(S7Consts.S7AreaDB, 101, 113, 1, S7Consts.S7WLBit, DB_Inspect_Conveyor_Write_Run);//User Offset 14 convert Byte to Bit = 112
                Insp_Conv_Run.Text = "FALSE";
                MainForm.log.Info("Global Tracking Page:Inspection Conveyor Run Changed to FALSE");

            }
            if (S7.GetBitAt(DB_Inspect_Conveyor_Read_Run, 0, 1) == true)
            {
                Buffer.SetByte(DB_Inspect_Conveyor_Write_Run, 0, 0);
                MainForm.Client.WriteArea(S7Consts.S7AreaDB, 101, 113, 1, S7Consts.S7WLBit, DB_Inspect_Conveyor_Write_Run);//User Offset 14 convert Byte to Bit = 112
                Insp_Conv_Run.Text = "TRUE";
                MainForm.log.Info("Global Tracking Page:Inspection Conveyor Run Changed to TRUE");

            }

        }

        private void CUCD_Text_Box_Click(object sender, EventArgs e)
        {


            OmniMsgOriginal ConfirmationMessage = new OmniMsgOriginal("Changing this requires a PLC Reboot", 1);
            ConfirmationMessage.ShowDialog();
            ConfirmationMessage.TopMost = true;
            try
            {
                do
                {
                    //waiting for answer
                }
                while (MainForm.Received_Answer == false);

                if (MainForm.received_answer_value == 1)
                {
                    MainForm.received_answer_value = -1;
                    MainForm.Received_Answer = false;


                    _ = MainForm.Client.DBRead(4, 200, 2, DB_CUCD_Read_Buffer);
                    if (S7.GetBitAt(DB_CUCD_Read_Buffer, 0, 5) == false)
                    {
                        Buffer.SetByte(DB_CUCD_Read_Buffer, 0, 1);
                        MainForm.Client.WriteArea(S7Consts.S7AreaDB, 4, 1605, 1, S7Consts.S7WLBit, DB_CUCD_Read_Buffer);//User Offset 14 convert Byte to Bit = 112
                        CUCD_Text_Box.Text = "CUCD";
                        Cam1toRejectDelayLabel.Visible = false;
                        TrigToRejDelayTextBox.Visible = false;
                        RejSensortoRejLabel.Visible = true;
                        ReTrigger_Reject.Visible = true;
                        MainForm.log.Info("Global Tracking Page: CUCD Selected");

                    }
                    if (S7.GetBitAt(DB_CUCD_Read_Buffer, 0, 5) == true)
                    {
                        Buffer.SetByte(DB_CUCD_Read_Buffer, 0, 0);
                        MainForm.Client.WriteArea(S7Consts.S7AreaDB, 4, 1605, 1, S7Consts.S7WLBit, DB_CUCD_Read_Buffer);//User Offset 14 convert Byte to Bit = 112
                        CUCD_Text_Box.Text = "ENCODER";
                        Cam1toRejectDelayLabel.Visible = true;
                        TrigToRejDelayTextBox.Visible = true;
                        RejSensortoRejLabel.Visible = false;
                        ReTrigger_Reject.Visible = false;
                        MainForm.log.Info("Global Tracking Page: Encoder Selected");

                    }
                }
                else
                {
                    MainForm.received_answer_value = -1;
                    MainForm.Received_Answer = false;
                }
            }
            catch
            {
                MessageBox.Show("PLC could not change");

            }
        }

        private void Master_PPS_Max_Click(object sender, EventArgs e)
        {
            popUpNumberPad = new PopUpNumberPad();
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Master_PPS_Max.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source.Configs[MainForm.Current_Recipe_Num.ToString()].Set("MasterPPSMax", Master_PPS_Max.Text);
                source.Save();

                S7.SetUIntAt(DB_Master_PPS_Max_Write_Buffer, 0, (ushort)Convert.ToInt32(Master_PPS_Max.Text));
                MainForm.Client.DBWrite(4, 132, 2, DB_Master_PPS_Max_Write_Buffer);

                MainForm.log.Info("Global Tracking Page: Master PPS Max Value Changed To" + Master_PPS_Max.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }

        }

        private void Master_PPS_Min_Click(object sender, EventArgs e)
        {
            popUpNumberPad = new PopUpNumberPad();
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Master_PPS_Min.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source.Configs[MainForm.Current_Recipe_Num.ToString()].Set("MasterPPSMin", Master_PPS_Min.Text);
                source.Save();

                S7.SetUIntAt(DB_Master_PPS_Min_Write_Buffer, 0, (ushort)Convert.ToInt32(Master_PPS_Min.Text));
                MainForm.Client.DBWrite(4, 134, 2, DB_Master_PPS_Min_Write_Buffer);

                MainForm.log.Info("Global Tracking Page: Master PPS Min Value Changed To" + Master_PPS_Min.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }

        }

        private void VFD_Max_Click(object sender, EventArgs e)
        {
            popUpNumberPad = new PopUpNumberPad();
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    VFD_Max.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source.Configs[MainForm.Current_Recipe_Num.ToString()].Set("VFDMax", VFD_Max.Text);
                source.Save();

                S7.SetUIntAt(DB_VFD_Max_Write_Buffer, 0, (ushort)Convert.ToInt32(VFD_Max.Text));
                MainForm.Client.DBWrite(4, 136, 2, DB_VFD_Max_Write_Buffer);

                MainForm.log.Info("Global Tracking Page: VFD Max Value Changed To" + VFD_Max.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }

        }

        private void Insp_Conv_Setpoint_Click(object sender, EventArgs e)
        {
            popUpNumberPad = new PopUpNumberPad();
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Insp_Conv_Setpoint.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source.Configs[MainForm.Current_Recipe_Num.ToString()].Set("InspectionSetPoint", Insp_Conv_Setpoint.Text);
                source.Save();

                S7.SetUIntAt(DB_Inspection_Set_Point_Write_Buffer, 0, (ushort)Convert.ToInt32(Insp_Conv_Setpoint.Text));
                MainForm.Client.DBWrite(101, 12, 2, DB_Inspection_Set_Point_Write_Buffer);

                MainForm.log.Info("Global Tracking Page: InspConvSetpoint Value Changed To" + Insp_Conv_Setpoint.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }


        }

        private void Rej_Conv_Setpoint_Click(object sender, EventArgs e)
        {
            popUpNumberPad = new PopUpNumberPad();
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Rej_Conv_Setpoint.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source.Configs[MainForm.Current_Recipe_Num.ToString()].Set("RejectionSetPoint", Rej_Conv_Setpoint.Text);
                source.Save();

                S7.SetUIntAt(DB_Rejection_Set_Point_Write_Buffer, 0, (ushort)Convert.ToInt32(Rej_Conv_Setpoint.Text));
                MainForm.Client.DBWrite(101, 28, 2, DB_Rejection_Set_Point_Write_Buffer);

                MainForm.log.Info("Global Tracking Page: RejConvSetpoint Value Changed To" + Rej_Conv_Setpoint.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }

        }
        public void HandleTextBoxFocusLoss()
        {
            //this.hiddenButton.Focus(); // Ensure this sets the focus to the hidden button
        }

        private void MainMenuFormBtn_Click(object sender, EventArgs e)
        {
            MainForm.log.Info("Global Tracking Page: Dashboard Button Pressed");
            MainForm.log.Info("*********************************************************************************");
            MainForm.log.Info("Global Tracking Page: Rejection Type Value is ---------------------" + CUCD_Text_Box);
            MainForm.log.Info("Global Tracking Page: Reject Pulsewidth Value is ------------------" + RejPulseWidthTextBox);
            MainForm.log.Info("Global Tracking Page: Cam1 to Reject Value is ---------------------" + TrigToRejDelayTextBox);
            MainForm.log.Info("Global Tracking Page: Reject Sensor to Reject Value is ------------" + ReTrigger_Reject);
            MainForm.log.Info("Global Tracking Page: Master PPS Max Value is ---------------------" + Master_PPS_Max);
            MainForm.log.Info("Global Tracking Page: Master PPS Min Value is ---------------------" + Master_PPS_Min);
            MainForm.log.Info("Global Tracking Page: VFD Max Value is ----------------------------" + VFD_Max);
            MainForm.log.Info("Global Tracking Page: Servo 1 Position Value is -------------------" + Servo1PositionTextBox);
            MainForm.log.Info("Global Tracking Page: Servo 2 Position Value is -------------------" + Servo2PositionTextBox);
            MainForm.log.Info("Global Tracking Page: Servo 3 Position Value is -------------------" + Servo3PositionTextBox);
            MainForm.log.Info("Global Tracking Page: Camera Timeout Value is ---------------------" + CameraTimeoutTextBox);
            MainForm.log.Info("Global Tracking Page: Max Consecutive Value is --------------------" + MaxConsecutiveTextBox);
            MainForm.log.Info("Global Tracking Page: Product Width Value is ----------------------" + ProductWidthTextBox);
            MainForm.log.Info("Global Tracking Page: Product Width Value is ----------------------" + RejectWidthTextBox);
            MainForm.log.Info("*********************************************************************************");
            mainForm.Show();
            mainForm.Focus();
            this.Close();

        }
    }
}
