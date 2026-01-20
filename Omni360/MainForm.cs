using FZ_Control;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Nini.Config;
using Omnicheck360;
using Omnicheck360.Properties;
using Sharp7;
using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Timer = System.Windows.Forms.Timer;

namespace Omnicheck360
{

    public partial class MainForm : BaseForm
    {
        private MainMenu MainMenuForm;
        private AdminScreen adminscreen;
        private RecipeChange rc;
        private Camera1InspectionSetup Camera1InspectionSetup;
        private static IniConfigSource source;
        private static IniConfigSource InterfaceSource;
        private static StringBuilder mystring;
        public static MainForm Instance = null;
        public static string MainCurrentUserTxt = "Operate Mode";
        public static int nUserAccess = 0;
        private static string PLC_IP = "0.0.0.0";
        private static int Rack = 0;
        private static int Slot = 0;
        public static int DataBlock = 0;
        public static bool SendErrorMsgServo1 = true;
        public static bool SendErrorMsgServo2 = true;
        public static bool SendErrorMsgServo3 = true;
        public static string OMRON_IP = "0.0.0.0";
        public static string FZ_PATH = "C:\\OMRON\\FZ_FH_FJ_Simulator\\651";
        public static int Camera1LineNo = 0;
        public static int Camera2LineNo = 0;
        public static int Camera3LineNo = 0;
        public static int Camera4LineNo = 0;
        public static int Servo1Enable = 0;
        public static int Servo2Enable = 0;
        public static int Servo3Enable = 0;
        private ISeries valueSeries;
        private ISeries upperThresholdSeries;
        private ISeries lowerThresholdSeries;
        public static bool reconnectOmron = false;
        public static S7Client Client;
        private int Starting_Recipe_Num = 0;
        public static int Current_Recipe_Num = 0;
        //write buffers
        public static byte[] DB_HeartBeat_Write_Buffer = new byte[1];
        public static byte[] DB_CounterReset_Write_Buffer = new byte[1];
        public static byte[] DB_DINT_Write_Buffer = new byte[100];
        public static byte[] DB_UINT_Write_Buffer = new byte[40];
        public static byte[] DB_Motor_Write_Buffer = new byte[30];
        public static byte[] DB_LightPulseWidth_Write_Buffer = new byte[32];
        public static byte[] DB_CameraTriggerPulseWidth_Write_Buffer = new byte[32];
        public static byte[] DB_RejectPulseWidth_Write_Buffer = new byte[4];
        public static byte[] DB_Timeout_Write_Buffer = new byte[4];
        public static byte[] DB_CameraEnable_Write_Buffer = new byte[1];
        public static byte[] DB_LightEnable_Write_Buffer = new byte[1];
        public static byte[] DB_ServoMove_Write_Buffer = new byte[1];
        public static byte[] DB_ResetMaxConsecutiveReject_Write_Buffer = new byte[1];
        public static byte[] DB_NextPageNew_Write_Buffer = new byte[1];
        public static byte[] DB_Variables_CounterReset_Write_Buffer = new byte[1];
        public static byte[] DB_ZeroSystem_Write_Buffer = new byte[1];
        public static byte[] DB_Jam_Write_Buffer = new byte[2];
        public static byte[] DB_Total_Inspection_Disable_Write_Buffer = new byte[1];
        public static byte[] DB_Cam1_Inspection_Disable_Write_Buffer = new byte[1];
        public static byte[] DB_Cam2_Inspection_Disable_Write_Buffer = new byte[1];
        public static byte[] DB_Cam3_Inspection_Disable_Write_Buffer = new byte[1];
        public static byte[] DB_Cam4_Inspection_Disable_Write_Buffer = new byte[1];
        public static byte[] DB_Cam5_Inspection_Disable_Write_Buffer = new byte[1];
        public static byte[] DB_Resync_Write_Buffer = new byte[1];
        //read buffers
        public byte[] DB_PLCtoGUI_Read_Buffer = new byte[112];
        public byte[] DB_PLCtoGUIBool_Read_Buffer = new byte[1];
        public static byte[] DB_ServoDone_Read_Buffer = new byte[2];
        public static byte[] DB_ServoError_Read_Buffer = new byte[1];
        public static byte[] DB_MaxConsecutive_Read_Buffer = new byte[2];
        public static byte[] DB_UintRead_Result = new byte[10];
        public static byte[] StackIn = new byte[1];
        public static byte[] StackOut = new byte[1];
        public static byte[] Total_Mod = new byte[1];
        public byte[] DB_Variables_Read_Buffer = new byte[100];
        public byte[] DB_BPM_Read_Buffer = new byte[2];
        public static byte[] DB_ESTOP_Pressed_Read_Buffer = new byte[8];
        public static byte[] DB_Servo1Done_Read_Buffer = new byte[1];
        public static byte[] DB_Servo2Done_Read_Buffer = new byte[1];
        public static byte[] DB_Servo3Done_Read_Buffer = new byte[1];
        public static byte[] DB_Jam_Read_Buffer = new byte[8];
        public static byte[] DB_Total_Inspection_Disable_Read_Buffer = new byte[8];
        public static byte[] DB_Cam1_Inspection_Disable_Read_Buffer = new byte[8];
        public static byte[] DB_Cam2_Inspection_Disable_Read_Buffer = new byte[8];
        public static byte[] DB_Cam3_Inspection_Disable_Read_Buffer = new byte[8];
        public static byte[] DB_Cam4_Inspection_Disable_Read_Buffer = new byte[8];
        public static byte[] DB_Cam5_Inspection_Disable_Read_Buffer = new byte[8];
        public static byte[] DB_RejectorDelay_Read_Buffer = new byte[1];
        public static byte[] DB_RejectPulseWidth_Read_Buffer = new byte[2];
        //Recipe Values
        public static int Camera1TriggerDelay = 0;
        public static int Camera2TriggerDelay = 0;
        public static int Camera3TriggerDelay = 0;
        public static int Camera4TriggerDelay = 0;
        public static int Camera5TriggerDelay = 0;
        public static int Camera1TriggerPulseWidth = 0;
        public static int Camera2TriggerPulseWidth = 0;
        public static int Camera3TriggerPulseWidth = 0;
        public static int Camera4TriggerPulseWidth = 0;
        public static int Camera5TriggerPulseWidth = 0;
        public static int RejectorDelay = 0;
        public static int ReTriggerDelay = 0;
        public static int RejectorPulseWidth = 0;
        public static int MaxConsecutiveRejects = 0;
        public static int ProductWidth = 0;
        public static int RejectWidth = 0;
        public static int MasterPPSMax = 0;
        public static int MasterPPSMin = 0;
        public static int VFDMax = 0;
        public static int Inspect_Setpoint = 0;
        public static int Reject_Setpoint = 0;
        public static int InspectionTimeout = 0;
        public static int Servo1Position = 0;
        public static int Servo2Position = 0;
        public static int Servo3Position = 0;
        public static bool Received_Answer = false;
        public StringBuilder data;
        public int maxlength = 10;
        public string tempint = "";
        public PLCData plcData;
        public static int received_answer_value = -1;
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Timer x = new Timer();
        private string data_string = "";
        private List<BarcodeItem> _barcodeItems;
        private int prevThroughputCam1 = 0, prevRejectCam1 = 0;
        private int prevThroughputCam2 = 0, prevRejectCam2 = 0;
        private string currentSize = "";
        private string currentFlavour = "";
        // Camera configuration arrays
        private int[] cam1UnitNumbers = { 3, 5, 6, 8, 3, 5, 6, 8 };
        private int[] cam2UnitNumbers = { 3, 5, 6, 7, 8, 10, 3, 5, 6, 7, 8, 10 };

