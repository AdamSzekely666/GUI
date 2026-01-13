using OmniCheck_360;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MatroxLDS
{
    static class Program
    {
        public static volatile SplashScreen splashForm = null;
        public static ManualResetEvent splashScreenReady = new ManualResetEvent(false);

        [STAThread]
        static void Main()
        {
            bool result;
            var mutex = new Mutex(true, Application.ProductName, out result);
            if (!result)
            {
                MessageBox.Show("Another instance is already running.");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Thread splashThread = new Thread(new ThreadStart(() =>
            {
                splashForm = new SplashScreen();
                splashForm.Shown += (sender, e) => splashScreenReady.Set();
                Application.Run(splashForm);
            }));
            splashThread.SetApartmentState(ApartmentState.STA);
            splashThread.Start();

            splashScreenReady.WaitOne();

            MainForm mainForm = new MainForm();

            // NOTE: do NOT auto-close the splash here. Keep splash open until MainForm decides to close it
            mainForm.Shown += (sender, e) =>
            {
                // Keep the UX of bringing main form to front without closing the splash
                try
                {
                    mainForm.TopMost = true;  // Ensure MainForm is on top
                    mainForm.BringToFront();  // Bring MainForm to front
                    mainForm.TopMost = false; // Revert TopMost property
                    mainForm.Activate();      // Activate MainForm
                }
                catch { }
            };

            Application.Run(mainForm);
            GlobalKeyboardHook.Unhook();
            GC.KeepAlive(mutex);
        }
    }
}