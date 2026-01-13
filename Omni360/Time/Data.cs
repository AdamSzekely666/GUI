using Matrox.DesignAssistant.OperatorAPI;
using Matrox.DesignAssistant.OperatorAPI.InputsPackage;
using Nini.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.IO;

namespace MatroxLDS
{
    public partial class Data : BaseForm
    {
        public MainForm mainForm;
        private int shiftCount = 3;
        private TimeSpan shift1Start = new TimeSpan(7, 0, 0);
        private TimeSpan shift1End = new TimeSpan(15, 0, 0);
        private TimeSpan shift2Start = new TimeSpan(15, 0, 0);
        private TimeSpan shift2End = new TimeSpan(23, 0, 0);
        private TimeSpan shift3Start = new TimeSpan(23, 0, 0);
        private TimeSpan shift3End = new TimeSpan(7, 0, 0); // next day
        private bool isInitializing = false;
        private IniConfigSource ini;

        public Data(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            dtpCustomFromTime.Visible = false;
            dtpCustomToTime.Visible = false;

        }

        private void Data_Load(object sender, EventArgs e)
        {
            InitializeComponent();
            isInitializing = true;
            ButtonAnimator.InitializeAnimation(btnGenerateReport, "blue");
            ButtonAnimator.InitializeAnimation(btnCustomReport, "blue");
            ButtonAnimator.InitializeAnimation(btnDailySummary, "blue");
            ButtonAnimator.InitializeAnimation(DashboardBtn, "blue");
            ButtonAnimator.InitializeAnimation(btnShiftReport, "blue");
            ButtonAnimator.InitializeAnimation(btnExportPdf, "blue");
            CurrentUserTxt.Text = mainForm.CurrentUserTxt.Text;
            DateTimeLabel.Text = mainForm.DateTimeLabel.Text;
            string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.ini");
            ini = new IniConfigSource("app.ini");
            LoadShiftConfigFromIni();

            dtpReportDate.Value = DateTime.Today;
            rbShift1.Checked = true;
            ShowShiftReportSection();
            UpdateShiftRadioButtonText();

            dtpShift1Start.Value = DateTime.Today + shift1Start;
            dtpShift1End.Value = (shift1End > shift1Start) ? DateTime.Today + shift1End : DateTime.Today.AddDays(1) + shift1End;

            dtpShift2Start.Value = DateTime.Today + shift2Start;
            dtpShift2End.Value = (shift2End > shift2Start) ? DateTime.Today + shift2End : DateTime.Today.AddDays(1) + shift2End;

            dtpShift3Start.Value = DateTime.Today + shift3Start;
            dtpShift3End.Value = (shift3End > shift3Start) ? DateTime.Today + shift3End : DateTime.Today.AddDays(1) + shift3End;

            dtpShift1Start.ValueChanged += ShiftTimes_ValueChanged;
            dtpShift1End.ValueChanged += ShiftTimes_ValueChanged;
            dtpShift2Start.ValueChanged += ShiftTimes_ValueChanged;
            dtpShift2End.ValueChanged += ShiftTimes_ValueChanged;
            dtpShift3Start.ValueChanged += ShiftTimes_ValueChanged;
            dtpShift3End.ValueChanged += ShiftTimes_ValueChanged;

            if (nudShiftCount != null)
            {
                nudShiftCount.Minimum = 1;
                nudShiftCount.Maximum = 3;
                nudShiftCount.ValueChanged += nudShiftCount_ValueChanged;
                nudShiftCount.Value = shiftCount;
            }

            UpdateShiftControlsVisibility();

            dgvRecentShifts.CellContentClick += dgvRecentShifts_CellContentClick;

            // --- Flavour Grid Initialization ---
            dgvRecentFlavours.CellContentClick += dgvRecentFlavours_CellContentClick;
            LoadLastFiveFlavours();
            if (dtpCustomFromFlavours != null)
                dtpCustomFromFlavours.ValueChanged += dtpCustomFromFlavours_ValueChanged;

            isInitializing = false;
            UpdateShiftRadioButtonText();
            LoadRecentShifts();
        }

