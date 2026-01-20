using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omnicheck360
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static volatile SplashScreen splashForm = null;

        [STAThread]
        static void Main()
        {
            bool result;
            var mutex = new System.Threading.Mutex(true, Application.ProductName, out result);

            if (!result)
            {
                MessageBox.Show("Another instance is already running.");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            splashForm = new SplashScreen();           

            Application.Run(new MainForm());

            GC.KeepAlive(mutex);
        }
    }
}
