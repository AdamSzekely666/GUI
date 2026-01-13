namespace MatroxLDS
{
    partial class UserManagerForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserManagerForm));
            this.userdataGridView = new System.Windows.Forms.DataGridView();
            this.flowLayoutPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.MainMenuFormBtn = new System.Windows.Forms.Button();
            this.btnEnrollCard = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.userdataGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // userdataGridView
            // 
            this.userdataGridView.AllowUserToAddRows = false;
            this.userdataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.userdataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.userdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.userdataGridView.DefaultCellStyle = dataGridViewCellStyle1;
            this.userdataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userdataGridView.Location = new System.Drawing.Point(0, 0);
            this.userdataGridView.Name = "userdataGridView";
            this.userdataGridView.Size = new System.Drawing.Size(800, 500);
            this.userdataGridView.TabIndex = 512;
            this.userdataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.userdataGridView_CellContentClick);
            this.userdataGridView.SelectionChanged += new System.EventHandler(this.userdataGridView_SelectionChanged);
            // 
            // flowLayoutPanelButtons
            // 
            this.flowLayoutPanelButtons.AutoScroll = true;
            this.flowLayoutPanelButtons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelButtons.Location = new System.Drawing.Point(88, 228);
            this.flowLayoutPanelButtons.Name = "flowLayoutPanelButtons";
            this.flowLayoutPanelButtons.Size = new System.Drawing.Size(326, 583);
            this.flowLayoutPanelButtons.TabIndex = 518;
            // 
            // MainMenuFormBtn
            // 
            this.MainMenuFormBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.MainMenuFormBtn.BackColor = System.Drawing.Color.Transparent;
            this.MainMenuFormBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MainMenuFormBtn.BackgroundImage")));
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
            this.MainMenuFormBtn.TabIndex = 519;
            this.MainMenuFormBtn.UseMnemonic = false;
            this.MainMenuFormBtn.UseVisualStyleBackColor = false;
            this.MainMenuFormBtn.Click += new System.EventHandler(this.MainMenuFormBtn_Click);
            // 
            // btnEnrollCard
            // 
            this.btnEnrollCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnrollCard.Location = new System.Drawing.Point(453, 764);
            this.btnEnrollCard.Name = "btnEnrollCard";
            this.btnEnrollCard.Size = new System.Drawing.Size(153, 47);
            this.btnEnrollCard.TabIndex = 520;
            this.btnEnrollCard.Text = "Enroll A Card";
            this.btnEnrollCard.UseVisualStyleBackColor = true;
            this.btnEnrollCard.Click += new System.EventHandler(this.btnEnrollCard_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.userdataGridView);
            this.panel1.Location = new System.Drawing.Point(437, 228);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 500);
            this.panel1.TabIndex = 521;
            // 
            // UserManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 1024);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnEnrollCard);
            this.Controls.Add(this.MainMenuFormBtn);
            this.Controls.Add(this.flowLayoutPanelButtons);
            this.Name = "UserManagerForm";
            this.Text = "UserManagerForm";
            this.Controls.SetChildIndex(this.flowLayoutPanelButtons, 0);
            this.Controls.SetChildIndex(this.MainMenuFormBtn, 0);
            this.Controls.SetChildIndex(this.btnEnrollCard, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.DateTimeLabel, 0);
            this.Controls.SetChildIndex(this.CurrentUserTxt, 0);
            ((System.ComponentModel.ISupportInitialize)(this.userdataGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView userdataGridView;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelButtons;
        internal System.Windows.Forms.Button MainMenuFormBtn;
        private System.Windows.Forms.Button btnEnrollCard;
        private System.Windows.Forms.Panel panel1;
    }
}