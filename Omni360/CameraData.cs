using System;
using System.Diagnostics;

namespace OmniCheck_360
{
    public class CameraData
    {
        public int CameraNumber { get; set; }
        public string Finish { get; set; }
        public string Base { get; set; }
        public string ISW { get; set; }
        public string Dent { get; set; }
        public string TotalThroughput { get; set; }
        public string TotalGood { get; set; }
        public string TotalBad { get; set; }
        public string Timestamp { get; set; }
        public string DownCan { get; set; }

        /// <summary>
        /// Checks completeness for known camera field differences (by index).
        /// Camera 0 (ECI1): Requires Finish, TotalGood, TotalBad, TotalThroughput
        /// Camera 1 (ECI2): Requires Base, ISW, TotalGood, TotalBad, TotalThroughput
        /// Extend for further cameras if needed.
        /// </summary>
        public bool IsCompleteForIndex(int cameraIndex)
        {
            switch (cameraIndex)
            {
                case 0: // Camera 1 / ECI1
                    return !string.IsNullOrEmpty(Finish)
                        && !string.IsNullOrEmpty(TotalGood)
                        && !string.IsNullOrEmpty(TotalBad)
                        && !string.IsNullOrEmpty(TotalThroughput);
                case 1: // Camera 2 / ECI2
                    return !string.IsNullOrEmpty(Base)
                        && !string.IsNullOrEmpty(ISW)
                        && !string.IsNullOrEmpty(TotalGood)
                        && !string.IsNullOrEmpty(TotalBad)
                        && !string.IsNullOrEmpty(TotalThroughput)
                        && !string.IsNullOrEmpty(Dent);
                // Add case 2 for Camera 3 if its required fields differ
                default:
                    // For safety, fallback to old behavior
                    return !string.IsNullOrEmpty(TotalThroughput);
            }
        }

        // For legacy compatibility, pick CameraNumber-1 (if it's set, otherwise default to 0)
        public bool IsComplete
        {
            get
            {
                int idx = CameraNumber > 0 ? CameraNumber - 1 : 0;
                return IsCompleteForIndex(idx);
            }
        }
    }
}