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
                // ========== DEBUG:  Log incoming data ==========
                System.Diagnostics.Debug.WriteLine("═══════════════════════════════════════════════════════");
                System.Diagnostics.Debug.WriteLine($"🔔 [UpdateModel] Received property update:");
                System.Diagnostics.Debug.WriteLine($"   Property Name: '{propertyName}'");
                System.Diagnostics.Debug.WriteLine($"   IsAvailable: {isAvailable}");
                System.Diagnostics.Debug.WriteLine($"   CurrentValue Type: {currentValue?.GetType().Name ?? "null"}");
                System.Diagnostics.Debug.WriteLine($"   CurrentValue: {(currentValue != null ? (currentValue is byte[] bytes ? $"byte[{bytes.Length}]" : currentValue.ToString()) : "null")}");
                System.Diagnostics.Debug.WriteLine($"   AvailableValues Count: {availableValues?.Count ?? 0}");

                var property = this.GetType().GetProperty(propertyName);

                if (property == null)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ [UpdateModel] Property '{propertyName}' NOT FOUND in InspectionEndEventResult!");
                    System.Diagnostics.Debug.WriteLine($"❌ Available properties in this class:");

                    foreach (var prop in this.GetType().GetProperties())
                    {
                        var propType = prop.PropertyType;
                        System.Diagnostics.Debug.WriteLine($"      - {prop.Name} (Type: {propType.Name})");
                    }

                    System.Diagnostics.Debug.WriteLine("═══════════════════════════════════════════════════════");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"✅ [UpdateModel] Property '{propertyName}' found in class");

                var complexVar = property.GetValue(this);

                if (complexVar == null)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ [UpdateModel] ComplexVariable for '{propertyName}' is null - this shouldn't happen!");
                    System.Diagnostics.Debug.WriteLine("═══════════════════════════════════════════════════════");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"✅ [UpdateModel] ComplexVariable instance exists");

                var complexVarType = complexVar.GetType();
                System.Diagnostics.Debug.WriteLine($"   ComplexVariable Type: {complexVarType.Name}");

                var valueProperty = complexVarType.GetProperty("CurrentValue");
                if (valueProperty != null && currentValue != null)
                {
                    try
                    {
                        valueProperty.SetValue(complexVar, currentValue);
                        System.Diagnostics.Debug.WriteLine($"✅ [UpdateModel] CurrentValue set successfully for '{propertyName}'");

                        // If it's a byte array (image), log the size
                        if (currentValue is byte[] imageBytes)
                        {
                            System.Diagnostics.Debug.WriteLine($"   📸 Image data:  {imageBytes.Length} bytes");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ [UpdateModel] Error setting CurrentValue for '{propertyName}': {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"   Expected Type: {valueProperty.PropertyType.Name}");
                        System.Diagnostics.Debug.WriteLine($"   Actual Type: {currentValue?.GetType().Name}");
                    }
                }
                else
                {
                    if (valueProperty == null)
                        System.Diagnostics.Debug.WriteLine($"⚠️ [UpdateModel] CurrentValue property not found in ComplexVariable");
                    if (currentValue == null)
                        System.Diagnostics.Debug.WriteLine($"⚠️ [UpdateModel] currentValue is null for '{propertyName}'");
                }

                var availableProperty = complexVarType.GetProperty("IsAvailable");
                if (availableProperty != null)
                {
                    availableProperty.SetValue(complexVar, isAvailable);
                    System.Diagnostics.Debug.WriteLine($"✅ [UpdateModel] IsAvailable set to {isAvailable}");
                }

                var availableValuesProperty = complexVarType.GetProperty("AvailableValues");
                if (availableValuesProperty != null && availableValues != null)
                {
                    availableValuesProperty.SetValue(complexVar, availableValues);
                    System.Diagnostics.Debug.WriteLine($"✅ [UpdateModel] AvailableValues set ({availableValues.Count} items)");
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                System.Diagnostics.Debug.WriteLine($"✅ [UpdateModel] PropertyChanged event fired for '{propertyName}'");
                System.Diagnostics.Debug.WriteLine("═══════════════════════════════════════════════════════");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("═══════════════════════════════════════════════════════");
                System.Diagnostics.Debug.WriteLine($"❌ [UpdateModel] EXCEPTION updating '{propertyName}':");
                System.Diagnostics.Debug.WriteLine($"   Message: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"   Exception Type: {ex.GetType().FullName}");
                System.Diagnostics.Debug.WriteLine($"   Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"   Inner Exception: {ex.InnerException.Message}");
                }
                System.Diagnostics.Debug.WriteLine("═══════════════════════════════════════════════════════");
            }
        }
    }
}