        // Configure which units show LAST_NG (red) vs FREEZE (blue)
        // Index 0 = UnitNo 1, Index 1 = UnitNo 2, etc.
        // Use FZ_Control.UPDATE_IMAGE enum instead of strings
        private FZ_Control.UPDATE_IMAGE[] cam1UpdateModes = {
        FZ_Control.UPDATE_IMAGE.FREEZE,
        FZ_Control.UPDATE_IMAGE.FREEZE,
        FZ_Control.UPDATE_IMAGE.FREEZE,
        FZ_Control.UPDATE_IMAGE.FREEZE,
        FZ_Control.UPDATE_IMAGE.NG_IMAGE,
        FZ_Control.UPDATE_IMAGE.NG_IMAGE,
        FZ_Control.UPDATE_IMAGE.NG_IMAGE,
        FZ_Control.UPDATE_IMAGE.NG_IMAGE

       // FZ_Control.UPDATE_IMAGE.NG_IMAGE
    };

        private FZ_Control.UPDATE_IMAGE[] cam2UpdateModes = {
        FZ_Control.UPDATE_IMAGE.FREEZE,
        FZ_Control.UPDATE_IMAGE.FREEZE,
        FZ_Control.UPDATE_IMAGE.FREEZE,
        FZ_Control.UPDATE_IMAGE.FREEZE,
        FZ_Control.UPDATE_IMAGE.FREEZE,
        FZ_Control.UPDATE_IMAGE.FREEZE,
        FZ_Control.UPDATE_IMAGE.NG_IMAGE,
        FZ_Control.UPDATE_IMAGE.NG_IMAGE,
        FZ_Control.UPDATE_IMAGE.NG_IMAGE,
        FZ_Control.UPDATE_IMAGE.NG_IMAGE,
        FZ_Control.UPDATE_IMAGE.NG_IMAGE,
        FZ_Control.UPDATE_IMAGE.NG_IMAGE
    };

        private int cam1CurrentIndex = 0; // Start at UnitNo 2 (index 1)
        private int cam2CurrentIndex = 0;

        private bool cam2Visible = false;

        // Dot indicators
        private List<Panel> cam1Dots = new List<Panel>();
        private List<Panel> cam2Dots = new List<Panel>();


        public MainForm()
        {
            Instance = this;
            InitializeComponent();
            ButtonAnimator.InitializeAnimation(MainMenuFormBtn, "blue");
            ButtonAnimator.InitializeAnimation(ResetCounterBtn, "blue");
            ButtonAnimator.InitializeAnimation(ResetMaxConsecutiveBtn, "orange");
            ButtonAnimator.InitializeAnimation(BtnSelectFlavour, "blue");


            data = new StringBuilder("");
            Client = new S7Client();
            plcData = new PLCData(); // Make sure PLCData is a valid class
            Client = new S7Client();
        }
        public void MainForm_Load(object sender, EventArgs e)
        {
            SplashScreen splash = new SplashScreen();
            if (transversed_pages == false)
            {
                splash.Show();
                splash.TopMost = false;
                this.TopMost = false;

                string cPath;
                string AppFile;
                string InterfaceFile;


                adminscreen = new AdminScreen(this);

                foreach (Process p in Process.GetProcesses())
                {
                    if (String.Compare(p.ProcessName, "FZ-PanDA", true) == 0)
                    {
                        p.Kill();
                    }
                }

                cPath = Environment.CurrentDirectory;
                AppFile = cPath + "\\app.ini";
                InterfaceFile = cPath + "\\InterfaceControl.ini";
                
                source = new IniConfigSource(AppFile);
                InterfaceSource = new IniConfigSource(InterfaceFile);

                PLC_IP = source.Configs["Connection"].Get("PLC");
                Rack = Convert.ToInt32(source.Configs["Connection"].Get("Rack"));
                Slot = Convert.ToInt32(source.Configs["Connection"].Get("Slot"));
                DataBlock = Convert.ToInt32(source.Configs["Connection"].Get("DataBlock"));

                OMRON_IP = source.Configs["Connection"].Get("OMRON");

                Camera1LineNo = Convert.ToInt32(source.Configs["Connection"].Get("Camera1Line"));
                Camera2LineNo = Convert.ToInt32(source.Configs["Connection"].Get("Camera2Line"));
                Camera3LineNo = Convert.ToInt32(source.Configs["Connection"].Get("Camera3Line"));
                Camera4LineNo = Convert.ToInt32(source.Configs["Connection"].Get("Camera4Line"));


                Servo1Enable = Convert.ToInt32(InterfaceSource.Configs["ServoEnable"].Get("Servo1"));
                Servo2Enable = Convert.ToInt32(InterfaceSource.Configs["ServoEnable"].Get("Servo2"));
                Servo3Enable = Convert.ToInt32(InterfaceSource.Configs["ServoEnable"].Get("Servo3"));

                //DateTimeUpdate.Start();
                timer1.Start();


            }

            ConnectOmronCore();
            InitializeCameraSystem();
            DoActiveButtons(0);
            CurrentRecipeName();
            LoadLastSelectedRecipe();
            PLC_Connection();
            PLCUpdate.Start();

            ResetMaxConsecutiveBtn.Visible = false;


            if (transversed_pages == false)

                splash.Close();


        }
        private void InitializeCameraSystem()
        {
            // Position imageWindows inside panelControl
            imageWindow1.Parent = panelControl;
            imageWindow1.Dock = DockStyle.Fill;

            imageWindow2.Parent = panelControl;
            imageWindow2.Dock = DockStyle.Fill;
            imageWindow2.Visible = false;

            // Set initial state
            //cam1CurrentIndex = 1; // Start at UnitNo 2
            //cam2CurrentIndex = 0;
            //cam2Visible = false;

            // Create initial camera 1 display with proper CoreRA connection
            UpdateCamera1Display();
            UpdateCamera2Display();
            imageWindow1.Visible = true;
            imageWindow2.Visible = false;
            // Create indicator dots
            CreateIndicatorDots();
            UpdateDotIndicators();

        }