        private void LoadShiftConfigFromIni()
        {
            var shifts = ini.Configs["Shifts"];
            if (shifts == null) shifts = ini.AddConfig("Shifts");
            foreach (string key in shifts.GetKeys())
            {
                Console.WriteLine($"{key} = {shifts.Get(key)}");
            }
            shiftCount = int.TryParse(shifts.Get("Count", "3"), out var cnt) ? cnt : 3;

            if (!TimeSpan.TryParse(shifts.Get("Shift1Start", "07:00"), out shift1Start))
                shift1Start = new TimeSpan(7, 0, 0);
            if (!TimeSpan.TryParse(shifts.Get("Shift1End", "15:00"), out shift1End))
                shift1End = new TimeSpan(15, 0, 0);
            if (!TimeSpan.TryParse(shifts.Get("Shift2Start", "15:00"), out shift2Start))
                shift2Start = new TimeSpan(15, 0, 0);
            if (!TimeSpan.TryParse(shifts.Get("Shift2End", "23:00"), out shift2End))
                shift2End = new TimeSpan(23, 0, 0);
            if (!TimeSpan.TryParse(shifts.Get("Shift3Start", "23:00"), out shift3Start))
                shift3Start = new TimeSpan(23, 0, 0);
            if (!TimeSpan.TryParse(shifts.Get("Shift3End", "07:00"), out shift3End))
                shift3End = new TimeSpan(7, 0, 0);
        }

        private void nudShiftCount_ValueChanged(object sender, EventArgs e)
        {
            shiftCount = (int)nudShiftCount.Value;
            UpdateShiftControlsVisibility();
        }

        private void ShiftTimes_ValueChanged(object sender, EventArgs e)
        {
            if (isInitializing) return;
            shift1Start = dtpShift1Start.Value.TimeOfDay;
            shift1End = dtpShift1End.Value.TimeOfDay;
            shift2Start = dtpShift2Start.Value.TimeOfDay;
            shift2End = dtpShift2End.Value.TimeOfDay;
            shift3Start = dtpShift3Start.Value.TimeOfDay;
            shift3End = dtpShift3End.Value.TimeOfDay;
            UpdateShiftRadioButtonText();
        }

        private void UpdateShiftControlsVisibility()
        {
            lblShift2Start.Visible = dtpShift2Start.Visible = lblShift2End.Visible = dtpShift2End.Visible = (shiftCount >= 2);
            lblShift3Start.Visible = dtpShift3Start.Visible = lblShift3End.Visible = dtpShift3End.Visible = (shiftCount == 3);
            rbShift2.Visible = (shiftCount >= 2);
            rbShift3.Visible = (shiftCount == 3);
        }

        private void UpdateShiftRadioButtonText()
        {
            rbShift1.Text = $"Shift 1 ({shift1Start:hh\\:mm} - {shift1End:hh\\:mm})";
            rbShift2.Text = $"Shift 2 ({shift2Start:hh\\:mm} - {shift2End:hh\\:mm})";
            rbShift3.Text = $"Shift 3 ({shift3Start:hh\\:mm} - {shift3End:hh\\:mm})";
        }

        private void btnShiftReport_Click(object sender, EventArgs e)
        {
            ShowShiftReportSection();
        }

        private void btnDailySummary_Click(object sender, EventArgs e)
        {
            ShowDailySummarySection();
        }

