namespace Omnicheck360{
    partial class Data
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
            this.DashboardBtn = new System.Windows.Forms.Button();
            this.btnExportPdf = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.nudShiftCount = new System.Windows.Forms.NumericUpDown();
            this.lblCustomRange2 = new System.Windows.Forms.Label();
            this.dtpShift3End = new System.Windows.Forms.DateTimePicker();
            this.lblShift3End = new System.Windows.Forms.Label();
            this.dtpShift3Start = new System.Windows.Forms.DateTimePicker();
            this.lblShift3Start = new System.Windows.Forms.Label();
            this.dtpShift2End = new System.Windows.Forms.DateTimePicker();
            this.lblShift2End = new System.Windows.Forms.Label();
            this.dtpShift2Start = new System.Windows.Forms.DateTimePicker();
            this.lblShift2Start = new System.Windows.Forms.Label();
            this.dtpShift1End = new System.Windows.Forms.DateTimePicker();
            this.lblShift1End = new System.Windows.Forms.Label();
            this.dtpShift1Start = new System.Windows.Forms.DateTimePicker();
            this.lblShift1Start = new System.Windows.Forms.Label();
            this.btnShiftReport = new System.Windows.Forms.Button();
            this.btnDailySummary = new System.Windows.Forms.Button();
            this.btnCustomReport = new System.Windows.Forms.Button();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpReportDate = new System.Windows.Forms.DateTimePicker();
            this.lblShift = new System.Windows.Forms.Label();
            this.rbShift1 = new System.Windows.Forms.RadioButton();
            this.rbShift2 = new System.Windows.Forms.RadioButton();
            this.rbShift3 = new System.Windows.Forms.RadioButton();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.lblCustomRange = new System.Windows.Forms.Label();
            this.dtpCustomFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpCustomTo = new System.Windows.Forms.DateTimePicker();
            this.dgvRecentShifts = new System.Windows.Forms.DataGridView();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShift = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOperator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colView = new System.Windows.Forms.DataGridViewButtonColumn();
            this.rtbShiftReport = new System.Windows.Forms.RichTextBox();
            this.dgvRecentFlavours = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtpCustomFromFlavours = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.nudShiftCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentShifts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentFlavours)).BeginInit();
            this.SuspendLayout();
            // 
            // DashboardBtn
            // 
            this.DashboardBtn.BackColor = System.Drawing.Color.Transparent;
            this.DashboardBtn.BackgroundImage = global::Omnicheck360.Properties.Resources.dashboard48x48;
            this.DashboardBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.DashboardBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.DashboardBtn.FlatAppearance.BorderSize = 0;
            this.DashboardBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.DashboardBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.DashboardBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DashboardBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.DashboardBtn.ForeColor = System.Drawing.Color.Black;
            this.DashboardBtn.Location = new System.Drawing.Point(840, 950);
            this.DashboardBtn.Margin = new System.Windows.Forms.Padding(6);
            this.DashboardBtn.Name = "DashboardBtn";
            this.DashboardBtn.Size = new System.Drawing.Size(240, 120);
            this.DashboardBtn.TabIndex = 554;
            this.DashboardBtn.UseMnemonic = false;
            this.DashboardBtn.UseVisualStyleBackColor = false;
            this.DashboardBtn.Click += new System.EventHandler(this.DashboardBtn_Click);
            // 
            // btnExportPdf
            // 
            this.btnExportPdf.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnExportPdf.FlatAppearance.BorderSize = 0;
            this.btnExportPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportPdf.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnExportPdf.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExportPdf.Location = new System.Drawing.Point(1195, 749);
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Size = new System.Drawing.Size(240, 120);
            this.btnExportPdf.TabIndex = 585;
            this.btnExportPdf.Text = "Generate PDF";
            this.btnExportPdf.Click += new System.EventHandler(this.btnExportPdf_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(92, 489);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 20);
            this.label1.TabIndex = 584;
            this.label1.Text = "Number of Shifts (1-3):";
            // 
            // nudShiftCount
            // 
            this.nudShiftCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.nudShiftCount.Location = new System.Drawing.Point(283, 486);
            this.nudShiftCount.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudShiftCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudShiftCount.Name = "nudShiftCount";
            this.nudShiftCount.Size = new System.Drawing.Size(37, 26);
            this.nudShiftCount.TabIndex = 583;
            this.nudShiftCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblCustomRange2
            // 
            this.lblCustomRange2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblCustomRange2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCustomRange2.Location = new System.Drawing.Point(1193, 166);
            this.lblCustomRange2.Name = "lblCustomRange2";
            this.lblCustomRange2.Size = new System.Drawing.Size(33, 23);
            this.lblCustomRange2.TabIndex = 582;
            this.lblCustomRange2.Text = "To:";
            // 
            // dtpShift3End
            // 
            this.dtpShift3End.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dtpShift3End.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpShift3End.Location = new System.Drawing.Point(560, 585);
            this.dtpShift3End.Name = "dtpShift3End";
            this.dtpShift3End.ShowUpDown = true;
            this.dtpShift3End.Size = new System.Drawing.Size(113, 26);
            this.dtpShift3End.TabIndex = 555;
            this.dtpShift3End.ValueChanged += new System.EventHandler(this.ShiftTimes_ValueChanged);
            // 
            // lblShift3End
            // 
            this.lblShift3End.AutoSize = true;
            this.lblShift3End.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblShift3End.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblShift3End.Location = new System.Drawing.Point(402, 594);
            this.lblShift3End.Name = "lblShift3End";
            this.lblShift3End.Size = new System.Drawing.Size(42, 20);
            this.lblShift3End.TabIndex = 556;
            this.lblShift3End.Text = "End:";
            // 
            // dtpShift3Start
            // 
            this.dtpShift3Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dtpShift3Start.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpShift3Start.Location = new System.Drawing.Point(243, 591);
            this.dtpShift3Start.Name = "dtpShift3Start";
            this.dtpShift3Start.ShowUpDown = true;
            this.dtpShift3Start.Size = new System.Drawing.Size(113, 26);
            this.dtpShift3Start.TabIndex = 557;
            this.dtpShift3Start.ValueChanged += new System.EventHandler(this.ShiftTimes_ValueChanged);
            // 
            // lblShift3Start
            // 
            this.lblShift3Start.AutoSize = true;
            this.lblShift3Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblShift3Start.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblShift3Start.Location = new System.Drawing.Point(30, 591);
            this.lblShift3Start.Name = "lblShift3Start";
            this.lblShift3Start.Size = new System.Drawing.Size(98, 20);
            this.lblShift3Start.TabIndex = 559;
            this.lblShift3Start.Text = "Shift 3 Start:";
            // 
            // dtpShift2End
            // 
            this.dtpShift2End.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dtpShift2End.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpShift2End.Location = new System.Drawing.Point(560, 555);
            this.dtpShift2End.Name = "dtpShift2End";
            this.dtpShift2End.ShowUpDown = true;
            this.dtpShift2End.Size = new System.Drawing.Size(113, 26);
            this.dtpShift2End.TabIndex = 561;
            this.dtpShift2End.ValueChanged += new System.EventHandler(this.ShiftTimes_ValueChanged);
            // 
            // lblShift2End
            // 
            this.lblShift2End.AutoSize = true;
            this.lblShift2End.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblShift2End.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblShift2End.Location = new System.Drawing.Point(402, 564);
            this.lblShift2End.Name = "lblShift2End";
            this.lblShift2End.Size = new System.Drawing.Size(42, 20);
            this.lblShift2End.TabIndex = 562;
            this.lblShift2End.Text = "End:";
            // 
            // dtpShift2Start
            // 
            this.dtpShift2Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dtpShift2Start.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpShift2Start.Location = new System.Drawing.Point(243, 561);
            this.dtpShift2Start.Name = "dtpShift2Start";
            this.dtpShift2Start.ShowUpDown = true;
            this.dtpShift2Start.Size = new System.Drawing.Size(113, 26);
            this.dtpShift2Start.TabIndex = 564;
            this.dtpShift2Start.ValueChanged += new System.EventHandler(this.ShiftTimes_ValueChanged);
            // 
            // lblShift2Start
            // 
            this.lblShift2Start.AutoSize = true;
            this.lblShift2Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblShift2Start.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblShift2Start.Location = new System.Drawing.Point(30, 561);
            this.lblShift2Start.Name = "lblShift2Start";
            this.lblShift2Start.Size = new System.Drawing.Size(98, 20);
            this.lblShift2Start.TabIndex = 566;
            this.lblShift2Start.Text = "Shift 2 Start:";
            // 
            // dtpShift1End
            // 
            this.dtpShift1End.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dtpShift1End.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpShift1End.Location = new System.Drawing.Point(560, 525);
            this.dtpShift1End.Name = "dtpShift1End";
            this.dtpShift1End.ShowUpDown = true;
            this.dtpShift1End.Size = new System.Drawing.Size(113, 26);
            this.dtpShift1End.TabIndex = 569;
            this.dtpShift1End.ValueChanged += new System.EventHandler(this.ShiftTimes_ValueChanged);
            // 
            // lblShift1End
            // 
            this.lblShift1End.AutoSize = true;
            this.lblShift1End.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblShift1End.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblShift1End.Location = new System.Drawing.Point(402, 534);
            this.lblShift1End.Name = "lblShift1End";
            this.lblShift1End.Size = new System.Drawing.Size(42, 20);
            this.lblShift1End.TabIndex = 571;
            this.lblShift1End.Text = "End:";
            // 
            // dtpShift1Start
            // 
            this.dtpShift1Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dtpShift1Start.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpShift1Start.Location = new System.Drawing.Point(243, 531);
            this.dtpShift1Start.Name = "dtpShift1Start";
            this.dtpShift1Start.ShowUpDown = true;
            this.dtpShift1Start.Size = new System.Drawing.Size(113, 26);
            this.dtpShift1Start.TabIndex = 572;
            this.dtpShift1Start.ValueChanged += new System.EventHandler(this.ShiftTimes_ValueChanged);
            // 
            // lblShift1Start
            // 
            this.lblShift1Start.AutoSize = true;
            this.lblShift1Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblShift1Start.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblShift1Start.Location = new System.Drawing.Point(30, 531);
            this.lblShift1Start.Name = "lblShift1Start";
            this.lblShift1Start.Size = new System.Drawing.Size(98, 20);
            this.lblShift1Start.TabIndex = 574;
            this.lblShift1Start.Text = "Shift 1 Start:";
            // 
            // btnShiftReport
            // 
            this.btnShiftReport.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShiftReport.FlatAppearance.BorderSize = 0;
            this.btnShiftReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShiftReport.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnShiftReport.Location = new System.Drawing.Point(15, 148);
            this.btnShiftReport.Name = "btnShiftReport";
            this.btnShiftReport.Size = new System.Drawing.Size(120, 60);
            this.btnShiftReport.TabIndex = 558;
            this.btnShiftReport.Text = "Shift Report";
            this.btnShiftReport.Click += new System.EventHandler(this.btnShiftReport_Click);
            // 
            // btnDailySummary
            // 
            this.btnDailySummary.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnDailySummary.FlatAppearance.BorderSize = 0;
            this.btnDailySummary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDailySummary.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDailySummary.Location = new System.Drawing.Point(247, 148);
            this.btnDailySummary.Name = "btnDailySummary";
            this.btnDailySummary.Size = new System.Drawing.Size(120, 60);
            this.btnDailySummary.TabIndex = 560;
            this.btnDailySummary.Text = "Daily Summary";
            this.btnDailySummary.Click += new System.EventHandler(this.btnDailySummary_Click);
            // 
            // btnCustomReport
            // 
            this.btnCustomReport.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCustomReport.FlatAppearance.BorderSize = 0;
            this.btnCustomReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomReport.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCustomReport.Location = new System.Drawing.Point(485, 148);
            this.btnCustomReport.Name = "btnCustomReport";
            this.btnCustomReport.Size = new System.Drawing.Size(120, 60);
            this.btnCustomReport.TabIndex = 563;
            this.btnCustomReport.Text = "Custom Report";
            this.btnCustomReport.Click += new System.EventHandler(this.btnCustomReport_Click);
            // 
            // lblDate
            // 
            this.lblDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDate.Location = new System.Drawing.Point(15, 237);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(100, 23);
            this.lblDate.TabIndex = 565;
            this.lblDate.Text = "Date";
            // 
            // dtpReportDate
            // 
            this.dtpReportDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dtpReportDate.Location = new System.Drawing.Point(140, 235);
            this.dtpReportDate.Name = "dtpReportDate";
            this.dtpReportDate.Size = new System.Drawing.Size(280, 26);
            this.dtpReportDate.TabIndex = 567;
            // 
            // lblShift
            // 
            this.lblShift.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblShift.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblShift.Location = new System.Drawing.Point(15, 273);
            this.lblShift.Name = "lblShift";
            this.lblShift.Size = new System.Drawing.Size(66, 23);
            this.lblShift.TabIndex = 568;
            this.lblShift.Text = "Shift";
            // 
            // rbShift1
            // 
            this.rbShift1.AutoSize = true;
            this.rbShift1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.rbShift1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rbShift1.Location = new System.Drawing.Point(87, 272);
            this.rbShift1.Name = "rbShift1";
            this.rbShift1.Size = new System.Drawing.Size(73, 24);
            this.rbShift1.TabIndex = 570;
            this.rbShift1.Text = "Shift 1";
            // 
            // rbShift2
            // 
            this.rbShift2.AutoSize = true;
            this.rbShift2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.rbShift2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rbShift2.Location = new System.Drawing.Point(283, 273);
            this.rbShift2.Name = "rbShift2";
            this.rbShift2.Size = new System.Drawing.Size(73, 24);
            this.rbShift2.TabIndex = 573;
            this.rbShift2.Text = "Shift 2";
            // 
            // rbShift3
            // 
            this.rbShift3.AutoSize = true;
            this.rbShift3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.rbShift3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rbShift3.Location = new System.Drawing.Point(485, 273);
            this.rbShift3.Name = "rbShift3";
            this.rbShift3.Size = new System.Drawing.Size(73, 24);
            this.rbShift3.TabIndex = 575;
            this.rbShift3.Text = "Shift 3";
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnGenerateReport.FlatAppearance.BorderSize = 0;
            this.btnGenerateReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnGenerateReport.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGenerateReport.Location = new System.Drawing.Point(840, 749);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(240, 120);
            this.btnGenerateReport.TabIndex = 576;
            this.btnGenerateReport.Text = "Generate Report";
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // lblCustomRange
            // 
            this.lblCustomRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblCustomRange.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCustomRange.Location = new System.Drawing.Point(806, 166);
            this.lblCustomRange.Name = "lblCustomRange";
            this.lblCustomRange.Size = new System.Drawing.Size(54, 23);
            this.lblCustomRange.TabIndex = 577;
            this.lblCustomRange.Text = "From:";
            // 
            // dtpCustomFrom
            // 
            this.dtpCustomFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dtpCustomFrom.Location = new System.Drawing.Point(873, 164);
            this.dtpCustomFrom.Name = "dtpCustomFrom";
            this.dtpCustomFrom.Size = new System.Drawing.Size(280, 26);
            this.dtpCustomFrom.TabIndex = 578;
            // 
            // dtpCustomTo
            // 
            this.dtpCustomTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dtpCustomTo.Location = new System.Drawing.Point(1230, 164);
            this.dtpCustomTo.Name = "dtpCustomTo";
            this.dtpCustomTo.Size = new System.Drawing.Size(280, 26);
            this.dtpCustomTo.TabIndex = 579;
            // 
            // dgvRecentShifts
            // 
            this.dgvRecentShifts.AllowUserToAddRows = false;
            this.dgvRecentShifts.AllowUserToDeleteRows = false;
            this.dgvRecentShifts.AllowUserToResizeRows = false;
            this.dgvRecentShifts.ColumnHeadersHeight = 30;
            this.dgvRecentShifts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDate,
            this.colShift,
            this.colOperator,
            this.colView});
            this.dgvRecentShifts.Location = new System.Drawing.Point(163, 317);
            this.dgvRecentShifts.Name = "dgvRecentShifts";
            this.dgvRecentShifts.ReadOnly = true;
            this.dgvRecentShifts.RowHeadersVisible = false;
            this.dgvRecentShifts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecentShifts.Size = new System.Drawing.Size(405, 150);
            this.dgvRecentShifts.TabIndex = 580;
            // 
            // colDate
            // 
            this.colDate.HeaderText = "Date";
            this.colDate.Name = "colDate";
            this.colDate.ReadOnly = true;
            // 
            // colShift
            // 
            this.colShift.HeaderText = "Shift";
            this.colShift.Name = "colShift";
            this.colShift.ReadOnly = true;
            // 
            // colOperator
            // 
            this.colOperator.HeaderText = "Operator";
            this.colOperator.Name = "colOperator";
            this.colOperator.ReadOnly = true;
            // 
            // colView
            // 
            this.colView.HeaderText = "View";
            this.colView.Name = "colView";
            this.colView.ReadOnly = true;
            this.colView.Text = "View";
            this.colView.UseColumnTextForButtonValue = true;
            // 
            // rtbShiftReport
            // 
            this.rtbShiftReport.BackColor = System.Drawing.SystemColors.Control;
            this.rtbShiftReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbShiftReport.Location = new System.Drawing.Point(915, 273);
            this.rtbShiftReport.Name = "rtbShiftReport";
            this.rtbShiftReport.Size = new System.Drawing.Size(684, 437);
            this.rtbShiftReport.TabIndex = 581;
            this.rtbShiftReport.Text = "";
            // 
            // dgvRecentFlavours
            // 
            this.dgvRecentFlavours.AllowUserToAddRows = false;
            this.dgvRecentFlavours.AllowUserToDeleteRows = false;
            this.dgvRecentFlavours.AllowUserToResizeRows = false;
            this.dgvRecentFlavours.ColumnHeadersHeight = 30;
            this.dgvRecentFlavours.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.dgvRecentFlavours.Location = new System.Drawing.Point(163, 652);
            this.dgvRecentFlavours.Name = "dgvRecentFlavours";
            this.dgvRecentFlavours.ReadOnly = true;
            this.dgvRecentFlavours.RowHeadersVisible = false;
            this.dgvRecentFlavours.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecentFlavours.Size = new System.Drawing.Size(405, 150);
            this.dgvRecentFlavours.TabIndex = 586;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Date";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Flavour";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Operator";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dtpCustomFromFlavours
            // 
            this.dtpCustomFromFlavours.Location = new System.Drawing.Point(15, 821);
            this.dtpCustomFromFlavours.Name = "dtpCustomFromFlavours";
            this.dtpCustomFromFlavours.Size = new System.Drawing.Size(404, 29);
            this.dtpCustomFromFlavours.TabIndex = 587;
            // 
            // Data
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.dtpCustomFromFlavours);
            this.Controls.Add(this.dgvRecentFlavours);
            this.Controls.Add(this.btnExportPdf);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudShiftCount);
            this.Controls.Add(this.lblCustomRange2);
            this.Controls.Add(this.dtpShift3End);
            this.Controls.Add(this.lblShift3End);
            this.Controls.Add(this.dtpShift3Start);
            this.Controls.Add(this.lblShift3Start);
            this.Controls.Add(this.dtpShift2End);
            this.Controls.Add(this.lblShift2End);
            this.Controls.Add(this.dtpShift2Start);
            this.Controls.Add(this.lblShift2Start);
            this.Controls.Add(this.dtpShift1End);
            this.Controls.Add(this.lblShift1End);
            this.Controls.Add(this.dtpShift1Start);
            this.Controls.Add(this.lblShift1Start);
            this.Controls.Add(this.btnShiftReport);
            this.Controls.Add(this.btnDailySummary);
            this.Controls.Add(this.btnCustomReport);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.dtpReportDate);
            this.Controls.Add(this.lblShift);
            this.Controls.Add(this.rbShift1);
            this.Controls.Add(this.rbShift2);
            this.Controls.Add(this.rbShift3);
            this.Controls.Add(this.btnGenerateReport);
            this.Controls.Add(this.lblCustomRange);
            this.Controls.Add(this.dtpCustomFrom);
            this.Controls.Add(this.dtpCustomTo);
            this.Controls.Add(this.dgvRecentShifts);
            this.Controls.Add(this.rtbShiftReport);
            this.Controls.Add(this.DashboardBtn);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Data";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Data_Load);
            this.Controls.SetChildIndex(this.DateTimeLabel, 0);
            this.Controls.SetChildIndex(this.CurrentUserTxt, 0);
            this.Controls.SetChildIndex(this.DashboardBtn, 0);
            this.Controls.SetChildIndex(this.rtbShiftReport, 0);
            this.Controls.SetChildIndex(this.dgvRecentShifts, 0);
            this.Controls.SetChildIndex(this.dtpCustomTo, 0);
            this.Controls.SetChildIndex(this.dtpCustomFrom, 0);
            this.Controls.SetChildIndex(this.lblCustomRange, 0);
            this.Controls.SetChildIndex(this.btnGenerateReport, 0);
            this.Controls.SetChildIndex(this.rbShift3, 0);
            this.Controls.SetChildIndex(this.rbShift2, 0);
            this.Controls.SetChildIndex(this.rbShift1, 0);
            this.Controls.SetChildIndex(this.lblShift, 0);
            this.Controls.SetChildIndex(this.dtpReportDate, 0);
            this.Controls.SetChildIndex(this.lblDate, 0);
            this.Controls.SetChildIndex(this.btnCustomReport, 0);
            this.Controls.SetChildIndex(this.btnDailySummary, 0);
            this.Controls.SetChildIndex(this.btnShiftReport, 0);
            this.Controls.SetChildIndex(this.lblShift1Start, 0);
            this.Controls.SetChildIndex(this.dtpShift1Start, 0);
            this.Controls.SetChildIndex(this.lblShift1End, 0);
            this.Controls.SetChildIndex(this.dtpShift1End, 0);
            this.Controls.SetChildIndex(this.lblShift2Start, 0);
            this.Controls.SetChildIndex(this.dtpShift2Start, 0);
            this.Controls.SetChildIndex(this.lblShift2End, 0);
            this.Controls.SetChildIndex(this.dtpShift2End, 0);
            this.Controls.SetChildIndex(this.lblShift3Start, 0);
            this.Controls.SetChildIndex(this.dtpShift3Start, 0);
            this.Controls.SetChildIndex(this.lblShift3End, 0);
            this.Controls.SetChildIndex(this.dtpShift3End, 0);
            this.Controls.SetChildIndex(this.lblCustomRange2, 0);
            this.Controls.SetChildIndex(this.nudShiftCount, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnExportPdf, 0);
            this.Controls.SetChildIndex(this.dgvRecentFlavours, 0);
            this.Controls.SetChildIndex(this.dtpCustomFromFlavours, 0);
            ((System.ComponentModel.ISupportInitialize)(this.nudShiftCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentShifts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentFlavours)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button DashboardBtn;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudShiftCount;
        private System.Windows.Forms.Label lblCustomRange2;
        private System.Windows.Forms.DateTimePicker dtpShift3End;
        private System.Windows.Forms.Label lblShift3End;
        private System.Windows.Forms.DateTimePicker dtpShift3Start;
        private System.Windows.Forms.Label lblShift3Start;
        private System.Windows.Forms.DateTimePicker dtpShift2End;
        private System.Windows.Forms.Label lblShift2End;
        private System.Windows.Forms.DateTimePicker dtpShift2Start;
        private System.Windows.Forms.Label lblShift2Start;
        private System.Windows.Forms.DateTimePicker dtpShift1End;
        private System.Windows.Forms.Label lblShift1End;
        private System.Windows.Forms.DateTimePicker dtpShift1Start;
        private System.Windows.Forms.Label lblShift1Start;
        private System.Windows.Forms.Button btnShiftReport;
        private System.Windows.Forms.Button btnDailySummary;
        private System.Windows.Forms.Button btnCustomReport;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpReportDate;
        private System.Windows.Forms.Label lblShift;
        private System.Windows.Forms.RadioButton rbShift1;
        private System.Windows.Forms.RadioButton rbShift2;
        private System.Windows.Forms.RadioButton rbShift3;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.Label lblCustomRange;
        private System.Windows.Forms.DateTimePicker dtpCustomFrom;
        private System.Windows.Forms.DateTimePicker dtpCustomTo;
        private System.Windows.Forms.DataGridView dgvRecentShifts;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShift;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOperator;
        private System.Windows.Forms.DataGridViewButtonColumn colView;
        private System.Windows.Forms.RichTextBox rtbShiftReport;
        private System.Windows.Forms.DataGridView dgvRecentFlavours;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DateTimePicker dtpCustomFromFlavours;
    }
}