        private void CreateIndicatorDots()
        {
            panelCam1Dots.Controls.Clear();
            panelCam2Dots.Controls.Clear();
            cam1Dots.Clear();
            cam2Dots.Clear();

            int dotSize = 8;
            int spacing = 5;
            int totalWidth = (dotSize * 5) + (spacing * 4);
            int startX = (panelCam1Dots.Width - totalWidth) / 2;
            int startY = (panelCam1Dots.Height - dotSize) / 2;

            // Create 5 dots for camera 1
            for (int i = 0; i < cam1UnitNumbers.Length; i++)
            {
                Panel dot = new Panel
                {
                    Width = dotSize,
                    Height = dotSize,
                    Left = startX + (i * (dotSize + spacing)),
                    Top = startY,
                    BackColor = Color.Gray
                };

                // Make circular
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(0, 0, dotSize, dotSize);
                dot.Region = new Region(path);

                panelCam1Dots.Controls.Add(dot);
                cam1Dots.Add(dot);
            }

            // Create 5 dots for camera 2
            for (int i = 0; i < cam2UnitNumbers.Length; i++)
            {
                Panel dot = new Panel
                {
                    Width = dotSize,
                    Height = dotSize,
                    Left = startX + (i * (dotSize + spacing)),
                    Top = startY,
                    BackColor = Color.Gray
                };

                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(0, 0, dotSize, dotSize);
                dot.Region = new Region(path);

                panelCam2Dots.Controls.Add(dot);
                cam2Dots.Add(dot);
            }
        }

        private void UpdateDotIndicators()
        {
            // Update camera 1 dots
            for (int i = 0; i < cam1Dots.Count; i++)
            {
                if (i == cam1CurrentIndex && !cam2Visible)
                {
                    // Currently displayed - bright color
                    cam1Dots[i].BackColor = (cam1UpdateModes[i] == FZ_Control.UPDATE_IMAGE.NG_IMAGE) ?
                        Color.Red : Color.DodgerBlue;
                }
                else
                {
                    // Not displayed - dim color
                    cam1Dots[i].BackColor = (cam1UpdateModes[i] == FZ_Control.UPDATE_IMAGE.NG_IMAGE) ?
                        Color.FromArgb(80, 255, 0, 0) : // Dim red
                        Color.FromArgb(80, 30, 144, 255); // Dim blue
                }
            }

            // Update camera 2 dots
            for (int i = 0; i < cam2Dots.Count; i++)
            {
                if (i == cam2CurrentIndex && cam2Visible)
                {
                    // Currently displayed - bright color
                    cam2Dots[i].BackColor = (cam2UpdateModes[i] == FZ_Control.UPDATE_IMAGE.NG_IMAGE) ?
                        Color.Red : Color.DodgerBlue;
                }
                else
                {
                    // Not displayed - dim color
                    cam2Dots[i].BackColor = (cam2UpdateModes[i] == FZ_Control.UPDATE_IMAGE.NG_IMAGE) ?
                        Color.FromArgb(80, 255, 0, 0) : // Dim red
                        Color.FromArgb(80, 30, 144, 255); // Dim blue
                }
            }
        }
        private void Cam1Scroll_Click(object sender, EventArgs e)
        {
            // Cycle to next unit
            cam1CurrentIndex = (cam1CurrentIndex + 1) % cam1UnitNumbers.Length;

            // Show camera 1
            cam2Visible = false;
            UpdateCamera1Display();
            UpdateDotIndicators();
        }

        private void Cam2Scroll_Click(object sender, EventArgs e)
        {
            if (!cam2Visible)
            {
                // First press - show camera 2
                cam2Visible = true;
            }
            else
            {
                // Subsequent presses - cycle units
                cam2CurrentIndex = (cam2CurrentIndex + 1) % cam2UnitNumbers.Length;
            }

            UpdateCamera2Display();
            UpdateDotIndicators();
        }