        private void btnCustomReport_Click(object sender, EventArgs e)
        {
            ShowCustomReportSection();
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            if (rbShift1.Visible && (rbShift1.Checked || rbShift2.Checked || rbShift3.Checked))
            {
                GenerateShiftSummaryReport();
            }
            else if (lblDate.Visible && dtpReportDate.Visible && !lblShift.Visible)
            {
                GenerateDailySummaryReport();
            }
            else if (lblCustomRange.Visible && lblCustomRange2.Visible && dtpCustomFrom.Visible && dtpCustomTo.Visible)
            {
                GenerateCustomReport();
            }
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string reportsDir = Path.Combine(desktopPath, "Reports");
            if (!Directory.Exists(reportsDir))
                Directory.CreateDirectory(reportsDir);

            string fileName = $"Omni360_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string pdfFilePath = Path.Combine(reportsDir, fileName);
            string reportTitle = "Omni360 Report";
            string textToExport = rtbShiftReport.Text;

            try
            {
                var document = new PdfSharpCore.Pdf.PdfDocument();
                document.Info.Title = reportTitle;
                var page = document.AddPage();
                var gfx = PdfSharpCore.Drawing.XGraphics.FromPdfPage(page);
                var fontTitle = new PdfSharpCore.Drawing.XFont("Verdana", 14, PdfSharpCore.Drawing.XFontStyle.Bold);
                var fontBody = new PdfSharpCore.Drawing.XFont("Consolas", 10, PdfSharpCore.Drawing.XFontStyle.Regular);

                double margin = 40;
                double y = margin;

                gfx.DrawString(reportTitle, fontTitle, PdfSharpCore.Drawing.XBrushes.Black, new PdfSharpCore.Drawing.XRect(margin, y, page.Width - margin, 20), PdfSharpCore.Drawing.XStringFormats.TopLeft);
                y += 30;

                var lines = textToExport.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                foreach (var line in lines)
                {
                    gfx.DrawString(line, fontBody, PdfSharpCore.Drawing.XBrushes.Black, new PdfSharpCore.Drawing.XPoint(margin, y));
                    y += 15;
                    if (y > page.Height - margin)
                    {
                        page = document.AddPage();
                        gfx = PdfSharpCore.Drawing.XGraphics.FromPdfPage(page);
                        y = margin;
                    }
                }

                document.Save(pdfFilePath);
                MessageBox.Show($"PDF exported to:\n{pdfFilePath}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export failed:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DashboardBtn_Click(object sender, EventArgs e)
        {
            MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];
            if (mainForm != null)
            {
                mainForm.Show();
                this.Close();
            }
        }

        private void dgvRecentShifts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Ensure the user clicked the "View" link column and a valid row
                if (e.RowIndex >= 0 && dgvRecentShifts.Columns[e.ColumnIndex].Name == "colView")
                {
                    // Use the correct column names as set up in your LoadRecentShifts method
                    string dateString = dgvRecentShifts.Rows[e.RowIndex].Cells["ShiftDate"].Value?.ToString();
                    string shiftString = dgvRecentShifts.Rows[e.RowIndex].Cells["Shift"].Value?.ToString();

                    if (string.IsNullOrEmpty(dateString) || string.IsNullOrEmpty(shiftString))
                    {
                        MessageBox.Show("Unable to determine shift or date for this row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!DateTime.TryParse(dateString, out DateTime date))
                    {
                        MessageBox.Show("Could not parse the date for this shift.");
                        return;
                    }

                    int shiftNum = 1;
                    if (shiftString == "2")
                        shiftNum = 2;
                    else if (shiftString == "3")
                        shiftNum = 3;

                    // Update the report date and select the correct shift radio button
                    dtpReportDate.Value = date;
                    rbShift1.Checked = (shiftNum == 1);
                    rbShift2.Checked = (shiftNum == 2);
                    rbShift3.Checked = (shiftNum == 3);

                    ShowShiftReportSection();
                    GenerateShiftSummaryReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing shift view: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ShowShiftReportSection()
        {
            lblDate.Visible = dtpReportDate.Visible = true;
            lblShift.Visible = rbShift1.Visible = rbShift2.Visible = true;
            rbShift3.Visible = (shiftCount == 3);
            btnGenerateReport.Visible = true;
            rtbShiftReport.Visible = true;
            dgvRecentShifts.Visible = true;
            lblCustomRange.Visible = dtpCustomFrom.Visible = dtpCustomTo.Visible = false;
            lblCustomRange2.Visible = false;

            lblShift1Start.Visible = dtpShift1Start.Visible = true;
            lblShift1End.Visible = dtpShift1End.Visible = true;
            lblShift2Start.Visible = dtpShift2Start.Visible = true;
            lblShift2End.Visible = dtpShift2End.Visible = true;
            lblShift3Start.Visible = dtpShift3Start.Visible = (shiftCount == 3);
            lblShift3End.Visible = dtpShift3End.Visible = (shiftCount == 3);
        }

        private void ShowDailySummarySection()
        {
            lblDate.Visible = dtpReportDate.Visible = true;
            lblShift.Visible = rbShift1.Visible = rbShift2.Visible = false;
            rbShift3.Visible = false;
            btnGenerateReport.Visible = true;
            rtbShiftReport.Visible = true;
            dgvRecentShifts.Visible = false;
            lblCustomRange.Visible = dtpCustomFrom.Visible = dtpCustomTo.Visible = false;
            lblCustomRange2.Visible = false;

            lblShift1Start.Visible = dtpShift1Start.Visible = true;
            lblShift1End.Visible = dtpShift1End.Visible = true;
            lblShift2Start.Visible = dtpShift2Start.Visible = true;
            lblShift2End.Visible = dtpShift2End.Visible = true;
            lblShift3Start.Visible = dtpShift3Start.Visible = false;
            lblShift3End.Visible = dtpShift3End.Visible = false;
        }

        private void ShowCustomReportSection()
        {
            lblDate.Visible = dtpReportDate.Visible = false;
            lblShift.Visible = rbShift1.Visible = rbShift2.Visible = rbShift3.Visible = false;
            btnGenerateReport.Visible = true;
            rtbShiftReport.Visible = true;
            dgvRecentShifts.Visible = false;
            lblCustomRange.Visible = dtpCustomFrom.Visible = dtpCustomTo.Visible = true;
            lblCustomRange2.Visible = true;

            lblShift1Start.Visible = dtpShift1Start.Visible = true;
            lblShift1End.Visible = dtpShift1End.Visible = true;
            lblShift2Start.Visible = dtpShift2Start.Visible = true;
            lblShift2End.Visible = dtpShift2End.Visible = true;
            lblShift3Start.Visible = dtpShift3Start.Visible = (shiftCount == 3);
            lblShift3End.Visible = dtpShift3End.Visible = (shiftCount == 3);
        }

        private void GenerateShiftSummaryReport()
        {
            DateTime selectedDate = dtpReportDate.Value.Date;
            int shiftNum = rbShift1.Checked ? 1 : rbShift2.Checked ? 2 : (shiftCount == 3 && rbShift3.Checked ? 3 : 1);
            DateTime shiftStart, shiftEnd;

            if (shiftNum == 1)
            {
                shiftStart = selectedDate.Date + shift1Start;
                shiftEnd = selectedDate.Date + shift1End;
                if (shiftEnd <= shiftStart) shiftEnd = shiftEnd.AddDays(1);
            }
            else if (shiftNum == 2)
            {
                shiftStart = selectedDate.Date + shift2Start;
                shiftEnd = selectedDate.Date + shift2End;
                if (shiftEnd <= shiftStart) shiftEnd = shiftEnd.AddDays(1);
            }
            else
            {
                shiftStart = selectedDate.Date + shift3Start;
                shiftEnd = selectedDate.Date + shift3End;
                if (shiftEnd <= shiftStart) shiftEnd = shiftEnd.AddDays(1);
            }

            string cameraSummary = GetCameraStatsString(shiftStart, shiftEnd);

            rtbShiftReport.Text = $"Shift Report for {selectedDate:yyyy-MM-dd}, Shift {shiftNum}\nPeriod: {shiftStart:yyyy-MM-dd HH:mm} to {shiftEnd:yyyy-MM-dd HH:mm}\n\n{cameraSummary}";
        }

        private void GenerateDailySummaryReport()
        {
            DateTime selectedDate = dtpReportDate.Value.Date;
            DateTime dayStart = selectedDate.Date;
            DateTime dayEnd = selectedDate.Date.AddDays(1);

            string cameraSummary = GetCameraStatsString(dayStart, dayEnd);

            rtbShiftReport.Text = $"Daily Summary for {selectedDate:yyyy-MM-dd}\nPeriod: {dayStart:yyyy-MM-dd HH:mm} to {dayEnd:yyyy-MM-dd HH:mm}\n\n{cameraSummary}";
        }

        private void GenerateCustomReport()
        {
            DateTime dateFrom = dtpCustomFrom.Value;
            DateTime dateTo = dtpCustomTo.Value;
            if (dateFrom > dateTo)
            {
                MessageBox.Show("Custom: The 'From' date cannot be after the 'To' date.", "Date Range Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string cameraSummary = GetCameraStatsString(dateFrom, dateTo);

            rtbShiftReport.Text = $"Custom Report\nPeriod: {dateFrom:yyyy-MM-dd HH:mm} to {dateTo:yyyy-MM-dd HH:mm}\n\n{cameraSummary}";
        }

        private void dtpCustomFrom_ValueChanged(object sender, EventArgs e)
        {
            // Show the time picker control (optional, for feedback)
            dtpCustomFromTime.Visible = false;

            // Use the existing time as initial value if available, otherwise use now
            DateTime initialTime = dtpCustomFromTime.Visible ? dtpCustomFromTime.Value : DateTime.Now;

            // Pop up your custom time picker, setting the initial time
            var timePicker = new PopUpTimePicker();
            timePicker.SetTime(initialTime);

            // Show as modal dialog with this form as owner, and ensure it's on top
            timePicker.TopMost = true; // Just in case
            if (timePicker.ShowDialog(this) == DialogResult.OK && timePicker.SelectedTime.HasValue)
            {
                DateTime datePart = dtpCustomFrom.Value.Date;
                DateTime timePart = timePicker.SelectedTime.Value;

                // Combine date and time
                DateTime combined = datePart.AddHours(timePart.Hour).AddMinutes(timePart.Minute);

                // Set in your dtpCustomFromTime for feedback
                dtpCustomFromTime.Value = combined;
            }
        }        
        private void dtpCustomTo_ValueChanged(object sender, EventArgs e)
        {
            dtpCustomToTime.Visible = false;
            var timePicker = new PopUpTimePicker();
            timePicker.SetTime(DateTime.Now);

            if (timePicker.ShowDialog() == DialogResult.OK && timePicker.SelectedTime.HasValue)
            {
                DateTime datePart = dtpCustomTo.Value.Date;
                DateTime timePart = timePicker.SelectedTime.Value;
                DateTime combined = datePart.AddHours(timePart.Hour).AddMinutes(timePart.Minute);
                dtpCustomToTime.Value = combined;
            }
        }
        private string GetCameraStatsString(DateTime start, DateTime end)
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InspectionResults.db");
            if (!File.Exists(dbPath))
                return "Database not found.";

            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                var cmd = new SQLiteCommand(
                    @"SELECT CameraNumber, COUNT(*) as Bottles, 
                     SUM(CASE WHEN MissingCap != '0' THEN 1 ELSE 0 END) as MissingCapFails,
                     SUM(CASE WHEN HighCap != '0' THEN 1 ELSE 0 END) as HighCapFails,
                     SUM(CASE WHEN TamperBand != '0' THEN 1 ELSE 0 END) as TamperBandFails,
                     SUM(CASE WHEN FillLevel != '0' THEN 1 ELSE 0 END) as FillLevelFails,
                     SUM(CASE WHEN TotalGood IS NOT NULL AND TotalGood != '' THEN CAST(TotalGood AS INTEGER) ELSE 0 END) as GoodCount,
                     SUM(CASE WHEN TotalBad IS NOT NULL AND TotalBad != '' THEN CAST(TotalBad AS INTEGER) ELSE 0 END) as BadCount,
                     MAX(TotalThroughput) as TotalThroughput,
                     MAX(UserName) as LastUser
              FROM CameraResults
              WHERE Timestamp BETWEEN @start AND @end
              GROUP BY CameraNumber
              ORDER BY CameraNumber", conn);
                cmd.Parameters.AddWithValue("@start", start.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@end", end.ToString("yyyy-MM-dd HH:mm:ss"));
                var adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }

            var summary = new StringBuilder();
            summary.AppendLine($"Camera Summary ({start:yyyy-MM-dd HH:mm} to {end:yyyy-MM-dd HH:mm}):");
            summary.AppendLine("-----------------------------------------------------");
            foreach (DataRow row in dt.Rows)
            {
                int camNum = Convert.ToInt32(row["CameraNumber"]);
                int bottles = Convert.ToInt32(row["Bottles"]);
                int missingCap = Convert.ToInt32(row["MissingCapFails"]);
                int highCap = Convert.ToInt32(row["HighCapFails"]);
                int tamperBand = Convert.ToInt32(row["TamperBandFails"]);
                int fillLevel = Convert.ToInt32(row["FillLevelFails"]);
                int goodCount = Convert.ToInt32(row["GoodCount"]);
                int badCount = Convert.ToInt32(row["BadCount"]);
                int throughput = row["TotalThroughput"] != DBNull.Value ? Convert.ToInt32(row["TotalThroughput"]) : 0;
                string user = row["LastUser"].ToString();

                summary.AppendLine($"Camera {camNum}:");
                summary.AppendLine($"  Bottles Processed: {bottles}");
                summary.AppendLine($"  Good Count:        {goodCount}");
                summary.AppendLine($"  Bad Count:         {badCount}");
                summary.AppendLine($"  Missing Cap Fails: {missingCap}");
                summary.AppendLine($"  High Cap Fails:    {highCap}");
                summary.AppendLine($"  TamperBand Fails:  {tamperBand}");
                summary.AppendLine($"  Fill Level Fails:  {fillLevel}");
                summary.AppendLine($"  Throughput:        {throughput}");
                summary.AppendLine($"  Last User:         {user}");
                summary.AppendLine();
            }
            if (dt.Rows.Count == 0)
                summary.AppendLine("No data for this time period.");

            return summary.ToString();
        }

        // ----------- FLAVOUR GRID SECTION ------------

        /// <summary>
        /// Loads the last 5 unique flavour+size combinations into dgvRecentFlavours.
        /// The Flavours column displays "Size Flavour" (e.g., "1L Aquafina").
        /// </summary>
        private void LoadLastFiveFlavours()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InspectionResults.db");
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                var cmd = new SQLiteCommand(
                    @"SELECT MAX(Timestamp) AS Date, 
                      (BottleSize || ' ' || BottleFlavour) AS Flavours, 
                      MAX(UserName) AS Operator
              FROM CameraResults
              WHERE BottleFlavour IS NOT NULL AND BottleFlavour != ''
              GROUP BY BottleFlavour, BottleSize
              ORDER BY Date DESC
              LIMIT 5", conn);

                var adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            dgvRecentFlavours.DataSource = null;
            dgvRecentFlavours.Columns.Clear(); // <-- Prevent duplicate columns!
            dgvRecentFlavours.DataSource = dt;
            SetRecentFlavoursGridColumns();
        }
        /// <summary>
        /// Loads 5 flavour+size combinations starting from the selected date.
        /// The Flavours column displays "Size Flavour" (e.g., "1L Aquafina").
        /// </summary>
        /// 
        private void LoadFiveFlavoursFromDate(DateTime dateFrom)
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InspectionResults.db");
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                var cmd = new SQLiteCommand(
                    @"SELECT MIN(Timestamp) AS Date, 
                              (BottleSize || ' ' || BottleFlavour) AS Flavours, 
                              MAX(UserName) AS Operator
                      FROM CameraResults
                      WHERE BottleFlavour IS NOT NULL AND BottleFlavour != ''
                        AND Timestamp >= @start
                      GROUP BY BottleFlavour, BottleSize
                      ORDER BY Date ASC
                      LIMIT 5", conn);

                cmd.Parameters.AddWithValue("@start", dateFrom.ToString("yyyy-MM-dd HH:mm:ss"));
                var adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            dgvRecentFlavours.DataSource = dt;
            SetRecentFlavoursGridColumns();
        }
        private void LoadRecentShifts()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InspectionResults.db");
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                // You may want to aggregate by shift, date, etc.
                // Example assumes you have Shift info, if not, you need to calculate which shift each record is in.
                // For the demo, let's assume you want last 5 unique dates+shifts with counts:
                var cmd = new SQLiteCommand(
                    @"SELECT 
                  DATE(Timestamp) AS ShiftDate,
                  CASE
                    WHEN TIME(Timestamp) >= '07:00' AND TIME(Timestamp) < '15:00' THEN '1'
                    WHEN TIME(Timestamp) >= '15:00' AND TIME(Timestamp) < '23:00' THEN '2'
                    ELSE '3'
                  END AS Shift,
                  COUNT(*) AS Bottles
              FROM CameraResults
              GROUP BY ShiftDate, Shift
              ORDER BY ShiftDate DESC, Shift DESC
              LIMIT 15", conn);

                var adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            dgvRecentShifts.DataSource = null;
            dgvRecentShifts.Columns.Clear();
            dgvRecentShifts.DataSource = dt;

            // Add "View" link column if not present
            if (!dgvRecentShifts.Columns.Contains("colView"))
            {
                var viewColumn = new DataGridViewLinkColumn();
                viewColumn.Name = "colView";
                viewColumn.HeaderText = "View";
                viewColumn.Text = "View";
                viewColumn.UseColumnTextForLinkValue = true;
                dgvRecentShifts.Columns.Add(viewColumn);
            }

            // Set headers and column order
            if (dgvRecentShifts.Columns.Contains("ShiftDate"))
                dgvRecentShifts.Columns["ShiftDate"].HeaderText = "Date";
            if (dgvRecentShifts.Columns.Contains("Shift"))
                dgvRecentShifts.Columns["Shift"].HeaderText = "Shift";
            if (dgvRecentShifts.Columns.Contains("Bottles"))
                dgvRecentShifts.Columns["Bottles"].HeaderText = "Bottles";

            dgvRecentShifts.ReadOnly = true;
            dgvRecentShifts.AllowUserToAddRows = false;
            dgvRecentShifts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void dtpCustomFromFlavours_ValueChanged(object sender, EventArgs e)
        {
            LoadFiveFlavoursFromDate(dtpCustomFromFlavours.Value);
        }

        /// <summary>
        /// Sets up column headers and adds the View link column if not already present.
        /// </summary>
        private void SetRecentFlavoursGridColumns()
        {
            if (dgvRecentFlavours.Columns.Contains("Date"))
                dgvRecentFlavours.Columns["Date"].HeaderText = "Date";
            if (dgvRecentFlavours.Columns.Contains("Flavours"))
                dgvRecentFlavours.Columns["Flavours"].HeaderText = "Flavours";
            if (dgvRecentFlavours.Columns.Contains("Operator"))
                dgvRecentFlavours.Columns["Operator"].HeaderText = "Operator";

            // Add "View" link column if not already present
            if (!dgvRecentFlavours.Columns.Contains("View"))
            {
                var viewColumn = new DataGridViewLinkColumn();
                viewColumn.Name = "View";
                viewColumn.HeaderText = "View";
                viewColumn.Text = "View";
                viewColumn.UseColumnTextForLinkValue = true;
                dgvRecentFlavours.Columns.Add(viewColumn);
            }

            dgvRecentFlavours.Columns["Date"].DisplayIndex = 0;
            dgvRecentFlavours.Columns["Flavours"].DisplayIndex = 1;
            dgvRecentFlavours.Columns["Operator"].DisplayIndex = 2;
            dgvRecentFlavours.Columns["View"].DisplayIndex = 3;

            dgvRecentFlavours.ReadOnly = true;
            dgvRecentFlavours.AllowUserToAddRows = false;
            dgvRecentFlavours.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvRecentFlavours.CellContentClick -= dgvRecentFlavours_CellContentClick;
            dgvRecentFlavours.CellContentClick += dgvRecentFlavours_CellContentClick;
        }

        /// <summary>
        /// Handles the View link click and populates details screen.
        /// </summary>
        private void dgvRecentFlavours_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvRecentFlavours.Columns["View"].Index && e.RowIndex >= 0)
            {
                var row = dgvRecentFlavours.Rows[e.RowIndex];
                string flavours = row.Cells["Flavours"].Value?.ToString();

                // Split "1L Aquafina" into size and flavour
                var split = flavours.Split(new[] { ' ' }, 2);
                if (split.Length == 2)
                {
                    string bottleSize = split[0];
                    string bottleFlavour = split[1];

                    // For period, use your preferred controls (example: current shift, today, or custom from-to)
                    DateTime start = dtpCustomFromFlavours.Value.Date; // e.g. start date from picker
                    DateTime end = DateTime.Now; // e.g. now, or another picker

                    GenerateFlavourReport(bottleSize, bottleFlavour, start, end);
                }
            }
        }
        private void GenerateFlavourReport(string bottleSize, string bottleFlavour, DateTime start, DateTime end)
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InspectionResults.db");
            if (!File.Exists(dbPath))
            {
                rtbShiftReport.Text = "Database not found.";
                return;
            }

            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                var cmd = new SQLiteCommand(
                    @"SELECT COUNT(*) as Bottles,
                     SUM(CASE WHEN MissingCap != '0' THEN 1 ELSE 0 END) as MissingCapFails,
                     SUM(CASE WHEN HighCap != '0' THEN 1 ELSE 0 END) as HighCapFails,
                     SUM(CASE WHEN TamperBand != '0' THEN 1 ELSE 0 END) as TamperBandFails,
                     SUM(CASE WHEN FillLevel != '0' THEN 1 ELSE 0 END) as FillLevelFails,
                     SUM(CASE WHEN TotalGood IS NOT NULL AND TotalGood != '' THEN CAST(TotalGood AS INTEGER) ELSE 0 END) as GoodCount,
                     SUM(CASE WHEN TotalBad IS NOT NULL AND TotalBad != '' THEN CAST(TotalBad AS INTEGER) ELSE 0 END) as BadCount,
                     GROUP_CONCAT(DISTINCT UserName) as Operators,
                     GROUP_CONCAT(DISTINCT CameraNumber) as Cameras
              FROM CameraResults
              WHERE BottleSize = @size AND BottleFlavour = @flavour AND Timestamp BETWEEN @start AND @end", conn);

                cmd.Parameters.AddWithValue("@size", bottleSize);
                cmd.Parameters.AddWithValue("@flavour", bottleFlavour);
                cmd.Parameters.AddWithValue("@start", start.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@end", end.ToString("yyyy-MM-dd HH:mm:ss"));
                var adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                var sb = new StringBuilder();
                sb.AppendLine($"Flavour Report for {bottleSize} {bottleFlavour}");
                sb.AppendLine($"Period: {start:yyyy-MM-dd HH:mm} to {end:yyyy-MM-dd HH:mm}");
                sb.AppendLine();
                sb.AppendLine($"Total Bottles Processed: {row["Bottles"]}");
                sb.AppendLine($"Good Count: {row["GoodCount"]}");
                sb.AppendLine($"Bad Count: {row["BadCount"]}");
                sb.AppendLine($"Missing Cap Fails: {row["MissingCapFails"]}");
                sb.AppendLine($"High Cap Fails: {row["HighCapFails"]}");
                sb.AppendLine($"Tamper Band Fails: {row["TamperBandFails"]}");
                sb.AppendLine($"Fill Level Fails: {row["FillLevelFails"]}");
                sb.AppendLine($"Operator(s): {row["Operators"]}");
                sb.AppendLine($"Camera(s): {row["Cameras"]}");

                rtbShiftReport.Text = sb.ToString();
            }
            else
            {
                rtbShiftReport.Text = "No data for this flavour in the selected time period.";
            }
        }

    }
}