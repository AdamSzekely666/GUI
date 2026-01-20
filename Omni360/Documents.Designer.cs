namespace Omnicheck360
{
    partial class Documents
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Documents));
            this.OperatingManualBtn = new System.Windows.Forms.Button();
            this.TroubleshootingManualBtn = new System.Windows.Forms.Button();
            this.PartsListBtn = new System.Windows.Forms.Button();
            this.MainMenuFormBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OperatingManualBtn
            // 
            this.OperatingManualBtn.BackColor = System.Drawing.Color.Transparent;
            this.OperatingManualBtn.BackgroundImage = global::Omnicheck360.Properties.Resources.data_processing;
            resources.ApplyResources(this.OperatingManualBtn, "OperatingManualBtn");
            this.OperatingManualBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.OperatingManualBtn.FlatAppearance.BorderSize = 0;
            this.OperatingManualBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.OperatingManualBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.OperatingManualBtn.ForeColor = System.Drawing.Color.Black;
            this.OperatingManualBtn.Name = "OperatingManualBtn";
            this.OperatingManualBtn.UseMnemonic = false;
            this.OperatingManualBtn.UseVisualStyleBackColor = false;
            this.OperatingManualBtn.Click += new System.EventHandler(this.OperatingManualBtn_Click);
            // 
            // TroubleshootingManualBtn
            // 
            this.TroubleshootingManualBtn.BackColor = System.Drawing.Color.Transparent;
            this.TroubleshootingManualBtn.BackgroundImage = global::Omnicheck360.Properties.Resources.troubleshooting__1_;
            resources.ApplyResources(this.TroubleshootingManualBtn, "TroubleshootingManualBtn");
            this.TroubleshootingManualBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.TroubleshootingManualBtn.FlatAppearance.BorderSize = 0;
            this.TroubleshootingManualBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.TroubleshootingManualBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.TroubleshootingManualBtn.ForeColor = System.Drawing.Color.Black;
            this.TroubleshootingManualBtn.Name = "TroubleshootingManualBtn";
            this.TroubleshootingManualBtn.UseMnemonic = false;
            this.TroubleshootingManualBtn.UseVisualStyleBackColor = false;
            this.TroubleshootingManualBtn.Click += new System.EventHandler(this.TroubleshootingManualBtn_Click);
            // 
            // PartsListBtn
            // 
            this.PartsListBtn.BackColor = System.Drawing.Color.Transparent;
            this.PartsListBtn.BackgroundImage = global::Omnicheck360.Properties.Resources.purchase;
            resources.ApplyResources(this.PartsListBtn, "PartsListBtn");
            this.PartsListBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.PartsListBtn.FlatAppearance.BorderSize = 0;
            this.PartsListBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.PartsListBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.PartsListBtn.ForeColor = System.Drawing.Color.Black;
            this.PartsListBtn.Name = "PartsListBtn";
            this.PartsListBtn.UseMnemonic = false;
            this.PartsListBtn.UseVisualStyleBackColor = false;
            this.PartsListBtn.Click += new System.EventHandler(this.PartsListBtn_Click);
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
            // Documents
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.MainMenuFormBtn);
            this.Controls.Add(this.PartsListBtn);
            this.Controls.Add(this.TroubleshootingManualBtn);
            this.Controls.Add(this.OperatingManualBtn);
            this.Name = "Documents";
            this.Load += new System.EventHandler(this.Documents_Load);
            this.Controls.SetChildIndex(this.DateTimeLabel, 0);
            this.Controls.SetChildIndex(this.CurrentUserTxt, 0);
            this.Controls.SetChildIndex(this.OperatingManualBtn, 0);
            this.Controls.SetChildIndex(this.TroubleshootingManualBtn, 0);
            this.Controls.SetChildIndex(this.PartsListBtn, 0);
            this.Controls.SetChildIndex(this.MainMenuFormBtn, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.Button OperatingManualBtn;
        internal System.Windows.Forms.Button TroubleshootingManualBtn;
        internal System.Windows.Forms.Button PartsListBtn;
        internal System.Windows.Forms.Button MainMenuFormBtn;
    }
}
