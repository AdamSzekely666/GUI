using Nini.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PdfSharp;
using PdfSharp.Drawing;
using System.IO;
using PdfSharp.Pdf;
using System.Globalization;
using System.Threading;

namespace Omnicheck360
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

        // Fields to save/restore original culture
        private CultureInfo originalCulture;
        private CultureInfo originalUICulture;

        public Data(MainForm mainForm)
        {
            // Save the original culture before changing
            originalCulture = Thread.CurrentThread.CurrentCulture;
            originalUICulture = Thread.CurrentThread.CurrentUICulture;

            // Set to French only for this form
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");

            InitializeComponent();
            MessageBox.Show(System.Globalization.CultureInfo.CurrentCulture.DisplayName);
            this.mainForm = mainForm;
            // Load the localization file ONCE at startup
            Localization.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "translation.json"));
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Restore original culture when form is closed
            Thread.CurrentThread.CurrentCulture = originalCulture;
            Thread.CurrentThread.CurrentUICulture = originalUICulture;
            base.OnFormClosed(e);
        }

        private void Data_Load(object sender, EventArgs e)
        {
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

            // ---- Set the three main DateTimePickers to French format explicitly (optional, since culture is French now)
            dtpReportDate.Format = DateTimePickerFormat.Long;
            dtpCustomFrom.Format = DateTimePickerFormat.Long;
            dtpCustomTo.Format = DateTimePickerFormat.Long;
            // ----------------------------------------

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

            // --- Apply localization to controls ---
            btnGenerateReport.Text = Localization.T("btnGenerateReport");
            btnCustomReport.Text = Localization.T("btnCustomReport");
            btnDailySummary.Text = Localization.T("btnDailySummary");
            btnShiftReport.Text = Localization.T("btnShiftReport");
            btnExportPdf.Text = Localization.T("btnExportPdf");
            label1.Text = Localization.T("labelShiftCount");
            lblCustomRange2.Text = Localization.T("lblCustomRangeTo");
            lblCustomRange.Text = Localization.T("lblCustomRangeFrom");
            lblDate.Text = Localization.T("lblDate");
            lblShift.Text = Localization.T("lblShift");
            lblShift1Start.Text = Localization.T("lblShift1Start");
            lblShift1End.Text = Localization.T("lblShift1End");
            lblShift2Start.Text = Localization.T("lblShift2Start");
            lblShift2End.Text = Localization.T("lblShift2End");
            lblShift3Start.Text = Localization.T("lblShift3Start");
            lblShift3End.Text = Localization.T("lblShift3End");
            UpdateShiftRadioButtonText();

            // DataGridView headers will be set after data is loaded
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
            rbShift1.Text = Localization.T("rbShift1", shift1Start.ToString(@"hh\:mm"), shift1End.ToString(@"hh\:mm"));
            rbShift2.Text = Localization.T("rbShift2", shift2Start.ToString(@"hh\:mm"), shift2End.ToString(@"hh\:mm"));
            rbShift3.Text = Localization.T("rbShift3", shift3Start.ToString(@"hh\:mm"), shift3End.ToString(@"hh\:mm"));
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
            string reportTitle = Localization.T("reportTitle");
            string textToExport = rtbShiftReport.Text;

            try
            {
                var document = new PdfDocument();
                document.Info.Title = reportTitle;
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var fontTitle = new XFont("Verdana", 14, XFontStyle.Bold);
                var fontBody = new XFont("Consolas", 10, XFontStyle.Regular);
                double margin = 40;
                double y = margin;

                gfx.DrawString(reportTitle, fontTitle, XBrushes.Black, new XRect(margin, y, page.Width - margin, 20), XStringFormats.TopLeft);
                y += 30;

                var lines = textToExport.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                foreach (var line in lines)
                {
                    gfx.DrawString(line, fontBody, XBrushes.Black, new XPoint(margin, y));
                    y += 15;
                    if (y > page.Height - margin)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        y = margin;
                    }
                }

                document.Save(pdfFilePath);
                MessageBox.Show(Localization.T("pdfExportSuccess", pdfFilePath), "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Localization.T("pdfExportError", ex.Message), "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (e.RowIndex >= 0 && dgvRecentShifts.Columns[e.ColumnIndex].Name == "colView")
                {
                    string dateString = dgvRecentShifts.Rows[e.RowIndex].Cells["ShiftDate"].Value?.ToString();
                    string shiftString = dgvRecentShifts.Rows[e.RowIndex].Cells["Shift"].Value?.ToString();

                    if (string.IsNullOrEmpty(dateString) || string.IsNullOrEmpty(shiftString))
                    {
                        MessageBox.Show(Localization.T("msgShiftRowError"), "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!DateTime.TryParse(dateString, out DateTime date))
                    {
                        MessageBox.Show(Localization.T("msgShiftDateParseError"), "Erreur");
                        return;
                    }

                    int shiftNum = 1;
                    if (shiftString == "2")
                        shiftNum = 2;
                    else if (shiftString == "3")
                        shiftNum = 3;

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
                MessageBox.Show(Localization.T("msgShiftRowException", ex.Message), "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            rtbShiftReport.Text = Localization.T("shiftReportTitle", selectedDate.ToString("yyyy-MM-dd"), shiftNum) + "\n"
                + Localization.T("shiftReportPeriod", shiftStart.ToString("yyyy-MM-dd HH:mm"), shiftEnd.ToString("yyyy-MM-dd HH:mm")) + "\n\n"
                + cameraSummary;
        }

        private void GenerateDailySummaryReport()
        {
            DateTime selectedDate = dtpReportDate.Value.Date;
            DateTime dayStart = selectedDate.Date;
            DateTime dayEnd = selectedDate.Date.AddDays(1);

            string cameraSummary = GetCameraStatsString(dayStart, dayEnd);

            rtbShiftReport.Text = Localization.T("dailySummaryTitle", selectedDate.ToString("yyyy-MM-dd")) + "\n"
                + Localization.T("shiftReportPeriod", dayStart.ToString("yyyy-MM-dd HH:mm"), dayEnd.ToString("yyyy-MM-dd HH:mm")) + "\n\n"
                + cameraSummary;
        }

        private void GenerateCustomReport()
        {
            DateTime dateFrom = dtpCustomFrom.Value;
            DateTime dateTo = dtpCustomTo.Value;
            if (dateFrom > dateTo)
            {
                MessageBox.Show(Localization.T("msgCustomFromAfterTo"), "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string cameraSummary = GetCameraStatsString(dateFrom, dateTo);

            rtbShiftReport.Text = Localization.T("customReportTitle") + "\n"
                + Localization.T("shiftReportPeriod", dateFrom.ToString("yyyy-MM-dd HH:mm"), dateTo.ToString("yyyy-MM-dd HH:mm")) + "\n\n"
                + cameraSummary;
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
                        SUM(CASE WHEN BadOCR != '0' THEN 1 ELSE 0 END) as BadOCRFails,
                        SUM(CASE WHEN BadBarcode != '0' THEN 1 ELSE 0 END) as BadBarcodeFails,
                        SUM(CASE WHEN TotalGood IS NOT NULL AND TotalGood != '' THEN CAST(TotalGood AS INTEGER) ELSE 0 END) as GoodCount,
                        SUM(CASE WHEN TotalBad IS NOT NULL AND TotalBad != '' THEN CAST(TotalBad AS INTEGER) ELSE 0 END) as BadCount,
                        MAX(TotalThroughput) as TotalThroughput,
                        MAX(UserName) as LastUser
                    FROM InspectionResults
                    WHERE Timestamp BETWEEN @start AND @end
                    GROUP BY CameraNumber
                    ORDER BY CameraNumber", conn);
                cmd.Parameters.AddWithValue("@start", start.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@end", end.ToString("yyyy-MM-dd HH:mm:ss"));
                var adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }

            var summary = new StringBuilder();
            summary.AppendLine(Localization.T("shiftReportSummaryTitle", start.ToString("yyyy-MM-dd HH:mm"), end.ToString("yyyy-MM-dd HH:mm")));
            summary.AppendLine("-----------------------------------------------------");
            foreach (DataRow row in dt.Rows)
            {
                int camNum = Convert.ToInt32(row["CameraNumber"]);
                int bottles = Convert.ToInt32(row["Bottles"]);
                int badOCR = Convert.ToInt32(row["BadOCRFails"]);
                int badBarcode = Convert.ToInt32(row["BadBarcodeFails"]);
                int goodCount = Convert.ToInt32(row["GoodCount"]);
                int badCount = Convert.ToInt32(row["BadCount"]);
                int throughput = row["TotalThroughput"] != DBNull.Value ? Convert.ToInt32(row["TotalThroughput"]) : 0;
                string user = row["LastUser"].ToString();

                summary.AppendLine(Localization.T("cameraLabel", camNum));
                summary.AppendLine(Localization.T("bottlesProcessed", bottles));
                summary.AppendLine(Localization.T("goodCount", goodCount));
                summary.AppendLine(Localization.T("badCount", badCount));
                summary.AppendLine(Localization.T("badOCR", badOCR));
                summary.AppendLine(Localization.T("badBarcode", badBarcode));
                summary.AppendLine(Localization.T("throughput", throughput));
                summary.AppendLine(Localization.T("lastUser", user));
                summary.AppendLine();
            }
            if (dt.Rows.Count == 0)
                summary.AppendLine(Localization.T("shiftReportSummaryNoData"));

            return summary.ToString();
        }

        // ----------- FLAVOUR GRID SECTION ------------

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
                    FROM InspectionResults
                    WHERE BottleFlavour IS NOT NULL AND BottleFlavour != ''
                    GROUP BY BottleFlavour, BottleSize
                    ORDER BY Date DESC
                    LIMIT 5", conn);

                var adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            dgvRecentFlavours.DataSource = null;
            dgvRecentFlavours.Columns.Clear();
            dgvRecentFlavours.DataSource = dt;
            SetRecentFlavoursGridColumns();
        }
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
                        FROM InspectionResults
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
                var cmd = new SQLiteCommand(
                    @"SELECT 
                        DATE(Timestamp) AS ShiftDate,
                        CASE
                            WHEN TIME(Timestamp) >= '07:00' AND TIME(Timestamp) < '15:00' THEN '1'
                            WHEN TIME(Timestamp) >= '15:00' AND TIME(Timestamp) < '23:00' THEN '2'
                            ELSE '3'
                        END AS Shift,
                        COUNT(*) AS Bottles
                    FROM InspectionResults
                    GROUP BY ShiftDate, Shift
                    ORDER BY ShiftDate DESC, Shift DESC
                    LIMIT 15", conn);

                var adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            dgvRecentShifts.DataSource = null;
            dgvRecentShifts.Columns.Clear();
            dgvRecentShifts.DataSource = dt;

            if (!dgvRecentShifts.Columns.Contains("colView"))
            {
                var viewColumn = new DataGridViewLinkColumn();
                viewColumn.Name = "colView";
                viewColumn.HeaderText = Localization.T("colView");
                viewColumn.Text = Localization.T("colView");
                viewColumn.UseColumnTextForLinkValue = true;
                dgvRecentShifts.Columns.Add(viewColumn);
            }

            if (dgvRecentShifts.Columns.Contains("ShiftDate"))
                dgvRecentShifts.Columns["ShiftDate"].HeaderText = Localization.T("colDate");
            if (dgvRecentShifts.Columns.Contains("Shift"))
                dgvRecentShifts.Columns["Shift"].HeaderText = Localization.T("colShift");
            if (dgvRecentShifts.Columns.Contains("Bottles"))
                dgvRecentShifts.Columns["Bottles"].HeaderText = Localization.T("colBottles");

            dgvRecentShifts.ReadOnly = true;
            dgvRecentShifts.AllowUserToAddRows = false;
            dgvRecentShifts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void dtpCustomFromFlavours_ValueChanged(object sender, EventArgs e)
        {
            LoadFiveFlavoursFromDate(dtpCustomFromFlavours.Value);
        }

        private void SetRecentFlavoursGridColumns()
        {
            if (dgvRecentFlavours.Columns.Contains("Date"))
                dgvRecentFlavours.Columns["Date"].HeaderText = Localization.T("colDate");
            if (dgvRecentFlavours.Columns.Contains("Flavours"))
                dgvRecentFlavours.Columns["Flavours"].HeaderText = Localization.T("colFlavours");
            if (dgvRecentFlavours.Columns.Contains("Operator"))
                dgvRecentFlavours.Columns["Operator"].HeaderText = Localization.T("colOperator");

            if (!dgvRecentFlavours.Columns.Contains("View"))
            {
                var viewColumn = new DataGridViewLinkColumn();
                viewColumn.Name = "View";
                viewColumn.HeaderText = Localization.T("colView");
                viewColumn.Text = Localization.T("colView");
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

        private void dgvRecentFlavours_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvRecentFlavours.Columns["View"].Index && e.RowIndex >= 0)
            {
                var row = dgvRecentFlavours.Rows[e.RowIndex];
                string flavours = row.Cells["Flavours"].Value?.ToString();

                var split = flavours.Split(new[] { ' ' }, 2);
                if (split.Length == 2)
                {
                    string bottleSize = split[0];
                    string bottleFlavour = split[1];

                    DateTime start = dtpCustomFromFlavours.Value.Date;
                    DateTime end = DateTime.Now;

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
                        SUM(CASE WHEN BadOCR != '0' THEN 1 ELSE 0 END) as BadOCRFails,
                        SUM(CASE WHEN BadBarcode != '0' THEN 1 ELSE 0 END) as BadBarcodeFails,
                        SUM(CASE WHEN TotalGood IS NOT NULL AND TotalGood != '' THEN CAST(TotalGood AS INTEGER) ELSE 0 END) as GoodCount,
                        SUM(CASE WHEN TotalBad IS NOT NULL AND TotalBad != '' THEN CAST(TotalBad AS INTEGER) ELSE 0 END) as BadCount,
                        GROUP_CONCAT(DISTINCT UserName) as Operators,
                        GROUP_CONCAT(DISTINCT CameraNumber) as Cameras
                    FROM InspectionResults
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
                sb.AppendLine(Localization.T("flavourReportTitle", bottleSize, bottleFlavour));
                sb.AppendLine(Localization.T("flavourReportPeriod", start.ToString("yyyy-MM-dd HH:mm"), end.ToString("yyyy-MM-dd HH:mm")));
                sb.AppendLine();
                sb.AppendLine(Localization.T("flavourReportBottles", row["Bottles"]));
                sb.AppendLine(Localization.T("flavourReportGood", row["GoodCount"]));
                sb.AppendLine(Localization.T("flavourReportBad", row["BadCount"]));
                sb.AppendLine(Localization.T("flavourReportBadOCR", row["BadOCRFails"]));
                sb.AppendLine(Localization.T("flavourReportBadBarcode", row["BadBarcodeFails"]));
                sb.AppendLine(Localization.T("flavourReportOperators", row["Operators"]));
                sb.AppendLine(Localization.T("flavourReportCameras", row["Cameras"]));

                rtbShiftReport.Text = sb.ToString();
            }
            else
            {
                rtbShiftReport.Text = Localization.T("flavourReportNoData");
            }
        }
    }
}