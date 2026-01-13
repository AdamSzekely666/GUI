using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WinForms;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using OmniCheck_360;
using OmniCheck_360.Properties;
using OpenHardwareMonitor.Hardware;
using OpenTK.Audio.OpenAL;
using Sharp7;
using SkiaSharp;
using System;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zebra.ADA.OperatorAPI;
using Zebra.ADA.OperatorAPI.Communication;
using Zebra.ADA.OperatorAPI.ControlsPackage;
using Zebra.ADA.OperatorAPI.InputsPackage;
using Zebra.ADA.OperatorAPI.OutputsPackage;
using System.Reflection;
using Opc.Ua;
using Opc.Ua.Client;
using System.ComponentModel;


namespace MatroxLDS
{

    public partial class MainForm : BaseForm
    {
        public RecipeChange recipeChange;
        public ManageInspectionHosts ManageInspectionHosts;
        public MainMenu MainMenuForm;
        private TrackingSystem trackingSystem;
        public static string MainCurrentUserTxt = "Operate Mode";
        public string SelectedRecipe;
        public int nUserAccess;
        public double temp = 0.0;
        public double frame_rate = 0.0;
        private const double MaxDatabaseSizeMB = 1; // Example: 500 MB
        private int xAxisStart = 0;
        public List<User> users;
        public Dictionary<string, Dictionary<string, int>> buttonPermissions;
        public string currentUserName; // Add this declaration
        private Timer inactivityTimer;
        private const int InactivityLimit = 30 * 60 * 1000; // 30 minutes in milliseconds
        public bool IsUserLoggedIn { get; set; } = false;
        public static MainForm Instance = null;
        private SplashScreen splashscreen; // Add this variable to reference the splash screen
        public const string OPERATOR_VIEW_NAME = "OperatorView";
        public const string PROJECT_NAME1 = "ECI1";
        public const string PROJECT_NAME2 = "ECI2";
        public const string HOST_NAME = "localhost";
        public const string TOTALGOOD = "TotalGoodCount";
        public const string TOTALBAD = "TotalBadCount";
        public const string TOTALTHROUGHPUT = "TotalThroughput";
        public const string RESETCOUNTERS1 = "ResetCounters";
        public const string RESETCOUNTERS2 = "ResetCounters";
        public const string C1CURRENTRECIPENAME = "CurrentRecipeName";
        public const string C2CURRENTRECIPENAME = "CurrentRecipeName";
        private readonly ConcurrentQueue<Zebra.ADA.OperatorAPI.ExecutionMessagesPackage.ExecutionMessage> _executionMessagesQueue
            = new ConcurrentQueue<Zebra.ADA.OperatorAPI.ExecutionMessagesPackage.ExecutionMessage>();

        // Keep track of handlers so we can unsubscribe cleanly per-project
        private readonly Dictionary<string, Zebra.ADA.OperatorAPI.ExecutionMessagesPackage.NewExecutionMessagesEventHandler> _executionHandlers
            = new Dictionary<string, Zebra.ADA.OperatorAPI.ExecutionMessagesPackage.NewExecutionMessagesEventHandler>();
        // Add these fields to MainForm (near other private fields)
        public delegate void FocusHandler();
        public Dictionary<string, (CartesianChart chart, ISeries series1, ISeries series2)> chartMappings;
        private readonly List<string> projectNames = new List<string>
            {
                { "ECI1" },
                { "ECI2" },
            };
        private int currentProjectIndex = 0;
        // Maps each project to its displays
        private readonly Dictionary<string, List<string>> projectDisplays = new Dictionary<string, List<string>>
            {
                { "ECI1", new List<string> { "All","Finish", "BrokenFinish","FinishLastReject" } },
                { "ECI2", new List<string> { "All", "BaseSearch", "BaseInspection", "ISWInspection", "DentInspection", "BaseLastReject",  "ISWLastReject", "DentLastReject" } },
            };

        // Tracks which display is currently shown for each project
        private readonly Dictionary<string, int> currentDisplayIndexes = new Dictionary<string, int>
            {
                { "ECI1", 0 },
                { "ECI2", 0 },
            };
        private Dictionary<string, DisplayControl> displayControls = new Dictionary<string, DisplayControl>();
        private DisplayControl currentDisplayControl = null;
        public OperatorViewPage CameraPage1;
        public OperatorViewPage CameraPage2;
        private int[] lastTotalGood = new int[3];
        private int[] lastTotalBad = new int[3];
        private CameraData[] latestCameraData = new CameraData[3];
        private readonly object dbLock = new object();
        private CameraData[] bufferedData = new CameraData[3];
        private DateTime[] bufferedTimestamps = new DateTime[3];
        private object bufferLock = new object();
        private readonly Dictionary<string, List<string>> cameraFieldMappings = new Dictionary<string, List<string>>
{
    { "ECI1", new List<string> { "FinishNumber" } },                // Camera 1 fields
    { "ECI2", new List<string> { "BaseNumber", "ISWNumber","DentNumber" } },     // Camera 2 fields
};
        public string currentSize;
        public string currentFlavour;
        private Button btnSelectFlavour;
        private int selectedCameraIndex = 0;
        private System.Windows.Forms.Timer plcTimer;
        private S7Client plcClient = null;
        private bool _plcPollingInProgress = false;
        private int _previousDownCan = 0;
        private readonly object _downCanLock = new object();
        private UsersManager _usersManager;
        private Action<string> _cardHandler;

        public event EventHandler FlavourChanged;
        protected virtual void OnFlavourChanged() => FlavourChanged?.Invoke(this, EventArgs.Empty);
        public UsersManager GetUsersManager()
        {
            return _usersManager;
        }
        public event EventHandler UserStateChanged;
        // ========== NEW:  OPC UA Fields ==========
        private OPCConnectionManager opcConnectionManager;
        private OPCDataModel opcDataModel;
        public DAOPCSession opcSession;  // Public so other forms can access it

        // OPC UA Bindings for Camera 1 (ECI1)
        private DAOPCBinding<int> c1FinishNumberBinding;
        private DAOPCBinding<int> c1TotalGoodBinding;
        private DAOPCBinding<int> c1TotalBadBinding;
        private DAOPCBinding<int> c1TotalThroughputBinding;
        private DAOPCBinding<string> c1CurrentRecipeBinding;

        // OPC UA Bindings for Camera 2 (ECI2)
        private DAOPCBinding<int> c2BaseNumberBinding;
        private DAOPCBinding<int> c2ISWNumberBinding;
        private DAOPCBinding<int> c2DentNumberBinding;
        private DAOPCBinding<int> c2TotalGoodBinding;
        private DAOPCBinding<int> c2TotalBadBinding;
        private DAOPCBinding<int> c2TotalThroughputBinding;
        private DAOPCBinding<string> c2CurrentRecipeBinding;
        // ========== END NEW FIELDS ==========

        protected virtual void OnUserStateChanged()
        {
            try
            {
                UserStateChanged?.Invoke(this, EventArgs.Empty);
            }
            catch
            {
                // Swallow subscriber exceptions to avoid UI crash if a listener misbehaves
            }
        }
        public void ReloadUsersFromConfig()
        {
            try
            {
                var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
                if (!File.Exists(configFilePath))
                    return;

                var json = File.ReadAllText(configFilePath);
                var config = JsonConvert.DeserializeObject<Configuration>(json);
                if (config == null)
                    return;

                // Update in-memory users and permissions
                users = config.Users ?? new List<User>();
                buttonPermissions = config.Buttons ?? new Dictionary<string, Dictionary<string, int>>();

                // If UsersManager exists, update its reference to the current users list
                _usersManager?.SetUsers(users);

                // Update UI button visibility immediately for the current user
                SetButtonVisibilityForCurrentUser();

                // Notify listeners so menus/dialogs can refresh (if you added UserStateChanged etc.)
                try { OnUserStateChanged(); } catch { }
            }
            catch (Exception ex)
            {
                // Log or show error as appropriate — avoid throwing back to caller
                Debug.WriteLine("ReloadUsersFromConfig error: " + ex.Message);
            }
        }
        public MainForm()
        {
            Instance = this;
            InitializeComponent();

            Cam1Scroll.Click -= Cam1Scroll_Click;
            Cam1Scroll.Click += Cam1Scroll_Click;
            Cam2Scroll.Click -= Cam2Scroll_Click;
            Cam2Scroll.Click += Cam2Scroll_Click;
            ButtonAnimator.InitializeAnimation(MainMenuFormBtn, "blue");
            ButtonAnimator.InitializeAnimation(ResetCounterBtn, "blue");
            ButtonAnimator.InitializeAnimation(Cam1Scroll, "blue");
            ButtonAnimator.InitializeAnimation(Cam2Scroll, "blue");
            ButtonAnimator.InitializeAnimation(AllCameraDisplay, "blue");
            ButtonAnimator.InitializeAnimation(BtnSelectFlavour, "blue");

            for (int i = 0; i < 3; i++)
            {
                lastTotalGood[i] = 0;
                lastTotalBad[i] = 0;
                latestCameraData[i] = new CameraData();
            }
            
            String cPath;
            cPath = Environment.CurrentDirectory;

            DateTimeUpdate.Start();
            ResetCounterBtn1.PageName = OPERATOR_VIEW_NAME;
            ResetCounterBtn1.ControlName = RESETCOUNTERS1;
            ResetCounterBtn1.ProjectName = PROJECT_NAME1;
            ResetCounterBtn1.HostName = HOST_NAME;

            ResetCounterBtn2.PageName = OPERATOR_VIEW_NAME;
            ResetCounterBtn2.ControlName = RESETCOUNTERS2;
            ResetCounterBtn2.ProjectName = PROJECT_NAME2;
            ResetCounterBtn2.HostName = HOST_NAME;


            CameraPage1 = HostManager.GetHost(HOST_NAME).Projects[PROJECT_NAME1].OperatorViews["OperatorView"];
            CameraPage2 = HostManager.GetHost(HOST_NAME).Projects[PROJECT_NAME2].OperatorViews["OperatorView"];
            CameraPage1.ValueElements[C1CURRENTRECIPENAME].ValueChanged += C1CurrentRecipeName;
            CameraPage2.ValueElements[C2CURRENTRECIPENAME].ValueChanged += C2CurrentRecipeName;

            plcTimer = new System.Windows.Forms.Timer();
            plcTimer.Interval = 1000; // Poll every 1 second (adjust as needed)
            plcTimer.Tick += PlcTimer_Tick;
            plcTimer.Start();

        }
        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                Program.splashForm?.UpdateProgress("MainForm Load Start");

