namespace Omnicheck360
{
    partial class OmniMsgMoveAll
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
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.ServoChangeDone = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblMsg
            // 
            this.lblMsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMsg.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblMsg.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMsg.Location = new System.Drawing.Point(75, 75);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(300, 175);
            this.lblMsg.TabIndex = 1;
            this.lblMsg.Text = "Messages go here.";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNo
            // 
            this.btnNo.BackgroundImage = global::Omnicheck360.Properties.Resources.close__1_;
            this.btnNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNo.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnNo.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.btnNo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNo.Location = new System.Drawing.Point(270, 285);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(90, 50);
            this.btnNo.TabIndex = 364;
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Visible = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnYes
            // 
            this.btnYes.BackgroundImage = global::Omnicheck360.Properties.Resources.checkmark__1_;
            this.btnYes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnYes.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnYes.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.btnYes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnYes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnYes.Location = new System.Drawing.Point(90, 285);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(90, 50);
            this.btnYes.TabIndex = 363;
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Visible = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(140, 275);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(170, 70);
            this.btnOK.TabIndex = 362;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ServoChangeDone
            // 
            this.ServoChangeDone.Interval = 1000;
            this.ServoChangeDone.Tick += new System.EventHandler(this.ServoChangeDone_Tick);
            // 
            // OmniMsgMoveAll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(450, 400);
            this.ControlBox = false;
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblMsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OmniMsgMoveAll";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OmniMsgMoveAll";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.OmniMsgMoveAll_Load);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Label lblMsg;
        internal System.Windows.Forms.Button btnNo;
        internal System.Windows.Forms.Button btnYes;
        internal System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Timer ServoChangeDone;
    }
}