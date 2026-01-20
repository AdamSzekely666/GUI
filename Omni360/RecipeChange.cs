using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Omnicheck360.Properties;
using Nini.Config;
using Sharp7;

namespace Omnicheck360
{
    public partial class RecipeChange : BaseForm
    {
        private MainForm mainForm;

        private static IniConfigSource source;
        private static IniConfigSource recipe_source;

        public string SelectedRecipe;

       // private Next1 next1;
        

        public RecipeChange(MainForm _mainForm)
        {
            InitializeComponent();
            ButtonAnimator.InitializeAnimation(MainMenuFormBtn, "blue");
            ButtonAnimator.InitializeAnimation(SelectRecipeBtn, "blue");

            mainForm = _mainForm;

        }

      

        private void RecipeChange_Load(object sender, EventArgs e)
        {
            timer1.Start();
            CurrentUserTxt.Text = mainForm.CurrentUserTxt.Text;

            RecipeChangeStatus.Text = "";
            ConnectOmronCore();
            LoadRecipeName();
        }

        private void LoadRecipeName()
        {
            int recipe_name_index;
            string cPath = Environment.CurrentDirectory;
            string cIniFile = cIniFile = cPath + "\\app.ini";

            source = new IniConfigSource(cIniFile);

            for (recipe_name_index = 0; recipe_name_index < 8; recipe_name_index++)
            {
                listBox1.Items.Add(source.Configs["RecipeName"].Get(recipe_name_index.ToString()));
            }
            listBox1.SelectedIndex = 0;
        }

        private void ConnectOmronCore()
        {
            try
            {
                    coreRA1.FzPath = MainForm.FZ_PATH;
                    coreRA1.ConnectMode = FZ_Control.ConnectionMode.Remote;
                    coreRA1.IpAddress = MainForm.OMRON_IP;
                    coreRA1.LineNo = MainForm.Camera1LineNo;
                    coreRA1.ConnectStart();
            }
            catch
            {
                MessageBox.Show("Failed to connect to Vision Controller!");
            }
        }