        private void UpdateCamera1Display()
        {
            try
            {
                // Dispose existing coreRA1 connection
                if (coreRA1 != null)
                {
                    coreRA1.Dispose();
                }

                // Create new CoreRA instance
                //coreRA2 = new FZ_Control.CoreRA();
                //coreRA2.ContainerControl = this;

                // Configure connection
                coreRA1.FzPath = FZ_PATH;
                coreRA1.ConnectMode = FZ_Control.ConnectionMode.Remote;
                coreRA1.IpAddress = OMRON_IP;
                coreRA1.LineNo = Camera1LineNo;

                // Connect
                var connectResult = coreRA1.ConnectStart();

                if (connectResult != FZ_Control.ConnectionState.Success)
                {
                    MessageBox.Show($"Vision Controller Camera 1 Connection fail! Unit: {cam1UnitNumbers[cam1CurrentIndex]}");
                    log.Error($"Camera 1 connection failed: {connectResult}, Unit: {cam1UnitNumbers[cam1CurrentIndex]}");
                    return;
                }

                // Reconnect imageWindow1 to the new coreRA2
                imageWindow1.ConnectCoreRAComponent = coreRA1;
                imageWindow1.UnitNo = cam1UnitNumbers[cam1CurrentIndex];
                imageWindow1.SubNo = 1;
                imageWindow1.WindowNo = 24;
                imageWindow1.UpdateImage = cam1UpdateModes[cam1CurrentIndex];

                // Show imageWindow2, hide imageWindow1
                imageWindow1.Visible = true;
                imageWindow2.Visible = false;
                imageWindow1.BringToFront();

                log.Info($"Camera 1 Display: UnitNo={imageWindow1.UnitNo}, LineNo={coreRA1.LineNo}, Mode={imageWindow1.UpdateImage}");
            }
            catch (Exception ex)
            {
                log.Error($"Error updating Camera 1 display: {ex.Message}");
                MessageBox.Show($"Error switching Camera 1: {ex.Message}");
            }
        }
        private void UpdateCamera2Display()
        {
            try
            {
                // Dispose existing coreRA2 connection
                if (coreRA2 != null)
                {
                    coreRA2.Dispose();
                }

                // Create new CoreRA instance
                //coreRA2 = new FZ_Control.CoreRA();
                //coreRA2.ContainerControl = this;

                // Configure connection
                coreRA2.FzPath = FZ_PATH;
                coreRA2.ConnectMode = FZ_Control.ConnectionMode.Remote;
                coreRA2.IpAddress = OMRON_IP;
                coreRA2.LineNo = Camera2LineNo; // Use the UnitNo as LineNo

                // Connect
                var connectResult = coreRA2.ConnectStart();

                if (connectResult != FZ_Control.ConnectionState.Success)
                {
                    MessageBox.Show($"Vision Controller Camera 2 Connection fail! Unit: {cam2UnitNumbers[cam2CurrentIndex]}");
                    log.Error($"Camera 2 connection failed: {connectResult}, Unit: {cam2UnitNumbers[cam2CurrentIndex]}");
                    return;
                }

                // Reconnect imageWindow2 to the new coreRA2
                imageWindow2.ConnectCoreRAComponent = coreRA2;
                imageWindow2.UnitNo = cam2UnitNumbers[cam2CurrentIndex];
                imageWindow2.SubNo = 1;
                imageWindow2.WindowNo = 24;
                imageWindow2.UpdateImage = cam2UpdateModes[cam2CurrentIndex];

                // Show imageWindow2, hide imageWindow1
                imageWindow1.Visible = false;
                imageWindow2.Visible = true;
                imageWindow2.BringToFront();

                log.Info($"Camera 2 Display: UnitNo={imageWindow2.UnitNo}, LineNo={coreRA2.LineNo}, Mode={imageWindow2.UpdateImage}");
            }
            catch (Exception ex)
            {
                log.Error($"Error updating Camera 2 display: {ex.Message}");
                MessageBox.Show($"Error switching Camera 2: {ex.Message}");
            }
        }

