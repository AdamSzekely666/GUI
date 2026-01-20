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
using System.Linq;
using FZ_Control;
using iText.StyledXmlParser.Exceptions;


namespace Omnicheck360
{
    public partial class Camera1InspectionSetup : BaseForm
    {
        public MainMenu MainMenu;
        public MainForm mainForm;
        public PopUpNumberPad popUpNumberPad;

        private static IniConfigSource source;
        private static IniConfigSource source2;
        private string cPath;
        private string AppFile;
        private string AppFile2;
        private string CameraStepUnitNumber = "";
        private string TrendMonitor1UnitNumber = "";
        private string CameraShutterSpeed1 = "";
        private string CameraGain = "";
        private string tempint = "";
        public static byte[] DB_TrigToCamDelay_Read_Buffer = new byte[2];
        public static byte[] DB_TrigToCamDelay_Write_Buffer = new byte[2];
        public static byte[] DB_CamPulseWidth_Read_Buffer = new byte[4];
        public static byte[] DB_CamPulseWidth_Write_Buffer = new byte[4];
        public static byte[] DB_Cam2PulseWidth_Read_Buffer = new byte[4];
        public static byte[] DB_Cam2PulseWidth_Write_Buffer = new byte[4];
        public static byte[] DB_TrigToCam2Delay_Read_Buffer = new byte[2];
        public static byte[] DB_TrigToCam2Delay_Write_Buffer = new byte[4];
        public static byte[] DB_RejectDelay_Read_Buffer = new byte[2];
        public static byte[] DB_RejectDelay_Write_Buffer = new byte[2];
        public static byte[] DB_RejectPulseWidth_Read_Buffer = new byte[2];
        public static byte[] DB_RejectPulseWidth_Write_Buffer = new byte[2];
        public static byte[] DB_Max_Consecutve_Read_Buffer = new byte[2];
        public static byte[] DB_Max_Consective_Write_Buffer = new byte[2];
        public static byte[] DB_Measured_Width_Read_Buffer = new byte[4];
        public static byte[] DB_Product_Width_Read_Buffer = new byte[2];
        public static byte[] DB_Product_Width_Write_Buffer = new byte[2];
        public static byte[] DB_TrigToAirPurge_Read_Buffer = new byte[2];
        public static byte[] DB_TrigToAirPurge_Write_Buffer = new byte[2];
        public static byte[] DB_AirPurge_PulseWidth_Read_Buffer = new byte[4];
        public static byte[] DB_AirPurge_PulseWidth_Write_Buffer = new byte[4];


        private string data_string = "";
        private StringBuilder data;
        private int maxlength = 10;
        public Camera1InspectionSetup(MainForm _mainForm)
        {
            InitializeComponent();
            ButtonAnimator.InitializeAnimation(MainMenuFormBtn, "blue");
            mainForm = _mainForm; // <--- Add this if missing!

            cPath = Environment.CurrentDirectory;
            AppFile = cPath + "\\VisionControllerData.ini";
            AppFile2 = cPath + "\\RecipeTracking.ini";

            source = new IniConfigSource(AppFile);
            source2 = new IniConfigSource(AppFile2);

            data = new StringBuilder("");

        }
        private void Camera1InspectionSetup_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
            timer1.Start();
            CurrentUserTxt.Text = mainForm.CurrentUserTxt.Text;

            GetInspectionUnitNumbers();

            ConnectOmron();

            SetInitialPLCValues();

            GetVisionData();

            timer2.Start();

