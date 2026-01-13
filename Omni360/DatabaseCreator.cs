using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace OmniCheck_360
{
    public static class DatabaseCreator
    {
        /// <summary>
        /// Creates a quarterly SQLite database in the DatabaseDBs folder, and creates tables per the InspectionPriorityConfig.
        /// </summary>
        /// <param name="config">InspectionPriorityConfig object</param>
        /// <param name="forDate">The date to determine year and quarter</param>
        public static void CreateDatabaseFromConfig(InspectionPriorityConfig config, DateTime forDate)
        {
            // 1. Determine year and quarter
            int year = forDate.Year;
            int quarter = ((forDate.Month - 1) / 3) + 1;
            string dbFileName = $"InspectionResults_{year}_Q{quarter}.db";

            // 2. Build DatabaseDBs directory path
            string dbFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DatabaseDBs");
            if (!Directory.Exists(dbFolder))
                Directory.CreateDirectory(dbFolder);

            // 3. Full DB path
            string dbPath = Path.Combine(dbFolder, dbFileName);

            bool newDb = !File.Exists(dbPath);

            // 4. Open or create the database
            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();

                // 5. For each camera, build a CREATE TABLE command
                foreach (var camera in config.cameras)
                {
                    string tableName = camera.name.Replace(" ", "_");

                    var sb = new StringBuilder();
                    sb.AppendLine($"CREATE TABLE IF NOT EXISTS [{tableName}] (");
                    sb.AppendLine("  Id INTEGER PRIMARY KEY AUTOINCREMENT,");
                    sb.AppendLine("  Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,");

                    for (int i = 0; i < camera.inspections.Count; i++)
                    {
                        var inspection = camera.inspections[i];
                        string colName = inspection.name.Replace(" ", "_");
                        sb.Append($"  [{colName}] INTEGER");
                        if (i < camera.inspections.Count - 1)
                            sb.AppendLine(",");
                        else
                            sb.AppendLine();
                    }
                    sb.AppendLine(");");

                    string createSql = sb.ToString();
                    using (var cmd = new SQLiteCommand(createSql, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // 6. (Optional) Create a summary/global table if needed here

                connection.Close();
            }
        }

        public static void EnsureCameraResultsTable(string dbPath)
        {
            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();
                string sql = @"CREATE TABLE IF NOT EXISTS CameraResults (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            CameraNumber INTEGER,
            Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
            MissingCap TEXT,
            HighCap TEXT,
            TamperBand TEXT,
            FillLevel TEXT,
            FillLevelIntensityNumber TEXT,
            FillLevelEdgeNumber TEXT,
            FillLevelBlobNumber TEXT,
            TotalGood TEXT,
            TotalBad TEXT,
            TotalThroughput TEXT,
            UserName TEXT,
            Recipe TEXT
        );";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}