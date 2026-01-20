namespace Omnicheck360
{
    partial class SplashScreen
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
            this.MainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Pgr1 = new System.Windows.Forms.ProgressBar();
            this.SplashTimer1 = new System.Windows.Forms.Timer(this.components);
            this.SplashTimer2 = new System.Windows.Forms.Timer(this.components);
            this.MainLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainLayoutPanel
            // 
            this.MainLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.MainLayoutPanel.BackgroundImage = global::Omnicheck360.Properties.Resources.FiltecLogo_black;
            this.MainLayoutPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.MainLayoutPanel.ColumnCount = 1;
            this.MainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1920F));
            this.MainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.MainLayoutPanel.Controls.Add(this.Pgr1, 0, 0);
            this.MainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainLayoutPanel.Name = "MainLayoutPanel";
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 218F));
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.MainLayoutPanel.Size = new System.Drawing.Size(1280, 1024);
            this.MainLayoutPanel.TabIndex = 1;
            // 
            // Pgr1
            // 
            this.Pgr1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Pgr1.BackColor = System.Drawing.Color.White;
            this.Pgr1.Location = new System.Drawing.Point(685, 895);
            this.Pgr1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 100);
            this.Pgr1.Maximum = 2500;
            this.Pgr1.Name = "Pgr1";
            this.Pgr1.Size = new System.Drawing.Size(550, 29);
            this.Pgr1.Step = 1;
            this.Pgr1.TabIndex = 2;
            // 
            // SplashTimer1
            // 
            this.SplashTimer1.Interval = 6000;
            this.SplashTimer1.Tick += new System.EventHandler(this.SplashTimer1_Tick);
            // 
            // SplashTimer2
            // 
            this.SplashTimer2.Tick += new System.EventHandler(this.SplashTimer2_Tick);
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1278, 1022);
            this.ControlBox = false;
            this.Controls.Add(this.MainLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashScreen";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SplashScreen_Load);
            this.MainLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.TableLayoutPanel MainLayoutPanel;
        internal System.Windows.Forms.Timer SplashTimer1;
        internal System.Windows.Forms.Timer SplashTimer2;
        internal System.Windows.Forms.ProgressBar Pgr1;
    }
}