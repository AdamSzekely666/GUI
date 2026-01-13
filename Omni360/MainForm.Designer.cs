namespace MatroxLDS
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.mainFormBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.mainMenuBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.circularBufferBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.C1RecipeNameLb = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.alphaBlendTextBox1 = new ZBobb.AlphaBlendTextBox();
            this.alphaBlendTextBox3 = new ZBobb.AlphaBlendTextBox();
            this.MainMenuFormBtn = new System.Windows.Forms.Button();
            this.ResetCounterBtn = new System.Windows.Forms.Button();
            this.DateTimeUpdate = new System.Windows.Forms.Timer(this.components);
            this.entityCommand1 = new System.Data.Entity.Core.EntityClient.EntityCommand();
            this.TotalCountValue = new ZBobb.AlphaBlendTextBox();
            this.TotalFailValue = new ZBobb.AlphaBlendTextBox();
            this.alphaBlendTextBox2 = new ZBobb.AlphaBlendTextBox();
            this.RejectPercentage = new ZBobb.AlphaBlendTextBox();
            this.FinishNumber = new ZBobb.AlphaBlendTextBox();
            this.BaseNumber = new ZBobb.AlphaBlendTextBox();
            this.ISWNumber = new ZBobb.AlphaBlendTextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.chartXAxisTextBox = new System.Windows.Forms.TextBox();
            this.databaseIDTextBox = new System.Windows.Forms.TextBox();
            this.coreTempTextbox1 = new System.Windows.Forms.TextBox();
            this.coreTempTextbox2 = new System.Windows.Forms.TextBox();
            this.coreTempTextbox3 = new System.Windows.Forms.TextBox();
            this.coreTempTextbox4 = new System.Windows.Forms.TextBox();
            this.ResetCounterBtn2 = new Zebra.ADA.OperatorAPI.ControlsPackage.ButtonUIControl();
            this.Cam1Scroll = new System.Windows.Forms.Button();
            this.Cam2Scroll = new System.Windows.Forms.Button();
            this.panelControl = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.C2RecipeNameLb = new System.Windows.Forms.Label();
            this.C3RecipeNameLb = new System.Windows.Forms.Label();
            this.AllCameraDisplay = new System.Windows.Forms.Button();
            this.BtnSelectFlavour = new System.Windows.Forms.Button();
            this.ResetCounterBtn1 = new Zebra.ADA.OperatorAPI.ControlsPackage.ButtonUIControl();
            this.ResetCounterBtn3 = new Zebra.ADA.OperatorAPI.ControlsPackage.ButtonUIControl();
            this.lblCurrentFlavour = new System.Windows.Forms.Label();
            this.txtPlcThroughput = new ZBobb.AlphaBlendTextBox();
            this.txtPlcRejects = new ZBobb.AlphaBlendTextBox();
            this.txtPlcRejectRate = new ZBobb.AlphaBlendTextBox();
            this.alphaBlendTextBox4 = new ZBobb.AlphaBlendTextBox();
            this.txtCPM = new ZBobb.AlphaBlendTextBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.txtDownCan = new ZBobb.AlphaBlendTextBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.DentNumber = new ZBobb.AlphaBlendTextBox();
            this.panelCam1Dots = new System.Windows.Forms.Panel();
            this.panelCam2Dots = new System.Windows.Forms.Panel();
            this.lblCardReaderStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mainFormBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainMenuBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.circularBufferBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.SuspendLayout();
            // 
            // mainFormBindingSource
            // 
            this.mainFormBindingSource.DataSource = typeof(MatroxLDS.MainForm);
            // 
            // mainMenuBindingSource
            // 
            this.mainMenuBindingSource.DataSource = typeof(MatroxLDS.MainMenu);
            // 
            // C1RecipeNameLb
            // 
            this.C1RecipeNameLb.BackColor = System.Drawing.Color.Transparent;
            this.C1RecipeNameLb.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.C1RecipeNameLb.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.C1RecipeNameLb.Location = new System.Drawing.Point(580, 145);
            this.C1RecipeNameLb.Name = "C1RecipeNameLb";
            this.C1RecipeNameLb.Size = new System.Drawing.Size(437, 37);
            this.C1RecipeNameLb.TabIndex = 476;
            this.C1RecipeNameLb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.C1RecipeNameLb.Visible = false;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 0;
            // 
            // alphaBlendTextBox1
            // 
            this.alphaBlendTextBox1.BackAlpha = 0;
            this.alphaBlendTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.alphaBlendTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.alphaBlendTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alphaBlendTextBox1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.alphaBlendTextBox1.Location = new System.Drawing.Point(33, 206);
            this.alphaBlendTextBox1.Name = "alphaBlendTextBox1";
            this.alphaBlendTextBox1.ReadOnly = true;
            this.alphaBlendTextBox1.Size = new System.Drawing.Size(247, 31);
            this.alphaBlendTextBox1.TabIndex = 509;
            this.alphaBlendTextBox1.TabStop = false;
            this.alphaBlendTextBox1.Text = "Total Count";
            // 
            // alphaBlendTextBox3
            // 
            this.alphaBlendTextBox3.BackAlpha = 0;
            this.alphaBlendTextBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.alphaBlendTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.alphaBlendTextBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alphaBlendTextBox3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.alphaBlendTextBox3.Location = new System.Drawing.Point(64, 333);
            this.alphaBlendTextBox3.Name = "alphaBlendTextBox3";
            this.alphaBlendTextBox3.ReadOnly = true;
            this.alphaBlendTextBox3.Size = new System.Drawing.Size(119, 37);
            this.alphaBlendTextBox3.TabIndex = 511;
            this.alphaBlendTextBox3.TabStop = false;
            this.alphaBlendTextBox3.Text = "Reject";
            // 
            // MainMenuFormBtn
            // 
            this.MainMenuFormBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.MainMenuFormBtn.BackColor = System.Drawing.Color.Transparent;
            this.MainMenuFormBtn.BackgroundImage = global::OmniCheck_360.Properties.Resources.mainmenu48x48;
            this.MainMenuFormBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.MainMenuFormBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.MainMenuFormBtn.FlatAppearance.BorderSize = 0;
            this.MainMenuFormBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.MainMenuFormBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.MainMenuFormBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MainMenuFormBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.MainMenuFormBtn.ForeColor = System.Drawing.Color.Black;
            this.MainMenuFormBtn.Location = new System.Drawing.Point(560, 880);
            this.MainMenuFormBtn.Name = "MainMenuFormBtn";
            this.MainMenuFormBtn.Size = new System.Drawing.Size(240, 120);
            this.MainMenuFormBtn.TabIndex = 514;
            this.MainMenuFormBtn.UseMnemonic = false;
            this.MainMenuFormBtn.UseVisualStyleBackColor = false;
            this.MainMenuFormBtn.Click += new System.EventHandler(this.MainMenuFormBtn_Click);
            // 
            // ResetCounterBtn
            // 
            this.ResetCounterBtn.BackColor = System.Drawing.Color.Transparent;
            this.ResetCounterBtn.BackgroundImage = global::OmniCheck_360.Properties.Resources.reset48x48;
            this.ResetCounterBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ResetCounterBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.ResetCounterBtn.FlatAppearance.BorderSize = 0;
            this.ResetCounterBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ResetCounterBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ResetCounterBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResetCounterBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.ResetCounterBtn.ForeColor = System.Drawing.Color.Black;
            this.ResetCounterBtn.Location = new System.Drawing.Point(840, 880);
            this.ResetCounterBtn.Name = "ResetCounterBtn";
            this.ResetCounterBtn.Size = new System.Drawing.Size(240, 120);
            this.ResetCounterBtn.TabIndex = 515;
            this.ResetCounterBtn.UseMnemonic = false;
            this.ResetCounterBtn.UseVisualStyleBackColor = false;
            this.ResetCounterBtn.Click += new System.EventHandler(this.ResetCounterBtn1_Click);
            // 
            // DateTimeUpdate
            // 
            this.DateTimeUpdate.Interval = 1000;
            this.DateTimeUpdate.Tick += new System.EventHandler(this.DateTimeTimer_Tick);
            // 
            // entityCommand1
            // 
            this.entityCommand1.CommandTimeout = 0;
            this.entityCommand1.CommandTree = null;
            this.entityCommand1.Connection = null;
            this.entityCommand1.EnablePlanCaching = true;
            this.entityCommand1.Transaction = null;
            // 
            // TotalCountValue
            // 
            this.TotalCountValue.BackAlpha = 0;
            this.TotalCountValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.TotalCountValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TotalCountValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalCountValue.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.TotalCountValue.Location = new System.Drawing.Point(17, 244);
            this.TotalCountValue.Name = "TotalCountValue";
            this.TotalCountValue.Size = new System.Drawing.Size(522, 73);
            this.TotalCountValue.TabIndex = 516;
            this.TotalCountValue.Text = "0";
            this.TotalCountValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TotalCountValue.Visible = false;
            // 
            // TotalFailValue
            // 
            this.TotalFailValue.BackAlpha = 0;
            this.TotalFailValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.TotalFailValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TotalFailValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalFailValue.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.TotalFailValue.Location = new System.Drawing.Point(17, 376);
            this.TotalFailValue.Name = "TotalFailValue";
            this.TotalFailValue.Size = new System.Drawing.Size(258, 37);
            this.TotalFailValue.TabIndex = 518;
            this.TotalFailValue.Text = "0";
            this.TotalFailValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TotalFailValue.Visible = false;
            // 
            // alphaBlendTextBox2
            // 
            this.alphaBlendTextBox2.BackAlpha = 0;
            this.alphaBlendTextBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.alphaBlendTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.alphaBlendTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alphaBlendTextBox2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.alphaBlendTextBox2.Location = new System.Drawing.Point(276, 333);
            this.alphaBlendTextBox2.Name = "alphaBlendTextBox2";
            this.alphaBlendTextBox2.ReadOnly = true;
            this.alphaBlendTextBox2.Size = new System.Drawing.Size(150, 37);
            this.alphaBlendTextBox2.TabIndex = 521;
            this.alphaBlendTextBox2.TabStop = false;
            this.alphaBlendTextBox2.Text = "Reject %";
            // 
            // RejectPercentage
            // 
            this.RejectPercentage.BackAlpha = 0;
            this.RejectPercentage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.RejectPercentage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RejectPercentage.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RejectPercentage.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.RejectPercentage.Location = new System.Drawing.Point(276, 379);
            this.RejectPercentage.Name = "RejectPercentage";
            this.RejectPercentage.Size = new System.Drawing.Size(123, 37);
            this.RejectPercentage.TabIndex = 522;
            this.RejectPercentage.Text = "0";
            this.RejectPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RejectPercentage.Visible = false;
            // 
            // FinishNumber
            // 
            this.FinishNumber.BackAlpha = 0;
            this.FinishNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.FinishNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FinishNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FinishNumber.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.FinishNumber.Location = new System.Drawing.Point(139, 443);
            this.FinishNumber.Name = "FinishNumber";
            this.FinishNumber.Size = new System.Drawing.Size(135, 37);
            this.FinishNumber.TabIndex = 549;
            this.FinishNumber.Text = "0";
            // 
            // BaseNumber
            // 
            this.BaseNumber.BackAlpha = 0;
            this.BaseNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BaseNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BaseNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BaseNumber.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.BaseNumber.Location = new System.Drawing.Point(139, 499);
            this.BaseNumber.Name = "BaseNumber";
            this.BaseNumber.Size = new System.Drawing.Size(135, 37);
            this.BaseNumber.TabIndex = 550;
            this.BaseNumber.Text = "0";
            // 
            // ISWNumber
            // 
            this.ISWNumber.BackAlpha = 0;
            this.ISWNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ISWNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ISWNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ISWNumber.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.ISWNumber.Location = new System.Drawing.Point(139, 555);
            this.ISWNumber.Name = "ISWNumber";
            this.ISWNumber.Size = new System.Drawing.Size(135, 37);
            this.ISWNumber.TabIndex = 551;
            this.ISWNumber.Text = "0";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            // 
            // chartXAxisTextBox
            // 
            this.chartXAxisTextBox.Location = new System.Drawing.Point(17, 829);
            this.chartXAxisTextBox.Name = "chartXAxisTextBox";
            this.chartXAxisTextBox.Size = new System.Drawing.Size(100, 20);
            this.chartXAxisTextBox.TabIndex = 555;
            this.chartXAxisTextBox.Visible = false;
            // 
            // databaseIDTextBox
            // 
            this.databaseIDTextBox.Location = new System.Drawing.Point(162, 829);
            this.databaseIDTextBox.Name = "databaseIDTextBox";
            this.databaseIDTextBox.Size = new System.Drawing.Size(100, 20);
            this.databaseIDTextBox.TabIndex = 556;
            this.databaseIDTextBox.Visible = false;
            // 
            // coreTempTextbox1
            // 
            this.coreTempTextbox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.coreTempTextbox1.Location = new System.Drawing.Point(17, 790);
            this.coreTempTextbox1.Name = "coreTempTextbox1";
            this.coreTempTextbox1.Size = new System.Drawing.Size(40, 20);
            this.coreTempTextbox1.TabIndex = 563;
            this.coreTempTextbox1.Visible = false;
            // 
            // coreTempTextbox2
            // 
            this.coreTempTextbox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.coreTempTextbox2.Location = new System.Drawing.Point(63, 790);
            this.coreTempTextbox2.Name = "coreTempTextbox2";
            this.coreTempTextbox2.Size = new System.Drawing.Size(40, 20);
            this.coreTempTextbox2.TabIndex = 564;
            this.coreTempTextbox2.Visible = false;
            // 
            // coreTempTextbox3
            // 
            this.coreTempTextbox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.coreTempTextbox3.Location = new System.Drawing.Point(109, 790);
            this.coreTempTextbox3.Name = "coreTempTextbox3";
            this.coreTempTextbox3.Size = new System.Drawing.Size(40, 20);
            this.coreTempTextbox3.TabIndex = 565;
            this.coreTempTextbox3.Visible = false;
            // 
            // coreTempTextbox4
            // 
            this.coreTempTextbox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.coreTempTextbox4.Location = new System.Drawing.Point(155, 790);
            this.coreTempTextbox4.Name = "coreTempTextbox4";
            this.coreTempTextbox4.Size = new System.Drawing.Size(40, 20);
            this.coreTempTextbox4.TabIndex = 566;
            this.coreTempTextbox4.Visible = false;
            // 
            // ResetCounterBtn2
            // 
            this.ResetCounterBtn2.ButtonLabel = "";
            this.ResetCounterBtn2.ControlName = null;
            this.ResetCounterBtn2.Enabled = false;
            this.ResetCounterBtn2.HostName = null;
            this.ResetCounterBtn2.Location = new System.Drawing.Point(1129, 925);
            this.ResetCounterBtn2.Name = "ResetCounterBtn2";
            this.ResetCounterBtn2.PageName = null;
            this.ResetCounterBtn2.ProjectName = null;
            this.ResetCounterBtn2.Size = new System.Drawing.Size(129, 25);
            this.ResetCounterBtn2.TabIndex = 571;
            this.ResetCounterBtn2.Visible = false;
            // 
            // Cam1Scroll
            // 
            this.Cam1Scroll.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Cam1Scroll.BackgroundImage = global::OmniCheck_360.Properties.Resources.camera48x481;
            this.Cam1Scroll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Cam1Scroll.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.Cam1Scroll.FlatAppearance.BorderSize = 0;
            this.Cam1Scroll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cam1Scroll.Location = new System.Drawing.Point(720, 724);
            this.Cam1Scroll.Name = "Cam1Scroll";
            this.Cam1Scroll.Size = new System.Drawing.Size(75, 75);
            this.Cam1Scroll.TabIndex = 573;
            this.Cam1Scroll.Text = "1";
            this.Cam1Scroll.UseVisualStyleBackColor = false;
            this.Cam1Scroll.Click += new System.EventHandler(this.Cam1Scroll_Click);
            // 
            // Cam2Scroll
            // 
            this.Cam2Scroll.BackgroundImage = global::OmniCheck_360.Properties.Resources.camera48x481;
            this.Cam2Scroll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Cam2Scroll.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.Cam2Scroll.FlatAppearance.BorderSize = 0;
            this.Cam2Scroll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cam2Scroll.Location = new System.Drawing.Point(948, 724);
            this.Cam2Scroll.Name = "Cam2Scroll";
            this.Cam2Scroll.Size = new System.Drawing.Size(75, 75);
            this.Cam2Scroll.TabIndex = 574;
            this.Cam2Scroll.Text = "2";
            this.Cam2Scroll.UseVisualStyleBackColor = true;
            this.Cam2Scroll.Click += new System.EventHandler(this.Cam2Scroll_Click);
            // 
            // panelControl
            // 
            this.panelControl.Location = new System.Drawing.Point(605, 205);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(533, 400);
            this.panelControl.TabIndex = 563;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImage = global::OmniCheck_360.Properties.Resources.finish;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox3.Location = new System.Drawing.Point(17, 436);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(44, 50);
            this.pictureBox3.TabIndex = 576;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImage = global::OmniCheck_360.Properties.Resources._base;
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox4.Location = new System.Drawing.Point(17, 492);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(44, 50);
            this.pictureBox4.TabIndex = 577;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackgroundImage = global::OmniCheck_360.Properties.Resources.isw;
            this.pictureBox5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox5.Location = new System.Drawing.Point(17, 548);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(44, 50);
            this.pictureBox5.TabIndex = 578;
            this.pictureBox5.TabStop = false;
            // 
            // C2RecipeNameLb
            // 
            this.C2RecipeNameLb.BackColor = System.Drawing.Color.Transparent;
            this.C2RecipeNameLb.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.C2RecipeNameLb.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.C2RecipeNameLb.Location = new System.Drawing.Point(580, 83);
            this.C2RecipeNameLb.Name = "C2RecipeNameLb";
            this.C2RecipeNameLb.Size = new System.Drawing.Size(437, 37);
            this.C2RecipeNameLb.TabIndex = 580;
            this.C2RecipeNameLb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.C2RecipeNameLb.Visible = false;
            // 
            // C3RecipeNameLb
            // 
            this.C3RecipeNameLb.BackColor = System.Drawing.Color.Transparent;
            this.C3RecipeNameLb.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.C3RecipeNameLb.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.C3RecipeNameLb.Location = new System.Drawing.Point(582, 21);
            this.C3RecipeNameLb.Name = "C3RecipeNameLb";
            this.C3RecipeNameLb.Size = new System.Drawing.Size(437, 37);
            this.C3RecipeNameLb.TabIndex = 581;
            this.C3RecipeNameLb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.C3RecipeNameLb.Visible = false;
            // 
            // AllCameraDisplay
            // 
            this.AllCameraDisplay.BackColor = System.Drawing.Color.Transparent;
            this.AllCameraDisplay.BackgroundImage = global::OmniCheck_360.Properties.Resources.camera48x48;
            this.AllCameraDisplay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.AllCameraDisplay.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.AllCameraDisplay.FlatAppearance.BorderSize = 0;
            this.AllCameraDisplay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.AllCameraDisplay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.AllCameraDisplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AllCameraDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.AllCameraDisplay.ForeColor = System.Drawing.Color.Black;
            this.AllCameraDisplay.Location = new System.Drawing.Point(280, 880);
            this.AllCameraDisplay.Name = "AllCameraDisplay";
            this.AllCameraDisplay.Size = new System.Drawing.Size(240, 120);
            this.AllCameraDisplay.TabIndex = 582;
            this.AllCameraDisplay.UseMnemonic = false;
            this.AllCameraDisplay.UseVisualStyleBackColor = false;
            this.AllCameraDisplay.Click += new System.EventHandler(this.AllCameraDisplay_Click);
            // 
            // BtnSelectFlavour
            // 
            this.BtnSelectFlavour.BackColor = System.Drawing.Color.Transparent;
            this.BtnSelectFlavour.BackgroundImage = global::OmniCheck_360.Properties.Resources._48x48flavour;
            this.BtnSelectFlavour.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnSelectFlavour.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.BtnSelectFlavour.FlatAppearance.BorderSize = 0;
            this.BtnSelectFlavour.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnSelectFlavour.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnSelectFlavour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSelectFlavour.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.BtnSelectFlavour.ForeColor = System.Drawing.Color.Black;
            this.BtnSelectFlavour.Location = new System.Drawing.Point(280, 715);
            this.BtnSelectFlavour.Name = "BtnSelectFlavour";
            this.BtnSelectFlavour.Size = new System.Drawing.Size(240, 120);
            this.BtnSelectFlavour.TabIndex = 583;
            this.BtnSelectFlavour.UseMnemonic = false;
            this.BtnSelectFlavour.UseVisualStyleBackColor = false;
            this.BtnSelectFlavour.Click += new System.EventHandler(this.BtnSelectFlavour_Click);
            // 
            // ResetCounterBtn1
            // 
            this.ResetCounterBtn1.ButtonLabel = "";
            this.ResetCounterBtn1.ControlName = null;
            this.ResetCounterBtn1.Enabled = false;
            this.ResetCounterBtn1.HostName = null;
            this.ResetCounterBtn1.Location = new System.Drawing.Point(1129, 880);
            this.ResetCounterBtn1.Name = "ResetCounterBtn1";
            this.ResetCounterBtn1.PageName = null;
            this.ResetCounterBtn1.ProjectName = null;
            this.ResetCounterBtn1.Size = new System.Drawing.Size(129, 25);
            this.ResetCounterBtn1.TabIndex = 584;
            this.ResetCounterBtn1.Visible = false;
            // 
            // ResetCounterBtn3
            // 
            this.ResetCounterBtn3.ButtonLabel = "";
            this.ResetCounterBtn3.ControlName = null;
            this.ResetCounterBtn3.Enabled = false;
            this.ResetCounterBtn3.HostName = null;
            this.ResetCounterBtn3.Location = new System.Drawing.Point(1129, 975);
            this.ResetCounterBtn3.Name = "ResetCounterBtn3";
            this.ResetCounterBtn3.PageName = null;
            this.ResetCounterBtn3.ProjectName = null;
            this.ResetCounterBtn3.Size = new System.Drawing.Size(129, 25);
            this.ResetCounterBtn3.TabIndex = 585;
            this.ResetCounterBtn3.Visible = false;
            // 
            // lblCurrentFlavour
            // 
            this.lblCurrentFlavour.AutoSize = true;
            this.lblCurrentFlavour.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentFlavour.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblCurrentFlavour.Location = new System.Drawing.Point(198, 153);
            this.lblCurrentFlavour.Name = "lblCurrentFlavour";
            this.lblCurrentFlavour.Size = new System.Drawing.Size(106, 37);
            this.lblCurrentFlavour.TabIndex = 590;
            this.lblCurrentFlavour.Text = "label1";
            // 
            // txtPlcThroughput
            // 
            this.txtPlcThroughput.BackAlpha = 0;
            this.txtPlcThroughput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtPlcThroughput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPlcThroughput.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlcThroughput.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.txtPlcThroughput.Location = new System.Drawing.Point(17, 244);
            this.txtPlcThroughput.Name = "txtPlcThroughput";
            this.txtPlcThroughput.Size = new System.Drawing.Size(522, 73);
            this.txtPlcThroughput.TabIndex = 517;
            this.txtPlcThroughput.Text = "0";
            this.txtPlcThroughput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPlcRejects
            // 
            this.txtPlcRejects.BackAlpha = 0;
            this.txtPlcRejects.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtPlcRejects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPlcRejects.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlcRejects.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.txtPlcRejects.Location = new System.Drawing.Point(17, 376);
            this.txtPlcRejects.Name = "txtPlcRejects";
            this.txtPlcRejects.Size = new System.Drawing.Size(258, 37);
            this.txtPlcRejects.TabIndex = 591;
            this.txtPlcRejects.Text = "0";
            this.txtPlcRejects.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPlcRejectRate
            // 
            this.txtPlcRejectRate.BackAlpha = 0;
            this.txtPlcRejectRate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtPlcRejectRate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPlcRejectRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlcRejectRate.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.txtPlcRejectRate.Location = new System.Drawing.Point(283, 376);
            this.txtPlcRejectRate.Name = "txtPlcRejectRate";
            this.txtPlcRejectRate.Size = new System.Drawing.Size(123, 37);
            this.txtPlcRejectRate.TabIndex = 592;
            this.txtPlcRejectRate.Text = "0";
            this.txtPlcRejectRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // alphaBlendTextBox4
            // 
            this.alphaBlendTextBox4.BackAlpha = 0;
            this.alphaBlendTextBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.alphaBlendTextBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.alphaBlendTextBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alphaBlendTextBox4.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.alphaBlendTextBox4.Location = new System.Drawing.Point(459, 333);
            this.alphaBlendTextBox4.Name = "alphaBlendTextBox4";
            this.alphaBlendTextBox4.ReadOnly = true;
            this.alphaBlendTextBox4.Size = new System.Drawing.Size(86, 37);
            this.alphaBlendTextBox4.TabIndex = 593;
            this.alphaBlendTextBox4.TabStop = false;
            this.alphaBlendTextBox4.Text = "CPM";
            // 
            // txtCPM
            // 
            this.txtCPM.BackAlpha = 0;
            this.txtCPM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtCPM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCPM.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCPM.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.txtCPM.Location = new System.Drawing.Point(441, 376);
            this.txtCPM.Name = "txtCPM";
            this.txtCPM.Size = new System.Drawing.Size(123, 37);
            this.txtCPM.TabIndex = 594;
            this.txtCPM.Text = "0";
            this.txtCPM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackgroundImage = global::OmniCheck_360.Properties.Resources.downcan;
            this.pictureBox6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox6.Location = new System.Drawing.Point(17, 661);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(44, 50);
            this.pictureBox6.TabIndex = 595;
            this.pictureBox6.TabStop = false;
            // 
            // txtDownCan
            // 
            this.txtDownCan.BackAlpha = 0;
            this.txtDownCan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtDownCan.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDownCan.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDownCan.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.txtDownCan.Location = new System.Drawing.Point(139, 668);
            this.txtDownCan.Name = "txtDownCan";
            this.txtDownCan.Size = new System.Drawing.Size(135, 37);
            this.txtDownCan.TabIndex = 596;
            this.txtDownCan.Text = "0";
            // 
            // pictureBox7
            // 
            this.pictureBox7.BackgroundImage = global::OmniCheck_360.Properties.Resources.dentedcan;
            this.pictureBox7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox7.Location = new System.Drawing.Point(17, 604);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(44, 50);
            this.pictureBox7.TabIndex = 598;
            this.pictureBox7.TabStop = false;
            // 
            // DentNumber
            // 
            this.DentNumber.BackAlpha = 0;
            this.DentNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DentNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DentNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DentNumber.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.DentNumber.Location = new System.Drawing.Point(139, 611);
            this.DentNumber.Name = "DentNumber";
            this.DentNumber.Size = new System.Drawing.Size(135, 37);
            this.DentNumber.TabIndex = 597;
            this.DentNumber.Text = "0";
            // 
            // panelCam1Dots
            // 
            this.panelCam1Dots.Location = new System.Drawing.Point(720, 697);
            this.panelCam1Dots.Name = "panelCam1Dots";
            this.panelCam1Dots.Size = new System.Drawing.Size(120, 21);
            this.panelCam1Dots.TabIndex = 599;
            // 
            // panelCam2Dots
            // 
            this.panelCam2Dots.Location = new System.Drawing.Point(948, 697);
            this.panelCam2Dots.Name = "panelCam2Dots";
            this.panelCam2Dots.Size = new System.Drawing.Size(120, 21);
            this.panelCam2Dots.TabIndex = 600;
            // 
            // lblCardReaderStatus
            // 
            this.lblCardReaderStatus.AutoSize = true;
            this.lblCardReaderStatus.Location = new System.Drawing.Point(22, 755);
            this.lblCardReaderStatus.Name = "lblCardReaderStatus";
            this.lblCardReaderStatus.Size = new System.Drawing.Size(35, 13);
            this.lblCardReaderStatus.TabIndex = 601;
            this.lblCardReaderStatus.Text = "label1";
            this.lblCardReaderStatus.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1280, 1024);
            this.Controls.Add(this.lblCardReaderStatus);
            this.Controls.Add(this.panelCam2Dots);
            this.Controls.Add(this.panelCam1Dots);
            this.Controls.Add(this.pictureBox7);
            this.Controls.Add(this.DentNumber);
            this.Controls.Add(this.txtDownCan);
            this.Controls.Add(this.pictureBox6);
            this.Controls.Add(this.txtCPM);
            this.Controls.Add(this.alphaBlendTextBox4);
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.txtPlcRejectRate);
            this.Controls.Add(this.txtPlcRejects);
            this.Controls.Add(this.txtPlcThroughput);
            this.Controls.Add(this.lblCurrentFlavour);
            this.Controls.Add(this.ResetCounterBtn3);
            this.Controls.Add(this.ResetCounterBtn1);
            this.Controls.Add(this.BtnSelectFlavour);
            this.Controls.Add(this.AllCameraDisplay);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.Cam2Scroll);
            this.Controls.Add(this.Cam1Scroll);
            this.Controls.Add(this.coreTempTextbox4);
            this.Controls.Add(this.coreTempTextbox3);
            this.Controls.Add(this.coreTempTextbox2);
            this.Controls.Add(this.coreTempTextbox1);
            this.Controls.Add(this.databaseIDTextBox);
            this.Controls.Add(this.chartXAxisTextBox);
            this.Controls.Add(this.ISWNumber);
            this.Controls.Add(this.BaseNumber);
            this.Controls.Add(this.FinishNumber);
            this.Controls.Add(this.RejectPercentage);
            this.Controls.Add(this.alphaBlendTextBox2);
            this.Controls.Add(this.TotalFailValue);
            this.Controls.Add(this.TotalCountValue);
            this.Controls.Add(this.ResetCounterBtn);
            this.Controls.Add(this.MainMenuFormBtn);
            this.Controls.Add(this.alphaBlendTextBox3);
            this.Controls.Add(this.alphaBlendTextBox1);
            this.Controls.Add(this.C1RecipeNameLb);
            this.Controls.Add(this.ResetCounterBtn2);
            this.Controls.Add(this.C3RecipeNameLb);
            this.Controls.Add(this.C2RecipeNameLb);
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Controls.SetChildIndex(this.C2RecipeNameLb, 0);
            this.Controls.SetChildIndex(this.C3RecipeNameLb, 0);
            this.Controls.SetChildIndex(this.ResetCounterBtn2, 0);
            this.Controls.SetChildIndex(this.C1RecipeNameLb, 0);
            this.Controls.SetChildIndex(this.alphaBlendTextBox1, 0);
            this.Controls.SetChildIndex(this.alphaBlendTextBox3, 0);
            this.Controls.SetChildIndex(this.MainMenuFormBtn, 0);
            this.Controls.SetChildIndex(this.ResetCounterBtn, 0);
            this.Controls.SetChildIndex(this.TotalCountValue, 0);
            this.Controls.SetChildIndex(this.TotalFailValue, 0);
            this.Controls.SetChildIndex(this.alphaBlendTextBox2, 0);
            this.Controls.SetChildIndex(this.RejectPercentage, 0);
            this.Controls.SetChildIndex(this.DateTimeLabel, 0);
            this.Controls.SetChildIndex(this.CurrentUserTxt, 0);
            this.Controls.SetChildIndex(this.FinishNumber, 0);
            this.Controls.SetChildIndex(this.BaseNumber, 0);
            this.Controls.SetChildIndex(this.ISWNumber, 0);
            this.Controls.SetChildIndex(this.chartXAxisTextBox, 0);
            this.Controls.SetChildIndex(this.databaseIDTextBox, 0);
            this.Controls.SetChildIndex(this.coreTempTextbox1, 0);
            this.Controls.SetChildIndex(this.coreTempTextbox2, 0);
            this.Controls.SetChildIndex(this.coreTempTextbox3, 0);
            this.Controls.SetChildIndex(this.coreTempTextbox4, 0);
            this.Controls.SetChildIndex(this.Cam1Scroll, 0);
            this.Controls.SetChildIndex(this.Cam2Scroll, 0);
            this.Controls.SetChildIndex(this.pictureBox3, 0);
            this.Controls.SetChildIndex(this.pictureBox4, 0);
            this.Controls.SetChildIndex(this.pictureBox5, 0);
            this.Controls.SetChildIndex(this.AllCameraDisplay, 0);
            this.Controls.SetChildIndex(this.BtnSelectFlavour, 0);
            this.Controls.SetChildIndex(this.ResetCounterBtn1, 0);
            this.Controls.SetChildIndex(this.ResetCounterBtn3, 0);
            this.Controls.SetChildIndex(this.lblCurrentFlavour, 0);
            this.Controls.SetChildIndex(this.txtPlcThroughput, 0);
            this.Controls.SetChildIndex(this.txtPlcRejects, 0);
            this.Controls.SetChildIndex(this.txtPlcRejectRate, 0);
            this.Controls.SetChildIndex(this.panelControl, 0);
            this.Controls.SetChildIndex(this.alphaBlendTextBox4, 0);
            this.Controls.SetChildIndex(this.txtCPM, 0);
            this.Controls.SetChildIndex(this.pictureBox6, 0);
            this.Controls.SetChildIndex(this.txtDownCan, 0);
            this.Controls.SetChildIndex(this.DentNumber, 0);
            this.Controls.SetChildIndex(this.pictureBox7, 0);
            this.Controls.SetChildIndex(this.panelCam1Dots, 0);
            this.Controls.SetChildIndex(this.panelCam2Dots, 0);
            this.Controls.SetChildIndex(this.lblCardReaderStatus, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mainFormBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainMenuBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.circularBufferBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.BindingSource mainFormBindingSource;
        private System.Windows.Forms.BindingSource mainMenuBindingSource;
        private System.Windows.Forms.BindingSource circularBufferBindingSource;
        public System.Windows.Forms.Label C1RecipeNameLb;
        private System.Windows.Forms.Label label6;
        private ZBobb.AlphaBlendTextBox alphaBlendTextBox1;
        private ZBobb.AlphaBlendTextBox alphaBlendTextBox3;
        internal System.Windows.Forms.Button MainMenuFormBtn;
        internal System.Windows.Forms.Button ResetCounterBtn;
        private System.Windows.Forms.Timer DateTimeUpdate;
        private System.Data.Entity.Core.EntityClient.EntityCommand entityCommand1;
        private ZBobb.AlphaBlendTextBox TotalCountValue;
        private ZBobb.AlphaBlendTextBox TotalFailValue;
        private ZBobb.AlphaBlendTextBox alphaBlendTextBox2;
        private ZBobb.AlphaBlendTextBox RejectPercentage;
        private ZBobb.AlphaBlendTextBox FinishNumber;
        private ZBobb.AlphaBlendTextBox BaseNumber;
        private ZBobb.AlphaBlendTextBox ISWNumber;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox chartXAxisTextBox;
        public System.Windows.Forms.TextBox databaseIDTextBox;
        private System.Windows.Forms.TextBox coreTempTextbox2;
        private System.Windows.Forms.TextBox coreTempTextbox3;
        private System.Windows.Forms.TextBox coreTempTextbox4;
        public System.Windows.Forms.TextBox coreTempTextbox1;
        private Zebra.ADA.OperatorAPI.ControlsPackage.ButtonUIControl ResetCounterBtn2;
        private System.Windows.Forms.Button Cam1Scroll;
        private System.Windows.Forms.Button Cam2Scroll;
        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox5;
        public System.Windows.Forms.Label C2RecipeNameLb;
        public System.Windows.Forms.Label C3RecipeNameLb;
        internal System.Windows.Forms.Button AllCameraDisplay;
        internal System.Windows.Forms.Button BtnSelectFlavour;
        private Zebra.ADA.OperatorAPI.ControlsPackage.ButtonUIControl ResetCounterBtn1;
        private Zebra.ADA.OperatorAPI.ControlsPackage.ButtonUIControl ResetCounterBtn3;
        private System.Windows.Forms.Label lblCurrentFlavour;
        private ZBobb.AlphaBlendTextBox txtPlcThroughput;
        private ZBobb.AlphaBlendTextBox txtPlcRejects;
        private ZBobb.AlphaBlendTextBox txtPlcRejectRate;
        private ZBobb.AlphaBlendTextBox alphaBlendTextBox4;
        private ZBobb.AlphaBlendTextBox txtCPM;
        private System.Windows.Forms.PictureBox pictureBox6;
        private ZBobb.AlphaBlendTextBox txtDownCan;
        private System.Windows.Forms.PictureBox pictureBox7;
        private ZBobb.AlphaBlendTextBox DentNumber;
        private System.Windows.Forms.Panel panelCam1Dots;
        private System.Windows.Forms.Panel panelCam2Dots;
        private System.Windows.Forms.Label lblCardReaderStatus;
    }
}