            MainForm.log.Info("*********************************************************************************");
            MainForm.log.Info("Finish Camera Inspection Page: Trigger to Camera Delay Value is ---" + TrigToCamDelayTextBox.Text);
            MainForm.log.Info("Finish Camera Inspection Page: Camera Trigger Pulsewidth Value is -" + TrigToCam2DelayTextBox.Text);
            MainForm.log.Info("Finish Camera Inspection Page: Rejector Delay Value is ------------" + RejectDelay.Text);
            MainForm.log.Info("Finish Camera Inspection Page: Rejector Pulsewidth Value is -------" + RejectPulseWidth.Text);
            MainForm.log.Info("Finish Camera Inspection Page: Camera Shutter Speed Value is ------" + CameraShutterSpeedTextBox.Text);
            MainForm.log.Info("Finish Camera Inspection Page: Camera Gain Value is ---------------" + CameraGainTextBox.Text);
            MainForm.log.Info("Finish Camera Inspection Page: Center Position X Value is ---------" + Center_Position_X.Text);
            MainForm.log.Info("Finish Camera Inspection Page: Center Position Y Value is ---------" + Center_Position_Y.Text);
            MainForm.log.Info("Finish Camera Inspection Page: Radius X Value is ------------------" + Radius.Text);
            MainForm.log.Info("Finish Camera Inspection Page: Radius Y Value is ------------------" + Width.Text);
            MainForm.log.Info("Finish Camera Inspection Page: Area Defect Level Value is ---------" + AreaDefectLevel.Text);
            MainForm.log.Info("Finish Camera Inspection Page: Lower Threshold Value is -----------" + AreaJudgeLower.Text);
            MainForm.log.Info("Finish Camera Inspection Page: Upper Threshold Value is -----------" + AreaJudgeUper.Text);
            MainForm.log.Info("*********************************************************************************");

        }
        private void ConnectOmron()
        {
            coreRA1.FzPath = MainForm.FZ_PATH;
            coreRA1.ConnectMode = FZ_Control.ConnectionMode.Remote;
            coreRA1.IpAddress = MainForm.OMRON_IP;
            coreRA1.LineNo = MainForm.Camera1LineNo;
            coreRA1.ConnectStart();

            coreRA2.FzPath = MainForm.FZ_PATH;
            coreRA2.ConnectMode = FZ_Control.ConnectionMode.Remote;
            coreRA2.IpAddress = MainForm.OMRON_IP;
            coreRA2.LineNo = MainForm.Camera2LineNo;
            coreRA2.ConnectStart();


        }
        private void SetInitialPLCValues()
        {
            int DB_Read_Result;

            DB_Read_Result = MainForm.Client.DBRead(4, 100, 2, DB_TrigToCamDelay_Read_Buffer);
            if (DB_Read_Result == 0)
            {
                TrigToCamDelayTextBox.Text = S7.GetUIntAt(DB_TrigToCamDelay_Read_Buffer, 0).ToString();
            }

            DB_Read_Result = MainForm.Client.DBRead(4, 102, 2, DB_TrigToCam2Delay_Read_Buffer);
            if (DB_Read_Result == 0)
            {
                TrigToCam2DelayTextBox.Text = S7.GetUIntAt(DB_TrigToCam2Delay_Read_Buffer, 0).ToString();
            }


            DB_Read_Result = MainForm.Client.DBRead(4, 140, 4, DB_CamPulseWidth_Read_Buffer);
            if (DB_Read_Result == 0)
            {
                Camera1PulseWidthTextBox.Text = S7.GetDIntAt(DB_CamPulseWidth_Read_Buffer, 0).ToString();
            }

            DB_Read_Result = MainForm.Client.DBRead(4, 144, 4, DB_Cam2PulseWidth_Read_Buffer);
            if (DB_Read_Result == 0)
            {
                Camera2PulseWidthTextBox.Text = S7.GetDIntAt(DB_Cam2PulseWidth_Read_Buffer, 0).ToString();
            }

            DB_Read_Result = MainForm.Client.DBRead(4, 110, 2, DB_RejectDelay_Read_Buffer);
            if (DB_Read_Result == 0)
            {
                RejectDelay.Text = S7.GetUIntAt(DB_RejectDelay_Read_Buffer, 0).ToString();
            }

            DB_Read_Result = MainForm.Client.DBRead(4, 112, 2, DB_RejectPulseWidth_Read_Buffer);
            if (DB_Read_Result == 0)
            {
                RejectPulseWidth.Text = S7.GetUIntAt(DB_RejectPulseWidth_Read_Buffer, 0).ToString();
            }

            DB_Read_Result = MainForm.Client.DBRead(4, 116, 2, DB_Max_Consecutve_Read_Buffer);
            if (DB_Read_Result == 0)
            {
                ConsReject.Text = S7.GetUIntAt(DB_Max_Consecutve_Read_Buffer, 0).ToString();
            }

            DB_Read_Result = MainForm.Client.DBRead(4, 114, 2, DB_Product_Width_Read_Buffer);
            if (DB_Read_Result == 0)
            {
                ProductWidth.Text = S7.GetUIntAt(DB_Product_Width_Read_Buffer, 0).ToString();
            }
            DB_Read_Result = MainForm.Client.DBRead(4, 56, 4, DB_Measured_Width_Read_Buffer);
            if (DB_Read_Result == 0)
            {
                MeasuredWidth.Text = S7.GetUDIntAt(DB_Measured_Width_Read_Buffer, 0).ToString();
            }
            DB_Read_Result = MainForm.Client.DBRead(4, 104, 2, DB_TrigToAirPurge_Read_Buffer);
            if (DB_Read_Result == 0)
            {
                AirPurgeDistance.Text = S7.GetUIntAt(DB_TrigToAirPurge_Read_Buffer, 0).ToString();
            }
            DB_Read_Result = MainForm.Client.DBRead(4, 148, 4, DB_AirPurge_PulseWidth_Read_Buffer);
            if (DB_Read_Result == 0)
            {
                AirPurgePulse.Text = S7.GetDIntAt(DB_AirPurge_PulseWidth_Read_Buffer, 0).ToString();
            }

        }
        private void GetInspectionUnitNumbers()
        {
            CameraStepUnitNumber = source.Configs["Camera1"].Get("CameraUnit");
            CameraShutterSpeed1 = source.Configs["Camera1"].Get("CameraShutterSpeed");
            CameraGain = source.Configs["Camera1"].Get("CameraGain");
            TrendMonitor1UnitNumber = source.Configs["Camera1"].Get("TrendMonitor1UnitNumber");

        }
        private void GetVisionData()
        {
            if (coreRA1.IsConnected)
            {
                string temp = "";
                StringBuilder sb = new StringBuilder();
                coreRA1.Macro_DirectExecute("MeasureStop");

                data_string = "GetUnitData " + CameraStepUnitNumber + "," + "\"" + CameraShutterSpeed1 + "\"" + ",dataValue$";
                coreRA1.Macro_DirectExecute(data_string);
                coreRA1.Macro_GetVariable("dataValue$", data, maxlength);
                temp = data.ToString();
                CameraShutterSpeedTextBox.Text = Convert.ToDecimal(temp).ToString("N0");

                data_string = "GetUnitData " + CameraStepUnitNumber + "," + "\"" + CameraGain + "\"" + ",dataValue$";
                coreRA1.Macro_DirectExecute(data_string);
                coreRA1.Macro_GetVariable("dataValue$", data, maxlength);
                temp = data.ToString();
                CameraGainTextBox.Text = Convert.ToDecimal(temp).ToString("N0");

                coreRA1.Macro_DirectExecute("DIM FIG&(10)");
                coreRA1.Macro_DirectExecute("GETUNITFIGURE 8,0 FIG&()");

                coreRA1.Macro_DirectExecute("tmp$ = STR$(FIG&(2))");
                coreRA1.Macro_GetVariable("tmp$", sb, 10);
                Center_Position_X.Text = sb.ToString();

                coreRA1.Macro_DirectExecute("tmp$ = STR$(FIG&(3))");
                coreRA1.Macro_GetVariable("tmp$", sb, 10);
                Center_Position_Y.Text = sb.ToString();

                coreRA1.Macro_DirectExecute("tmp$ = STR$(FIG&(4))");
                coreRA1.Macro_GetVariable("tmp$", sb, 10);
                Radius.Text = sb.ToString();

                coreRA1.Macro_DirectExecute("tmp$ = STR$(FIG&(5))");
                coreRA1.Macro_GetVariable("tmp$", sb, 10);
                Width.Text = sb.ToString();

                //data_string = "GetUnitData 8," + "\"colorWound\"" + ",dataValue$";
                //coreRA1.Macro_DirectExecute(data_string);
                //coreRA1.Macro_GetVariable("dataValue$", data, maxlength);
                //tempint = data.ToString();

                coreRA1.Macro_DirectExecute("MeasureStart");

            }
            if (coreRA2.IsConnected)
            {
                string temp = "";
                StringBuilder sb = new StringBuilder();
                coreRA2.Macro_DirectExecute("MeasureStop");

                data_string = "GetUnitData " + CameraStepUnitNumber + "," + "\"" + CameraShutterSpeed1 + "\"" + ",dataValue$";
                coreRA2.Macro_DirectExecute(data_string);
                coreRA2.Macro_GetVariable("dataValue$", data, maxlength);
                temp = data.ToString();
                Camera2ShutterSpeedTextBox.Text = Convert.ToDecimal(temp).ToString("N0");

                data_string = "GetUnitData " + CameraStepUnitNumber + "," + "\"" + CameraGain + "\"" + ",dataValue$";
                coreRA2.Macro_DirectExecute(data_string);
                coreRA2.Macro_GetVariable("dataValue$", data, maxlength);
                temp = data.ToString();
                Camera2GainTextBox.Text = Convert.ToDecimal(temp).ToString("N0");

                coreRA2.Macro_DirectExecute("DIM FIG&(10)");
                coreRA2.Macro_DirectExecute("GETUNITFIGURE 2,0 FIG&()");

                coreRA2.Macro_DirectExecute("tmp$ = STR$(FIG&(2))");
                coreRA2.Macro_GetVariable("tmp$", sb, 10);
                Center2_Position_X.Text = sb.ToString();

                coreRA2.Macro_DirectExecute("tmp$ = STR$(FIG&(3))");
                coreRA2.Macro_GetVariable("tmp$", sb, 10);
                Center2_Position_Y.Text = sb.ToString();

                coreRA2.Macro_DirectExecute("tmp$ = STR$(FIG&(4))");
                coreRA2.Macro_GetVariable("tmp$", sb, 10);
                Radius2.Text = sb.ToString();

                coreRA2.Macro_DirectExecute("tmp$ = STR$(FIG&(5))");
                coreRA2.Macro_GetVariable("tmp$", sb, 10);
                Width2.Text = sb.ToString();

                //data_string = "GetUnitData 8," + "\"colorWound\"" + ",dataValue$";
                //coreRA2.Macro_DirectExecute(data_string);
                //coreRA2.Macro_GetVariable("dataValue$", data, maxlength);
                //tempint = data.ToString();

                coreRA2.Macro_DirectExecute("MeasureStart");

            }

            else
            {
                MessageBox.Show("Vision Controller Not Connected");
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {

            //data_string = "GetUnitData 6," + "\"area\"" + ",dataValue$";
            //coreRA1.Macro_DirectExecute(data_string);
            //coreRA1.Macro_GetVariable("dataValue$", data, maxlength);
            //tempint = data.ToString();
            //InspectionValue.Text = Convert.ToDecimal(tempint).ToString("N0");
        }
        private void TrigToCamDelayTextBox_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    TrigToCamDelayTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source2.Configs[MainForm.Current_Recipe_Num.ToString()].Set("Camera1TriggerDelay", TrigToCamDelayTextBox.Text);
                source2.Save();

                S7.SetUIntAt(DB_TrigToCamDelay_Write_Buffer, 0, (ushort)Convert.ToInt32(TrigToCamDelayTextBox.Text));
                MainForm.Client.DBWrite(4, 100, 2, DB_TrigToCamDelay_Write_Buffer);

                MainForm.log.Info("Camera 2 Setup Page: Trigger to Camera 1 Delay Value Changed to" + TrigToCamDelayTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }
        }
        private void TrigToCam2DelayTextBox_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    TrigToCam2DelayTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source2.Configs[MainForm.Current_Recipe_Num.ToString()].Set("Camera1TriggerDelay", TrigToCam2DelayTextBox.Text);
                source2.Save();

                S7.SetUIntAt(DB_TrigToCam2Delay_Write_Buffer, 0, (ushort)Convert.ToInt32(TrigToCam2DelayTextBox.Text));
                MainForm.Client.DBWrite(4, 102, 2, DB_TrigToCam2Delay_Write_Buffer);

                MainForm.log.Info("Camera 2 Setup Page: Trigger to Camera 2 Delay Value Changed to" + TrigToCam2DelayTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }
        }
        private void CameraPulseWidthTextBox_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Camera1PulseWidthTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source2.Configs[MainForm.Current_Recipe_Num.ToString()].Set("Camera1TriggerPulseWidth", Camera1PulseWidthTextBox.Text);
                source2.Save();

