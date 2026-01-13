using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sharp7;
using System.Linq;

namespace MatroxLDS
{
    public partial class BaseForm : Form
    {

        private System.Windows.Forms.Timer faultHideTimer;
        private const int FaultDisplayDurationMs = 3000; // 3 seconds


        public BaseForm()
        {
            InitializeComponent();
            faultHideTimer = new System.Windows.Forms.Timer();
            faultHideTimer.Interval = FaultDisplayDurationMs;
            faultHideTimer.Tick += FaultHideTimer_Tick;

        }

        public void ShowFaultMessage(string message, Color color)
        {
            if (faultDisplayLabel.InvokeRequired)
            {
                faultDisplayLabel.Invoke(new Action(() => ShowFaultMessage(message, color)));
                return;
            }
            faultDisplayLabel.Text = message;
            faultDisplayLabel.ForeColor = Color.Red;
            faultDisplayLabel.Visible = true;
            faultDisplayLabel.BringToFront();

            faultHideTimer.Stop();
            faultHideTimer.Start();
        }
        public void HideFaultMessage()
        {
            if (faultDisplayLabel.InvokeRequired)
            {
                faultDisplayLabel.Invoke(new Action(HideFaultMessage));
                return;
            }
            faultDisplayLabel.Visible = false;
            faultHideTimer.Stop();
        }

        private void FaultHideTimer_Tick(object sender, EventArgs e)
        {
            HideFaultMessage();
        }





    }
}