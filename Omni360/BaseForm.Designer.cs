namespace MatroxLDS
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
        protected void InitializeComponent()
        {
            this.entityCommand1 = new System.Data.Entity.Core.EntityClient.EntityCommand();
            this.CurrentUserTxt = new ZBobb.AlphaBlendTextBox();
            this.DateTimeLabel = new ZBobb.AlphaBlendTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.faultDisplayLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // entityCommand1
            // 
            this.entityCommand1.CommandTimeout = 0;
            this.entityCommand1.CommandTree = null;
            this.entityCommand1.Connection = null;
            this.entityCommand1.EnablePlanCaching = true;
            this.entityCommand1.Transaction = null;
            // 
            // CurrentUserTxt
            // 
            this.CurrentUserTxt.BackAlpha = 10;
            this.CurrentUserTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.CurrentUserTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CurrentUserTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentUserTxt.Location = new System.Drawing.Point(12, 964);
            this.CurrentUserTxt.Name = "CurrentUserTxt";
            this.CurrentUserTxt.ReadOnly = true;
            this.CurrentUserTxt.Size = new System.Drawing.Size(232, 28);
            this.CurrentUserTxt.TabIndex = 508;
            this.CurrentUserTxt.TabStop = false;
            this.CurrentUserTxt.Text = "Operate Mode";
            // 
            // DateTimeLabel
            // 
            this.DateTimeLabel.BackAlpha = 10;
            this.DateTimeLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DateTimeLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DateTimeLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.DateTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DateTimeLabel.Location = new System.Drawing.Point(12, 890);
            this.DateTimeLabel.Multiline = true;
            this.DateTimeLabel.Name = "DateTimeLabel";
            this.DateTimeLabel.ReadOnly = true;
            this.DateTimeLabel.Size = new System.Drawing.Size(157, 50);
            this.DateTimeLabel.TabIndex = 507;
            this.DateTimeLabel.TabStop = false;
            this.DateTimeLabel.Text = "DateTime";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::OmniCheck_360.Properties.Resources.FiltecLogo_black1;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(280, 130);
            this.pictureBox1.TabIndex = 509;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pictureBox2.Location = new System.Drawing.Point(12, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(1268, 130);
            this.pictureBox2.TabIndex = 510;
            this.pictureBox2.TabStop = false;
            // 
            // faultDisplayLabel
            // 
            this.faultDisplayLabel.AutoSize = true;
            this.faultDisplayLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.faultDisplayLabel.Location = new System.Drawing.Point(768, 68);
            this.faultDisplayLabel.Name = "faultDisplayLabel";
            this.faultDisplayLabel.Size = new System.Drawing.Size(0, 24);
            this.faultDisplayLabel.TabIndex = 511;
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1280, 1040);
            this.Controls.Add(this.faultDisplayLabel);
            this.Controls.Add(this.CurrentUserTxt);
            this.Controls.Add(this.DateTimeLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BaseForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Data.Entity.Core.EntityClient.EntityCommand entityCommand1;
        public ZBobb.AlphaBlendTextBox CurrentUserTxt;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        public ZBobb.AlphaBlendTextBox DateTimeLabel;
        private System.Windows.Forms.Label faultDisplayLabel;
    }
}