        public void DoActiveButtons(int nUser)
        {
            MainMenuForm = new MainMenu(this);

            // Main Form
            if (CanAccessButton("MainMenuFormBtn", "MainForm", nUser))
            {
                MainMenuFormBtn.Visible = true;
            }
            else
            {
                MainMenuFormBtn.Visible = false;
            }

            if (CanAccessButton("ResetCounterBtn", "MainForm", nUser))
            {
                ResetCounterBtn.Visible = true;
            }
            else
            {
                ResetCounterBtn.Visible = false;
            }

            if (CanAccessButton("ChangeRecipeBtn", "MainMenuForm", nUser))
            {
                MainMenuForm.RecipeChangeBtn.Visible = true;
            }
            else
            {
                MainMenuForm.RecipeChangeBtn.Visible = false;
            }

            if (CanAccessButton("RecipeSetupBtn", "MainMenuForm", nUser))
            {
                MainMenuForm.RecipeSetupBtn.Visible = true;
            }
            else
            {
                MainMenuForm.RecipeSetupBtn.Visible = false;
            }

            if (CanAccessButton("DocumentsBtn", "MainMenuForm", nUser))
            {
                MainMenuForm.DocumentsBtn.Visible = true;
            }
            else
            {
                MainMenuForm.DocumentsBtn.Visible = false;
            }

            if (CanAccessButton("AccessControlBtn", "MainMenuForm", nUser))
            {
                MainMenuForm.AdminSettingsBtn.Visible = true;
            }
            else
            {
                MainMenuForm.AdminSettingsBtn.Visible = false;
            }

            if (CanAccessButton("UPSSettingBtn", "MainMenuForm", nUser))
            {
                MainMenuForm.UPSSettingBtn.Visible = true;
            }
            else
            {
                MainMenuForm.UPSSettingBtn.Visible = false;
            }

            if (nUser == 0)
            {
                MainMenuForm.UserLoginBtn.BackgroundImage = Resources.userlogin48x48;
                MainMenuForm.UserLoginBtn.Tag = "login";
            }
            else
            {
                MainMenuForm.UserLoginBtn.BackgroundImage = global::Omnicheck360.Properties.Resources.Logout48x48;
                MainMenuForm.UserLoginBtn.Tag = "logout";
            }

            if (CanAccessButton("DashboardBtn", "MainMenuForm", nUser))
            {
                MainMenuForm.DashboardBtn.Visible = true;
            }
            else
            {
                MainMenuForm.DashboardBtn.Visible = false;
            }

            if (CanAccessButton("QuitApplicationBtn", "MainMenuForm", nUser))
            {
                MainMenuForm.QuitApplicationBtn.Visible = true;
            }
            else
            {
                MainMenuForm.QuitApplicationBtn.Visible = false;
            }

            if (CanAccessButton("GlobalTrackingBtn", "MainMenuForm", nUser))
            {
                MainMenuForm.GlobalTrackingBtn.Visible = true;
            }
            else
            {
                MainMenuForm.GlobalTrackingBtn.Visible = false;
            }

            if (CanAccessButton("InterfaceControlBtn", "MainMenuForm", nUser))
            {
                MainMenuForm.InterfaceControlBtn.Visible = true;
            }
            else
            {
                MainMenuForm.InterfaceControlBtn.Visible = false;
            }
            if (CanAccessButton("Data", "MainMenuForm", nUser))
            {
                MainMenuForm.Data.Visible = true;
            }
            else
            {
                MainMenuForm.Data.Visible = false;
            }


        }
        public bool CanAccessButton(string sName, string sForm, int nVal)
        {

            string cPath = Directory.GetCurrentDirectory(); ;
            string cs = "Data Source=" + cPath + "\\omnisec.db";
            SQLiteCommand SQLcommand = default(SQLiteCommand);
            string sTable = "security";
            DataSet ds = new DataSet();
            string sVal = "";
            bool lCont = true;
            bool lCanAccess = true;

            if (Omnicheck360.MainForm.nUserAccess == 5)
            {
                lCanAccess = true;

            }
            else
            {

                using (SQLiteConnection SQLConnect = new SQLiteConnection(cs))
                {
                    SQLConnect.Open();

                    SQLcommand = SQLConnect.CreateCommand();
                    SQLcommand.CommandText = "SELECT * FROM " + sTable + " WHERE FormName = '" + sForm + "' AND btnName = '" + sName.ToUpper() + "'";
                    //"SELECT * FROM main:security" ' LIMIT 5"
                    //Dim SQLreader As SQLiteDataReader = SQLcommand.ExecuteReader()
                    SQLiteDataAdapter da = new SQLiteDataAdapter(SQLcommand.CommandText, SQLConnect);
                    da.Fill(ds, "security");
                    int nRows = ds.Tables[0].Rows.Count;
                    // There should be only one!
                    if (nRows != 1)
                    {
                        //MsgBox("No record found!")
                        lCont = false;
                        lCanAccess = false;
                    }
                    try
                    {
                        sVal = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                    }
                    catch
                    {
                        MessageBox.Show("Button Access Routine Failed!");
                    }
                    SQLConnect.Close();
                }
                if (lCont)
                {
                    if (int.Parse(sVal) > nVal)
                    {
                        lCanAccess = false;
                    }
                    else
                    {
                        lCanAccess = true;
                    }
                }
            }
            return lCanAccess;
        }
        private void PLC_Connection()
        {
            int Connection_Result;

            Connection_Result = Client.ConnectTo(PLC_IP, Rack, Slot);

            if (Connection_Result == 0)
            {
            }
            else
            {
                MessageBox.Show("PLC Connection Failed");
            }
        }
        private void PLCUpdate_Tick(object sender, EventArgs e)
        {
           // Console.WriteLine("[DEBUG] PLCUpdate_Tick started.");

            int PLC_Status = 0;
            int PLC_Status_Result = 0;

            // Check PLC status
            PLC_Status_Result = Client.PlcGetStatus(ref PLC_Status);
           // Console.WriteLine($"[DEBUG] PLC_Status_Result: {PLC_Status_Result}, PLC_Status: {PLC_Status}");

            if (PLC_Status_Result == 0)
            {
                switch (PLC_Status)
                {
                    case S7Consts.S7CpuStatusRun:
                        {
                         //   Console.WriteLine("[DEBUG] PLC is in RUN mode.");

                            try
                            {
                                int dbNumber = 4; // Data Block number
                                int startByte = 0; // Starting byte
                                int size = 204; // Ensure we can access byte 200

                                // Allocate buffer with the correct size
                                byte[] DB_Variables_Read_Buffer = new byte[size];
                             //   Console.WriteLine($"[DEBUG] Allocated buffer size: {DB_Variables_Read_Buffer.Length}");

                                // Read data from the PLC
                                int result = plcData.client1.DBRead(dbNumber, startByte, size, DB_Variables_Read_Buffer);
                              //  Console.WriteLine($"[DEBUG] DBRead result: {result}");

                                if (result == 0) // 0 means success
                                {
                                    // Extract and update values for display
                                    TotalCountValue.Text = S7.GetDIntAt(DB_Variables_Read_Buffer, 0).ToString();  // Bytes 0-3 (Cam1 throughput)
                                    TotalFailValue.Text = S7.GetDIntAt(DB_Variables_Read_Buffer, 20).ToString(); // Bytes 20-23
                                    BPMValue.Text = S7.GetDIntAt(DB_Variables_Read_Buffer, 48).ToString();

                                    // Calculate and display reject percentage
                                    int totalFails = int.Parse(TotalFailValue.Text);
                                    int totalCount = int.Parse(TotalCountValue.Text);

                                    if (totalCount > 0)
                                    {
                                        double rejectPercentage = ((double)totalFails / totalCount) * 100;
                                        RejectPercentage.Text = rejectPercentage.ToString("F2") + " %"; // 2 decimals
                                    }
                                    else
                                    {
                                        RejectPercentage.Text = "0.00 %";
                                    }

                                    // --- Monitor "Consecutive Bad Reached" (offset 200.2) ---
                                    if (DB_Variables_Read_Buffer.Length > 200)
                                    {
                                        byte plcByte = DB_Variables_Read_Buffer[200];
                                        bool consecutiveBadReached = (plcByte & (1 << 2)) != 0;

                                        // Update the button visibility on the UI thread
                                        if (ResetMaxConsecutiveBtn.InvokeRequired)
                                        {
                                            ResetMaxConsecutiveBtn.Invoke(new Action(() =>
                                            {
                                                ResetMaxConsecutiveBtn.Visible = consecutiveBadReached;
                                            }));
                                        }
                                        else
                                        {
                                            ResetMaxConsecutiveBtn.Visible = consecutiveBadReached;
                                        }
                                    }

                                    // --- Camera-specific logic for DB logging ---
                                    int currThroughputCam1 = S7.GetDIntAt(DB_Variables_Read_Buffer, 0);
                                    int currThroughputCam2 = S7.GetDIntAt(DB_Variables_Read_Buffer, 4);
                                    int currRejectCam1 = S7.GetDIntAt(DB_Variables_Read_Buffer, 24);
                                    int currRejectCam2 = S7.GetDIntAt(DB_Variables_Read_Buffer, 28);

                                    // Camera 1 (OCR)
                                    if (currThroughputCam1 > prevThroughputCam1)
                                    {
                                        bool badOCR = currRejectCam1 > prevRejectCam1;
                                        //InsertCameraDataToDB(
                                        //    cameraNumber: 1,
                                        //    badOCR: badOCR ? "1" : "0",
                                        //    badBarcode: "0",
                                        //    totalGood: (currThroughputCam1 - currRejectCam1).ToString(),
                                        //    totalBad: currRejectCam1.ToString(),
                                        //    totalThroughput: currThroughputCam1.ToString(),
                                        //    userName: CurrentUserTxt.Text,
                                        //    bottleSize: currentSize,         // Assign actual value from your context
                                        //    bottleFlavour: currentFlavour,   // Assign actual value from your context
                                        //    description: DescriptionNameLb.Text
                                        //);
                                    }

                                    // Camera 2 (Barcode)
                                    if (currThroughputCam2 > prevThroughputCam2)
                                    {
                                        bool badBarcode = currRejectCam2 > prevRejectCam2;
                                        //InsertCameraDataToDB(
                                        //    cameraNumber: 2,
                                        //    badOCR: "0",
                                        //    badBarcode: badBarcode ? "1" : "0",
                                        //    totalGood: (currThroughputCam2 - currRejectCam2).ToString(),
                                        //    totalBad: currRejectCam2.ToString(),
                                        //    totalThroughput: currThroughputCam2.ToString(),
                                        //    userName: CurrentUserTxt.Text,
                                        //    bottleSize: currentSize,         // Assign actual value from your context
                                        //    bottleFlavour: currentFlavour,   // Assign actual value from your context
                                        //    description: DescriptionNameLb.Text
                                        //);
                                    }

                                    // Update previous values for next tick
                                    prevThroughputCam1 = currThroughputCam1;
                                    prevRejectCam1 = currRejectCam1;
                                    prevThroughputCam2 = currThroughputCam2;
                                    prevRejectCam2 = currRejectCam2;

                                    // Log extracted values
                                   // Console.WriteLine($"[DEBUG] TotalCount: {TotalCountValue.Text}, TotalFails: {TotalFailValue.Text}, RejectPercentage: {RejectPercentage.Text}");
                                }
                                else
                                {
                                  //  Console.WriteLine($"[ERROR] Error reading from PLC: {result}");
                                    MessageBox.Show($"Error reading from PLC: {result}");
                                }
                            }
                            catch (Exception ex)
                            {
                              //  Console.WriteLine($"[ERROR] Exception in PLCUpdate_Tick: {ex.Message}");
                                MessageBox.Show($"Exception: {ex.Message}");
                            }

                            break;
                        }

                    case S7Consts.S7CpuStatusStop:
                        {
                           // Console.WriteLine("[DEBUG] PLC is in STOP mode.");
                            break;
                        }

                    default:
                        {
                            //Console.WriteLine("[DEBUG] PLC status is UNKNOWN.");
                            break;
                        }
                }
            }
            else
            {
               // Console.WriteLine("[ERROR] Failed to get PLC status.");
            }

          //  Console.WriteLine("[DEBUG] PLCUpdate_Tick completed.");
        }
        public void CurrentRecipeName()
        {
            string cPath;
            string AppFile;
            string final_recipe = " N/A ";
            int Recipe_index = 0;

            cPath = Environment.CurrentDirectory;
            AppFile = cPath + "\\app.ini";
            source = new IniConfigSource(AppFile);

            mystring = new StringBuilder();
            rc = new RecipeChange(this);

            log.Info("Recipe Change Page: Recipe Changed to " + final_recipe);
            log.Info("Dashboard Page: GUI Started");



            try
            {
                    coreRA1.Macro_GetVariable("SceneNo", mystring, 10);
                    Starting_Recipe_Num = Convert.ToInt32(mystring.ToString());
                    //RecipeNameLb.Text = source.Configs["RecipeName"].Get(mystring.ToString());
                    Current_Recipe_Num = Starting_Recipe_Num;
                    coreRA1.Macro_DirectExecute("MeasureStop");
                    coreRA1.Macro_DirectExecute("ClearMeasureData");
                    coreRA1.Macro_DirectExecute("MeasureStart");
           }
            catch
            {
                MessageBox.Show("Scene Get Fail");
            }
        }
        private void ConnectOmronCore()
        {

            try
            {
                //    coreRA1.FzPath = FZ_PATH;
                //    coreRA1.ConnectMode = FZ_Control.ConnectionMode.Remote;
                //    coreRA1.IpAddress = OMRON_IP;
                //    coreRA1.LineNo = Camera1LineNo;

                //    switch (coreRA1.ConnectStart())
                //    {
                //        case FZ_Control.ConnectionState.InvalidArgumentError:
                //            MessageBox.Show("Vision Controller CoreRA1 Connection fail!");
                //            break;

                //        case FZ_Control.ConnectionState.DirectoryNotFoundError:
                //            MessageBox.Show("Vision Controller CoreRA1 Connection fail!");
                //            break;

                //        default:
                //            break;
                //    }
                //coreRA2.FzPath = FZ_PATH;
                //coreRA2.ConnectMode = FZ_Control.ConnectionMode.Remote;
                //coreRA2.IpAddress = OMRON_IP;
                //coreRA2.LineNo = Camera2LineNo;
                
                //switch (coreRA2.ConnectStart())
                //{
                //    case FZ_Control.ConnectionState.InvalidArgumentError:
                //        MessageBox.Show("Vision Controller coreRA2 Connection fail!");
                //        break;

                //    case FZ_Control.ConnectionState.DirectoryNotFoundError:
                //        MessageBox.Show("Vision Controller coreRA2 Connection fail!");
                //        break;

                //    default:
                //        break;
                //}

            }
            catch
            {
                MessageBox.Show("Failed to connect to Vision Controller!");
            }

        }
        private void MainMenuFormBtn_Click(object sender, EventArgs e)
        {
            log.Info("Dashboard Page: Main Menu Button Pressed");
            this.TopMost = false;
            MainMenuForm.ShowDialog();
            MainMenuForm.Focus();
            MainMenuForm.TopMost = true;

        }
        private void ResetCounterBtn_Click(object sender, EventArgs e)
        {
            try
            {
                OmniMsgOriginal ConfirmationMessage = new OmniMsgOriginal("Please Confirm Reset", 1);
                ConfirmationMessage.ShowDialog();
                ConfirmationMessage.TopMost = true;

                do
                {
                    // Waiting for answer
                }
                while (Received_Answer == false);

                if (received_answer_value == 1)
                {
                    received_answer_value = -1;
                    Received_Answer = false;
                    log.Info("Dashboard Page: Reset All Counters Button Pressed");

                    // Write 0 directly to Total Throughput and Total Rejects
                    Client.DBWrite(4, 0, 4, new byte[4]); // Write 0 to Total Throughput (DWORD at offset 0)
                    Client.DBWrite(4, 20, 4, new byte[4]); // Write 0 to Total Rejects (DWORD at offset 4)
                    Client.DBWrite(4, 24, 4, new byte[4]); // Write 0 to Total Rejects (DWORD at offset 4)

                   // Console.WriteLine("[DEBUG] Successfully reset Total Throughput and Total Rejects to 0.");

                    // Reset the chart directly in this method
                    //if (cartesianChart1 != null) // Ensure the cartesian chart is initialized
                    //{
                    //    // Clear value series
                    //    if (valueSeries is LineSeries<double> valueLineSeries)
                    //    {
                    //        valueLineSeries.Values = Array.Empty<double>(); // Reset value series
                    //    }

                    //    // Clear threshold series (upper and lower)
                    //    if (upperThresholdSeries is LineSeries<double> upperLineSeries)
                    //    {
                    //        upperLineSeries.Values = Enumerable.Repeat(0.0, 200).ToArray(); // Reset to default upper threshold
                    //    }

                    //    if (lowerThresholdSeries is LineSeries<double> lowerLineSeries)
                    //    {
                    //        lowerLineSeries.Values = Enumerable.Repeat(0.0, 200).ToArray(); // Reset to default lower threshold
                    //    }

                    //    cartesianChart1.Update(); // Update the chart to reflect the changes
                    //    Console.WriteLine("[DEBUG] Cartesian chart successfully reset.");
                    //}
                }
                else
                {
                    received_answer_value = -1;
                    Received_Answer = false;
                }
            }
            catch (Exception ex)
            {
               // Console.WriteLine($"[ERROR] Exception in ResetCounterBtn_Click: {ex.Message}");
                MessageBox.Show("PLC could not reset counters");
            }

            // Reset Vision System Data inline
                coreRA1.Macro_DirectExecute("MeasureStop");
                coreRA1.Macro_DirectExecute("ClearMeasureData");
                coreRA1.Macro_DirectExecute("PutPort" + "\"" + "ParallelIo" + "\"" + "," + 102 + "," + 0);
                coreRA1.Macro_DirectExecute("MeasureStart");
        }
        private void ResetMaxConsecutiveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Dashboard Page: Reset Max Consecutive Reject Button Pressed");

