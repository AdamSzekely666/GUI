namespace MatroxLDS
{
    partial class AllCameraDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AllCameraDisplay));
            this.DashboardBtn = new System.Windows.Forms.Button();
            this.displayControl1 = new Zebra.ADA.OperatorAPI.ControlsPackage.DisplayControl();
            this.displayControl2 = new Zebra.ADA.OperatorAPI.ControlsPackage.DisplayControl();
            this.picECI1Inspect = new System.Windows.Forms.PictureBox();
            this.picECI1Reject = new System.Windows.Forms.PictureBox();
            this.picECI2DentInspect = new System.Windows.Forms.PictureBox();
            this.picECI2ISWInspect = new System.Windows.Forms.PictureBox();
            this.picECI2BaseInspect = new System.Windows.Forms.PictureBox();
            this.picECI2BaseReject = new System.Windows.Forms.PictureBox();
            this.picECI2ISWReject = new System.Windows.Forms.PictureBox();
            this.picECI2DentReject = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picECI1Inspect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picECI1Reject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picECI2DentInspect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picECI2ISWInspect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picECI2BaseInspect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picECI2BaseReject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picECI2ISWReject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picECI2DentReject)).BeginInit();
            this.SuspendLayout();
            // 
            // CurrentUserTxt
            // 
            this.CurrentUserTxt.Size = new System.Drawing.Size(252, 28);
            // 
            // DashboardBtn
            // 
            this.DashboardBtn.BackColor = System.Drawing.Color.Transparent;
            this.DashboardBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("DashboardBtn.BackgroundImage")));
            this.DashboardBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.DashboardBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.DashboardBtn.FlatAppearance.BorderSize = 2;
            this.DashboardBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.DashboardBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.DashboardBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DashboardBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.DashboardBtn.ForeColor = System.Drawing.Color.Black;
            this.DashboardBtn.Location = new System.Drawing.Point(560, 880);
            this.DashboardBtn.Name = "DashboardBtn";
            this.DashboardBtn.Size = new System.Drawing.Size(240, 120);
            this.DashboardBtn.TabIndex = 516;
            this.DashboardBtn.UseMnemonic = false;
            this.DashboardBtn.UseVisualStyleBackColor = false;
            this.DashboardBtn.Click += new System.EventHandler(this.DashboardBtn_Click);
            // 
            // displayControl1
            // 
            this.displayControl1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.displayControl1.ControlName = "Finish";
            this.displayControl1.DisplayName = "Finish";
            this.displayControl1.HostName = "localhost";
            this.displayControl1.Location = new System.Drawing.Point(50, 137);
            this.displayControl1.Name = "displayControl1";
            this.displayControl1.PageName = "OperatorView";
            this.displayControl1.ProjectName = "ECI1";
            this.displayControl1.Size = new System.Drawing.Size(533, 400);
            this.displayControl1.TabIndex = 517;
            // 
            // displayControl2
            // 
            this.displayControl2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.displayControl2.ControlName = "BaseInspection";
            this.displayControl2.DisplayName = "BaseInspection";
            this.displayControl2.HostName = "localhost";
            this.displayControl2.Location = new System.Drawing.Point(690, 137);
            this.displayControl2.Name = "displayControl2";
            this.displayControl2.PageName = "OperatorView";
            this.displayControl2.ProjectName = "ECI2";
            this.displayControl2.Size = new System.Drawing.Size(533, 400);
            this.displayControl2.TabIndex = 518;
            // 
            // picECI1Inspect
            // 
            this.picECI1Inspect.BackgroundImage = global::OmniCheck_360.Properties.Resources.finish;
            this.picECI1Inspect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picECI1Inspect.Location = new System.Drawing.Point(236, 590);
            this.picECI1Inspect.Name = "picECI1Inspect";
            this.picECI1Inspect.Size = new System.Drawing.Size(44, 50);
            this.picECI1Inspect.TabIndex = 577;
            this.picECI1Inspect.TabStop = false;
            // 
            // picECI1Reject
            // 
            this.picECI1Reject.BackgroundImage = global::OmniCheck_360.Properties.Resources.camerafreeze;
            this.picECI1Reject.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picECI1Reject.Location = new System.Drawing.Point(363, 590);
            this.picECI1Reject.Name = "picECI1Reject";
            this.picECI1Reject.Size = new System.Drawing.Size(44, 50);
            this.picECI1Reject.TabIndex = 578;
            this.picECI1Reject.TabStop = false;
            // 
            // picECI2DentInspect
            // 
            this.picECI2DentInspect.BackgroundImage = global::OmniCheck_360.Properties.Resources.isw;
            this.picECI2DentInspect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picECI2DentInspect.Location = new System.Drawing.Point(900, 702);
            this.picECI2DentInspect.Name = "picECI2DentInspect";
            this.picECI2DentInspect.Size = new System.Drawing.Size(44, 50);
            this.picECI2DentInspect.TabIndex = 601;
            this.picECI2DentInspect.TabStop = false;
            // 
            // picECI2ISWInspect
            // 
            this.picECI2ISWInspect.BackgroundImage = global::OmniCheck_360.Properties.Resources.isw;
            this.picECI2ISWInspect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picECI2ISWInspect.Location = new System.Drawing.Point(900, 646);
            this.picECI2ISWInspect.Name = "picECI2ISWInspect";
            this.picECI2ISWInspect.Size = new System.Drawing.Size(44, 50);
            this.picECI2ISWInspect.TabIndex = 600;
            this.picECI2ISWInspect.TabStop = false;
            // 
            // picECI2BaseInspect
            // 
            this.picECI2BaseInspect.BackgroundImage = global::OmniCheck_360.Properties.Resources._base;
            this.picECI2BaseInspect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picECI2BaseInspect.Location = new System.Drawing.Point(900, 590);
            this.picECI2BaseInspect.Name = "picECI2BaseInspect";
            this.picECI2BaseInspect.Size = new System.Drawing.Size(44, 50);
            this.picECI2BaseInspect.TabIndex = 599;
            this.picECI2BaseInspect.TabStop = false;
            // 
            // picECI2BaseReject
            // 
            this.picECI2BaseReject.BackgroundImage = global::OmniCheck_360.Properties.Resources.camerafreeze;
            this.picECI2BaseReject.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picECI2BaseReject.Location = new System.Drawing.Point(1004, 590);
            this.picECI2BaseReject.Name = "picECI2BaseReject";
            this.picECI2BaseReject.Size = new System.Drawing.Size(44, 50);
            this.picECI2BaseReject.TabIndex = 602;
            this.picECI2BaseReject.TabStop = false;
            // 
            // picECI2ISWReject
            // 
            this.picECI2ISWReject.BackgroundImage = global::OmniCheck_360.Properties.Resources.camerafreeze;
            this.picECI2ISWReject.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picECI2ISWReject.Location = new System.Drawing.Point(1004, 646);
            this.picECI2ISWReject.Name = "picECI2ISWReject";
            this.picECI2ISWReject.Size = new System.Drawing.Size(44, 50);
            this.picECI2ISWReject.TabIndex = 603;
            this.picECI2ISWReject.TabStop = false;
            // 
            // picECI2DentReject
            // 
            this.picECI2DentReject.BackgroundImage = global::OmniCheck_360.Properties.Resources.camerafreeze;
            this.picECI2DentReject.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picECI2DentReject.Location = new System.Drawing.Point(1004, 702);
            this.picECI2DentReject.Name = "picECI2DentReject";
            this.picECI2DentReject.Size = new System.Drawing.Size(44, 50);
            this.picECI2DentReject.TabIndex = 604;
            this.picECI2DentReject.TabStop = false;
            // 
            // AllCameraDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1280, 1024);
            this.Controls.Add(this.picECI2DentReject);
            this.Controls.Add(this.picECI2ISWReject);
            this.Controls.Add(this.picECI2BaseReject);
            this.Controls.Add(this.picECI2DentInspect);
            this.Controls.Add(this.picECI2ISWInspect);
            this.Controls.Add(this.picECI2BaseInspect);
            this.Controls.Add(this.picECI1Reject);
            this.Controls.Add(this.picECI1Inspect);
            this.Controls.Add(this.displayControl2);
            this.Controls.Add(this.displayControl1);
            this.Controls.Add(this.DashboardBtn);
            this.Name = "AllCameraDisplay";
            this.Load += new System.EventHandler(this.AllCameraDisplay_Load);
            this.Controls.SetChildIndex(this.DateTimeLabel, 0);
            this.Controls.SetChildIndex(this.CurrentUserTxt, 0);
            this.Controls.SetChildIndex(this.DashboardBtn, 0);
            this.Controls.SetChildIndex(this.displayControl1, 0);
            this.Controls.SetChildIndex(this.displayControl2, 0);
            this.Controls.SetChildIndex(this.picECI1Inspect, 0);
            this.Controls.SetChildIndex(this.picECI1Reject, 0);
            this.Controls.SetChildIndex(this.picECI2BaseInspect, 0);
            this.Controls.SetChildIndex(this.picECI2ISWInspect, 0);
            this.Controls.SetChildIndex(this.picECI2DentInspect, 0);
            this.Controls.SetChildIndex(this.picECI2BaseReject, 0);
            this.Controls.SetChildIndex(this.picECI2ISWReject, 0);
            this.Controls.SetChildIndex(this.picECI2DentReject, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picECI1Inspect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picECI1Reject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picECI2DentInspect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picECI2ISWInspect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picECI2BaseInspect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picECI2BaseReject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picECI2ISWReject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picECI2DentReject)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.Button DashboardBtn;
        private Zebra.ADA.OperatorAPI.ControlsPackage.DisplayControl displayControl1;
        private Zebra.ADA.OperatorAPI.ControlsPackage.DisplayControl displayControl2;
        private System.Windows.Forms.PictureBox picECI1Inspect;
        private System.Windows.Forms.PictureBox picECI1Reject;
        private System.Windows.Forms.PictureBox picECI2DentInspect;
        private System.Windows.Forms.PictureBox picECI2ISWInspect;
        private System.Windows.Forms.PictureBox picECI2BaseInspect;
        private System.Windows.Forms.PictureBox picECI2BaseReject;
        private System.Windows.Forms.PictureBox picECI2ISWReject;
        private System.Windows.Forms.PictureBox picECI2DentReject;
    }
}