                // Set default user
                currentUserName = "Operate";
                nUserAccess = 1; // Assuming 1 is the security level for "Operate"
                UpdateCurrentUserText();

                LoadConfig(Program.splashForm);
                SetButtonVisibilityForCurrentUser(Program.splashForm); // Set visibility based on default user

                try
                {
                    // Initialize card mapping manager using existing users list from config.json
                    _usersManager = new UsersManager(users);

                    // Start global service; uses default port COM5/9600 - make configurable later
                    CardReaderService.Instance.StatusChanged += (s) =>
                    {
                        if (this.IsHandleCreated)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                // Defensive: check control exists
                                try
                                {
                                    if (lblCardReaderStatus == null) return;
                                    lblCardReaderStatus.Text = s ?? string.Empty;
                                    lblCardReaderStatus.ForeColor = (s ?? "").StartsWith("Connected", StringComparison.OrdinalIgnoreCase)
                                        ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                                }
                                catch { }
                            }));
                        }
                    };

                    // Keep a named handler to allow clean unsubscribe
                    _cardHandler = (cardId) => OnCardPresented(cardId);
                    CardReaderService.Instance.CardPresented += _cardHandler;
                    CardReaderService.Instance.Start("COM5", 9600);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Card initialization error: {ex.Message}");
                }

                string dbPath = GetQuarterlyDatabasePath(DateTime.Now);
                CreateQuarterlyDatabase(dbPath); // <-- This is your new hard-coded method
                await InitializeOPCConnection();
                // Wait for runtime projects to be started.
                await EnsureHostProjectRunning(HOST_NAME, PROJECT_NAME1);
                await EnsureHostProjectRunning(HOST_NAME, PROJECT_NAME2);

                // now safe to access Projects and OperatorViews
                try
                {
                    CameraPage1 = HostManager.GetHost(HOST_NAME).Projects[PROJECT_NAME1].OperatorViews[OPERATOR_VIEW_NAME];
                    CameraPage2 = HostManager.GetHost(HOST_NAME).Projects[PROJECT_NAME2].OperatorViews[OPERATOR_VIEW_NAME];
                }
                catch (Exception ex)
                {
                    Program.splashForm?.UpdateProgress($"Failed binding OperatorViews: {ex.Message}");
                    Debug.WriteLine("OperatorViews binding failed: " + ex.Message);
                }

                // then attach handlers (guard for null)
                try { CameraPage1?.ValueElements[C1CURRENTRECIPENAME].ValueChanged += C1CurrentRecipeName; } catch { }
                try { CameraPage2?.ValueElements[C2CURRENTRECIPENAME].ValueChanged += C2CurrentRecipeName; } catch { }

                // Start displays (safe: StartDisplays will be defensive now)
                try { StartDisplays(Program.splashForm); } catch (Exception ex) { Debug.WriteLine("StartDisplays failed: " + ex.Message); }

                // Connect textboxes and other UI wiring
                try { ConnectToAllTextboxes(Program.splashForm); } catch (Exception ex) { Debug.WriteLine("ConnectToAllTextboxes failed: " + ex.Message); }

                // Load flavour persisted state
                try { LoadCurrentFlavourFromDb(); } catch (Exception ex) { Debug.WriteLine("LoadCurrentFlavourFromDb failed: " + ex.Message); }

                // Start background worker
                try
                {
                    if (!backgroundWorker1.IsBusy)
                    {
                        Program.splashForm?.UpdateProgress("Starting background worker...");
                        backgroundWorker1.RunWorkerAsync();
                        Program.splashForm?.UpdateProgress("Background worker started.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Background worker start failed: " + ex.Message);
                }

                // Create and await display controls
                try { await CreateAllDisplayControls(); } catch (Exception ex) { Debug.WriteLine("CreateAllDisplayControls failed: " + ex.Message); }

                // Update current recipe label safely
                try
                {
                    string currentRecipeC1 = CameraPage1?.ValueElements["CurrentRecipeName"].Value?.ToString();
                    lblCurrentFlavour.Text = $" {currentRecipeC1}";
                }
                catch { }

                // Initialize other displays: create/hide extras
                try
                {
                    for (int i = 1; i < projectNames.Count; i++)
                    {
                        ShowDisplay(projectNames[i], projectDisplays[projectNames[i]][0]); // Show "All" for Camera 2 and 3
                        Application.DoEvents(); // Ensure the UI/control updates
                        if (displayControls.TryGetValue($"{projectNames[i]}_{projectDisplays[projectNames[i]][0]}", out var ctrl))
                            ctrl.Visible = false; // Hide them
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Extra display initialization failed: " + ex.Message);
                }

                // Show Camera 1 "All" and keep it visible
                try { ShowDisplay(projectNames[0], projectDisplays[projectNames[0]][0]); } catch { }

                foreach (var project in projectNames)
                {
                    currentDisplayIndexes[project] = 0;
                }

                InitializeInactivityTimer(Program.splashForm);

                // Register event handlers
                this.MouseMove += MainForm_MouseMove;
                this.KeyPress += MainForm_KeyPress;

                Program.splashForm?.UpdateProgress("MainForm Load Complete");

                // Now the app is done with splash updates — close it here (move CloseSplashSafely to the end)
                try { CloseSplashSafely(); } catch { }

                // Show initial dots as soon as displays have been loaded and indexes set
                DrawDisplayDots(panelCam1Dots, projectDisplays["ECI1"], currentDisplayIndexes["ECI1"]);
                DrawDisplayDots(panelCam2Dots, projectDisplays["ECI2"], currentDisplayIndexes["ECI2"]);
            }
            catch (Exception ex)
            {
                // defensive: don't assume splash exists
                try { Program.splashForm?.UpdateProgress($"Error during initialization: {ex.Message}"); } catch { }
                Debug.WriteLine("MainForm_Load unhandled exception: " + ex);
            }
        }
        private async Task InitializeOPCConnection()
        {
            try
            {
                Program.splashForm?.UpdateProgress("Initializing OPC UA connection...");

                // Create data model and connection manager
                opcDataModel = new OPCDataModel();
                opcConnectionManager = new OPCConnectionManager(opcDataModel);

                // Connect to OPC UA server (adjust server name and port as needed)
                bool connected = opcConnectionManager.Connect("localhost", "4840");

                if (connected)
                {
                    opcSession = opcConnectionManager.Session;
                    Program.splashForm?.UpdateProgress("OPC UA connected successfully");

                    // Initialize bindings after connection
                    InitializeOPCBindings();
                }
                else
                {
                    Program.splashForm?.UpdateProgress("OPC UA connection failed - using fallback");
                    Debug.WriteLine("OPC UA connection failed");
                }
            }
            catch (Exception ex)
            {
                Program.splashForm?.UpdateProgress($"OPC UA error: {ex.Message}");
                Debug.WriteLine($"OPC UA initialization error: {ex.Message}");
            }
        }
        private void InitializeOPCBindings()
        {
            try
            {
                if (opcSession == null) return;

                // Camera 1 (ECI1) bindings
                c1FinishNumberBinding = new DAOPCBinding<int>(opcSession, "ECI1.FinishNumber");
                c1TotalGoodBinding = new DAOPCBinding<int>(opcSession, "ECI1.TotalGoodCount");
                c1TotalBadBinding = new DAOPCBinding<int>(opcSession, "ECI1.TotalBadCount");
                c1TotalThroughputBinding = new DAOPCBinding<int>(opcSession, "ECI1.TotalThroughput");
                c1CurrentRecipeBinding = new DAOPCBinding<string>(opcSession, "ECI1.CurrentRecipeName");

                // Camera 2 (ECI2) bindings
                c2BaseNumberBinding = new DAOPCBinding<int>(opcSession, "ECI2.BaseNumber");
                c2ISWNumberBinding = new DAOPCBinding<int>(opcSession, "ECI2.ISWNumber");
                c2DentNumberBinding = new DAOPCBinding<int>(opcSession, "ECI2.DentNumber");
                c2TotalGoodBinding = new DAOPCBinding<int>(opcSession, "ECI2.TotalGoodCount");
                c2TotalBadBinding = new DAOPCBinding<int>(opcSession, "ECI2.TotalBadCount");
                c2TotalThroughputBinding = new DAOPCBinding<int>(opcSession, "ECI2.TotalThroughput");
                c2CurrentRecipeBinding = new DAOPCBinding<string>(opcSession, "ECI2.CurrentRecipeName");

                // Subscribe to Value.PropertyChanged events
                c1FinishNumberBinding.Value.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(DAComplexVariable<int>.CurrentValue))
                    {
                        UpdateUI(() =>
                        {
                            Debug.WriteLine($"C1 Finish:  {c1FinishNumberBinding.Value.CurrentValue}");
                            // Update your UI control here
                        });
                    }
                };

                c1TotalGoodBinding.Value.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(DAComplexVariable<int>.CurrentValue))
                    {
                        UpdateUI(() =>
                        {
                            Debug.WriteLine($"C1 Total Good: {c1TotalGoodBinding.Value.CurrentValue}");
                        });
                    }
                };

                c1TotalBadBinding.Value.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(DAComplexVariable<int>.CurrentValue))
                    {
                        UpdateUI(() =>
                        {
                            Debug.WriteLine($"C1 Total Bad: {c1TotalBadBinding.Value.CurrentValue}");
                        });
                    }
                };

                c1TotalThroughputBinding.Value.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(DAComplexVariable<int>.CurrentValue))
                    {
                        UpdateUI(() =>
                        {
                            Debug.WriteLine($"C1 Throughput: {c1TotalThroughputBinding.Value.CurrentValue}");
                        });
                    }
                };

                c2BaseNumberBinding.Value.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(DAComplexVariable<int>.CurrentValue))
                    {
                        UpdateUI(() =>
                        {
                            Debug.WriteLine($"C2 Base:  {c2BaseNumberBinding.Value.CurrentValue}");
                        });
                    }
                };

                c2ISWNumberBinding.Value.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(DAComplexVariable<int>.CurrentValue))
                    {
                        UpdateUI(() =>
                        {
                            Debug.WriteLine($"C2 ISW: {c2ISWNumberBinding.Value.CurrentValue}");
                        });
                    }
                };

                c2DentNumberBinding.Value.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(DAComplexVariable<int>.CurrentValue))
                    {
                        UpdateUI(() =>
                        {
                            Debug.WriteLine($"C2 Dent: {c2DentNumberBinding.Value.CurrentValue}");
                        });
                    }
                };

                c2TotalGoodBinding.Value.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(DAComplexVariable<int>.CurrentValue))
                    {
                        UpdateUI(() =>
                        {
                            Debug.WriteLine($"C2 Total Good: {c2TotalGoodBinding.Value.CurrentValue}");
                        });
                    }
                };

                c2TotalBadBinding.Value.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(DAComplexVariable<int>.CurrentValue))
                    {
                        UpdateUI(() =>
                        {
                            Debug.WriteLine($"C2 Total Bad: {c2TotalBadBinding.Value.CurrentValue}");
                        });
                    }
                };

                c2TotalThroughputBinding.Value.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(DAComplexVariable<int>.CurrentValue))
                    {
                        UpdateUI(() =>
                        {
                            Debug.WriteLine($"C2 Throughput:  {c2TotalThroughputBinding.Value.CurrentValue}");
                        });
                    }
                };

                c1CurrentRecipeBinding.Value.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(DAComplexVariable<string>.CurrentValue))
                    {
                        UpdateUI(() =>
                        {
                            Debug.WriteLine($"C1 Recipe: {c1CurrentRecipeBinding.Value.CurrentValue}");
                            // Update lblCurrentFlavour or similar
                        });
                    }
                };

                c2CurrentRecipeBinding.Value.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(DAComplexVariable<string>.CurrentValue))
                    {
                        UpdateUI(() =>
                        {
                            Debug.WriteLine($"C2 Recipe: {c2CurrentRecipeBinding.Value.CurrentValue}");
                        });
                    }
                };

                Debug.WriteLine("OPC UA bindings initialized successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing OPC bindings: {ex.Message}");
            }
        }

        private void UpdateUI(Action action)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }
        private void OnCardPresented(string cardId)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => OnCardPresented(cardId)));
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(cardId)) return;
                string normalized = cardId.Trim();

                var username = _usersManager?.GetUserByCardId(normalized);
                if (!string.IsNullOrEmpty(username))
                {
                    // locate user object loaded from config.json
                    var userObj = users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
                    if (userObj != null)
                    {
                        LogInUser(userObj);
                        ShowFaultMessage($"Card login: {userObj.Username}", System.Drawing.Color.Green);
                        return;
                    }
                }

                // Unknown card
                ShowFaultMessage($"Unknown card: {normalized}", System.Drawing.Color.Orange);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OnCardPresented exception: " + ex.Message);
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                if (_cardHandler != null) CardReaderService.Instance.CardPresented -= _cardHandler;
                CardReaderService.Instance.Stop();
            }
            catch { }
            base.OnFormClosing(e);
        }
        private void BufferAndInsertCameraData(int cameraIndex)
        {
            lock (bufferLock)
            {
                try
                {
                    var src = latestCameraData[cameraIndex];
                    if (src == null)
                    {
                        Debug.WriteLine($"BufferAndInsertCameraData: latestCameraData[{cameraIndex}] is null; skipping.");
                        return;
                    }

                    // Snapshot current camera data (prevent later mutation)
                    var snapshot = new CameraData
                    {
                        CameraNumber = src.CameraNumber,
                        Finish = src.Finish,
                        Base = src.Base,
                        ISW = src.ISW,
                        Dent = src.Dent,
                        TotalGood = src.TotalGood,
                        TotalBad = src.TotalBad,
                        TotalThroughput = src.TotalThroughput,
                        DownCan = string.IsNullOrEmpty(src.DownCan) ? (string.IsNullOrEmpty(txtDownCan?.Text) ? "0" : txtDownCan.Text) : src.DownCan
                    };

                    // (optional) keep in buffer array if other code uses it
                    if (bufferedData != null && cameraIndex < bufferedData.Length)
                    {
                        bufferedData[cameraIndex] = snapshot;
                        bufferedTimestamps[cameraIndex] = DateTime.UtcNow;
                    }

                    Debug.WriteLine($"Buffered camera[{cameraIndex}] snapshot at {DateTime.UtcNow:O} - Finish={snapshot.Finish}, Base={snapshot.Base}, ISW={snapshot.ISW}, Dent={snapshot.Dent}, Total={snapshot.TotalThroughput}, DownCan={snapshot.DownCan}");

                    // Insert this camera's snapshot immediately
                    try
                    {
                        InsertCameraDataToDB(cameraIndex, snapshot, DateTime.UtcNow);
                        Debug.WriteLine($"Insert triggered for camera[{cameraIndex}]");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error inserting camera[{cameraIndex}] snapshot: {ex.Message}");
                        Debug.WriteLine(ex.StackTrace);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("BufferAndInsertCameraData exception: " + ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                }
            }
        }
        public static void CreateQuarterlyDatabase(string dbPath)
        {
            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();

                // CameraResults table (UPDATED: removed BottleSize/BottleFlavour, added Recipe)
                var sb = new StringBuilder();
                sb.AppendLine("CREATE TABLE IF NOT EXISTS [CameraResults] (");
                sb.AppendLine("  Id INTEGER PRIMARY KEY AUTOINCREMENT,");
                sb.AppendLine("  CameraNumber INTEGER,");
                sb.AppendLine("  Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,");
                sb.AppendLine("  Finish TEXT,");
                sb.AppendLine("  Base TEXT,");
                sb.AppendLine("  ISW TEXT,");
                sb.AppendLine("  Dent TEXT,"); // <-- NEW FIELD
                sb.AppendLine("  DownCan TEXT,"); // <-- NEW FIELD
                sb.AppendLine("  TotalGood TEXT,");
                sb.AppendLine("  TotalBad TEXT,");
                sb.AppendLine("  TotalThroughput TEXT,");
                sb.AppendLine("  UserName TEXT,");
                sb.AppendLine("  Recipe TEXT");
                sb.AppendLine(");"); using (var cmd = new SQLiteCommand(sb.ToString(), connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // CurrentFlavour table (unchanged)
                using (var cmd = new SQLiteCommand(
                    "CREATE TABLE IF NOT EXISTS [CurrentFlavour] (" +
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "FlavourSize TEXT, FlavourName TEXT, LastUpdated DATETIME DEFAULT CURRENT_TIMESTAMP);",
                    connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // ResetEvents table (unchanged)
                using (var cmd = new SQLiteCommand(
                    "CREATE TABLE IF NOT EXISTS [ResetEvents] (" +
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "ResetTimestamp DATETIME DEFAULT CURRENT_TIMESTAMP);",
                    connection))
                {
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        private void InsertCameraDataToDB(int cameraIndex, CameraData camData, DateTime timestamp)
        {
            string dbPath = GetQuarterlyDatabasePath(timestamp);
            try
            {
                Debug.WriteLine($"InsertCameraDataToDB: opening DB at {dbPath} for camera {cameraIndex}");

                DatabaseCreator.EnsureCameraResultsTable(dbPath);

                using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandText = @"INSERT INTO CameraResults 
    (CameraNumber, Timestamp, Finish, Base, ISW, Dent, DownCan,
     TotalGood, TotalBad, TotalThroughput, UserName, Recipe)
    VALUES (@CameraNumber, @Timestamp, @Finish, @Base, @ISW, @Dent, @DownCan,
     @TotalGood, @TotalBad, @TotalThroughput, @UserName, @Recipe);";

                        cmd.Parameters.AddWithValue("@CameraNumber", camData.CameraNumber);
                        cmd.Parameters.AddWithValue("@Timestamp", timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        cmd.Parameters.AddWithValue("@Finish", string.IsNullOrEmpty(camData.Finish) ? "0" : camData.Finish);
                        cmd.Parameters.AddWithValue("@Base", string.IsNullOrEmpty(camData.Base) ? "0" : camData.Base);
                        cmd.Parameters.AddWithValue("@ISW", string.IsNullOrEmpty(camData.ISW) ? "0" : camData.ISW);
                        cmd.Parameters.AddWithValue("@Dent", string.IsNullOrEmpty(camData.Dent) ? "0" : camData.Dent);
                        cmd.Parameters.AddWithValue("@TotalGood", string.IsNullOrEmpty(camData.TotalGood) ? "0" : camData.TotalGood);
                        cmd.Parameters.AddWithValue("@TotalBad", string.IsNullOrEmpty(camData.TotalBad) ? "0" : camData.TotalBad);
                        cmd.Parameters.AddWithValue("@TotalThroughput", string.IsNullOrEmpty(camData.TotalThroughput) ? "0" : camData.TotalThroughput);

                        string downCanValue = string.IsNullOrEmpty(camData.DownCan) ? (string.IsNullOrEmpty(txtDownCan?.Text) ? "0" : txtDownCan.Text) : camData.DownCan;
                        cmd.Parameters.AddWithValue("@DownCan", downCanValue);

                        cmd.Parameters.AddWithValue("@UserName", string.IsNullOrEmpty(CurrentUserTxt.Text) ? "" : CurrentUserTxt.Text);
                        cmd.Parameters.AddWithValue("@Recipe", string.IsNullOrEmpty(SelectedRecipe) ? "" : SelectedRecipe);

                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    connection.Close();
                }

                Debug.WriteLine($"Inserted CameraResults row: Camera={camData.CameraNumber}, Timestamp={timestamp:O}, DownCan={camData.DownCan}, Total={camData.TotalThroughput}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"InsertCameraDataToDB error (cameraIndex={cameraIndex}, dbPath={dbPath}): {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                // don't rethrow if you want the app to continue; rethrow if you want to let caller handle it
            }
        }
        private void InsertCameraDataToDB_Internal(SQLiteConnection connection, CameraData camData, DateTime timestamp)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            try
            {
                // Ensure table exists (no-op if already created)
                DatabaseCreator.EnsureCameraResultsTable(connection.ConnectionString.Replace("Data Source=", "").Replace(";Version=3;", ""));

                string sql = @"INSERT INTO CameraResults 
    (CameraNumber, Timestamp, Finish, Base, ISW, Dent, DownCan,
     TotalGood, TotalBad, TotalThroughput, UserName, Recipe)
    VALUES (@CameraNumber, @Timestamp, @Finish, @Base, @ISW, @Dent, @DownCan,
     @TotalGood, @TotalBad, @TotalThroughput, @UserName, @Recipe);";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@CameraNumber", camData.CameraNumber);
                    cmd.Parameters.AddWithValue("@Timestamp", timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    cmd.Parameters.AddWithValue("@Finish", string.IsNullOrEmpty(camData.Finish) ? "0" : camData.Finish);
                    cmd.Parameters.AddWithValue("@Base", string.IsNullOrEmpty(camData.Base) ? "0" : camData.Base);
                    cmd.Parameters.AddWithValue("@ISW", string.IsNullOrEmpty(camData.ISW) ? "0" : camData.ISW);
                    cmd.Parameters.AddWithValue("@Dent", string.IsNullOrEmpty(camData.Dent) ? "0" : camData.Dent);
                    cmd.Parameters.AddWithValue("@TotalGood", string.IsNullOrEmpty(camData.TotalGood) ? "0" : camData.TotalGood);
                    cmd.Parameters.AddWithValue("@TotalBad", string.IsNullOrEmpty(camData.TotalBad) ? "0" : camData.TotalBad);
                    cmd.Parameters.AddWithValue("@TotalThroughput", string.IsNullOrEmpty(camData.TotalThroughput) ? "0" : camData.TotalThroughput);

                    // Use downcan from snapshot
                    string downCanValue = string.IsNullOrEmpty(camData.DownCan) ? (string.IsNullOrEmpty(txtDownCan?.Text) ? "0" : txtDownCan.Text) : camData.DownCan;
                    cmd.Parameters.AddWithValue("@DownCan", downCanValue);

                    cmd.Parameters.AddWithValue("@UserName", string.IsNullOrEmpty(CurrentUserTxt.Text) ? "" : CurrentUserTxt.Text);
                    cmd.Parameters.AddWithValue("@Recipe", string.IsNullOrEmpty(SelectedRecipe) ? "" : SelectedRecipe);

                    cmd.ExecuteNonQuery();
                }

                Debug.WriteLine($"Inserted CameraResults row: Camera={camData.CameraNumber}, Timestamp={timestamp:O}, DownCan={camData.DownCan}, Total={camData.TotalThroughput}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("InsertCameraDataToDB_Internal error: " + ex.Message);
                Debug.WriteLine(ex.StackTrace);
                throw;
            }
        }
        private async void StartDisplays(SplashScreen splashScreen)
        {
            try { splashScreen?.UpdateProgress("Displays Starting"); } catch { }

            try
            {
                if (ResetCounterBtn1 != null)
                {
                    await ResetCounterBtn1.ConnectAsync();
                }
            }
            catch (Exception ex)
            {
                try { splashScreen?.UpdateProgress("ResetCounterBtn1 connect failed: " + ex.Message); } catch { }
                Debug.WriteLine("ResetCounterBtn1.ConnectAsync failed: " + ex.Message);
            }

            try
            {
                if (ResetCounterBtn2 != null)
                {
                    await ResetCounterBtn2.ConnectAsync();
                }
            }
            catch (Exception ex)
            {
                try { splashScreen?.UpdateProgress("ResetCounterBtn2 connect failed: " + ex.Message); } catch { }
                Debug.WriteLine("ResetCounterBtn2.ConnectAsync failed: " + ex.Message);
            }

            try { splashScreen?.UpdateProgress("Displays Started Please wait 2 minutes"); } catch { }
        }
        private void ConnectToAllTextboxes(SplashScreen splashScreen)
        {
            // CAMERA 1
            CameraPage1.ValueElements["FinishNumber"].ValueChanged += (sender, args) =>
            {
                string newValue = CameraPage1.ValueElements["FinishNumber"].Value?.ToString();
                if (FinishNumber.InvokeRequired)
                {
                    FinishNumber.Invoke(new Action(() => FinishNumber.Text = newValue));
                }
                else
                {
                    FinishNumber.Text = newValue;
                }
                UpdateCameraAndInsertIfComplete(0, CameraPage1);
                var val = args.NewValue?.ToString();
                if (!string.IsNullOrEmpty(val) && int.TryParse(val, out int v) && v > 0)
                    ShowFaultMessage("Finish Failed", Color.Red);

            };
            CameraPage2.ValueElements["BaseNumber"].ValueChanged += (sender, args) =>
            {
                string newValue = CameraPage2.ValueElements["BaseNumber"].Value?.ToString();
                if (BaseNumber.InvokeRequired)
                {
                    BaseNumber.Invoke(new Action(() => BaseNumber.Text = newValue));
                }
                else
                {
                    BaseNumber.Text = newValue;
                }
                UpdateCameraAndInsertIfComplete(1, CameraPage2);
                var val = args.NewValue?.ToString();
                if (!string.IsNullOrEmpty(val) && int.TryParse(val, out int v) && v > 0)
                    ShowFaultMessage("Base Failed", Color.Red);

            };

            CameraPage2.ValueElements["ISWNumber"].ValueChanged += (sender, args) =>
            {
                string newValue = CameraPage2.ValueElements["ISWNumber"].Value?.ToString();
                if (ISWNumber.InvokeRequired)
                {
                    ISWNumber.Invoke(new Action(() => ISWNumber.Text = newValue));
                }
                else
                {
                    ISWNumber.Text = newValue;
                }
                UpdateCameraAndInsertIfComplete(1, CameraPage2);
                var val = args.NewValue?.ToString();
                if (!string.IsNullOrEmpty(val) && int.TryParse(val, out int v) && v > 0)
                    ShowFaultMessage("ISW Failed", Color.Red);

            };

            CameraPage2.ValueElements["DentNumber"].ValueChanged += (sender, args) =>
            {
                string newValue = CameraPage2.ValueElements["DentNumber"].Value?.ToString();
                if (DentNumber.InvokeRequired)
                {
                    DentNumber.Invoke(new Action(() => DentNumber.Text = newValue));
                }
                else
                {
                    DentNumber.Text = newValue;
                }
                UpdateCameraAndInsertIfComplete(1, CameraPage2);
                var val = args.NewValue?.ToString();
                if (!string.IsNullOrEmpty(val) && int.TryParse(val, out int v) && v > 0)
                    ShowFaultMessage("Dent Failed", Color.Red);


            };
        }

        private void UpdateCameraAndInsertIfComplete(int cameraIndex, dynamic cameraPage)
        {
            //Debug.WriteLine($"UpdateCameraAndInsertIfComplete called for camera {cameraIndex}", "Debug");

            // Camera 1 (ECI1) fields
            if (cameraIndex == 0)
            {
                try { latestCameraData[cameraIndex].Finish = cameraPage.ValueElements["FinishNumber"].Value?.ToString(); }
                catch { latestCameraData[cameraIndex].Finish = null; }
            }
            // Camera 2 (ECI2) fields
            else if (cameraIndex == 1)
            {
                try { latestCameraData[cameraIndex].Base = cameraPage.ValueElements["BaseNumber"].Value?.ToString(); }
                catch { latestCameraData[cameraIndex].Base = null; }
                try { latestCameraData[cameraIndex].ISW = cameraPage.ValueElements["ISWNumber"].Value?.ToString(); }
                catch { latestCameraData[cameraIndex].ISW = null; }
                try { latestCameraData[cameraIndex].Dent = cameraPage.ValueElements["DentNumber"].Value?.ToString(); }
                catch { latestCameraData[cameraIndex].Dent = null; }

            }

            // Common fields for both cameras
            try { latestCameraData[cameraIndex].TotalGood = cameraPage.ValueElements["TotalGoodCount"].Value?.ToString(); }
            catch { latestCameraData[cameraIndex].TotalGood = null; }
            try { latestCameraData[cameraIndex].TotalBad = cameraPage.ValueElements["TotalBadCount"].Value?.ToString(); }
            catch { latestCameraData[cameraIndex].TotalBad = null; }
            try { latestCameraData[cameraIndex].TotalThroughput = cameraPage.ValueElements["TotalThroughput"].Value?.ToString(); }
            catch { latestCameraData[cameraIndex].TotalThroughput = null; }

            latestCameraData[cameraIndex].CameraNumber = cameraIndex + 1;

            // Only insert when required fields for the camera are present
            if (latestCameraData[cameraIndex].IsCompleteForIndex(cameraIndex))
            {
                BufferAndInsertCameraData(cameraIndex);
            }
            UpdateCameraDisplayBySelectedCamera();
        }
        private void UpdateCameraDisplayBySelectedCamera()
        {
            // Use any control for the check, here TotalCountValue
            if (TotalCountValue.InvokeRequired)
            {
                TotalCountValue.Invoke(new Action(UpdateCameraDisplayBySelectedCamera));
                return;
            }

            var camData = latestCameraData[selectedCameraIndex];
            if (camData == null) return;

            // Regular values
            TotalCountValue.Text = camData.TotalThroughput ?? "";
            //FinishNumber.Text = camData.Finish ?? "";
            //BaseNumber.Text = camData.Base ?? "";
            //ISWNumber.Text = camData.ISW ?? "";


            // Reject and fail
            TotalFailValue.Text = camData.TotalBad ?? "";
            double totalCount = 0, totalFail = 0;
            double.TryParse(camData.TotalThroughput, out totalCount);
            double.TryParse(camData.TotalBad, out totalFail);
            string percentageText = "0 %";
            if (totalCount > 0)
            {
                double percent = (totalFail / totalCount) * 100.0;
                percentageText = percent.ToString("0.##") + " %";
            }
            RejectPercentage.Text = percentageText;
        }
        private double GetValidData(string newValue)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                // Console.WriteLine("Received null or empty value.");
                return 0; // Default value for null or empty input
            }

            try
            {
                return Convert.ToDouble(newValue);
            }
            catch (FormatException)
            {
                //  Console.WriteLine("Value conversion error. Received invalid format.");
                return 0; // Default value for invalid format
            }
        }
        private void C1CurrentRecipeName(object sender, ValueChangedEventArgs args)
        {
            try
            {
                string newValue = (args.NewValue)?.ToString() ?? "";
                if (C1RecipeNameLb.InvokeRequired)
                    C1RecipeNameLb.Invoke(new Action(() => C1RecipeNameLb.Text = newValue));
                else
                    C1RecipeNameLb.Text = newValue;

                // persist and notify listeners
                try
                {
                    SaveCurrentFlavourToDb(currentSize ?? "", newValue);
                    OnFlavourChanged();
                    Debug.WriteLine($"C1CurrentRecipeName: saved flavour '{newValue}' and raised FlavourChanged.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("C1CurrentRecipeName: error saving flavour: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating RecipeNameLb: {ex.Message}");
            }
        }
        private void C2CurrentRecipeName(object sender, ValueChangedEventArgs args)
        {
            try
            {
                string newValue = (args.NewValue)?.ToString() ?? "";
                if (C2RecipeNameLb.InvokeRequired)
                    C2RecipeNameLb.Invoke(new Action(() => C2RecipeNameLb.Text = newValue));
                else
                    C2RecipeNameLb.Text = newValue;

                // persist and notify listeners
                try
                {
                    SaveCurrentFlavourToDb(currentSize ?? "", newValue);
                    OnFlavourChanged();
                    Debug.WriteLine($"C2CurrentRecipeName: saved flavour '{newValue}' and raised FlavourChanged.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("C2CurrentRecipeName: error saving flavour: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Program.splashForm.UpdateProgress($"Error updating RecipeNameLb: {ex.Message}");
            }
        }
        private void DateTimeTimer_Tick(object sender, EventArgs e)
        {
            string time = DateTime.Now.ToString("g");
            DateTimeLabel.Text = time;
        }
        private void MainMenuFormBtn_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            using (var mainMenu = new MainMenu(this))
            {
                mainMenu.ShowDialog();
            }
        }
        private void AllCameraDisplay_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            using (var AllCameraDisplay = new AllCameraDisplay(this))
            {
                AllCameraDisplay.ShowDialog();
            }

        }
        public class Configuration
        {
            public List<User> Users { get; set; }
            public Dictionary<string, Dictionary<string, int>> Buttons { get; set; }
        }
        public class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public int SecurityLevel { get; set; }
        }
        private void LoadConfig(SplashScreen splashscreen)
        {
            try
            {
                splashscreen.UpdateProgress("Loading configuration...");
                var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
                var json = File.ReadAllText(configFilePath);
                var config = JsonConvert.DeserializeObject<Configuration>(json);
                users = config.Users;
                buttonPermissions = config.Buttons;

                // Log loaded permissions
                foreach (var formButtons in buttonPermissions)
                {
                    foreach (var btn in formButtons.Value)
                    {
                        //Console.WriteLine($"Loaded Button: {btn.Key}, Required Level: {btn.Value}");
                    }
                }

                splashscreen.UpdateProgress("Configuration loaded.");
            }
            catch (Exception ex)
            {
                splashscreen.UpdateProgress($"Error loading config: {ex.Message}");
                MessageBox.Show($"Error loading config: {ex.Message}");
            }
        }
        public void SetButtonVisibilityForCurrentUser(SplashScreen splashscreen = null)
        {
            if (splashscreen != null)
                splashscreen.UpdateProgress("Setting button visibility...");

            // Update MainForm buttons
            foreach (var formButtons in buttonPermissions)
            {
                foreach (var btn in formButtons.Value)
                {
                    var buttonName = btn.Key;
                    var requiredLevel = btn.Value;
                    var button = this.Controls.Find(buttonName, true).FirstOrDefault() as Button;
                    if (button != null)
                    {
                        bool isVisible = nUserAccess >= requiredLevel;
                        button.Visible = isVisible;
                        //Console.WriteLine($"MainForm Button: {buttonName}, Required Level: {requiredLevel}, Current Level: {nUserAccess}, Visible: {isVisible}");
                    }
                }
            }

            // Update MainMenu buttons
            foreach (var btn in buttonPermissions["MainMenu"])
            {
                var buttonName = btn.Key;
                var requiredLevel = btn.Value;
                var button = Controls.Find(buttonName, true).FirstOrDefault() as Button;
                if (button != null)
                {
                    bool isVisible = nUserAccess >= requiredLevel;
                    button.Visible = isVisible;
                    //Console.WriteLine($"MainMenu Button: {buttonName}, Required Level: {requiredLevel}, Current Level: {nUserAccess}, Visible: {isVisible}");
                }
            }

            if (splashscreen != null)
                splashscreen.UpdateProgress("Button visibility set.");
        }
        private void InitializeInactivityTimer(SplashScreen splashscreen)
        {
            splashscreen.UpdateProgress("Initializing inactivity timer...");
            inactivityTimer = new Timer
            {
                Interval = InactivityLimit
            };
            inactivityTimer.Tick += InactivityTimer_Tick;
            inactivityTimer.Start();
            splashscreen.UpdateProgress("Inactivity timer initialized.");
        }
        private void InactivityTimer_Tick(object sender, EventArgs e)
        {
            LogOutToOperatingUser();
        }
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            ResetInactivityTimer();
        }
        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            ResetInactivityTimer();
        }
        public void ResetInactivityTimer()
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }
        public void LogInUser(User user)
        {
            currentUserName = user.Username; // Set the current user’s name
            nUserAccess = user.SecurityLevel;
            IsUserLoggedIn = true; // Set login state to true
            UpdateCurrentUserText(); // Update the username text box
            SetButtonVisibilityForCurrentUser(); // Update button visibility

            // Notify listeners (e.g. MainMenu) so they can refresh immediately
            OnUserStateChanged();
        }

        public void LogOutToOperatingUser()
        {
            currentUserName = "Operate"; // Set the current user’s name to "Operate"
            nUserAccess = 1; // Assuming 1 is the security level for "Operate"
            IsUserLoggedIn = false; // Set login state to false
            UpdateCurrentUserText();
            SetButtonVisibilityForCurrentUser();

            // Notify listeners (e.g. MainMenu) so they can refresh immediately
            OnUserStateChanged();

            //MessageBox.Show("You have been logged out due to inactivity.");
        }
        private void UpdateCurrentUserText()
        {
            CurrentUserTxt.Text = currentUserName; // Update the CurrentUserTxt text box
        }
        public void AuthenticateUser(string username, string password)
        {
            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                LogInUser(user); // Set user context and permissions
                MessageBox.Show("Login successful!");
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }
        private async void ResetCounterBtn1_Click(object sender, EventArgs e)
        {
            try
            {
                // Perform existing button click actions
                await ResetCounterBtn1.DoClick();
                await ResetCounterBtn2.DoClick();

                string dbPath = GetQuarterlyDatabasePath(DateTime.Now); // or use the record's timestamp if inserting/querying for a specific time
                DatabaseCreator.EnsureCameraResultsTable(dbPath);
                using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    connection.Open();
                    using (var cmd = new SQLiteCommand("INSERT INTO ResetEvents (ResetTimestamp) VALUES (CURRENT_TIMESTAMP);", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while clicking the reset button: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                S7Client s7Client = new S7Client();
                int result = s7Client.ConnectTo("192.168.0.21", 0, 1); // PLC IP/rack/slot
                if (result == 0)
                {
                    // Zero for DINT
                    byte[] zeroBuf = new byte[4];
                    S7.SetDIntAt(zeroBuf, 0, 0);

                    // Write zero to throughput
                    int write1 = s7Client.DBWrite(4, 60, 4, zeroBuf); // Adjust DB/offset
                                                                       // Write zero to rejects
                    int write2 = s7Client.DBWrite(4, 20, 4, zeroBuf); // Adjust DB/offset
                    int write3 = s7Client.DBWrite(4,0,4, zeroBuf);
                    int write4 = s7Client.DBWrite(4,4,4, zeroBuf);
                    int write5 = s7Client.DBWrite(4,24, 4, zeroBuf);
                    int write6 = s7Client.DBWrite(4,28,4, zeroBuf);
                    int write7 = s7Client.DBWrite(4,56,4, zeroBuf);
                    int write8 = s7Client.DBWrite(4, 64, 4, zeroBuf);

                    s7Client.Disconnect();

                    // Optionally check write1/write2 for errors and notify the user
                }
                else
                {
                    MessageBox.Show("PLC connection failed for counter reset.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("PLC reset error: " + ex.Message);
            }
        }
        private DateTime? GetLastResetTimestamp()
        {
            string dbPath = GetQuarterlyDatabasePath(DateTime.Now); // or use the record's timestamp if inserting/querying for a specific time
            DatabaseCreator.EnsureCameraResultsTable(dbPath);
            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand("SELECT ResetTimestamp FROM ResetEvents ORDER BY ResetTimestamp DESC LIMIT 1;", connection))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null && DateTime.TryParse(result.ToString(), out var dt))
                        return dt;
                }
                connection.Close();
            }
            return null;
        }
        private void ScrollProjectDisplay(string projectName)
        {
            var displays = projectDisplays[projectName];
            int idx = currentDisplayIndexes[projectName];

            // Show display at current index
            ShowDisplay(projectName, displays[idx]);

            // Move to next index, wrapping if needed
            idx = (idx + 1) % displays.Count;
            currentDisplayIndexes[projectName] = idx;
        }
        private async Task CreateAllDisplayControls()
        {
            foreach (var project in projectNames)
            {
                foreach (var display in projectDisplays[project])
                {
                    var ctrl = new DisplayControl
                    {
                        PageName = OPERATOR_VIEW_NAME,
                        ProjectName = project,
                        DisplayName = display,
                        HostName = HOST_NAME,
                        Dock = DockStyle.Fill,
                        Visible = false // Start hidden
                    };

                   await ctrl.ConnectAsync(); 
                    panelControl.Controls.Add(ctrl); 
                    displayControls[$"{project}_{display}"] = ctrl;
                }
            }
        }
        public void ShowDisplay(string project, string display)
        {
            string key = $"{project}_{display}";
            if (!displayControls.ContainsKey(key))
                return;

            if (currentDisplayControl != null)
                currentDisplayControl.Visible = false;

            currentDisplayControl = displayControls[key];
            currentDisplayControl.Visible = true;
            currentDisplayControl.BringToFront();
        }
        private void Cam1Scroll_Click(object sender, EventArgs e)
        {
            selectedCameraIndex = 0; // Camera 1
            UpdateCameraDisplayBySelectedCamera();

            var displays = projectDisplays["ECI1"];

            // Advance the index to the next display FIRST
            int idx = (currentDisplayIndexes["ECI1"] + 1) % displays.Count;
            currentDisplayIndexes["ECI1"] = idx;

            // Show the display for the new index
            ShowDisplay("ECI1", displays[idx]);

            // Update dots panel for camera 1 (with the new index!)
            DrawDisplayDots(panelCam1Dots, displays, idx);
        }
        private void Cam2Scroll_Click(object sender, EventArgs e)
        {
            selectedCameraIndex = 1; // Camera 2
            UpdateCameraDisplayBySelectedCamera();

            var displays = projectDisplays["ECI2"];

            // Advance index first
            int idx = (currentDisplayIndexes["ECI2"] + 1) % displays.Count;
            currentDisplayIndexes["ECI2"] = idx;

            // Show the display for the new index
            ShowDisplay("ECI2", displays[idx]);

            // Update dots panel for camera 2
            DrawDisplayDots(panelCam2Dots, displays, idx);
        }
        public static class FlavourIniReader
        {
            // List your actual bottle sizes here, or use a pattern if you have many
            private static readonly HashSet<string> FlavourSections = new HashSet<string>
    {
        "355mL", "473mL", "355mLSleek"
        // Add more sizes as needed
    };

            public static Dictionary<string, List<string>> ReadFlavours(string iniPath)
            {
                var dict = new Dictionary<string, List<string>>();
                string currentSection = null;

                foreach (var line in File.ReadAllLines(iniPath))
                {
                    var trimmed = line.Trim();
                    if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(";"))
                        continue;

                    // Detect section
                    if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                    {
                        var section = trimmed.Substring(1, trimmed.Length - 2);
                        currentSection = FlavourSections.Contains(section) ? section : null;
                        if (currentSection != null && !dict.ContainsKey(currentSection))
                            dict[currentSection] = new List<string>();
                        continue;
                    }

                    // Only add flavours to flavour sections
                    if (currentSection != null && trimmed.Contains("=") && !trimmed.StartsWith("["))
                    {
                        var parts = trimmed.Split(new[] { '=' }, 2);
                        if (parts.Length == 2)
                            dict[currentSection].Add(parts[1].Trim());
                    }
                }

                // Remove any sections that ended up empty
                var keysToRemove = new List<string>();
                foreach (var kv in dict)
                {
                    if (kv.Value.Count == 0)
                        keysToRemove.Add(kv.Key);
                }
                foreach (var k in keysToRemove)
                    dict.Remove(k);

                return dict;
            }
        }
        // MainForm.cs — replace existing BtnSelectFlavour_Click
        private void BtnSelectFlavour_Click(object sender, EventArgs e)
        {
            // Use hardcoded recipe names if you can't fetch Options list
            var recipes = new List<string> { "355mL", "473mL", "355mLSleek" };

            Form recipeDialog = new Form
            {
                Text = "Select Recipe",
                Size = new Size(300, 180),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent
            };

            Button[] recipeButtons = new Button[recipes.Count];
            string selectedRecipe = null;
            for (int i = 0; i < recipes.Count; i++)
            {
                recipeButtons[i] = new Button
                {
                    Text = recipes[i],
                    Size = new Size(240, 30),
                    Location = new Point(25, 20 + i * 40)
                };
                int idx = i;
                recipeButtons[i].Click += (s, evt) =>
                {
                    selectedRecipe = recipes[idx];
                    recipeDialog.DialogResult = DialogResult.OK;
                    recipeDialog.Close();
                };
                recipeDialog.Controls.Add(recipeButtons[i]);
            }

            var result = recipeDialog.ShowDialog(this);
            if (result != DialogResult.OK || string.IsNullOrEmpty(selectedRecipe))
                return;

            var confirm = MessageBox.Show($"Select recipe \"{selectedRecipe}\"", "Confirm Recipe Change", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            var recipeListBox1 = CameraPage1.ListBoxElements["RecipeListbox"];
            var recipeListBox2 = CameraPage2.ListBoxElements["RecipeListbox"];

            recipeListBox1.Value = new List<string> { selectedRecipe };
            recipeListBox2.Value = new List<string> { selectedRecipe };

            // update label and save to DB so Data.LoadRecentFlavours can read it
            lblCurrentFlavour.Text = $"{selectedRecipe}";

            try
            {
                // Save current flavour to DB (size may be in currentSize; if not available pass empty)
                SaveCurrentFlavourToDb(currentSize ?? "", selectedRecipe);
                // Raise event so Data and other subscribers update their UI
                OnFlavourChanged();
                Debug.WriteLine($"BtnSelectFlavour_Click: saved flavour '{selectedRecipe}' (size='{currentSize}') and raised FlavourChanged.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("BtnSelectFlavour_Click: error saving flavour: " + ex.Message);
            }
        }
        public void ArchiveCameraSettingsBeforeRecipeSwitch(string oldFlavour, string oldSize)
        {
            // 1. Determine paths
            string appBasePath = AppDomain.CurrentDomain.BaseDirectory;
            string archiveRoot = Path.Combine(appBasePath, "ArchivedRecipeValues");
            string flavour = oldFlavour ?? "UnknownFlavour";
            string size = oldSize ?? "UnknownSize";
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string folderName = $"{flavour}_{size}_{timestamp}";
            string destinationFolder = Path.Combine(archiveRoot, folderName);

            string[] cameraFiles = {
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "images", "Camera_1_Settings.txt"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "images", "Camera_2_Settings.txt"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "images", "Camera_3_Settings.txt")
    };

            // 2. Check all files exist
            if (!cameraFiles.All(File.Exists))
            {
                MessageBox.Show(
                    "Not all camera setting files are present. Please Try Again",
                    "Missing Camera Files",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // 3. Create destination folder
            Directory.CreateDirectory(destinationFolder);

            // 4. Move and rename files with inline retry logic
            for (int i = 0; i < cameraFiles.Length; i++)
            {
                string source = cameraFiles[i];
                string destName = $"{flavour}_{size}_{timestamp}_Camera{i + 1}.txt";
                string destPath = Path.Combine(destinationFolder, destName);

                const int retries = 5;
                const int delayMs = 200;
                bool moved = false;
                for (int attempt = 0; attempt < retries; attempt++)
                {
                    try
                    {
                        File.Move(source, destPath);
                        moved = true;
                        break;
                    }
                    catch (IOException)
                    {
                        if (attempt == retries - 1)
                            throw;
                        System.Threading.Thread.Sleep(delayMs);
                    }
                }
            }

            // Optionally: log or show a message that archive completed
            // e.g., Debug.WriteLine($"Archived camera settings to {destinationFolder}");
        }
        private void SaveCurrentFlavourToDb(string size, string flavour)
        {
            string dbPath = GetQuarterlyDatabasePath(DateTime.Now); // or use the record's timestamp if inserting/querying for a specific time
            DatabaseCreator.EnsureCameraResultsTable(dbPath);
            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();
                // Remove previous so only 1 row is stored (optional)
                using (var delCmd = new SQLiteCommand("DELETE FROM CurrentFlavour", connection))
                {
                    delCmd.ExecuteNonQuery();
                }
                using (var cmd = new SQLiteCommand("INSERT INTO CurrentFlavour (FlavourSize, FlavourName, LastUpdated) VALUES (@size, @flavour, CURRENT_TIMESTAMP)", connection))
                {
                    cmd.Parameters.AddWithValue("@size", size);
                    cmd.Parameters.AddWithValue("@flavour", flavour);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        private void LoadCurrentFlavourFromDb()
        {
            string dbPath = GetQuarterlyDatabasePath(DateTime.Now); // or use the record's timestamp if inserting/querying for a specific time
            DatabaseCreator.EnsureCameraResultsTable(dbPath);
            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand("SELECT FlavourSize, FlavourName FROM CurrentFlavour ORDER BY LastUpdated DESC LIMIT 1", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currentSize = reader["FlavourSize"]?.ToString();
                        currentFlavour = reader["FlavourName"]?.ToString();
                        lblCurrentFlavour.Text = $"Current Flavour: {currentSize} - {currentFlavour}";
                    }
                }
                connection.Close();
            }
        }
        public static string[] GetBottleSizesFromIni()
        {
            var iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.ini");
            var bottleSizes = new List<string>();
            string section = "[MotorPositions]";
            bool inSection = false;

            foreach (var line in File.ReadAllLines(iniPath))
            {
                var trimmed = line.Trim();
                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    inSection = trimmed.Equals(section, StringComparison.OrdinalIgnoreCase);
                    continue;
                }
                if (inSection && trimmed.Contains("="))
                {
                    var parts = trimmed.Split(new[] { '=' }, 2);
                    var size = parts[0].Trim();
                    if (!bottleSizes.Contains(size))
                        bottleSizes.Add(size);
                }
                if (inSection && trimmed.StartsWith("[") && !trimmed.Equals(section, StringComparison.OrdinalIgnoreCase))
                    break;
            }
            return bottleSizes.ToArray();
        }
        private static void EnsureIndexes(DateTime date)
        {
            string dbPath = GetQuarterlyDatabasePath(date);
            DatabaseCreator.EnsureCameraResultsTable(dbPath);
            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_cameraresults_timestamp ON CameraResults(Timestamp);";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_cameraresults_bottleflavour_bottlesize ON CameraResults(BottleFlavour, BottleSize);";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_cameraresults_time_flavour_size ON CameraResults(Timestamp, BottleFlavour, BottleSize);";
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public static string GetQuarterlyDatabasePath(DateTime date)
        {
            int year = date.Year;
            int quarter = ((date.Month - 1) / 3) + 1;
            string dbFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DatabaseDBs");
            if (!Directory.Exists(dbFolder))
                Directory.CreateDirectory(dbFolder);
            string dbFile = $"InspectionResults_{year}_Q{quarter}.db";
            return Path.Combine(dbFolder, dbFile);
        }

        // Returns all db paths covering a date range (start to end, inclusive)
        public static List<string> GetQuarterlyDbPathsInRange(DateTime start, DateTime end)
        {
            var result = new List<string>();
            DateTime current = new DateTime(start.Year, ((start.Month - 1) / 3) * 3 + 1, 1);
            DateTime final = new DateTime(end.Year, ((end.Month - 1) / 3) * 3 + 1, 1);
            while (current <= final)
            {
                string dbPath = GetQuarterlyDatabasePath(current);
                if (File.Exists(dbPath)) result.Add(dbPath);
                current = current.AddMonths(3);
            }
            return result;
        }
        // PLC polling event handler
        // ---- replace existing PlcTimer_Tick and PollPlcAndUpdateUIAsync with this ----

        private async void PlcTimer_Tick(object sender, EventArgs e)
        {
            // Guard to ensure timer tick fires and we attempt polling async
          //  Debug.WriteLine($"PlcTimer_Tick fired: {DateTime.Now:O}"); 
            await PollPlcAndUpdateUIAsync();
        }

        private async Task PollPlcAndUpdateUIAsync()
        {
            // Prevent overlapping executions
            if (_plcPollingInProgress)
            {
               // Debug.WriteLine("Poll already running; skipping this tick.");
                return;
            }

            _plcPollingInProgress = true;
            try
            {
                int plcThroughput = 0;
                int plcRejects = 0;
                int plcCPM = 0;
                int plcDownCan = 0;

                // Ensure we have a connected client (done inline)
                if (plcClient == null)
                    plcClient = new S7Client();

                if (!plcClient.Connected)
                {
                    int connectResult = plcClient.ConnectTo("192.168.0.21", 0, 1); // keep same IP/rack/slot
                    if (connectResult != 0)
                    {
                      //  Debug.WriteLine($"PLC ConnectTo failed: {connectResult} - {plcClient.ErrorText(connectResult)}");
                        // Update UI to indicate offline
                        if (this.IsHandleCreated)
                        {
                            this.Invoke((Action)(() =>
                            {
                                txtPlcThroughput.Text = "PLC Offline";
                                txtPlcRejects.Text = "PLC Offline";
                                txtPlcRejectRate.Text = "PLC Offline";
                                txtCPM.Text = "PLC Offline";
                                txtDownCan.Text = "PLC Offline";
                            }));
                        }
                        return;
                    }
                   // Debug.WriteLine("PLC connected successfully.");
                }

                // Do the blocking reads on a background thread to avoid UI thread blocking
                await Task.Run(() =>
                {
                    try
                    {
                        byte[] buffer = new byte[4];

                        // Throughput (DB4, offset 60)
                        int r1 = plcClient.DBRead(4, 60, 4, buffer);
                        if (r1 == 0) plcThroughput = S7.GetDIntAt(buffer, 0);
                       // else Debug.WriteLine($"DBRead throughput failed: {r1} - {plcClient.ErrorText(r1)}");

                        // Rejects (DB4, offset 20)
                        int r2 = plcClient.DBRead(4, 20, 4, buffer);
                        if (r2 == 0) plcRejects = S7.GetDIntAt(buffer, 0);
                       // else Debug.WriteLine($"DBRead rejects failed: {r2} - {plcClient.ErrorText(r2)}");

                        // CPM (DB4, offset 48)
                        int r3 = plcClient.DBRead(4, 48, 4, buffer);
                        if (r3 == 0) plcCPM = S7.GetDIntAt(buffer, 0);
                      //  else Debug.WriteLine($"DBRead CPM failed: {r3} - {plcClient.ErrorText(r3)}");

                        // DownCan (DB4, offset 64)
                        int r4 = plcClient.DBRead(4, 64, 4, buffer);
                        if (r4 == 0) plcDownCan = S7.GetDIntAt(buffer, 0);
                       // else Debug.WriteLine($"DBRead DownCan failed: {r4} - {plcClient.ErrorText(r4)}");
                    }
                    catch (Exception ex)
                    {
                      //  Debug.WriteLine($"Background DBRead exception: {ex}");
                        // If an exception occurs here we let outer catch handle UI update
                        throw;
                    }
                });

                // Update UI on UI thread
                if (this.IsHandleCreated)
                {
                    this.Invoke((Action)(() =>
                    {
                        try
                        {
                            txtPlcThroughput.Text = plcThroughput.ToString();
                            txtPlcRejects.Text = plcRejects.ToString();
                            double rejectRate = (plcThroughput > 0) ? ((double)plcRejects / plcThroughput) * 100.0 : 0.0;
                            txtPlcRejectRate.Text = rejectRate.ToString("0.##") + " %";
                            txtCPM.Text = plcCPM.ToString();
                            txtDownCan.Text = plcDownCan.ToString();
                            HandlePlcDownCanValue(plcDownCan);
                        }
                        catch (Exception ex)
                        {
                           // Debug.WriteLine($"UI update exception: {ex}");
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
              //  Debug.WriteLine("PollPlcAndUpdateUIAsync exception: " + ex);
                if (this.IsHandleCreated)
                {
                    this.Invoke((Action)(() =>
                    {
                        txtPlcThroughput.Text = "PLC Error";
                        txtPlcRejects.Text = "PLC Error";
                        txtPlcRejectRate.Text = "PLC Error";
                        txtCPM.Text = "PLC Error";
                        txtDownCan.Text = "PLC Error";
                    }));
                }

                // If there's a persistent fatal error, try disconnecting so next tick attempts reconnect
                try
                {
                    if (plcClient != null && plcClient.Connected)
                    {
                        plcClient.Disconnect();
                      //  Debug.WriteLine("PLC client disconnected after exception.");
                    }
                }
                catch { /* swallow disconnect exceptions */ }
            }
            finally
            {
                _plcPollingInProgress = false;
            }
        }
        private void DrawDisplayDots(Panel panel, List<string> displays, int currentIndex)
        {
            // constants
            int normalDotSize = 4;
            int selectedDotSize = 8;
            int spacing = 8;
            int topPadding = 2;

            // Clear old dots
            panel.Controls.Clear();

            int totalWidth = 0;
            for (int i = 0; i < displays.Count; i++)
            {
                int size = i == currentIndex ? selectedDotSize : normalDotSize;
                totalWidth += size + spacing;
            }
            totalWidth -= spacing;
            panel.Width = totalWidth;

            int x = 0;

            for (int i = 0; i < displays.Count; i++)
            {
                bool isSelected = i == currentIndex;
                bool isReject = displays[i].EndsWith("LastReject");

                var dot = new Panel();
                int size = isSelected ? selectedDotSize : normalDotSize;
                dot.Width = size;
                dot.Height = size;
                dot.Left = x;
                dot.Top = topPadding;
                dot.BackColor = Color.Transparent;

                dot.Paint += (s, e) =>
                {
                    Color color = isReject ? Color.Red : Color.Blue;
                    var g = e.Graphics;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    var rect = new Rectangle(0, 0, dot.Width - 1, dot.Height - 1);
                    using (var brush = new SolidBrush(color))
                        g.FillEllipse(brush, rect);
                };

                panel.Controls.Add(dot);
                x += size + spacing;
            }
        }
        private void HandlePlcDownCanValue(int plcDownCan)
        {
            lock (_downCanLock)
            {
                // If the counter increased since last poll, show a DownCan fault message.
                if (plcDownCan > _previousDownCan)
                {
                    ShowFaultMessage($"Down Can Detected", Color.Red);
                }

                // Update previous value for next comparison
                _previousDownCan = plcDownCan;
            }
        }
        // Add this helper into your MainForm class (near other helpers)
        private void CloseSplashSafely()
        {
            try
            {
                // Capture reference
                var splash = Program.splashForm;
                if (splash == null) return;

                // Remove global reference immediately so other code sees null and won't try to call it.
                try { Program.splashForm = null; } catch { }

                // If the splash has a valid handle and is not disposed, ask it to close on its UI thread.
                try
                {
                    if (splash.IsHandleCreated && !splash.IsDisposed)
                    {
                        try
                        {
                            splash.BeginInvoke(new Action(() =>
                            {
                                try
                                {
                                    if (splash.IsDisposed || splash.Disposing) return;
                                    try { splash.TopMost = false; } catch { }
                                    try { splash.Hide(); } catch { }
                                    try { splash.Close(); } catch { }
                                    try { splash.Dispose(); } catch { }
                                }
                                catch { }
                            }));
                        }
                        catch (ObjectDisposedException) { /* already disposed - nothing to do */ }
                        catch (InvalidOperationException) { /* handle invalid - ignore */ }
                    }
                    else
                    {
                        // No handle or already disposed: try direct dispose
                        try { if (!splash.IsDisposed && !splash.Disposing) { splash.TopMost = false; splash.Hide(); splash.Close(); splash.Dispose(); } } catch { }
                    }
                }
                catch
                {
                    // final best-effort: nothing else
                }
            }
            catch
            {
                // swallow - this is cleanup
            }
        }
        private async Task EnsureHostProjectRunning(string hostName, string projectName)
    {
        try
        {
            object host = null;

            // 1) Try the usual API: HostManager.GetHost(hostName)
            try
            {
                host = HostManager.GetHost(hostName);
            }
            catch (Exception ex)
            {
                // HostManager.GetHost may throw if host not present; swallow and try reflection fallback below
                Debug.WriteLine($"HostManager.GetHost threw: {ex.Message}");
                host = null;
            }

            // 2) If HostManager didn't return a host, try to find a factory method on HostManager
            if (host == null)
            {
                var hmType = typeof(HostManager);
                // Look for any public static method that accepts a single string and returns something (likely a Host)
                var candidate = hmType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(m =>
                    {
                        var ps = m.GetParameters();
                        return ps.Length == 1 && ps[0].ParameterType == typeof(string);
                    });

                if (candidate != null)
                {
                    try
                    {
                        Debug.WriteLine($"Invoking HostManager factory '{candidate.Name}' to create host '{hostName}'...");
                        host = candidate.Invoke(null, new object[] { hostName });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"HostManager factory invocation failed: {ex.Message}");
                        host = null;
                    }
                }
            }

            // 3) If still no host, bail with a clear log (library may require pre-configured host)
            if (host == null)
            {
                Debug.WriteLine($"Host '{hostName}' not found in HostManager and no factory available. Create or register the Host before calling EnsureHostProjectRunning.");
                return;
            }

            // 4) Ensure host is connected. Try common Connected property and ConnectAsync method.
            try
            {
                var hostType = host.GetType();

                // Check for Connected property
                var connectedProp = hostType.GetProperty("Connected", BindingFlags.Public | BindingFlags.Instance);
                bool connected = false;
                if (connectedProp != null)
                {
                    var val = connectedProp.GetValue(host);
                    if (val is bool b && b) connected = true;
                }

                // If not connected, call ConnectAsync() if present
                if (!connected)
                {
                    var connectMethod = hostType.GetMethod("ConnectAsync", BindingFlags.Public | BindingFlags.Instance);
                    if (connectMethod != null)
                    {
                        Debug.WriteLine($"Calling ConnectAsync on host '{hostName}'...");
                        var task = (Task)connectMethod.Invoke(host, null);
                        await task.ConfigureAwait(false);
                        Debug.WriteLine($"Host '{hostName}' ConnectAsync finished.");
                    }
                    else
                    {
                        Debug.WriteLine($"Host '{hostName}' has no ConnectAsync method and is not marked connected. Continuing (host may already be usable).");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error ensuring host connection for '{hostName}': {ex.Message}");
            }

            // 5) Ensure project exists and is running
            try
            {
                var hostType = host.GetType();
                var projectsProp = hostType.GetProperty("Projects", BindingFlags.Public | BindingFlags.Instance);

                object projectObj = null;
                bool projectExists = false;

                if (projectsProp != null)
                {
                    var projects = projectsProp.GetValue(host) as System.Collections.IDictionary;
                    if (projects != null)
                    {
                        projectExists = projects.Contains(projectName);
                        if (projectExists)
                            projectObj = projects[projectName];
                    }
                    else
                    {
                        // fallback: try dynamic indexer access
                        try
                        {
                            var idx = hostType.GetProperty("Item", BindingFlags.Public | BindingFlags.Instance);
                            // last resort: try host.Projects[projectName] via dynamic
                            dynamic dynHost = host;
                            try
                            {
                                projectObj = dynHost.Projects[projectName];
                                projectExists = projectObj != null;
                            }
                            catch { projectExists = false; projectObj = null; }
                        }
                        catch { projectExists = false; projectObj = null; }
                    }
                }

                // If project not found, call StartProjectAsync (host.StartProjectAsync) which may create/start the project
                if (!projectExists)
                {
                    var startMethod = hostType.GetMethod("StartProjectAsync", BindingFlags.Public | BindingFlags.Instance);
                    if (startMethod != null)
                    {
                        Debug.WriteLine($"Project '{projectName}' not found — calling StartProjectAsync on host '{hostName}'...");
                        var task = (Task)startMethod.Invoke(host, new object[] { projectName });
                        await task.ConfigureAwait(false);
                        Debug.WriteLine($"Requested StartProjectAsync('{projectName}') on '{hostName}'.");
                    }
                    else
                    {
                        Debug.WriteLine($"Project '{projectName}' not found and host does not expose StartProjectAsync. Please ensure the project is present on the host.");
                    }

                    // After requesting start, attempt to refresh project reference (best-effort)
                    try
                    {
                        var projects = projectsProp?.GetValue(host) as System.Collections.IDictionary;
                        if (projects != null && projects.Contains(projectName))
                        {
                            projectObj = projects[projectName];
                            projectExists = true;
                        }
                    }
                    catch { /* ignore */ }
                }

                // If we have a projectObj, ensure it's running
                if (projectExists && projectObj != null)
                {
                    var pType = projectObj.GetType();
                    PropertyInfo runningProp = pType.GetProperty("IsRunning") ?? pType.GetProperty("Running") ?? pType.GetProperty("State");
                    bool isRunning = false;
                    if (runningProp != null)
                    {
                        var val = runningProp.GetValue(projectObj);
                        if (val is bool bb) isRunning = bb;
                        else if (val != null && val.ToString().Equals("running", StringComparison.OrdinalIgnoreCase)) isRunning = true;
                    }

                    if (!isRunning)
                    {
                        var startMethod = hostType.GetMethod("StartProjectAsync", BindingFlags.Public | BindingFlags.Instance);
                        if (startMethod != null)
                        {
                            Debug.WriteLine($"Starting project '{projectName}' on host '{hostName}'...");
                            var t = (Task)startMethod.Invoke(host, new object[] { projectName });
                            await t.ConfigureAwait(false);
                            Debug.WriteLine($"StartProjectAsync('{projectName}') completed.");
                        }
                        else
                        {
                            Debug.WriteLine($"Project '{projectName}' exists but cannot determine running state and StartProjectAsync not found.");
                        }
                    }
                    else
                    {
                        Debug.WriteLine($"Project '{projectName}' on host '{hostName}' is already running.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error ensuring project '{projectName}' is running on host '{hostName}': {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"EnsureHostProjectRunning unexpected error for {hostName}/{projectName}: {ex.Message}");
        }
    }
}
}


