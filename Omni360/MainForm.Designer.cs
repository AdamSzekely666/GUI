namespace Omnicheck360
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
        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label6 = new System.Windows.Forms.Label();
            this.alphaBlendTextBox1 = new ZBobb.AlphaBlendTextBox();
            this.alphaBlendTextBox3 = new ZBobb.AlphaBlendTextBox();
            this.MainMenuFormBtn = new System.Windows.Forms.Button();
            this.ResetCounterBtn = new System.Windows.Forms.Button();
            this.TotalCountValue = new ZBobb.AlphaBlendTextBox();
            this.TotalFailValue = new ZBobb.AlphaBlendTextBox();
            this.alphaBlendTextBox2 = new ZBobb.AlphaBlendTextBox();
            this.RejectPercentage = new ZBobb.AlphaBlendTextBox();
            this.PLCUpdate = new System.Windows.Forms.Timer(this.components);
            this.sqLiteCommand1 = new System.Data.SQLite.SQLiteCommand();
            this.coreRA1 = new FZ_Control.CoreRA();
            this.BPMValue = new ZBobb.AlphaBlendTextBox();
            this.alphaBlendTextBox5 = new ZBobb.AlphaBlendTextBox();
            this.imageWindow1 = new FZ_Control.ImageWindow();
            this.coreRA3 = new FZ_Control.CoreRA();
            this.ResetMaxConsecutiveBtn = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.OmronRej = new ZBobb.AlphaBlendTextBox();
            this.TotalTrigger = new ZBobb.AlphaBlendTextBox();
            this.coreRA4 = new FZ_Control.CoreRA();
            this.imageWindow2 = new FZ_Control.ImageWindow();
            this.coreRA2 = new FZ_Control.CoreRA();
            this.BtnSelectFlavour = new System.Windows.Forms.Button();
            this.DescriptionNameLb = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.FinishNumber = new ZBobb.AlphaBlendTextBox();
            this.BaseNumber = new ZBobb.AlphaBlendTextBox();
            this.ISWNumber = new ZBobb.AlphaBlendTextBox();
            this.DentNumber = new ZBobb.AlphaBlendTextBox();
            this.DownCanNumber = new ZBobb.AlphaBlendTextBox();
            this.Cam1Scroll = new System.Windows.Forms.Button();
            this.Cam2Scroll = new System.Windows.Forms.Button();
            this.panelCam1Dots = new System.Windows.Forms.Panel();
            this.panelCam2Dots = new System.Windows.Forms.Panel();
            this.panelControl = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.panelControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // DateTimeLabel
            // 
            resources.ApplyResources(this.DateTimeLabel, "DateTimeLabel");
            // 
            // CurrentUserTxt
            // 
            resources.ApplyResources(this.CurrentUserTxt, "CurrentUserTxt");
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // alphaBlendTextBox1
            // 
            this.alphaBlendTextBox1.BackAlpha = 0;
            this.alphaBlendTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.alphaBlendTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.alphaBlendTextBox1, "alphaBlendTextBox1");
            this.alphaBlendTextBox1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.alphaBlendTextBox1.Name = "alphaBlendTextBox1";
            this.alphaBlendTextBox1.ReadOnly = true;
            // 
            // alphaBlendTextBox3
            // 
            this.alphaBlendTextBox3.BackAlpha = 0;
            this.alphaBlendTextBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.alphaBlendTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.alphaBlendTextBox3, "alphaBlendTextBox3");
            this.alphaBlendTextBox3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.alphaBlendTextBox3.Name = "alphaBlendTextBox3";
            this.alphaBlendTextBox3.ReadOnly = true;
            // 
            // MainMenuFormBtn
            // 
            this.MainMenuFormBtn.BackColor = System.Drawing.Color.Transparent;
            this.MainMenuFormBtn.BackgroundImage = global::Omnicheck360.Properties.Resources.mainmaneu48x48;
            resources.ApplyResources(this.MainMenuFormBtn, "MainMenuFormBtn");
            this.MainMenuFormBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.MainMenuFormBtn.FlatAppearance.BorderSize = 0;
            this.MainMenuFormBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.MainMenuFormBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.MainMenuFormBtn.ForeColor = System.Drawing.Color.Black;
            this.MainMenuFormBtn.Name = "MainMenuFormBtn";
            this.MainMenuFormBtn.UseMnemonic = false;
            this.MainMenuFormBtn.UseVisualStyleBackColor = false;
            this.MainMenuFormBtn.Click += new System.EventHandler(this.MainMenuFormBtn_Click);
            // 
            // ResetCounterBtn
            // 
            this.ResetCounterBtn.BackColor = System.Drawing.Color.Transparent;
            this.ResetCounterBtn.BackgroundImage = global::Omnicheck360.Properties.Resources.resetcount48x48;
            resources.ApplyResources(this.ResetCounterBtn, "ResetCounterBtn");
            this.ResetCounterBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.ResetCounterBtn.FlatAppearance.BorderSize = 0;
            this.ResetCounterBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ResetCounterBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ResetCounterBtn.ForeColor = System.Drawing.Color.Black;
            this.ResetCounterBtn.Name = "ResetCounterBtn";
            this.ResetCounterBtn.UseMnemonic = false;
            this.ResetCounterBtn.UseVisualStyleBackColor = false;
            this.ResetCounterBtn.Click += new System.EventHandler(this.ResetCounterBtn_Click);
            // 
            // TotalCountValue
            // 
            this.TotalCountValue.BackAlpha = 0;
            this.TotalCountValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.TotalCountValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.TotalCountValue, "TotalCountValue");
            this.TotalCountValue.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.TotalCountValue.Name = "TotalCountValue";
            this.TotalCountValue.ReadOnly = true;
            // 
            // TotalFailValue
            // 
            this.TotalFailValue.BackAlpha = 0;
            this.TotalFailValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.TotalFailValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.TotalFailValue, "TotalFailValue");
            this.TotalFailValue.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.TotalFailValue.Name = "TotalFailValue";
            this.TotalFailValue.ReadOnly = true;
            // 
            // alphaBlendTextBox2
            // 
            this.alphaBlendTextBox2.BackAlpha = 0;
            this.alphaBlendTextBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.alphaBlendTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.alphaBlendTextBox2, "alphaBlendTextBox2");
            this.alphaBlendTextBox2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.alphaBlendTextBox2.Name = "alphaBlendTextBox2";
            this.alphaBlendTextBox2.ReadOnly = true;
            // 
            // RejectPercentage
            // 
            this.RejectPercentage.BackAlpha = 0;
            this.RejectPercentage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.RejectPercentage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.RejectPercentage, "RejectPercentage");
            this.RejectPercentage.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.RejectPercentage.Name = "RejectPercentage";
            // 
            // PLCUpdate
            // 
            this.PLCUpdate.Interval = 250;
            this.PLCUpdate.Tick += new System.EventHandler(this.PLCUpdate_Tick);
            // 
            // sqLiteCommand1
            // 
            this.sqLiteCommand1.CommandText = null;
            // 
            // coreRA1
            // 
            this.coreRA1.ContainerControl = this;
            // 
            // BPMValue
            // 
            this.BPMValue.BackAlpha = 0;
            this.BPMValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BPMValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.BPMValue, "BPMValue");
            this.BPMValue.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.BPMValue.Name = "BPMValue";
            this.BPMValue.ReadOnly = true;
            // 
            // alphaBlendTextBox5
            // 
            this.alphaBlendTextBox5.BackAlpha = 0;
            this.alphaBlendTextBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.alphaBlendTextBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.alphaBlendTextBox5, "alphaBlendTextBox5");
            this.alphaBlendTextBox5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.alphaBlendTextBox5.Name = "alphaBlendTextBox5";
            this.alphaBlendTextBox5.ReadOnly = true;
            // 
            // imageWindow1
            // 
            this.imageWindow1.BackColor = System.Drawing.SystemColors.Control;
            this.imageWindow1.ConnectCoreRAComponent = this.coreRA1;
            resources.ApplyResources(this.imageWindow1, "imageWindow1");
            this.imageWindow1.Name = "imageWindow1";
            this.imageWindow1.SubNo = 1;
            this.imageWindow1.UnitNo = 2;
            this.imageWindow1.WindowNo = 24;
            // 
            // coreRA3
            // 
            this.coreRA3.ContainerControl = this;
            // 
            // ResetMaxConsecutiveBtn
            // 
            this.ResetMaxConsecutiveBtn.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.ResetMaxConsecutiveBtn, "ResetMaxConsecutiveBtn");
            this.ResetMaxConsecutiveBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.ResetMaxConsecutiveBtn.FlatAppearance.BorderSize = 0;
            this.ResetMaxConsecutiveBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ResetMaxConsecutiveBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ResetMaxConsecutiveBtn.ForeColor = System.Drawing.Color.Black;
            this.ResetMaxConsecutiveBtn.Name = "ResetMaxConsecutiveBtn";
            this.ResetMaxConsecutiveBtn.UseMnemonic = false;
            this.ResetMaxConsecutiveBtn.UseVisualStyleBackColor = false;
            this.ResetMaxConsecutiveBtn.Click += new System.EventHandler(this.ResetMaxConsecutiveBtn_Click);
            // 
            // timer2
            // 
            this.timer2.Interval = 250;
            // 
            // OmronRej
            // 
            this.OmronRej.BackAlpha = 0;
            this.OmronRej.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.OmronRej.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.OmronRej, "OmronRej");
            this.OmronRej.Name = "OmronRej";
            this.OmronRej.ReadOnly = true;
            // 
            // TotalTrigger
            // 
            this.TotalTrigger.BackAlpha = 0;
            this.TotalTrigger.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.TotalTrigger.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.TotalTrigger, "TotalTrigger");
            this.TotalTrigger.Name = "TotalTrigger";
            this.TotalTrigger.ReadOnly = true;
            // 
            // coreRA4
            // 
            this.coreRA4.ContainerControl = this;
            // 
            // imageWindow2
            // 
            this.imageWindow2.BackColor = System.Drawing.SystemColors.Control;
            this.imageWindow2.ConnectCoreRAComponent = this.coreRA2;
            resources.ApplyResources(this.imageWindow2, "imageWindow2");
            this.imageWindow2.Name = "imageWindow2";
            this.imageWindow2.SubNo = 1;
            this.imageWindow2.UnitNo = 1;
            this.imageWindow2.WindowNo = 24;
            // 
            // coreRA2
            // 
            this.coreRA2.ContainerControl = this;
            this.coreRA2.LineNo = 1;
            // 
            // BtnSelectFlavour
            // 
            this.BtnSelectFlavour.BackColor = System.Drawing.Color.Transparent;
            this.BtnSelectFlavour.BackgroundImage = global::Omnicheck360.Properties.Resources._48x48flavour;
            resources.ApplyResources(this.BtnSelectFlavour, "BtnSelectFlavour");
            this.BtnSelectFlavour.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.BtnSelectFlavour.FlatAppearance.BorderSize = 0;
            this.BtnSelectFlavour.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnSelectFlavour.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnSelectFlavour.ForeColor = System.Drawing.Color.Black;
            this.BtnSelectFlavour.Name = "BtnSelectFlavour";
            this.BtnSelectFlavour.UseMnemonic = false;
            this.BtnSelectFlavour.UseVisualStyleBackColor = false;
            this.BtnSelectFlavour.Click += new System.EventHandler(this.ChangeRecipeBtn_Click);
            // 
            // DescriptionNameLb
            // 
            this.DescriptionNameLb.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.DescriptionNameLb, "DescriptionNameLb");
            this.DescriptionNameLb.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.DescriptionNameLb.Name = "DescriptionNameLb";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pictureBox2.BackgroundImage = global::Omnicheck360.Properties.Resources.finish;
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pictureBox3.BackgroundImage = global::Omnicheck360.Properties.Resources._base;
            resources.ApplyResources(this.pictureBox3, "pictureBox3");
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pictureBox4.BackgroundImage = global::Omnicheck360.Properties.Resources.isw;
            resources.ApplyResources(this.pictureBox4, "pictureBox4");
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pictureBox5.BackgroundImage = global::Omnicheck360.Properties.Resources.dentedcan;
            resources.ApplyResources(this.pictureBox5, "pictureBox5");
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pictureBox6.BackgroundImage = global::Omnicheck360.Properties.Resources.downcan;
            resources.ApplyResources(this.pictureBox6, "pictureBox6");
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.TabStop = false;
            // 
            // FinishNumber
            // 
            this.FinishNumber.BackAlpha = 0;
            this.FinishNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.FinishNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.FinishNumber, "FinishNumber");
            this.FinishNumber.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.FinishNumber.Name = "FinishNumber";
            // 
            // BaseNumber
            // 
            this.BaseNumber.BackAlpha = 0;
            this.BaseNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BaseNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.BaseNumber, "BaseNumber");
            this.BaseNumber.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.BaseNumber.Name = "BaseNumber";
            // 
            // ISWNumber
            // 
            this.ISWNumber.BackAlpha = 0;
            this.ISWNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ISWNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.ISWNumber, "ISWNumber");
            this.ISWNumber.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.ISWNumber.Name = "ISWNumber";
            // 
            // DentNumber
            // 
            this.DentNumber.BackAlpha = 0;
            this.DentNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DentNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.DentNumber, "DentNumber");
            this.DentNumber.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.DentNumber.Name = "DentNumber";
            // 
            // DownCanNumber
            // 
            this.DownCanNumber.BackAlpha = 0;
            this.DownCanNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DownCanNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.DownCanNumber, "DownCanNumber");
            this.DownCanNumber.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.DownCanNumber.Name = "DownCanNumber";
            // 
            // Cam1Scroll
            // 
            this.Cam1Scroll.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Cam1Scroll.BackgroundImage = global::Omnicheck360.Properties.Resources.camera48x48;
            resources.ApplyResources(this.Cam1Scroll, "Cam1Scroll");
            this.Cam1Scroll.Name = "Cam1Scroll";
            this.Cam1Scroll.UseVisualStyleBackColor = false;
            // 
            // Cam2Scroll
            // 
            this.Cam2Scroll.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Cam2Scroll.BackgroundImage = global::Omnicheck360.Properties.Resources.camera48x48;
            resources.ApplyResources(this.Cam2Scroll, "Cam2Scroll");
            this.Cam2Scroll.Name = "Cam2Scroll";
            this.Cam2Scroll.UseVisualStyleBackColor = false;
            // 
            // panelCam1Dots
            // 
            resources.ApplyResources(this.panelCam1Dots, "panelCam1Dots");
            this.panelCam1Dots.Name = "panelCam1Dots";
            // 
            // panelCam2Dots
            // 
            resources.ApplyResources(this.panelCam2Dots, "panelCam2Dots");
            this.panelCam2Dots.Name = "panelCam2Dots";
            // 
            // panelControl
            // 
            this.panelControl.Controls.Add(this.imageWindow1);
            resources.ApplyResources(this.panelControl, "panelControl");
            this.panelControl.Name = "panelControl";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.panelCam2Dots);
            this.Controls.Add(this.panelCam1Dots);
            this.Controls.Add(this.Cam2Scroll);
            this.Controls.Add(this.Cam1Scroll);
            this.Controls.Add(this.DownCanNumber);
            this.Controls.Add(this.DentNumber);
            this.Controls.Add(this.ISWNumber);
            this.Controls.Add(this.BaseNumber);
            this.Controls.Add(this.FinishNumber);
            this.Controls.Add(this.pictureBox6);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.ResetMaxConsecutiveBtn);
            this.Controls.Add(this.DescriptionNameLb);
            this.Controls.Add(this.BtnSelectFlavour);
            this.Controls.Add(this.TotalTrigger);
            this.Controls.Add(this.OmronRej);
            this.Controls.Add(this.BPMValue);
            this.Controls.Add(this.alphaBlendTextBox5);
            this.Controls.Add(this.RejectPercentage);
            this.Controls.Add(this.alphaBlendTextBox2);
            this.Controls.Add(this.TotalFailValue);
            this.Controls.Add(this.TotalCountValue);
            this.Controls.Add(this.ResetCounterBtn);
            this.Controls.Add(this.MainMenuFormBtn);
            this.Controls.Add(this.alphaBlendTextBox3);
            this.Controls.Add(this.alphaBlendTextBox1);
            this.Controls.Add(this.imageWindow2);
            this.Name = "MainForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Controls.SetChildIndex(this.imageWindow2, 0);
            this.Controls.SetChildIndex(this.alphaBlendTextBox1, 0);
            this.Controls.SetChildIndex(this.alphaBlendTextBox3, 0);
            this.Controls.SetChildIndex(this.MainMenuFormBtn, 0);
            this.Controls.SetChildIndex(this.ResetCounterBtn, 0);
            this.Controls.SetChildIndex(this.TotalCountValue, 0);
            this.Controls.SetChildIndex(this.TotalFailValue, 0);
            this.Controls.SetChildIndex(this.alphaBlendTextBox2, 0);
            this.Controls.SetChildIndex(this.RejectPercentage, 0);
            this.Controls.SetChildIndex(this.alphaBlendTextBox5, 0);
            this.Controls.SetChildIndex(this.BPMValue, 0);
            this.Controls.SetChildIndex(this.OmronRej, 0);
            this.Controls.SetChildIndex(this.TotalTrigger, 0);
            this.Controls.SetChildIndex(this.DateTimeLabel, 0);
            this.Controls.SetChildIndex(this.CurrentUserTxt, 0);
            this.Controls.SetChildIndex(this.BtnSelectFlavour, 0);
            this.Controls.SetChildIndex(this.DescriptionNameLb, 0);
            this.Controls.SetChildIndex(this.ResetMaxConsecutiveBtn, 0);
            this.Controls.SetChildIndex(this.pictureBox2, 0);
            this.Controls.SetChildIndex(this.pictureBox3, 0);
            this.Controls.SetChildIndex(this.pictureBox4, 0);
            this.Controls.SetChildIndex(this.pictureBox5, 0);
            this.Controls.SetChildIndex(this.pictureBox6, 0);
            this.Controls.SetChildIndex(this.FinishNumber, 0);
            this.Controls.SetChildIndex(this.BaseNumber, 0);
            this.Controls.SetChildIndex(this.ISWNumber, 0);
            this.Controls.SetChildIndex(this.DentNumber, 0);
            this.Controls.SetChildIndex(this.DownCanNumber, 0);
            this.Controls.SetChildIndex(this.Cam1Scroll, 0);
            this.Controls.SetChildIndex(this.Cam2Scroll, 0);
            this.Controls.SetChildIndex(this.panelCam1Dots, 0);
            this.Controls.SetChildIndex(this.panelCam2Dots, 0);
            this.Controls.SetChildIndex(this.panelControl, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.panelControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label6;
        private ZBobb.AlphaBlendTextBox alphaBlendTextBox1;
        private ZBobb.AlphaBlendTextBox alphaBlendTextBox3;
        internal System.Windows.Forms.Button MainMenuFormBtn;
        internal System.Windows.Forms.Button ResetCounterBtn;
        public ZBobb.AlphaBlendTextBox TotalCountValue;
        public ZBobb.AlphaBlendTextBox TotalFailValue;
        public ZBobb.AlphaBlendTextBox alphaBlendTextBox2;
        public ZBobb.AlphaBlendTextBox RejectPercentage;
        public System.Windows.Forms.Timer PLCUpdate;
        private System.Data.SQLite.SQLiteCommand sqLiteCommand1;
        public FZ_Control.CoreRA coreRA1;
        private ZBobb.AlphaBlendTextBox BPMValue;
        private ZBobb.AlphaBlendTextBox alphaBlendTextBox5;
        public FZ_Control.ImageWindow imageWindow1;
        internal System.Windows.Forms.Button ResetMaxConsecutiveBtn;
        public System.Windows.Forms.Timer timer2;
        public ZBobb.AlphaBlendTextBox OmronRej;
        public ZBobb.AlphaBlendTextBox TotalTrigger;
        public FZ_Control.CoreRA coreRA3;
        public FZ_Control.ImageWindow imageWindow2;
        public FZ_Control.CoreRA coreRA2;
        public FZ_Control.CoreRA coreRA4;
        internal System.Windows.Forms.Button BtnSelectFlavour;
        public System.Windows.Forms.Label DescriptionNameLb;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        public ZBobb.AlphaBlendTextBox FinishNumber;
        public ZBobb.AlphaBlendTextBox DownCanNumber;
        public ZBobb.AlphaBlendTextBox DentNumber;
        public ZBobb.AlphaBlendTextBox ISWNumber;
        public ZBobb.AlphaBlendTextBox BaseNumber;
        private System.Windows.Forms.Button Cam1Scroll;
        private System.Windows.Forms.Panel panelCam1Dots;
        private System.Windows.Forms.Button Cam2Scroll;
        private System.Windows.Forms.Panel panelCam2Dots;
        private System.Windows.Forms.Panel panelControl;
    }
}