                S7.SetDIntAt(DB_CamPulseWidth_Write_Buffer, 0, Convert.ToInt32(Camera1PulseWidthTextBox.Text));
                MainForm.Client.DBWrite(4, 140, 4, DB_CamPulseWidth_Write_Buffer);

                MainForm.log.Info("Camera 2 Setup Page: Camera 2 Pulse Width Value Changed to" + Camera1PulseWidthTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }
        }
        private void Camera2PulseWidthTextBox_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Camera2PulseWidthTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source2.Configs[MainForm.Current_Recipe_Num.ToString()].Set("Camera1TriggerPulseWidth", Camera2PulseWidthTextBox.Text);
                source2.Save();

                S7.SetDIntAt(DB_Cam2PulseWidth_Write_Buffer, 0, Convert.ToInt32(Camera2PulseWidthTextBox.Text));
                MainForm.Client.DBWrite(4, 144, 4, DB_Cam2PulseWidth_Write_Buffer);

                MainForm.log.Info("Camera 2 Setup Page: Camera 2 Pulse Width Value Changed to" + Camera2PulseWidthTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }
        }
        private void RejectDelay_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    RejectDelay.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source2.Configs[MainForm.Current_Recipe_Num.ToString()].Set("Camera1TriggerPulseWidth", RejectDelay.Text);
                source2.Save();

                S7.SetIntAt(DB_RejectDelay_Write_Buffer, 0, Convert.ToInt16(RejectDelay.Text));
                MainForm.Client.DBWrite(4, 110, 2, DB_RejectDelay_Write_Buffer);

                MainForm.log.Info("Camera 2 Setup Page: Camera 2 Pulse Width Value Changed to" + RejectDelay.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }
        }
        private void RejectPulse_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    RejectPulseWidth.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source2.Configs[MainForm.Current_Recipe_Num.ToString()].Set("Camera1TriggerPulseWidth", RejectPulseWidth.Text);
                source2.Save();

                S7.SetIntAt(DB_RejectPulseWidth_Write_Buffer, 0, Convert.ToInt16(RejectPulseWidth.Text));
                MainForm.Client.DBWrite(4, 112, 2, DB_RejectPulseWidth_Write_Buffer);

                MainForm.log.Info("Camera 2 Setup Page: Camera 2 Pulse Width Value Changed to" + RejectPulseWidth.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }
        }

        private void ConsecutiveReject_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    ConsReject.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source2.Configs[MainForm.Current_Recipe_Num.ToString()].Set("Camera1TriggerPulseWidth", ConsReject.Text);
                source2.Save();

                S7.SetIntAt(DB_Max_Consective_Write_Buffer, 0, Convert.ToInt16(ConsReject.Text));
                MainForm.Client.DBWrite(4, 116, 2, DB_Max_Consective_Write_Buffer);

                MainForm.log.Info("Camera 2 Setup Page: Camera 2 Pulse Width Value Changed to" + ConsReject.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }
        }
        private void AirPurgeDistance_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    AirPurgeDistance.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source2.Configs[MainForm.Current_Recipe_Num.ToString()].Set("Camera1TriggerPulseWidth", AirPurgeDistance.Text);
                source2.Save();

                S7.SetIntAt(DB_TrigToAirPurge_Write_Buffer, 0, Convert.ToInt16(AirPurgeDistance.Text));
                MainForm.Client.DBWrite(4, 104, 2, DB_TrigToAirPurge_Write_Buffer);

                MainForm.log.Info("Camera 2 Setup Page: Camera 2 Pulse Width Value Changed to" + AirPurgeDistance.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }
        }
        private void AirPurgePulse_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    AirPurgePulse.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source2.Configs[MainForm.Current_Recipe_Num.ToString()].Set("Camera1TriggerPulseWidth", AirPurgePulse.Text);
                source2.Save();

                S7.SetDIntAt(DB_AirPurge_PulseWidth_Write_Buffer, 0, Convert.ToInt32(AirPurgePulse.Text));
                MainForm.Client.DBWrite(4, 148, 4, DB_AirPurge_PulseWidth_Write_Buffer);

                MainForm.log.Info("Camera 2 Setup Page: Camera 2 Pulse Width Value Changed to" + AirPurgePulse.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }
        }

        private void ProductWidth_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    ProductWidth.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            try
            {
                source2.Configs[MainForm.Current_Recipe_Num.ToString()].Set("Camera1TriggerPulseWidth", ProductWidth.Text);
                source2.Save();

                S7.SetIntAt(DB_Product_Width_Write_Buffer, 0, Convert.ToInt16(ProductWidth.Text));
                MainForm.Client.DBWrite(4, 114, 2, DB_Product_Width_Write_Buffer);

                MainForm.log.Info("Camera 2 Setup Page: Camera 2 Pulse Width Value Changed to" + ProductWidth.Text);
            }
            catch
            {
                MessageBox.Show("Failed To Change The Value");
            }
        }

        private void CameraShutterSpeedTextBox_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    CameraShutterSpeedTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();


            data_string = "SetUnitData " + CameraStepUnitNumber.ToString() + "," + "\"" + CameraShutterSpeed1 + "\"" + "," + CameraShutterSpeedTextBox.Text.ToString();
            coreRA1.Macro_DirectExecute(data_string);
            coreRA1.Macro_DirectExecute("SaveData");
        }
        private void CameraGainTextBox_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    CameraGainTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            data_string = "SetUnitData " + CameraStepUnitNumber.ToString() + "," + "\"" + CameraGain + "\"" + "," + CameraGainTextBox.Text.ToString();
            coreRA1.Macro_DirectExecute(data_string);
            coreRA1.Macro_DirectExecute("SaveData");
        }
        private void Center_Position_X_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Center_Position_X.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            //This is to write data
            coreRA1.Macro_DirectExecute("MeasureStop");  //you need to put the FH into setup mode before you can change figures etc.
            coreRA1.Macro_DirectExecute("DIM FIG&(10)"); //allocate memory in the FH to hold the figure data structure.
            coreRA1.Macro_DirectExecute("GETUNITFIGURE 5,0 FIG&()"); //Grab a snapshot of the current figure into the FIG variable.  This will be easier than creating the whole structure from scratch
            coreRA1.Macro_DirectExecute(String.Format("FIG&(2) = {0}", Center_Position_X.Text));//set variable
            coreRA1.Macro_DirectExecute("SETUNITFIGURE 5,0, FIG&()"); //write the modified figure back to the processing unit
            coreRA1.Macro_DirectExecute("MeasureStart"); //put the FH back into run mode
            coreRA1.Macro_DirectExecute("SaveData");

        }
        private void Center_Position_Y_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Center_Position_Y.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            //This is to write data
            coreRA1.Macro_DirectExecute("MeasureStop");  //you need to put the FH into setup mode before you can change figures etc.
            coreRA1.Macro_DirectExecute("DIM FIG&(10)"); //allocate memory in the FH to hold the figure data structure.
            coreRA1.Macro_DirectExecute("GETUNITFIGURE 5,0 FIG&()"); //Grab a snapshot of the current figure into the FIG variable.  This will be easier than creating the whole structure from scratch
            coreRA1.Macro_DirectExecute(String.Format("FIG&(3) = {0}", Center_Position_Y.Text));//set variable
            coreRA1.Macro_DirectExecute("SETUNITFIGURE 5,0, FIG&()"); //write the modified figure back to the processing unit
            coreRA1.Macro_DirectExecute("MeasureStart"); //put the FH back into run mode
            coreRA1.Macro_DirectExecute("SaveData");

        }
        private void Radius_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Radius.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            //This is to write data
            coreRA1.Macro_DirectExecute("MeasureStop");  //you need to put the FH into setup mode before you can change figures etc.
            coreRA1.Macro_DirectExecute("DIM FIG&(10)"); //allocate memory in the FH to hold the figure data structure.
            coreRA1.Macro_DirectExecute("GETUNITFIGURE 5,0 FIG&()"); //Grab a snapshot of the current figure into the FIG variable.  This will be easier than creating the whole structure from scratch
            coreRA1.Macro_DirectExecute(String.Format("FIG&(4) = {0}", Radius.Text));//set variable
            coreRA1.Macro_DirectExecute("SETUNITFIGURE 5,0, FIG&()"); //write the modified figure back to the processing unit
            coreRA1.Macro_DirectExecute("MeasureStart"); //put the FH back into run mode
            coreRA1.Macro_DirectExecute("SaveData");

        }
        private void Width_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Width.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            //This is to write data
            coreRA1.Macro_DirectExecute("MeasureStop");  //you need to put the FH into setup mode before you can change figures etc.
            coreRA1.Macro_DirectExecute("DIM FIG&(10)"); //allocate memory in the FH to hold the figure data structure.
            coreRA1.Macro_DirectExecute("GETUNITFIGURE 5,0 FIG&()"); //Grab a snapshot of the current figure into the FIG variable.  This will be easier than creating the whole structure from scratch
            coreRA1.Macro_DirectExecute(String.Format("FIG&(5) = {0}", Width.Text));//set variable
            coreRA1.Macro_DirectExecute("SETUNITFIGURE 5,0, FIG&()"); //write the modified figure back to the processing unit
            coreRA1.Macro_DirectExecute("MeasureStart"); //put the FH back into run mode
            coreRA1.Macro_DirectExecute("SaveData");

        }
        private void MainMenuFormBtn_Click(object sender, EventArgs e)
        {
            try
            {
                MainForm.log.Info("*********************************************************************************");
                MainForm.log.Info("Finish Camera Inspection Page: Trigger to Camera Delay Value is ---" + TrigToCamDelayTextBox.Text);
                MainForm.log.Info("Finish Camera Inspection Page: Camera Trigger Pulsewidth Value is -" + TrigToCam2DelayTextBox.Text);
                MainForm.log.Info("Finish Camera Inspection Page: Rejector Delay Value is ------------" + RejectDelay.Text);
                MainForm.log.Info("Finish Camera Inspection Page: Rejector Pulsewidth Value is -------" + RejectPulseWidth.Text);
                MainForm.log.Info("Finish Camera Inspection Page: Camera Shutter Speed Value is ------" + CameraShutterSpeedTextBox.Text);
                MainForm.log.Info("Finish Camera Inspection Page: Camera Gain Value is ---------------" + CameraGainTextBox.Text);
                MainForm.log.Info("Finish Camera Inspection Page: Center Position X Value is ---------" + Center_Position_X.Text);
                MainForm.log.Info("Finish Camera Inspection Page: Center Position Y Value is ---------" + Center_Position_Y.Text);
                MainForm.log.Info("Finish Camera Inspection Page: Radius X Value is ------------------" + Radius.Text);
                MainForm.log.Info("Finish Camera Inspection Page: Radius Y Value is ------------------" + Width.Text);
                MainForm.log.Info("Finish Camera Inspection Page: Area Defect Level Value is ---------" + AreaDefectLevel.Text);
                MainForm.log.Info("Finish Camera Inspection Page: Lower Threshold Value is -----------" + AreaJudgeLower.Text);
                MainForm.log.Info("Finish Camera Inspection Page: Upper Threshold Value is -----------" + AreaJudgeUper.Text);
                MainForm.log.Info("*********************************************************************************");

                timer2.Stop();
                coreRA1.Dispose();
                this.Close();
            }
            catch
            { }

        }
        private void AreaDefectLevel_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    AreaDefectLevel.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            data_string = "SetUnitData 6," +  "\"areaJudge\"" + "," + AreaDefectLevel.Text.ToString();
            coreRA1.Macro_DirectExecute(data_string);
            coreRA1.Macro_DirectExecute("SaveData");

        }
        private void AreaJudgeUper_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    AreaJudgeUper.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            data_string = "SetUnitData 6," + "\"upperArea\"" + "," + AreaJudgeUper.Text.ToString();
            coreRA1.Macro_DirectExecute(data_string);
            coreRA1.Macro_DirectExecute("SaveData");

        }
        private void AreaJudgeLower_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    AreaJudgeLower.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            data_string = "SetUnitData 6," + "\"lowerArea\"" + "," + AreaJudgeLower.Text.ToString();
            coreRA1.Macro_DirectExecute(data_string);
            coreRA1.Macro_DirectExecute("SaveData");


        }
        private void Camera2ShutterSpeedTextBox_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Camera2ShutterSpeedTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();


            data_string = "SetUnitData " + CameraStepUnitNumber.ToString() + "," + "\"" + CameraShutterSpeed1 + "\"" + "," + Camera2ShutterSpeedTextBox.Text.ToString();
            coreRA2.Macro_DirectExecute(data_string);
            coreRA2.Macro_DirectExecute("SaveData");
        }
        private void Camera2GainTextBox_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Camera2GainTextBox.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            data_string = "SetUnitData " + CameraStepUnitNumber.ToString() + "," + "\"" + CameraGain + "\"" + "," + Camera2GainTextBox.Text.ToString();
            coreRA2.Macro_DirectExecute(data_string);
            coreRA2.Macro_DirectExecute("SaveData");
        }
        private void Center2_Position_X_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Center2_Position_X.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            //This is to write data
            coreRA2.Macro_DirectExecute("MeasureStop");  //you need to put the FH into setup mode before you can change figures etc.
            coreRA2.Macro_DirectExecute("DIM FIG&(10)"); //allocate memory in the FH to hold the figure data structure.
            coreRA2.Macro_DirectExecute("GETUNITFIGURE 2,0 FIG&()"); //Grab a snapshot of the current figure into the FIG variable.  This will be easier than creating the whole structure from scratch
            coreRA2.Macro_DirectExecute(String.Format("FIG&(2) = {0}", Center2_Position_X.Text));//set variable
            coreRA2.Macro_DirectExecute("SETUNITFIGURE 2,0, FIG&()"); //write the modified figure back to the processing unit
            coreRA2.Macro_DirectExecute("MeasureStart"); //put the FH back into run mode
            coreRA2.Macro_DirectExecute("SaveData");

        }
        private void Center2_Position_Y_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Center2_Position_Y.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            //This is to write data
            coreRA2.Macro_DirectExecute("MeasureStop");  //you need to put the FH into setup mode before you can change figures etc.
            coreRA2.Macro_DirectExecute("DIM FIG&(10)"); //allocate memory in the FH to hold the figure data structure.
            coreRA2.Macro_DirectExecute("GETUNITFIGURE 2,0 FIG&()"); //Grab a snapshot of the current figure into the FIG variable.  This will be easier than creating the whole structure from scratch
            coreRA2.Macro_DirectExecute(String.Format("FIG&(3) = {0}", Center2_Position_Y.Text));//set variable
            coreRA2.Macro_DirectExecute("SETUNITFIGURE 2,0, FIG&()"); //write the modified figure back to the processing unit
            coreRA2.Macro_DirectExecute("MeasureStart"); //put the FH back into run mode
            coreRA2.Macro_DirectExecute("SaveData");

        }
        private void Radius2_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Radius2.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            //This is to write data
            coreRA2.Macro_DirectExecute("MeasureStop");  //you need to put the FH into setup mode before you can change figures etc.
            coreRA2.Macro_DirectExecute("DIM FIG&(10)"); //allocate memory in the FH to hold the figure data structure.
            coreRA2.Macro_DirectExecute("GETUNITFIGURE 2,0 FIG&()"); //Grab a snapshot of the current figure into the FIG variable.  This will be easier than creating the whole structure from scratch
            coreRA2.Macro_DirectExecute(String.Format("FIG&(4) = {0}", Radius2.Text));//set variable
            coreRA2.Macro_DirectExecute("SETUNITFIGURE 2,0, FIG&()"); //write the modified figure back to the processing unit
            coreRA2.Macro_DirectExecute("MeasureStart"); //put the FH back into run mode
            coreRA2.Macro_DirectExecute("SaveData");

        }
        private void Width2_Click(object sender, EventArgs e)
        {
            using (PopUpNumberPad numberPadForm = new PopUpNumberPad())
            {
                if (numberPadForm.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine("PopUpNumberPad dialog shown");
                    Width2.Text = numberPadForm.Number; // Set the text box value
                    //Console.WriteLine($"Number selected: {numberPadForm.Number}");
                }
            }
            HandleTextBoxFocusLoss();

            //This is to write data
            coreRA2.Macro_DirectExecute("MeasureStop");  //you need to put the FH into setup mode before you can change figures etc.
            coreRA2.Macro_DirectExecute("DIM FIG&(10)"); //allocate memory in the FH to hold the figure data structure.
            coreRA2.Macro_DirectExecute("GETUNITFIGURE 2,0 FIG&()"); //Grab a snapshot of the current figure into the FIG variable.  This will be easier than creating the whole structure from scratch
            coreRA2.Macro_DirectExecute(String.Format("FIG&(5) = {0}", Width2.Text));//set variable
            coreRA2.Macro_DirectExecute("SETUNITFIGURE 2,0, FIG&()"); //write the modified figure back to the processing unit
            coreRA2.Macro_DirectExecute("MeasureStart"); //put the FH back into run mode
            coreRA2.Macro_DirectExecute("SaveData");

        }

        public void HandleTextBoxFocusLoss()
        {
            this.hiddenButton.Focus(); // Ensure this sets the focus to the hidden button
        }

    }
}