                int dbNumber = 4;      // Use your actual DB number
                int byteOffset = 200;  // Byte 200
                int bitOffset = 3;     // Bit 3

                // Read the current byte from the PLC
                byte[] buffer = new byte[1];
                int readResult = Client.DBRead(dbNumber, byteOffset, 1, buffer);
                if (readResult != 0)
                {
                    MessageBox.Show("Failed to read PLC for consecutive reject reset.");
                    return;
                }

                // Set bit 3 to 1
                buffer[0] |= (byte)(1 << bitOffset);
                int writeResult = Client.DBWrite(dbNumber, byteOffset, 1, buffer);
                if (writeResult != 0)
                {
                    MessageBox.Show("Failed to write PLC for consecutive reject reset.");
                    return;
                }

                ResetMaxConsecutiveBtn.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("PLC could not Reset Consecutive Reject Error: " + ex.Message);
            }
        }
        private void ChangeRecipeBtn_Click(object sender, EventArgs e)
        {
            ShowFlavourSelector();
        }
        private void ShowFlavourSelector()
        {
            // Load the barcode JSON once if not already loaded
            if (_barcodeItems == null)
            {
                var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Barcodlist.json");
                _barcodeItems = BarcodeLoader.Load(jsonPath);
            }

            // Build the flavoursBySize dictionary as Dictionary<string, List<BarcodeItem>>
            var flavoursBySize = new Dictionary<string, List<BarcodeItem>>();

            foreach (var item in _barcodeItems)
            {
                string size = item.CodeDeProduit?.ToString() ?? "Unknown";
                if (!flavoursBySize.ContainsKey(size))
                    flavoursBySize[size] = new List<BarcodeItem>();
                flavoursBySize[size].Add(new BarcodeItem
                {
                    Recipes = item.Recipes,
                    Index = item.Index,
                    UPC = item.UPC,
                    CodeDeProduit = item.CodeDeProduit,
                    Description = item.Description
                });
            }

            var flavourForm = new FlavourSelectorForm(flavoursBySize);

            if (flavourForm.ShowDialog() == DialogResult.OK)
            {
                // Barcode entered
                if (!string.IsNullOrEmpty(flavourForm.EnteredBarcode))
                {
                    var matches = _barcodeItems
                        .Where(b => b.UPC == flavourForm.EnteredBarcode)
                        .ToList();

                    if (matches.Count == 0)
                    {
                        MessageBox.Show("Barcode not found!", "Error");
                    }
                    else if (matches.Count == 1)
                    {
                        var match = matches[0];
                        SetRecipeLabels(
                            $"Code du Produit: {match.CodeDeProduit ?? ""}",
                            match.Description,
                            match.UPC
                        );
                        SendToInspectionSystem(match.Index ?? 0, match.UPC);
                        MessageBox.Show($"Flavour set to:\nCode du Produit: {match.CodeDeProduit ?? ""}\nDescription: {match.Description}", "Success");
                    }
                    else
                    {
                        // Multiple matches: show ambiguity popup
                        using (var ambiguityForm = new BarcodeAmbiguityForm(matches))
                        {
                            if (ambiguityForm.ShowDialog() == DialogResult.OK && ambiguityForm.SelectedItem != null)
                            {
                                var selected = ambiguityForm.SelectedItem;
                                SetRecipeLabels(
                                    $"Code du Produit: {selected.CodeDeProduit ?? ""}",
                                    selected.Description,
                                    selected.UPC
                                );
                                SendToInspectionSystem(selected.Index ?? 0, selected.UPC);
                                MessageBox.Show($"Flavour set to:\nCode du Produit: {selected.CodeDeProduit ?? ""}\nDescription: {selected.Description}", "Success");
                            }
                        }
                    }
                }
                // Flavour selected by button
                else if (!string.IsNullOrEmpty(flavourForm.SelectedFlavour))
                {
                    // Find the selected item by description
                    var selectedItem = _barcodeItems.FirstOrDefault(b => b.Description == flavourForm.SelectedFlavour);
                    if (selectedItem != null)
                    {
                        SetRecipeLabels(
                            $"Code du Produit: {selectedItem.CodeDeProduit ?? ""}",
                            selectedItem.Description,
                            selectedItem.UPC
                        );
                        SendToInspectionSystem(selectedItem.Index ?? 0, selectedItem.UPC);
                    }
                    else
                    {
                        SetRecipeLabels(flavourForm.SelectedFlavour, flavourForm.SelectedFlavour, string.Empty);
                    }
                }
            }
        }
        public void SetRecipeLabels(string codeLabel, string description, string upc)
        {
            DescriptionNameLb.Text = description ?? string.Empty;
            SaveLastSelectedRecipe(codeLabel, description, upc);
            // Set current size and flavour for database use
            currentSize = codeLabel ?? "";
            currentFlavour = description ?? "";
        }
        public void SendToInspectionSystem(int index, string upc)
        {
            // --- coreRA1 ---
            coreRA1.Macro_DirectExecute("MeasureStop");
            //codeLabelValue = new string(codeLabelValue.Where(char.IsDigit).ToArray());
            //if (string.IsNullOrEmpty(codeLabelValue))
            //    codeLabelValue = index.ToString();
            //string quotedCodeLabelValue = $"\"{codeLabelValue}\"";
            //string hashString = $"\"{new string('#', codeLabelValue.Length)}\"";

            //string dataStringCode1 = $"SetUnitData 8,\"teachString0\", {quotedCodeLabelValue}";
            //coreRA1.Macro_DirectExecute(dataStringCode1);

            //string dataStringHashes1 = $"SetUnitData 8,\"formatString0\", {hashString}";
            //coreRA1.Macro_DirectExecute(dataStringHashes1);

            //coreRA1.Macro_DirectExecute("SetSceneTitle 0,\"" + codeLabelValue + "\"");
            //coreRA1.Macro_DirectExecute("SaveData");
            //coreRA1.Macro_DirectExecute("MeasureStart");

            // --- coreRA2 ---
            coreRA2.Macro_DirectExecute("MeasureStop");
            string quotedUpc = $"\"{upc}\"";// Send UPC value to coreRA2, unit 2
            string dataStringCode2 = $"SetUnitData 2,\"judgeCompString\", {quotedUpc}";
            coreRA2.Macro_DirectExecute(dataStringCode2);
            coreRA2.Macro_DirectExecute("SetSceneTitle 0,\"" + upc + "\"");
            coreRA2.Macro_DirectExecute("SaveData");
            coreRA2.Macro_DirectExecute("MeasureStart");
        }
        private void SaveLastSelectedRecipe(string recipeName, string description, string upc)
        {
            string cPath = Environment.CurrentDirectory;
            string AppFile = Path.Combine(cPath, "app.ini");
            var source = new IniConfigSource(AppFile);

            if (source.Configs["LastSelectedRecipe"] == null)
                source.AddConfig("LastSelectedRecipe");

            source.Configs["LastSelectedRecipe"].Set("RecipeName", recipeName);
            source.Configs["LastSelectedRecipe"].Set("Description", description);
            source.Configs["LastSelectedRecipe"].Set("UPC", upc);

            source.Save();
        }

        private void LoadLastSelectedRecipe()
        {
            string cPath = Environment.CurrentDirectory;
            string AppFile = Path.Combine(cPath, "app.ini");
            var source = new IniConfigSource(AppFile);

            var lastRecipeConfig = source.Configs["LastSelectedRecipe"];
            if (lastRecipeConfig != null)
            {
                string lastRecipeName = lastRecipeConfig.Get("RecipeName");
                string lastDescription = lastRecipeConfig.Get("Description");
                string lastUpc = lastRecipeConfig.Get("UPC");

                if (!string.IsNullOrEmpty(lastRecipeName) || !string.IsNullOrEmpty(lastDescription))
                {
                    SetRecipeLabels(lastRecipeName, lastDescription, lastUpc);
                }
            }
        }
        public static void InsertCameraDataToDB(
    int cameraNumber,
    string badOCR,
    string badBarcode,
    string totalGood,
    string totalBad,
    string totalThroughput,
    string userName,
    string bottleSize,
    string bottleFlavour,
    string recipeName,
    string description,
    string upc)
        {
            string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InspectionResults.db");
            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();

                // Ensure the table exists
                string createTableSql = @"CREATE TABLE IF NOT EXISTS InspectionResults (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
                    CameraNumber INTEGER,
                    BadOCR TEXT,
                    BadBarcode TEXT,
                    TotalGood TEXT,
                    TotalBad TEXT,
                    TotalThroughput TEXT,
                    UserName TEXT,
                    BottleSize TEXT,
                    BottleFlavour TEXT,
                    RecipeName TEXT,
                    Description TEXT,
                    UPC TEXT
                );";
                using (var createCmd = new SQLiteCommand(createTableSql, connection))
                {
                    createCmd.ExecuteNonQuery();
                }

                // Insert the result
                string insertSql = @"INSERT INTO InspectionResults 
                    (CameraNumber, BadOCR, BadBarcode, TotalGood, TotalBad, TotalThroughput, UserName, BottleSize, BottleFlavour, RecipeName, Description, UPC)
                    VALUES
                    (@CameraNumber, @BadOCR, @BadBarcode, @TotalGood, @TotalBad, @TotalThroughput, @UserName, @BottleSize, @BottleFlavour, @RecipeName, @Description, @UPC);";

                using (var cmd = new SQLiteCommand(insertSql, connection))
                {
                    cmd.Parameters.AddWithValue("@CameraNumber", cameraNumber);
                    cmd.Parameters.AddWithValue("@BadOCR", badOCR ?? "");
                    cmd.Parameters.AddWithValue("@BadBarcode", badBarcode ?? "");
                    cmd.Parameters.AddWithValue("@TotalGood", totalGood ?? "");
                    cmd.Parameters.AddWithValue("@TotalBad", totalBad ?? "");
                    cmd.Parameters.AddWithValue("@TotalThroughput", totalThroughput ?? "");
                    cmd.Parameters.AddWithValue("@UserName", userName ?? "");
                    cmd.Parameters.AddWithValue("@BottleSize", bottleSize ?? "");
                    cmd.Parameters.AddWithValue("@BottleFlavour", bottleFlavour ?? "");
                    cmd.Parameters.AddWithValue("@RecipeName", recipeName ?? "");
                    cmd.Parameters.AddWithValue("@Description", description ?? "");
                    cmd.Parameters.AddWithValue("@UPC", upc ?? "");
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

    }
}    
    
