using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace MatroxLDS
{
    /// <summary>
    /// Unified event result for InspectionEnd events from both ECI1 and ECI2 cameras.
    /// Property names MUST match the OPC UA Event "Name" column exactly.
    /// </summary>
    public class InspectionEndEventResult : IEventResult, INotifyPropertyChanged, ICloneable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // ========== ECI2 IMAGE PROPERTIES (from screenshot) ==========
        /// <summary>All images combined - matches "All" in Events tab</summary>
        public DAComplexVariable<byte[]> All { get; set; }

        /// <summary>Find can image - matches "FindCan" in Events tab</summary>
        public DAComplexVariable<byte[]> FindCan { get; set; }

        /// <summary>Dent image - matches "DentImage" in Events tab</summary>
        public DAComplexVariable<byte[]> DentImage { get; set; }

        /// <summary>ISW image - matches "ISWImage" in Events tab</summary>
        public DAComplexVariable<byte[]> ISWImage { get; set; }

        /// <summary>Base image - matches "BaseImage" in Events tab</summary>
        public DAComplexVariable<byte[]> BaseImage { get; set; }

        /// <summary>Find can failed image - matches "FindCanFailedImage" in Events tab</summary>
        public DAComplexVariable<byte[]> FindCanFailedImage { get; set; }

        /// <summary>Dent failed image - matches "DentFailedImage" in Events tab</summary>
        public DAComplexVariable<byte[]> DentFailedImage { get; set; }

        /// <summary>ISW failed image - matches "ISWFailedImage" in Events tab</summary>
        public DAComplexVariable<byte[]> ISWFailedImage { get; set; }

        /// <summary>Base failed image - matches "BaseFailedImage" in Events tab</summary>
        public DAComplexVariable<byte[]> BaseFailedImage { get; set; }

        // ========== ECI1 IMAGE PROPERTIES (you'll need to show me ECI1 Events tab) ==========
        /// <summary>Finish defects display</summary>
        public DAComplexVariable<byte[]> Finish { get; set; }

        /// <summary>Broken finish defects display</summary>
        public DAComplexVariable<byte[]> BrokenFinish { get; set; }

        /// <summary>Last rejected finish display</summary>
        public DAComplexVariable<byte[]> FinishLastReject { get; set; }

        /// <summary>Last rejected broken finish display</summary>
        public DAComplexVariable<byte[]> BrokenFinishLastReject { get; set; }

        // ========== COMMON INSPECTION RESULTS ==========
        public DAComplexVariable<string> Status_Result { get; set; }
        public DAComplexVariable<double> Status_ExecutionTime { get; set; }
        public DAComplexVariable<DateTime> Camera_InspectionDate { get; set; }
        public DAComplexVariable<int> Camera_ImageWidth { get; set; }
        public DAComplexVariable<int> Camera_ImageHeight { get; set; }
        public DAComplexVariable<int> TotalGoodCount { get; set; }
        public DAComplexVariable<int> TotalBadCount { get; set; }
        public DAComplexVariable<int> TotalThroughput { get; set; }

        public InspectionEndEventResult()
        {
            // Initialize ECI2 properties
            All = new DAComplexVariable<byte[]>();
            FindCan = new DAComplexVariable<byte[]>();
            DentImage = new DAComplexVariable<byte[]>();
            ISWImage = new DAComplexVariable<byte[]>();
            BaseImage = new DAComplexVariable<byte[]>();
            FindCanFailedImage = new DAComplexVariable<byte[]>();
            DentFailedImage = new DAComplexVariable<byte[]>();
            ISWFailedImage = new DAComplexVariable<byte[]>();
            BaseFailedImage = new DAComplexVariable<byte[]>();

            // Initialize ECI1 properties
            Finish = new DAComplexVariable<byte[]>();
            BrokenFinish = new DAComplexVariable<byte[]>();
            FinishLastReject = new DAComplexVariable<byte[]>();
            BrokenFinishLastReject = new DAComplexVariable<byte[]>();

            // Initialize common properties
            Status_Result = new DAComplexVariable<string>();
            Status_ExecutionTime = new DAComplexVariable<double>();
            Camera_InspectionDate = new DAComplexVariable<DateTime>();
            Camera_ImageWidth = new DAComplexVariable<int>();
            Camera_ImageHeight = new DAComplexVariable<int>();
            TotalGoodCount = new DAComplexVariable<int>();
            TotalBadCount = new DAComplexVariable<int>();
            TotalThroughput = new DAComplexVariable<int>();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void UpdateModel(string propertyName, bool isAvailable, object currentValue, List<string> availableValues)
        {
            try
            {
                var property = this.GetType().GetProperty(propertyName);

                if (property == null)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ [EventResult] Property '{propertyName}' not found (ignoring)");
                    return;
                }

                var complexVar = property.GetValue(this);

                if (complexVar == null)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ [EventResult] ComplexVariable for '{propertyName}' is null");
                    return;
                }

                var complexVarType = complexVar.GetType();

                var valueProperty = complexVarType.GetProperty("CurrentValue");
                if (valueProperty != null && currentValue != null)
                {
                    try
                    {
                        valueProperty.SetValue(complexVar, currentValue);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"⚠️ [EventResult] Error setting CurrentValue for '{propertyName}': {ex.Message}");
                    }
                }

                var availableProperty = complexVarType.GetProperty("IsAvailable");
                if (availableProperty != null)
                {
                    availableProperty.SetValue(complexVar, isAvailable);
                }

                var availableValuesProperty = complexVarType.GetProperty("AvailableValues");
                if (availableValuesProperty != null && availableValues != null)
                {
                    availableValuesProperty.SetValue(complexVar, availableValues);
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ [EventResult] Error updating '{propertyName}': {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"   Stack trace: {ex.StackTrace}");
            }
        }
    }
}