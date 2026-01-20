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

namespace Omnicheck360
{
    public partial class InterfaceControl : BaseForm
    {
        public MainMenu MainMenu;
        public MainForm mainForm;
        public PopUpNumberPad popUpNumberPad;

        private static IniConfigSource source;

        private string cPath;
        private string InterfaceFile;

        public InterfaceControl(MainForm _mainForm)
        {
            InitializeComponent();
            mainForm = _mainForm;

            cPath = Environment.CurrentDirectory;
            InterfaceFile = cPath + "\\InterfaceControl.ini";
            source = new IniConfigSource(InterfaceFile);

        }

        private void InterfaceControl_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            timer1.Start();
            CurrentUserTxt.Text = mainForm.CurrentUserTxt.Text;

            SetLastConfig();
        }

        private void Camera1Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Camera1Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Camera 1 Enabled");
                source.Configs["CameraEnable"].Set("Camera1", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Camera 1 Disabled");
                source.Configs["CameraEnable"].Set("Camera1", "0");
                source.Save();
            }
        }

        private void Camera2Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Camera2Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Camera 2 Enabled");
                source.Configs["CameraEnable"].Set("Camera2", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Camera 2 Disabled");
                source.Configs["CameraEnable"].Set("Camera2", "0");
                source.Save();
            }
        }

        private void Camera3Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Camera3Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Camera 3 Enabled");
                source.Configs["CameraEnable"].Set("Camera3", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Camera 3 Disabled");
                source.Configs["CameraEnable"].Set("Camera3", "0");
                source.Save();
            }
        }

        private void Camera4Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Camera4Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Camera 4 Enabled");
                source.Configs["CameraEnable"].Set("Camera4", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Camera 4 Disabled");
                source.Configs["CameraEnable"].Set("Camera4", "0");
                source.Save();
            }
        }

        private void Camera5Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Camera5Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Camera 5 Enabled");
                source.Configs["CameraEnable"].Set("Camera5", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Camera 5 Disabled");
                source.Configs["CameraEnable"].Set("Camera5", "0");
                source.Save();
            }
        }

        private void Camera6Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Camera6Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Camera 6 Enabled");
                source.Configs["CameraEnable"].Set("Camera6", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Camera 6 Disabled");
                source.Configs["CameraEnable"].Set("Camera6", "0");
                source.Save();
            }
        }

        private void Camera7Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Camera7Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Camera 7 Enabled");
                source.Configs["CameraEnable"].Set("Camera7", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Camera 7 Disabled");
                source.Configs["CameraEnable"].Set("Camera7", "0");
                source.Save();
            }
        }

        private void Camera8Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Camera8Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Camera 8 Enabled");
                source.Configs["CameraEnable"].Set("Camera8", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Camera 8 Disabled");
                source.Configs["CameraEnable"].Set("Camera8", "0");
                source.Save();
            }
        }

        private void Light1Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Light1Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Light 1 Enabled");
                source.Configs["LightEnable"].Set("Light1", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Light 1 Disabled");
                source.Configs["LightEnable"].Set("Light1", "0");
                source.Save();
            }
        }

        private void Light2Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Light2Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Light 2 Enabled");
                source.Configs["LightEnable"].Set("Light2", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Light 2 Disabled");
                source.Configs["LightEnable"].Set("Light2", "0");
                source.Save();
            }
        }

        private void Light3Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Light3Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Light 3 Enabled");
                source.Configs["LightEnable"].Set("Light3", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Light 3 Disabled");
                source.Configs["LightEnable"].Set("Light3", "0");
                source.Save();
            }
        }

        private void Light4Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Light4Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Light 4 Enabled");
                source.Configs["LightEnable"].Set("Light4", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Light 4 Disabled");
                source.Configs["LightEnable"].Set("Light4", "0");
                source.Save();
            }
        }

        private void Light5Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Light5Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Light 5 Enabled");
                source.Configs["LightEnable"].Set("Light5", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Light 5 Disabled");
                source.Configs["LightEnable"].Set("Light5", "0");
                source.Save();
            }
        }

        private void Light6Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Light6Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Light 6 Enabled");
                source.Configs["LightEnable"].Set("Light6", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Light 6 Disabled");
                source.Configs["LightEnable"].Set("Light6", "0");
                source.Save();
            }
        }

        private void Light7Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Light7Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Light 7 Enabled");
                source.Configs["LightEnable"].Set("Light7", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Light 7 Disabled");
                source.Configs["LightEnable"].Set("Light7", "0");
                source.Save();
            }
        }

        private void Light8Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Light8Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Light 8 Enabled");
                source.Configs["LightEnable"].Set("Light8", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Light 8 Disabled");
                source.Configs["LightEnable"].Set("Light8", "0");
                source.Save();
            }
        }

        private void Servo1Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Servo1Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Servo 1 Enabled");
                source.Configs["ServoEnable"].Set("Servo1", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Servo 1 Disabled");
                source.Configs["ServoEnable"].Set("Servo1", "0");
                source.Save();
            }
        }

        private void Servo2Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Servo2Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Servo 2 Enabled");
                source.Configs["ServoEnable"].Set("Servo2", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Servo 2 Disabled");
                source.Configs["ServoEnable"].Set("Servo2", "0");
                source.Save();
            }
        }

        private void Servo3Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (Servo3Enable.Checked == true)
            {
                MainForm.log.Info("Interface Control Page: Servo 3 Enabled");
                source.Configs["ServoEnable"].Set("Servo3", "1");
                source.Save();
            }
            else
            {
                MainForm.log.Info("Interface Control Page: Servo 3 Disabled");
                source.Configs["ServoEnable"].Set("Servo3", "0");
                source.Save();
            }
        }

        private void SetLastConfig()
        {
                Camera1Enable.Checked = true;

            if (MainForm.Servo1Enable == 1)
            {
                Servo1Enable.Checked = true;
            }
            else
            {
                Servo1Enable.Checked = false;
            }

            if (MainForm.Servo2Enable == 1)
            {
                Servo2Enable.Checked = true;
            }
            else
            {
                Servo2Enable.Checked = false;
            }

            if (MainForm.Servo3Enable == 1)
            {
                Servo3Enable.Checked = true;
            }

            else
            {
                Servo3Enable.Checked = false;
            }


        }



        //Dashboard Button
        //**************************************
        private void DashboardBtn_Click(object sender, EventArgs e)
        {
            MainForm.log.Info("Interface Control Page: Dashboard Button Pressed");
            mainForm.Show();
            mainForm.Focus();
            this.Close();
        }

        private void DashboardBtn_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void DashboardBtn_MouseUp(object sender, MouseEventArgs e)
        {
        }
        //**************************************

        //Dashboard Button
        //**************************************
        private void RestartInterfaceBtn_Click(object sender, EventArgs e)
        {
            MainForm.log.Info("Interface Control Page: Restart Interface Button Pressed"); 
            MainForm.Client.Disconnect();            
            Application.Restart();
            Environment.Exit(0);
        }

        private void RestartInterfaceBtn_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void RestartInterfaceBtn_MouseUp(object sender, MouseEventArgs e)
        {
        }
        //**************************************
    }
}
