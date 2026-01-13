using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace MatroxLDS
{
    /// <summary>
    /// Unified event result for InspectionEnd events from both ECI1 and ECI2 cameras. 
    /// Contains image data and inspection results for all display types.
    /// </summary>
    public class InspectionEndEventResult : IEventResult, INotifyPropertyChanged, ICloneable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // ========== SHARED PROPERTIES (Both ECI1 and ECI2) ==========
        /// <summary>All defects combined view (both cameras)</summary>
        public DAComplexVariable<byte[]> All { get; set; }

        // ========== ECI1 SPECIFIC PROPERTIES ==========
        /// <summary>Finish defects display</summary>
        public DAComplexVariable<byte[]> Finish { get; set; }

        /// <summary>Broken finish defects display</summary>
        public DAComplexVariable<byte[]> BrokenFinish { get; set; }

        /// <summary>Last rejected finish display</summary>
        public DAComplexVariable<byte[]> FinishLastReject { get; set; }

        /// <summary>Last rejected broken finish display</summary>
        public DAComplexVariable<byte[]> BrokenFinishLastReject { get; set; }

        // ========== ECI2 SPECIFIC PROPERTIES ==========
        /// <summary>Find can display</summary>
        public DAComplexVariable<byte[]> FindCan { get; set; }

        /// <summary>Dent defects display</summary>
        public DAComplexVariable<byte[]> DentImage { get; set; }

        /// <summary>ISW (Internal Sidewall) defects display</summary>
        public DAComplexVariable<byte[]> ISWImage { get; set; }

        /// <summary>Base defects display</summary>
        public DAComplexVariable<byte[]> BaseImage { get; set; }

        /// <summary>Failed find can display</summary>
        public DAComplexVariable<byte[]> FindCanFailedImage { get; set; }

        /// <summary>Failed dent capture display</summary>
        public DAComplexVariable<byte[]> DentFailedImage { get; set; }

        /// <summary>Failed ISW capture display</summary>
        public DAComplexVariable<byte[]> ISWFailedImage { get; set; }

        /// <summary>Failed base display</summary>
        public DAComplexVariable<byte[]> BaseFailedImage { get; set; }

        // ========== COMMON INSPECTION RESULTS ==========
        /// <summary>Inspection status (Pass/Fail)</summary>
        public DAComplexVariable<string> Status_Result { get; set; }

        /// <summary>Inspection execution time in milliseconds</summary>
        public DAComplexVariable<double> Status_ExecutionTime { get; set; }

        /// <summary>Inspection date/time timestamp</summary>
        public DAComplexVariable<DateTime> Camera_InspectionDate { get; set; }

        /// <summary>Camera image width</summary>
        public DAComplexVariable<int> Camera_ImageWidth { get; set; }

        /// <summary>Camera image height</summary>
        public DAComplexVariable<int> Camera_ImageHeight { get; set; }

        // ========== ADDITIONAL RESULT DATA (if needed) ==========
        /// <summary>Total good count</summary>
        public DAComplexVariable<int> TotalGoodCount { get; set; }

        /// <summary>Total bad count</summary>
        public DAComplexVariable<int> TotalBadCount { get; set; }

        /// <summary>Total throughput</summary>
        public DAComplexVariable<int> TotalThroughput { get; set; }

        /// <summary>
        /// Constructor - initializes all properties to prevent null references
        /// </summary>
        public InspectionEndEventResult()
        {
            // Initialize shared properties
            All = new DAComplexVariable<byte[]>();

            // Initialize ECI1 properties
            Finish = new DAComplexVariable<byte[]>();
            BrokenFinish = new DAComplexVariable<byte[]>();
            FinishLastReject = new DAComplexVariable<byte[]>();
            BrokenFinishLastReject = new DAComplexVariable<byte[]>();

            // Initialize ECI2 properties
            FindCan = new DAComplexVariable<byte[]>();
            DentImage = new DAComplexVariable<byte[]>();
            ISWImage = new DAComplexVariable<byte[]>();
            BaseImage = new DAComplexVariable<byte[]>();
            FindCanFailedImage = new DAComplexVariable<byte[]>();
            DentFailedImage = new DAComplexVariable<byte[]>();
            ISWFailedImage = new DAComplexVariable<byte[]>();
            BaseFailedImage = new DAComplexVariable<byte[]>();

            // Initialize common result properties
            Status_Result = new DAComplexVariable<string>();
            Status_ExecutionTime = new DAComplexVariable<double>();
            Camera_InspectionDate = new DAComplexVariable<DateTime>();
            Camera_ImageWidth = new DAComplexVariable<int>();
            Camera_ImageHeight = new DAComplexVariable<int>();

            // Initialize count properties
            TotalGoodCount = new DAComplexVariable<int>();
            TotalBadCount = new DAComplexVariable<int>();
            TotalThroughput = new DAComplexVariable<int>();
        }

        /// <summary>
        /// Creates a shallow copy of this event result
        /// Required by ICloneable interface
        /// </summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Updates a property value when new data arrives from OPC UA server
        /// Called automatically by DAOPCEvent
        /// Required by IEventResult interface
        /// </summary>
        /// <param name="propertyName">Name of the property to update</param>
        /// <param name="isAvailable">Whether the property value is available</param>
        /// <param name="currentValue">The new value</param>
        /// <param name="availableValues">List of available values (for enums)</param>
        public void UpdateModel(string propertyName, bool isAvailable, object currentValue, List<string> availableValues)
        {
            try
            {
                // Use reflection to find the property by name
                var property = this.GetType().GetProperty(propertyName);

                if (property == null)
                {
                    // Property not found - might be a property we don't care about
                    System.Diagnostics.Debug.WriteLine($"⚠️ [EventResult] Property '{propertyName}' not found (ignoring)");
                    return;
                }

                // Get the DAComplexVariable instance for this property
                var complexVar = property.GetValue(this);

                if (complexVar == null)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ [EventResult] ComplexVariable for '{propertyName}' is null");
                    return;
                }

                // Get the type of the complex variable
                var complexVarType = complexVar.GetType();

                // Update CurrentValue property
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

                // Update IsAvailable property
                var availableProperty = complexVarType.GetProperty("IsAvailable");
                if (availableProperty != null)
                {
                    availableProperty.SetValue(complexVar, isAvailable);
                }

                // Update AvailableValues property (for enum-type values)
                var availableValuesProperty = complexVarType.GetProperty("AvailableValues");
                if (availableValuesProperty != null && availableValues != null)
                {
                    availableValuesProperty.SetValue(complexVar, availableValues);
                }

                // Notify any listeners that this property changed
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

                // Log successful update (comment out in production for performance)
                // System.Diagnostics.Debug. WriteLine($"✅ [EventResult] Updated '{propertyName}' (Available: {isAvailable})");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ [EventResult] Error updating '{propertyName}': {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"   Stack trace: {ex.StackTrace}");
            }
        }
    }
}