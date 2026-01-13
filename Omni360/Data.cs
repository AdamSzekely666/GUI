using Nini.Config;
using OmniCheck_360;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zebra.ADA.OperatorAPI;
using Zebra.ADA.OperatorAPI.InputsPackage;

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
        private TimeSpan shift3End = new TimeSpan(7, 0, 0);
        private bool isInitializing = false;
        private IniConfigSource ini;

        public Data(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            if (mainForm != null)
            {
                mainForm.FlavourChanged += MainForm_FlavourChanged;
            }
            dtpCustomFromTime.Visible = false;
            dtpCustomToTime.Visible = false;

            // Wiring new event handlers
            dtpCustomFrom.ValueChanged += dtpCustomFrom_ValueChanged;
            dtpCustomTo.ValueChanged += dtpCustomTo_ValueChanged;
        }
        private void MainForm_FlavourChanged(object sender, EventArgs e)
        {
            // refresh the recent flavours list when the main form signals change
            LoadRecentFlavours();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                if (mainForm != null) mainForm.FlavourChanged -= MainForm_FlavourChanged;
            }
            catch { }
            base.OnFormClosing(e);
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
            dgvRecentFlavours.CellContentClick += dgvRecentFlavours_CellContentClick;
            isInitializing = false;
            UpdateShiftRadioButtonText();
            LoadRecentShifts();
            LoadRecentFlavours();
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

        private void LoadRecentShifts()
        {
            var allDbFiles = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DatabaseDBs"), "InspectionResults_*.db");
            var orderedDbFiles = allDbFiles.OrderByDescending(f => f).ToList();

            DataTable dtAll = new DataTable();
            dtAll.Columns.Add("ShiftDate", typeof(string));
            dtAll.Columns.Add("Shift", typeof(string));
            dtAll.Columns.Add("Throughput", typeof(int));

            foreach (var dbPath in orderedDbFiles)
            {
                DatabaseCreator.EnsureCameraResultsTable(dbPath);
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
                    MAX(CAST(TotalThroughput AS INTEGER)) - MIN(CAST(TotalThroughput AS INTEGER)) AS Throughput
                FROM CameraResults
                WHERE CameraNumber = 1
                GROUP BY ShiftDate, Shift
                ORDER BY ShiftDate DESC, Shift DESC
                LIMIT 15", conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataRow row = dtAll.NewRow();
                            row["ShiftDate"] = reader["ShiftDate"].ToString();
                            row["Shift"] = reader["Shift"].ToString();
                            row["Throughput"] = reader["Throughput"] != DBNull.Value ? Convert.ToInt32(reader["Throughput"]) : 0;
                            dtAll.Rows.Add(row);
                        }
                    }
                }
                if (dtAll.Rows.Count >= 15)
                    break;
            }

            var orderedRows = dtAll.AsEnumerable()
                .OrderByDescending(r => r.Field<string>("ShiftDate"))
                .ThenByDescending(r => r.Field<string>("Shift"))
                .Take(15);

            dgvRecentShifts.DataSource = null;
            dgvRecentShifts.Columns.Clear();
            if (orderedRows.Any())
            {
                var topRows = orderedRows.CopyToDataTable();
                dgvRecentShifts.DataSource = topRows;
            }
            else
            {
                dgvRecentShifts.DataSource = dtAll.Clone();
            }

            if (!dgvRecentShifts.Columns.Contains("colView"))
            {
                var viewColumn = new DataGridViewLinkColumn();
                viewColumn.Name = "colView";
                viewColumn.HeaderText = "View";
                viewColumn.Text = "View";
                viewColumn.UseColumnTextForLinkValue = true;
                dgvRecentShifts.Columns.Add(viewColumn);
            }

            if (dgvRecentShifts.Columns.Contains("ShiftDate"))
                dgvRecentShifts.Columns["ShiftDate"].HeaderText = "Date";
            if (dgvRecentShifts.Columns.Contains("Shift"))
                dgvRecentShifts.Columns["Shift"].HeaderText = "Shift";
            if (dgvRecentShifts.Columns.Contains("Throughput"))
                dgvRecentShifts.Columns["Throughput"].HeaderText = "Throughput (Cam 1)";

            dgvRecentShifts.ReadOnly = true;
            dgvRecentShifts.AllowUserToAddRows = false;
            dgvRecentShifts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

            string fileName = $"ECI_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string pdfFilePath = Path.Combine(reportsDir, fileName);
            string reportTitle = "ECI Report";
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
                if (e.RowIndex >= 0 && dgvRecentShifts.Columns[e.ColumnIndex].Name == "colView")
                {
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

        // Add event handlers for custom date change (even if these are just stubs)
        private void dtpCustomFrom_ValueChanged(object sender, EventArgs e)
        {
            // Optionally trigger a report, or leave blank
            // Example: GenerateCustomReport();
        }

        private void dtpCustomTo_ValueChanged(object sender, EventArgs e)
        {
            // Optionally trigger a report, or leave blank
            // Example: GenerateCustomReport();
        }

        private string GetCameraStatsString(DateTime start, DateTime end)
        {
            var dbPaths = MainForm.GetQuarterlyDbPathsInRange(start, end);
            if (dbPaths.Count == 0)
                return "Database not found.";

            var resultTable = new DataTable();
            resultTable.Columns.Add("CameraNumber", typeof(int));
            resultTable.Columns.Add("Finish", typeof(int));
            resultTable.Columns.Add("Base", typeof(int));
            resultTable.Columns.Add("ISW", typeof(int));
            resultTable.Columns.Add("Dent", typeof(int));
            resultTable.Columns.Add("DownCan", typeof(int));
            resultTable.Columns.Add("GoodCount", typeof(int));
            resultTable.Columns.Add("BadCount", typeof(int));
            resultTable.Columns.Add("Throughput", typeof(int));
            resultTable.Columns.Add("LastUser", typeof(string));
            resultTable.Columns.Add("Recipe", typeof(string));

            var aggregate = new Dictionary<int, DataRow>();

            foreach (var dbPath in dbPaths)
            {
                DatabaseCreator.EnsureCameraResultsTable(dbPath);
                using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    var cmd = new SQLiteCommand(
                        @"SELECT CameraNumber,
                            MAX(CAST(Finish AS INTEGER)) as Finish,
                            MAX(CAST(Base AS INTEGER)) as Base,
                            MAX(CAST(ISW AS INTEGER)) as ISW,
                            MAX(CAST(Dent AS INTEGER)) as Dent,
                            MAX(CAST(DownCan AS INTEGER)) as DownCan,
                            MAX(CAST(TotalGood AS INTEGER)) - MIN(CAST(TotalGood AS INTEGER)) AS GoodCount,
                            MAX(CAST(TotalBad AS INTEGER)) - MIN(CAST(TotalBad AS INTEGER)) AS BadCount,
                            MAX(CAST(TotalThroughput AS INTEGER)) - MIN(CAST(TotalThroughput AS INTEGER)) AS Throughput,
                            MAX(UserName) as LastUser,
                            MAX(Recipe) as Recipe
                        FROM CameraResults
                        WHERE Timestamp BETWEEN @start AND @end
                        GROUP BY CameraNumber
                        ORDER BY CameraNumber", conn);

                    cmd.Parameters.AddWithValue("@start", start.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@end", end.ToString("yyyy-MM-dd HH:mm:ss"));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int camNum = Convert.ToInt32(reader["CameraNumber"]);
                            if (!aggregate.ContainsKey(camNum))
                            {
                                var row = resultTable.NewRow();
                                row["CameraNumber"] = camNum;
                                row["Finish"] = 0;
                                row["Base"] = 0;
                                row["ISW"] = 0;
                                row["Dent"] = 0;
                                row["DownCan"] = 0;
                                row["GoodCount"] = 0;
                                row["BadCount"] = 0;
                                row["Throughput"] = 0;
                                row["LastUser"] = "";
                                row["Recipe"] = "";
                                aggregate[camNum] = row;
                                resultTable.Rows.Add(row);
                            }
                            var aggRow = aggregate[camNum];
                            aggRow["Finish"] = Convert.ToInt32(reader["Finish"]);
                            aggRow["Base"] = Convert.ToInt32(reader["Base"]);
                            aggRow["ISW"] = Convert.ToInt32(reader["ISW"]);
                            aggRow["Dent"] = Convert.ToInt32(reader["Dent"]);
                            aggRow["DownCan"] = Convert.ToInt32(reader["DownCan"]);
                            aggRow["GoodCount"] = Convert.ToInt32(reader["GoodCount"]);
                            aggRow["BadCount"] = Convert.ToInt32(reader["BadCount"]);
                            aggRow["Throughput"] = Convert.ToInt32(reader["Throughput"]);
                            aggRow["LastUser"] = reader["LastUser"].ToString();
                            aggRow["Recipe"] = reader["Recipe"].ToString();
                        }
                    }
                }
            }

            var summary = new StringBuilder();
            summary.AppendLine($"Camera Summary ({start:yyyy-MM-dd HH:mm} to {end:yyyy-MM-dd HH:mm}):");
            summary.AppendLine("-----------------------------------------------------");
            foreach (DataRow row in resultTable.Rows)
            {
                int camNum = Convert.ToInt32(row["CameraNumber"]);
                int finish = Convert.ToInt32(row["Finish"]);
                int baseVal = Convert.ToInt32(row["Base"]);
                int isw = Convert.ToInt32(row["ISW"]);
                int dent = Convert.ToInt32(row["Dent"]);
                int downCan = Convert.ToInt32(row["DownCan"]);
                int goodCount = Convert.ToInt32(row["GoodCount"]);
                int badCount = Convert.ToInt32(row["BadCount"]);
                int throughput = Convert.ToInt32(row["Throughput"]);
                string user = row["LastUser"].ToString();
                string recipe = row["Recipe"].ToString();

                summary.AppendLine($"Camera {camNum}:");
                summary.AppendLine($"  Finish:     {finish}");
                summary.AppendLine($"  Base:       {baseVal}");
                summary.AppendLine($"  ISW:        {isw}");
                summary.AppendLine($"  Dent:       {dent}");
                summary.AppendLine($"  DownCan:    {downCan}");
                summary.AppendLine($"  Good Count: {goodCount}");
                summary.AppendLine($"  Bad Count:  {badCount}");
                summary.AppendLine($"  Throughput: {throughput}");
                summary.AppendLine($"  User:       {user}");
                summary.AppendLine($"  Recipe:     {recipe}");
                summary.AppendLine();
            }
            if (resultTable.Rows.Count == 0)
                summary.AppendLine("No data for this time period.");

            return summary.ToString();
        }
        // Data.cs — paste inside the Data class
        private void LoadRecentFlavours()
        {
            try
            {
                // Build a DataTable matching the dgv columns: Date, Flavour, Operator
                var dt = new DataTable();
                dt.Columns.Add("Date", typeof(string));
                dt.Columns.Add("Flavour", typeof(string));
                dt.Columns.Add("Operator", typeof(string));

                // Search DBs newest-first and collect entries from the CurrentFlavour table
                var dbFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DatabaseDBs");
                if (!Directory.Exists(dbFolder))
                {
                    dgvRecentFlavours.DataSource = dt;
                    return;
                }

                var dbFiles = Directory.GetFiles(dbFolder, "InspectionResults_*.db")
                                       .OrderByDescending(f => f).ToList();

                foreach (var dbPath in dbFiles)
                {
                    try
                    {
                        DatabaseCreator.EnsureCameraResultsTable(dbPath);
                        using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                        {
                            conn.Open();
                            using (var cmd = new SQLiteCommand("SELECT FlavourSize, FlavourName, LastUpdated FROM CurrentFlavour ORDER BY LastUpdated DESC LIMIT 50", conn))
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var date = reader["LastUpdated"]?.ToString() ?? "";
                                    var size = reader["FlavourSize"]?.ToString() ?? "";
                                    var flavour = reader["FlavourName"]?.ToString() ?? "";

                                    // show either size+flavour or just flavour depending on preference
                                    var flavourDisplay = string.IsNullOrEmpty(size) ? flavour : $"{size} - {flavour}";

                                    var row = dt.NewRow();
                                    row["Date"] = date;
                                    row["Flavour"] = flavourDisplay;
                                    row["Operator"] = ""; // MainForm stores user in CameraResults, CurrentFlavour doesn't include operator. Leave blank or set from MainForm if available.
                                    dt.Rows.Add(row);
                                }
                            }
                            conn.Close();
                        }
                    }
                    catch
                    {
                        // ignore DB read errors for robustness; continue to next DB
                    }

                    // stop early if we have enough rows
                    if (dt.Rows.Count >= 50) break;
                }

                // Bind to grid on UI thread
                // replace the binding block at the end of LoadRecentFlavours with this
                if (dgvRecentFlavours.InvokeRequired)
                {
                    dgvRecentFlavours.BeginInvoke(new Action(() =>
                    {
                        dgvRecentFlavours.DataSource = dt;
                        dgvRecentFlavours.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dgvRecentFlavours.ReadOnly = true;
                        dgvRecentFlavours.AllowUserToAddRows = false;

                        // add a View link column if it doesn't exist
                        if (!dgvRecentFlavours.Columns.Contains("colViewFlavour"))
                        {
                            var viewColumn = new DataGridViewLinkColumn
                            {
                                Name = "colViewFlavour",
                                HeaderText = "View",
                                Text = "View",
                                UseColumnTextForLinkValue = true
                            };
                            dgvRecentFlavours.Columns.Add(viewColumn);
                        }
                    }));
                }
                else
                {
                    dgvRecentFlavours.DataSource = dt;
                    dgvRecentFlavours.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvRecentFlavours.ReadOnly = true;
                    dgvRecentFlavours.AllowUserToAddRows = false;

                    // add a View link column if it doesn't exist
                    if (!dgvRecentFlavours.Columns.Contains("colViewFlavour"))
                    {
                        var viewColumn = new DataGridViewLinkColumn
                        {
                            Name = "colViewFlavour",
                            HeaderText = "View",
                            Text = "View",
                            UseColumnTextForLinkValue = true
                        };
                        dgvRecentFlavours.Columns.Add(viewColumn);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoadRecentFlavours error: " + ex.Message);
            }
        }
        // add this method to the Data class
        // add this method to the Data class
        // Data.cs - add/replace this method in the Data class
        private void dgvRecentFlavours_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                var col = dgvRecentFlavours.Columns[e.ColumnIndex];
                if (col == null) return;

                if (col.Name == "colViewFlavour")
                {
                    string dateText = dgvRecentFlavours.Rows[e.RowIndex].Cells["Date"].Value?.ToString() ?? "";
                    string flavourText = dgvRecentFlavours.Rows[e.RowIndex].Cells["Flavour"].Value?.ToString() ?? "";
                    // If you stored "Size - Flavour", extract the actual flavour name
                    string flavourName = flavourText;
                    if (flavourText.Contains(" - "))
                    {
                        var parts = flavourText.Split(new[] { " - " }, StringSplitOptions.None);
                        if (parts.Length >= 2)
                            flavourName = parts[1].Trim();
                        else
                            flavourName = parts[0].Trim();
                    }

                    // Parse date (LastUpdated format written by DB); fallback to DateTime.Now if parse fails
                    DateTime parsed;
                    if (!DateTime.TryParse(dateText, out parsed))
                    {
                        // If the Date column stored only a date (yyyy-MM-dd), try that format
                        if (!DateTime.TryParseExact(dateText, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out parsed))
                            parsed = DateTime.Now;
                    }

                    // Call generator to populate the grey area
                    string report = GenerateFlavourSummary(flavourName, parsed);
                    rtbShiftReport.Text = report;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("dgvRecentFlavours_CellContentClick error: " + ex.Message);
            }
        }        // Data.cs - add this method to the Data class
        // Generates a flavour-specific summary and returns text to display in rtbShiftReport.
        // By default it summarizes one calendar day containing the provided timestamp.
        // Modify the time window if you want a different range (e.g., +/- N minutes).
        private string GenerateFlavourSummary(string flavour, DateTime timestamp)
        {
            try
            {
                // Use the whole day containing the timestamp by default
                DateTime start = timestamp.Date;
                DateTime end = start.AddDays(1);

                var dbPaths = MainForm.GetQuarterlyDbPathsInRange(start, end);
                if (dbPaths.Count == 0)
                    return $"No database files found for {start:yyyy-MM-dd}.";

                var resultTable = new DataTable();
                resultTable.Columns.Add("CameraNumber", typeof(int));
                resultTable.Columns.Add("Finish", typeof(int));
                resultTable.Columns.Add("Base", typeof(int));
                resultTable.Columns.Add("ISW", typeof(int));
                resultTable.Columns.Add("Dent", typeof(int));
                resultTable.Columns.Add("DownCan", typeof(int));
                resultTable.Columns.Add("GoodCount", typeof(int));
                resultTable.Columns.Add("BadCount", typeof(int));
                resultTable.Columns.Add("Throughput", typeof(int));
                resultTable.Columns.Add("LastUser", typeof(string));
                resultTable.Columns.Add("Recipe", typeof(string));

                var aggregate = new Dictionary<int, DataRow>();

                foreach (var dbPath in dbPaths)
                {
                    try
                    {
                        DatabaseCreator.EnsureCameraResultsTable(dbPath);
                        using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                        {
                            conn.Open();
                            // Filter by Recipe matching flavour. Use = for exact match; change to LIKE if needed.
                            var cmd = new SQLiteCommand(
                                @"SELECT CameraNumber,
                            MAX(CAST(Finish AS INTEGER)) as Finish,
                            MAX(CAST(Base AS INTEGER)) as Base,
                            MAX(CAST(ISW AS INTEGER)) as ISW,
                            MAX(CAST(Dent AS INTEGER)) as Dent,
                            MAX(CAST(DownCan AS INTEGER)) as DownCan,
                            MAX(CAST(TotalGood AS INTEGER)) - MIN(CAST(TotalGood AS INTEGER)) AS GoodCount,
                            MAX(CAST(TotalBad AS INTEGER)) - MIN(CAST(TotalBad AS INTEGER)) AS BadCount,
                            MAX(CAST(TotalThroughput AS INTEGER)) - MIN(CAST(TotalThroughput AS INTEGER)) AS Throughput,
                            MAX(UserName) as LastUser,
                            MAX(Recipe) as Recipe
                        FROM CameraResults
                        WHERE Timestamp BETWEEN @start AND @end
                          AND (Recipe = @flavour OR Recipe LIKE @flavourLike)
                        GROUP BY CameraNumber
                        ORDER BY CameraNumber", conn);

                            cmd.Parameters.AddWithValue("@start", start.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@end", end.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@flavour", flavour);
                            cmd.Parameters.AddWithValue("@flavourLike", $"%{flavour}%");

                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int camNum = Convert.ToInt32(reader["CameraNumber"]);
                                    if (!aggregate.ContainsKey(camNum))
                                    {
                                        var row = resultTable.NewRow();
                                        row["CameraNumber"] = camNum;
                                        row["Finish"] = 0;
                                        row["Base"] = 0;
                                        row["ISW"] = 0;
                                        row["Dent"] = 0;
                                        row["DownCan"] = 0;
                                        row["GoodCount"] = 0;
                                        row["BadCount"] = 0;
                                        row["Throughput"] = 0;
                                        row["LastUser"] = "";
                                        row["Recipe"] = "";
                                        aggregate[camNum] = row;
                                        resultTable.Rows.Add(row);
                                    }
                                    var aggRow = aggregate[camNum];
                                    aggRow["Finish"] = Convert.ToInt32(reader["Finish"]);
                                    aggRow["Base"] = Convert.ToInt32(reader["Base"]);
                                    aggRow["ISW"] = Convert.ToInt32(reader["ISW"]);
                                    aggRow["Dent"] = Convert.ToInt32(reader["Dent"]);
                                    aggRow["DownCan"] = Convert.ToInt32(reader["DownCan"]);
                                    aggRow["GoodCount"] = Convert.ToInt32(reader["GoodCount"]);
                                    aggRow["BadCount"] = Convert.ToInt32(reader["BadCount"]);
                                    aggRow["Throughput"] = Convert.ToInt32(reader["Throughput"]);
                                    aggRow["LastUser"] = reader["LastUser"].ToString();
                                    aggRow["Recipe"] = reader["Recipe"].ToString();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("GenerateFlavourSummary DB read error: " + ex.Message);
                    }
                }

                var sb = new StringBuilder();
                sb.AppendLine($"Flavour Report for '{flavour}' on {start:yyyy-MM-dd}");
                sb.AppendLine($"Period: {start:yyyy-MM-dd HH:mm} to {end:yyyy-MM-dd HH:mm}");
                sb.AppendLine("-----------------------------------------------------");

                if (resultTable.Rows.Count == 0)
                {
                    sb.AppendLine("No camera data for this flavour in the selected period.");
                    return sb.ToString();
                }

                foreach (DataRow row in resultTable.Rows)
                {
                    int camNum = Convert.ToInt32(row["CameraNumber"]);
                    int finish = Convert.ToInt32(row["Finish"]);
                    int baseVal = Convert.ToInt32(row["Base"]);
                    int isw = Convert.ToInt32(row["ISW"]);
                    int dent = Convert.ToInt32(row["Dent"]);
                    int downCan = Convert.ToInt32(row["DownCan"]);
                    int goodCount = Convert.ToInt32(row["GoodCount"]);
                    int badCount = Convert.ToInt32(row["BadCount"]);
                    int throughput = Convert.ToInt32(row["Throughput"]);
                    string user = row["LastUser"].ToString();
                    string recipe = row["Recipe"].ToString();

                    sb.AppendLine($"Camera {camNum}:");
                    sb.AppendLine($"  Finish:     {finish}");
                    sb.AppendLine($"  Base:       {baseVal}");
                    sb.AppendLine($"  ISW:        {isw}");
                    sb.AppendLine($"  Dent:       {dent}");
                    sb.AppendLine($"  DownCan:    {downCan}");
                    sb.AppendLine($"  Good Count: {goodCount}");
                    sb.AppendLine($"  Bad Count:  {badCount}");
                    sb.AppendLine($"  Throughput: {throughput}");
                    sb.AppendLine($"  User:       {user}");
                    sb.AppendLine($"  Recipe:     {recipe}");
                    sb.AppendLine();
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GenerateFlavourSummary error: " + ex.Message);
                return "Error generating flavour report: " + ex.Message;
            }
        }
    }
}