        //Settings for Dashboard Button
        //********************************************************
        private void DashboardBtn_Click(object sender, EventArgs e)
        {
            MainForm.log.Info("Recipe Change Page: Dashboard Button Pressed");
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
        //********************************************************

        //Select Recipe Button
        //********************************************************
        private void SelectRecipeBtn_Click(object sender, EventArgs e)
        {
            int RecipeNum = 0;

            RecipeChangeStatus.Text = "";
            System.Threading.Thread.Sleep(1000);

            string cPath = Environment.CurrentDirectory;
            string cIniFile = cPath + "\\app.ini";

            source = new IniConfigSource(cIniFile);

            string received_recipe = textBox1.Text;

            if (received_recipe.Length != 0)
            {

                if (listBox1.Items.Contains(received_recipe))
                {
                    try
                    {
                        RecipeNum = listBox1.Items.IndexOf(received_recipe);
                        RecipeChangeStatus.Text = RecipeNum.ToString();
                        listBox1.SelectedIndex = RecipeNum;
                        SelectedRecipe = source.Configs["RecipeName"].Get(RecipeNum.ToString());
                        MainForm.Current_Recipe_Num = RecipeNum;

                        ChangeScene(RecipeNum);

                        GetTrackingValues();

                        SetTrackingValues();

                        MainForm.log.Info("Recipe Change Page: Recipe Changed to " + SelectedRecipe);
                        RecipeChangeStatus.ForeColor = Color.Green;
                        RecipeChangeStatus.Text = "Recipe Change Successfully!";
                      //  mainForm.RecipeNameLb.Text = SelectedRecipe;
                       // next1.RecipeNameLb.Text = SelectedRecipe;
                        textBox1.Text = String.Empty;
                        textBox1.Focus();
                    }
                    catch
                    {

                        MainForm.log.Info("Recipe Change Page: Recipe Change Failed");
                        RecipeChangeStatus.ForeColor = Color.Red;
                        RecipeChangeStatus.Text = "Recipe Change Failed!";
                        textBox1.Text = String.Empty;
                        textBox1.Focus();

                    }
                }
                else
                {
                   // mainForm.RecipeNameLb.Text = " Recipe not found";
                    textBox1.Text = String.Empty;
                    textBox1.Focus();
                }

            }
            else
            {
                try
                {
                    RecipeNum = listBox1.SelectedIndex;
                    SelectedRecipe = source.Configs["RecipeName"].Get(RecipeNum.ToString());
                    MainForm.Current_Recipe_Num = RecipeNum;

                    ChangeScene(RecipeNum);

                    GetTrackingValues();

                    SetTrackingValues();

                    MainForm.log.Info("Recipe Change Page: Recipe Changed to " + SelectedRecipe);
                    RecipeChangeStatus.ForeColor = Color.Green;
                    RecipeChangeStatus.Text = "Recipe Change Successfully!";
                  //  mainForm.RecipeNameLb.Text = SelectedRecipe;
                   // next1.RecipeNameLb.Text = SelectedRecipe;
                    textBox1.Focus();

                }
                catch
                {
                    MainForm.log.Info("Recipe Change Page: Recipe Change Failed");
                    RecipeChangeStatus.ForeColor = Color.Red;
                    RecipeChangeStatus.Text = "Recipe Change Failed!";
                    
                    textBox1.Focus();
                }

            }

            
        }

        private void SelectRecipeBtn_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void SelectRecipeBtn_MouseUp(object sender, MouseEventArgs e)
        {
        }
        //********************************************************

        public void GetTrackingValues()
        {
            int RecipeNum = 0;

            string cPath = Environment.CurrentDirectory;
            string cIniFile1 = cPath + "\\RecipeTracking.ini";

            recipe_source = new IniConfigSource(cIniFile1);

            try
            {
                RecipeNum = listBox1.SelectedIndex;

                MainForm.Camera1TriggerDelay = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("Camera1TriggerDelay"));
                MainForm.Camera2TriggerDelay = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("Camera2TriggerDelay"));
                MainForm.Camera3TriggerDelay = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("Camera3TriggerDelay"));
                MainForm.Camera4TriggerDelay = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("Camera4TriggerDelay"));
                MainForm.Camera5TriggerDelay = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("Camera5TriggerDelay"));
                MainForm.Camera1TriggerPulseWidth = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("Camera1TriggerPulseWidth"));
                MainForm.Camera2TriggerPulseWidth = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("Camera2TriggerPulseWidth"));
                MainForm.Camera3TriggerPulseWidth = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("Camera3TriggerPulseWidth"));
                MainForm.Camera4TriggerPulseWidth = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("Camera4TriggerPulseWidth"));
                MainForm.Camera5TriggerPulseWidth = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("Camera5TriggerPulseWidth"));
                MainForm.RejectorDelay = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("RejectorDelay"));
                MainForm.ReTriggerDelay = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("ReTriggerDelay"));
                MainForm.RejectorPulseWidth = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("RejectorPulseWidth"));
                MainForm.MasterPPSMax = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("MasterPPSMax"));
                MainForm.MasterPPSMin = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("MasterPPSMin"));
                MainForm.VFDMax = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("VFDMax"));
                MainForm.Inspect_Setpoint = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("InspectionSetPoint"));
                MainForm.Reject_Setpoint = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("RejectionSetPoint"));
                MainForm.MaxConsecutiveRejects = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("MaxConsecutiveRejects"));
                MainForm.ProductWidth = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("ProductWidth"));
                MainForm.RejectWidth = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("RejectWidth"));
                MainForm.InspectionTimeout = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("InspectionTimeout"));
                MainForm.Servo1Position = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("Servo1Position"));
                MainForm.Servo2Position = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("Servo2Position"));
                MainForm.Servo3Position = Convert.ToInt32(recipe_source.Configs[RecipeNum.ToString()].Get("Servo3Position"));
            }
            catch
            {
                MessageBox.Show("Could not read Recipe Tracking Values");
            }
        }

        public void SetTrackingValues()
        {
            try
            {
                S7.SetUIntAt(MainForm.DB_UINT_Write_Buffer, 0, (ushort)MainForm.Camera1TriggerDelay);
                S7.SetUIntAt(MainForm.DB_UINT_Write_Buffer, 2, (ushort)MainForm.Camera2TriggerDelay);
                S7.SetUIntAt(MainForm.DB_UINT_Write_Buffer, 4, (ushort)MainForm.Camera3TriggerDelay);
                S7.SetUIntAt(MainForm.DB_UINT_Write_Buffer, 6, (ushort)MainForm.Camera4TriggerDelay);
                S7.SetUIntAt(MainForm.DB_UINT_Write_Buffer, 8, (ushort)MainForm.Camera5TriggerDelay);

                S7.SetUIntAt(MainForm.DB_UINT_Write_Buffer, 10, (ushort)MainForm.RejectorDelay);
                S7.SetUIntAt(MainForm.DB_UINT_Write_Buffer, 30, (ushort)MainForm.ReTriggerDelay);

                S7.SetUIntAt(MainForm.DB_UINT_Write_Buffer, 12, (ushort)MainForm.RejectorPulseWidth);
                S7.SetUIntAt(MainForm.DB_UINT_Write_Buffer, 14, (ushort)MainForm.ProductWidth);
                S7.SetUIntAt(MainForm.DB_UINT_Write_Buffer, 38, (ushort)MainForm.RejectWidth);
                S7.SetUIntAt(MainForm.DB_UINT_Write_Buffer, 16, (ushort)MainForm.MaxConsecutiveRejects);
                S7.SetUIntAt(MainForm.DB_UINT_Write_Buffer, 32, (ushort)MainForm.MasterPPSMax);
                S7.SetUIntAt(MainForm.DB_UINT_Write_Buffer, 34, (ushort)MainForm.MasterPPSMin);
                S7.SetUIntAt(MainForm.DB_UINT_Write_Buffer, 36, (ushort)MainForm.VFDMax);

                MainForm.Client.DBWrite(4, 100, 40, MainForm.DB_UINT_Write_Buffer);


                S7.SetDIntAt(MainForm.DB_CameraTriggerPulseWidth_Write_Buffer, 0, MainForm.Camera1TriggerPulseWidth);
                S7.SetDIntAt(MainForm.DB_CameraTriggerPulseWidth_Write_Buffer, 4, MainForm.Camera2TriggerPulseWidth);
                S7.SetDIntAt(MainForm.DB_CameraTriggerPulseWidth_Write_Buffer, 8, MainForm.Camera3TriggerPulseWidth);
                S7.SetDIntAt(MainForm.DB_CameraTriggerPulseWidth_Write_Buffer, 12, MainForm.Camera4TriggerPulseWidth);
                S7.SetDIntAt(MainForm.DB_CameraTriggerPulseWidth_Write_Buffer, 16, MainForm.Camera5TriggerPulseWidth);
                S7.SetDIntAt(MainForm.DB_CameraTriggerPulseWidth_Write_Buffer, 20, MainForm.InspectionTimeout);

                MainForm.Client.DBWrite(4, 140, 24, MainForm.DB_CameraTriggerPulseWidth_Write_Buffer);

                S7.SetDIntAt(MainForm.DB_DINT_Write_Buffer, 88, MainForm.Servo1Position);
                S7.SetDIntAt(MainForm.DB_DINT_Write_Buffer, 92, MainForm.Servo2Position);
                S7.SetDIntAt(MainForm.DB_DINT_Write_Buffer, 96, MainForm.Servo3Position);

                MainForm.Client.DBWrite(MainForm.DataBlock, 0, 100, MainForm.DB_DINT_Write_Buffer);

                if (MainForm.Servo1Enable == 1 || MainForm.Servo2Enable == 1 || MainForm.Servo3Enable == 1)
                {
                    Buffer.SetByte(MainForm.DB_ServoMove_Write_Buffer, 0, 1);
                    MainForm.Client.WriteArea(S7Consts.S7AreaDB, MainForm.DataBlock, 5239, 1, S7Consts.S7WLBit, MainForm.DB_ServoMove_Write_Buffer);


                    OmniMsgMoveAll message = new OmniMsgMoveAll("Please Wait...", 0, MainForm.DB_ServoDone_Read_Buffer);
                    message.ShowDialog();
                    message.TopMost = true;
                }

            }
            catch
            {
                MessageBox.Show("Could not set Recipe Tracking Values");
            }
        }

        public void ChangeScene(int _recipeNum)
        {
            int RecipeNum = _recipeNum;

                coreRA1.Macro_DirectExecute("MeasureStop");
                coreRA1.Macro_DirectExecute("ChangeScene " + RecipeNum);
                coreRA1.Macro_DirectExecute("SaveData");
                coreRA1.Macro_DirectExecute("MeasureStart");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void SceneName_Click(object sender, EventArgs e)
        {
            try
            {
                

                    coreRA1.Macro_DirectExecute("MeasureStop");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 0 + "," + "\"" + "591ML PET FANTA BIG RED" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 1 + "," + "\"" + "591ML PET FANTA ORANGE" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 2 + "," + "\"" + "591ML PET FANTA GRAPE" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 3 + "," + "\"" + "591ML PET FANTA SORREL" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 4 + "," + "\"" + "591ML PET SCH GRAPEFRUIT" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 5 + "," + "\"" + "591ML PET FANTA BANANA" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 6 + "," + "\"" + "591ML PET SCH GINGERALE" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 7 + "," + "\"" + "591ML PET SCH CLUB SODA" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 8 + "," + "\"" + "591ML PET COKE NO SUGAR" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 9 + "," + "\"" + "591ML PET COKE ORIGINAL" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 10 + "," + "\"" + "591ML PET SPRITE LEMON" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 11 + "," + "\"" + "591ML PET GREEN PUNCH" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 12 + "," + "\"" + "591ML PET SURINAME SODA WATER" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 13 + "," + "\"" + "591ML PET PREDATOR GOLD" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 14 + "," + "\"" + "2L PET FANTA BIG RED" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 15 + "," + "\"" + "2L PET FANTA ORANGE" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 16 + "," + "\"" + "2L PET FANTA GRAPE" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 17 + "," + "\"" + "2L PET FANTA SORREL" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 18 + "," + "\"" + "2L PET FANTA BANANA" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 19 + "," + "\"" + "2L PET FSCH GINGERALE" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 20 + "," + "\"" + "2L PET COKE NO SUGAR" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 21 + "," + "\"" + "2L PET COKE ORIGINAL" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 22 + "," + "\"" + "2L PET SPRITE LEMON" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 23 + "," + "\"" + "2L PET MMFC PRTUGAL" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 24 + "," + "\"" + "2L PET MMFC ORANGE" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 25 + "," + "\"" + "2L PET GREEN PUNCH (NEW)" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 26 + "," + "\"" + "2L CHERRY BOUQUET" + "\"");
                    coreRA1.Macro_DirectExecute("SetSceneTitle" + " " + 27 + "," + "\"" + "2L SURNAME GIGNER" + "\"");

                    coreRA1.Macro_DirectExecute("SaveData");
                    coreRA1.Macro_DirectExecute("MeasureStart");
                    MessageBox.Show("COMPLETE1");
              


                    coreRA2.Macro_DirectExecute("MeasureStop");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 0 + "," + "\"" + "591ML PET FANTA BIG RED" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 1 + "," + "\"" + "591ML PET FANTA ORANGE" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 2 + "," + "\"" + "591ML PET FANTA GRAPE" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 3 + "," + "\"" + "591ML PET FANTA SORREL" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 4 + "," + "\"" + "591ML PET SCH GRAPEFRUIT" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 5 + "," + "\"" + "591ML PET FANTA BANANA" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 6 + "," + "\"" + "591ML PET SCH GINGERALE" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 7 + "," + "\"" + "591ML PET SCH CLUB SODA" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 8 + "," + "\"" + "591ML PET COKE NO SUGAR" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 9 + "," + "\"" + "591ML PET COKE ORIGINAL" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 10 + "," + "\"" + "591ML PET SPRITE LEMON" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 11 + "," + "\"" + "591ML PET GREEN PUNCH" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 12 + "," + "\"" + "591ML PET SURINAME SODA WATER" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 13 + "," + "\"" + "591ML PET PREDATOR GOLD" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 14 + "," + "\"" + "2L PET FANTA BIG RED" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 15 + "," + "\"" + "2L PET FANTA ORANGE" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 16 + "," + "\"" + "2L PET FANTA GRAPE" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 17 + "," + "\"" + "2L PET FANTA SORREL" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 18 + "," + "\"" + "2L PET FANTA BANANA" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 19 + "," + "\"" + "2L PET FSCH GINGERALE" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 20 + "," + "\"" + "2L PET COKE NO SUGAR" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 21 + "," + "\"" + "2L PET COKE ORIGINAL" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 22 + "," + "\"" + "2L PET SPRITE LEMON" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 23 + "," + "\"" + "2L PET MMFC PRTUGAL" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 24 + "," + "\"" + "2L PET MMFC ORANGE" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 25 + "," + "\"" + "2L PET GREEN PUNCH (NEW)" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 26 + "," + "\"" + "2L CHERRY BOUQUET" + "\"");
                    coreRA2.Macro_DirectExecute("SetSceneTitle" + " " + 27 + "," + "\"" + "2L SURNAME GIGNER" + "\"");

                    coreRA2.Macro_DirectExecute("SaveData");
                    coreRA2.Macro_DirectExecute("MeasureStart");
                    MessageBox.Show("COMPLETE2");
               


                    coreRA3.Macro_DirectExecute("MeasureStop");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 0 + "," + "\"" + "591ML PET FANTA BIG RED" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 1 + "," + "\"" + "591ML PET FANTA ORANGE" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 2 + "," + "\"" + "591ML PET FANTA GRAPE" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 3 + "," + "\"" + "591ML PET FANTA SORREL" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 4 + "," + "\"" + "591ML PET SCH GRAPEFRUIT" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 5 + "," + "\"" + "591ML PET FANTA BANANA" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 6 + "," + "\"" + "591ML PET SCH GINGERALE" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 7 + "," + "\"" + "591ML PET SCH CLUB SODA" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 8 + "," + "\"" + "591ML PET COKE NO SUGAR" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 9 + "," + "\"" + "591ML PET COKE ORIGINAL" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 10 + "," + "\"" + "591ML PET SPRITE LEMON" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 11 + "," + "\"" + "591ML PET GREEN PUNCH" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 12 + "," + "\"" + "591ML PET SURINAME SODA WATER" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 13 + "," + "\"" + "591ML PET PREDATOR GOLD" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 14 + "," + "\"" + "2L PET FANTA BIG RED" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 15 + "," + "\"" + "2L PET FANTA ORANGE" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 16 + "," + "\"" + "2L PET FANTA GRAPE" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 17 + "," + "\"" + "2L PET FANTA SORREL" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 18 + "," + "\"" + "2L PET FANTA BANANA" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 19 + "," + "\"" + "2L PET FSCH GINGERALE" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 20 + "," + "\"" + "2L PET COKE NO SUGAR" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 21 + "," + "\"" + "2L PET COKE ORIGINAL" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 22 + "," + "\"" + "2L PET SPRITE LEMON" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 23 + "," + "\"" + "2L PET MMFC PRTUGAL" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 24 + "," + "\"" + "2L PET MMFC ORANGE" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 25 + "," + "\"" + "2L PET GREEN PUNCH (NEW)" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 26 + "," + "\"" + "2L CHERRY BOUQUET" + "\"");
                    coreRA3.Macro_DirectExecute("SetSceneTitle" + " " + 27 + "," + "\"" + "2L SURNAME GIGNER" + "\"");

                    coreRA3.Macro_DirectExecute("SaveData");
                    coreRA3.Macro_DirectExecute("MeasureStart");
                    MessageBox.Show("COMPLETE3");
                


            }
            catch
            {
                MessageBox.Show("FAILED");
            }
        }

        private void CopyScene_Click(object sender, EventArgs e)
        {
            try
            {
                coreRA1.Macro_DirectExecute("MeasureStop");
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 1);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 2);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 3);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 4);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 5);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 6);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 7);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 8);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 9);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 10);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 11);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 12);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 13);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 14);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 15);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 16);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 17);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 18);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 19);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 20);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 21);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 22);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 23);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 24);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 25);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 26);
                coreRA1.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 27);

                coreRA1.Macro_DirectExecute("SaveData");
                coreRA1.Macro_DirectExecute("MeasureStart");
                MessageBox.Show("COMPLETE");

                coreRA2.Macro_DirectExecute("MeasureStop");
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 1);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 2);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 3);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 4);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 5);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 6);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 7);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 8);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 9);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 10);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 11);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 12);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 13);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 14);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 15);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 16);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 17);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 18);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 19);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 20);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 21);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 22);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 23);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 24);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 25);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 26);
                coreRA2.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 27);

                coreRA2.Macro_DirectExecute("SaveData");
                coreRA2.Macro_DirectExecute("MeasureStart");
                MessageBox.Show("COMPLETE");

                coreRA3.Macro_DirectExecute("MeasureStop");
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 1);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 2);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 3);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 4);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 5);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 6);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 7);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 8);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 9);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 10);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 11);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 12);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 13);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 14);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 15);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 16);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 17);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 18);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 19);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 20);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 21);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 22);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 23);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 24);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 25);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 26);
                coreRA3.Macro_DirectExecute("CopyScene" + " " + 0 + "," + 27);

                coreRA3.Macro_DirectExecute("SaveData");
                coreRA3.Macro_DirectExecute("MeasureStart");
                MessageBox.Show("COMPLETE");



            }
            catch
            {
                MessageBox.Show("FAILED");
            }
        }

        private void MainMenuFormBtn_Click(object sender, EventArgs e)
        {
            MainForm.log.Info("Recipe Change Page: Dashboard Button Pressed");
            mainForm.Show();
            mainForm.Focus();
            this.Close();

        }
    }
    
}
