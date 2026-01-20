using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nini.Config;

namespace Omnicheck360
{
    public static class Utils
    {
        public static bool AdminLoggingChecked = false;
        
        public static void ButtonPressed(string btnName, string cFormName)
        {
         string sButtonPressed = "";
         string sUser = "";
         string sDataToWrite;
         string cOutputFile;
         bool lWriteData;
         bool lCont;

        lWriteData = true;
        //Default
        lCont = (AdminLoggingChecked == true ? true : false);

        if (lCont == true)
        {
            if (MainForm.nUserAccess == 0)
            {
                sUser = "Operate Mode ";
            }
            else if (MainForm.nUserAccess == 1)
            {
                sUser = "Operator ";
            }
            else if (MainForm.nUserAccess == 2)
            {
                sUser = "Maintenance ";
            }
            else if (MainForm.nUserAccess == 3)
            {
                sUser = "Administrator ";
            }
            else if (MainForm.nUserAccess == 4)
            {
                sUser = "Omnifission ";
            }

                if (btnName == "btnMenu" & cFormName == "MainForm")
            {
                sButtonPressed = "Main Form: Menu Button ";
            }
            else if (btnName == "btnResetCounters" & cFormName == "MainForm")
            {
                sButtonPressed = "Main Form: Reset Counters ";
            }
            else if (btnName == "btnConfigScreen" & cFormName == "MainMenu")
            {
                sButtonPressed = "Main Menu: Change Progam ";
            }
            else if (btnName == "btnLogs" & cFormName == "MainMenu")
            {
                sButtonPressed = "Main Menu: System Messages ";
            }
            else if (btnName == "btnDocuments" & cFormName == "MainMenu")
            {
                sButtonPressed = "Main Menu: Documents ";
            }
            else if (btnName == "btnViewResultLog" & cFormName == "MainMenu")
            {
                sButtonPressed = "Main Menu: Measurement Log ";
            }
            else if (btnName == "btnSettings" & cFormName == "MainMenu")
            {
                sButtonPressed = "Main Menu: System Settings ";
            }
            else if (btnName == "btnAdminSettings" & cFormName == "MainMenu")
            {
                sButtonPressed = "Main Menu: Administration Settings ";
            }
            else if (btnName == "btnChangeLogin" & cFormName == "MainMenu")
            {
                sButtonPressed = "Main Menu: User Login ";
            }
            else if (btnName == "btnMainForm" & cFormName == "MainMenu")
            {
                sButtonPressed = "Main Menu: Dashboard ";
            }
            else if (btnName == "BtnChangeProgram" & cFormName == "ConfigScreen")
            {
                sButtonPressed = "Change Program: Change Program ";
            }
            else if (btnName == "btnClose" & cFormName == "ConfigScreen")
            {
                sButtonPressed = "Change Program: Menu ";
            }
            else if (btnName == "btnDoc1" & cFormName == "Documents")
            {
                sButtonPressed = "Documents: Electrical Drawings ";
            }
            else if (btnName == "btnDoc2" & cFormName == "Documents")
            {
                sButtonPressed = "Documents: Operating Manual ";
            }
            else if (btnName == "btnDoc3" & cFormName == "Documents")
            {
                sButtonPressed = "Documents: Parts List ";
            }
            else if (btnName == "btnDoc4" & cFormName == "Documents")
            {
                sButtonPressed = "Documents: Troubleshooting Manual ";
            }
            else if (btnName == "btnClose" & cFormName == "Documents")
            {
                sButtonPressed = "Documents: Menu ";
            }
            else if (btnName == "btnClearSystemLog" & cFormName == "LogsForm")
            {
                sButtonPressed = "System Messages: Clear System Messages ";
            }
            else if (btnName == "btnClearResultLog" & cFormName == "LogsForm")
            {
                sButtonPressed = "System Messages: Clear Current Results ";
            }
            else if (btnName == "btnClose" & cFormName == "LogsForm")
            {
                sButtonPressed = "System Messages: Menu ";
            }
            else if (btnName == "btnLoadLogFile" & cFormName == "OutputLogView")
            {
                sButtonPressed = "Measurement Log: Load File ";
            }
            else if (btnName == "btnClose" & cFormName == "OutputLogView")
            {
                sButtonPressed = "Measurement Log: Menu ";
            }
            else if (btnName == "btnVerifyHead" & cFormName == "ProfileData")
            {
                sButtonPressed = "Profile Data: Verify Head ";
            }
            else if (btnName == "btnPLC" & cFormName == "MMenu2")
            {
                sButtonPressed = "Tracking System: Menu ";
            }
            else if (btnName == "btnGetProfileData" & cFormName == "ProfileData")
            {
                sButtonPressed = "Profile Data: Profile Display ";
            }
            else if (btnName == "btnGrabImage" & cFormName == "ProfileData")
            {
                sButtonPressed = "Profile Data: Grab Image ";
            }
            else if (btnName == "btnClose" & cFormName == "ProfileData")
            {
                sButtonPressed = "Profile Data: Menu ";
            }
            else if (btnName == "btnGoToWindows" & cFormName == "AdminScreen")
            {
                sButtonPressed = "Administration Settings: Exit to Desktop ";
            }
            else if (btnName == "btnRebootComputer" & cFormName == "AdminScreen")
            {
                sButtonPressed = "Administration Settings: Reboot Computer ";
            }
            else if (btnName == "cmdSave" & cFormName == "AdminScreen")
            {
                sButtonPressed = "Administration Settings: Save Settings ";
            }
            else if (btnName == "btnClose" & cFormName == "AdminScreen")
            {
                sButtonPressed = "Administration Settings: Menu ";
            }
            else {
                lWriteData = false;
            }

            if (lWriteData == true)
            {
                sDataToWrite = DateTime.Now.ToString("hh:mm dddd, dd MMMM yyyy") + " " + sUser + " " + sButtonPressed;
                cOutputFile = Directory.GetCurrentDirectory() + "\\translog.log";
                System.IO.File.AppendAllText(cOutputFile, sDataToWrite + System.Environment.NewLine);
            }
         }

       }

        public static unsafe float Int32BitsToSingle(int value)
        {
            return *(float*)(&value);
        }

        public static unsafe int SingleToInt32Bits(float value)
        {
            return *(int*)(&value);
        }

    }
   

      
}


