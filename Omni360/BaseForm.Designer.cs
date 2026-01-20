namespace Omnicheck360
{
    partial class BaseForm
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
            this.OmnifissionLogo = new System.Windows.Forms.PictureBox();
            this.WhiteSpace = new System.Windows.Forms.PictureBox();
            this.entityCommand1 = new System.Data.Entity.Core.EntityClient.EntityCommand();
            this.DateTimeLabel = new ZBobb.AlphaBlendTextBox();
            this.CurrentUserTxt = new ZBobb.AlphaBlendTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.OmnifissionLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WhiteSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // OmnifissionLogo
            // 
            this.OmnifissionLogo.BackColor = System.Drawing.Color.Transparent;
            this.OmnifissionLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.OmnifissionLogo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.OmnifissionLogo.InitialImage = null;
            this.OmnifissionLogo.Location = new System.Drawing.Point(0, 0);
            this.OmnifissionLogo.Name = "OmnifissionLogo";
            this.OmnifissionLogo.Size = new System.Drawing.Size(1280, 130);
            this.OmnifissionLogo.TabIndex = 463;
            this.OmnifissionLogo.TabStop = false;
            this.OmnifissionLogo.Click += new System.EventHandler(this.OmnifissionLogo_Click);
            // 
            // WhiteSpace
            // 
            this.WhiteSpace.Location = new System.Drawing.Point(0, 0);
            this.WhiteSpace.Name = "WhiteSpace";
            this.WhiteSpace.Size = new System.Drawing.Size(1920, 106);
            this.WhiteSpace.TabIndex = 479;
            this.WhiteSpace.TabStop = false;
            // 
            // entityCommand1
            // 
            this.entityCommand1.CommandTimeout = 0;
            this.entityCommand1.CommandTree = null;
            this.entityCommand1.Connection = null;
            this.entityCommand1.EnablePlanCaching = true;
            this.entityCommand1.Transaction = null;
            // 
            // DateTimeLabel
            // 
            this.DateTimeLabel.BackAlpha = 10;
            this.DateTimeLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DateTimeLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DateTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.DateTimeLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.DateTimeLabel.Location = new System.Drawing.Point(12, 900);
            this.DateTimeLabel.Multiline = true;
            this.DateTimeLabel.Name = "DateTimeLabel";
            this.DateTimeLabel.Size = new System.Drawing.Size(231, 60);
            this.DateTimeLabel.TabIndex = 507;
            this.DateTimeLabel.Text = "DateTime";
            // 
            // CurrentUserTxt
            // 
            this.CurrentUserTxt.BackAlpha = 10;
            this.CurrentUserTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.CurrentUserTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CurrentUserTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.CurrentUserTxt.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.CurrentUserTxt.Location = new System.Drawing.Point(12, 964);
            this.CurrentUserTxt.Name = "CurrentUserTxt";
            this.CurrentUserTxt.Size = new System.Drawing.Size(300, 28);
            this.CurrentUserTxt.TabIndex = 508;
            this.CurrentUserTxt.Text = "Operte Mode";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Omnicheck360.Properties.Resources.FiltecLogo_black;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(280, 130);
            this.pictureBox1.TabIndex = 509;
            this.pictureBox1.TabStop = false;
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1280, 1024);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.CurrentUserTxt);
            this.Controls.Add(this.DateTimeLabel);
            this.Controls.Add(this.OmnifissionLogo);
            this.Controls.Add(this.WhiteSpace);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BaseForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.OmnifissionLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WhiteSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.PictureBox OmnifissionLogo;
        internal System.Windows.Forms.PictureBox WhiteSpace;
        private System.Data.Entity.Core.EntityClient.EntityCommand entityCommand1;
        public ZBobb.AlphaBlendTextBox DateTimeLabel;
        public ZBobb.AlphaBlendTextBox CurrentUserTxt;
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}