using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace MatroxLDS
{
    /// <summary>
    /// Holds properties Image Results properties. Like in the model, the name are automatically matched, so they must be the same.
    /// </summary>
    public class InspectionEndEventResult: IEventResult, INotifyPropertyChanged, ICloneable
    {
        private DAComplexVariable<double> _executionTime;

        public event PropertyChangedEventHandler PropertyChanged;

        public DAComplexVariable<double> Status_ExecutionTime
        {
            get { return _executionTime; }

            set
            {
                _executionTime = value;
                _executionTime.CurrentValue = value.CurrentValue * 1000;
            }
        }

        public DAComplexVariable<byte[]> ImageWriter_Image { get; set; }

        public DAComplexVariable<double> IntensityChecker_Average { get; set; }

        public DAComplexVariable<double> IntensityChecker_Contrast { get; set; }

        public DAComplexVariable<string> Camera_InspectionDate { get; set; }

        public DAComplexVariable<double> IntensityChecker_MaxPixelValue { get; set; }

        public DAComplexVariable<double> IntensityChecker_MinPixelValue { get; set; }

        public DAComplexVariable<double> IntensityChecker_NumberOfPixels { get; set; }

        public DAComplexVariable<double> IntensityChecker_StandardDeviation { get; set; }

        public DAComplexVariable<string> IntensityChecker_AverageConditionStatus { get; set; }

        public DAComplexVariable<string> IntensityChecker_CountConditionStatus { get; set; }

        public DAComplexVariable<string> Status_Result { get; set; }

        public MemoryStream RenderedImage { get; set; }
        public InspectionEndEventResult()
        {
            ImageWriter_Image = new DAComplexVariable<byte[]>();
            IntensityChecker_Average = new DAComplexVariable<double>();
            IntensityChecker_Contrast = new DAComplexVariable<double>();
            IntensityChecker_MinPixelValue = new DAComplexVariable<double>();
            IntensityChecker_MaxPixelValue = new DAComplexVariable<double>();
            IntensityChecker_StandardDeviation = new DAComplexVariable<double>();
            IntensityChecker_NumberOfPixels = new DAComplexVariable<double>();
            Status_Result = new DAComplexVariable<string>();
            Status_ExecutionTime = new DAComplexVariable<double>();
            Camera_InspectionDate = new DAComplexVariable<string>();
        }

        /// <summary>
        /// Redirects the result value from the event notification to the correct property.
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="isAvailable"></param>
        /// <param name="currentValue"></param>
        public void UpdateModel(string variableName, bool isAvailable, object currentValue, List<string> availableValues)
        {
            switch (variableName)
            {
                case "ImageWriter_Image":
                    ImageWriter_Image = new DAComplexVariable<byte[]>(isAvailable, currentValue, availableValues);
                    NotifyPropertyChanged(nameof(ImageWriter_Image));
                    break;
                case "IntensityChecker_Average":
                    IntensityChecker_Average = new DAComplexVariable<double>(isAvailable, currentValue, availableValues);
                    NotifyPropertyChanged(nameof(IntensityChecker_Average));
                    break;
                case "IntensityChecker_Contrast":
                    IntensityChecker_Contrast = new DAComplexVariable<double>(isAvailable, currentValue, availableValues);
                    NotifyPropertyChanged(nameof(IntensityChecker_Contrast));
                    break;
                case "IntensityChecker_MinPixelValue":
                    IntensityChecker_MinPixelValue = new DAComplexVariable<double>(isAvailable, currentValue, availableValues);
                    NotifyPropertyChanged(nameof(IntensityChecker_MinPixelValue));
                    break;
                case "IntensityChecker_MaxPixelValue":
                    IntensityChecker_MaxPixelValue = new DAComplexVariable<double>(isAvailable, currentValue, availableValues);
                    NotifyPropertyChanged(nameof(IntensityChecker_MaxPixelValue));
                    break;
                case "IntensityChecker_StandardDeviation":
                    IntensityChecker_StandardDeviation = new DAComplexVariable<double>(isAvailable, currentValue, availableValues);
                    NotifyPropertyChanged(nameof(IntensityChecker_StandardDeviation));
                    break;
                case "IntensityChecker_NumberOfPixels":
                    IntensityChecker_NumberOfPixels = new DAComplexVariable<double>(isAvailable, currentValue, availableValues);
                    NotifyPropertyChanged(nameof(IntensityChecker_NumberOfPixels));
                    break;
                case "IntensityChecker_AverageConditionStatus":
                    IntensityChecker_AverageConditionStatus = new DAComplexVariable<string>(isAvailable, currentValue, availableValues);
                    NotifyPropertyChanged(nameof(IntensityChecker_NumberOfPixels));
                    break;
                case "IntensityChecker_CountConditionStatus":
                    IntensityChecker_CountConditionStatus = new DAComplexVariable<string>(isAvailable, currentValue, availableValues);
                    NotifyPropertyChanged(nameof(IntensityChecker_CountConditionStatus));
                    break;
                case "Status_Result":
                    Status_Result = new DAComplexVariable<string>(isAvailable, currentValue, availableValues);
                    NotifyPropertyChanged(nameof(Status_Result));
                    break;
                case "Status_ExecutionTime":
                    Status_ExecutionTime = new DAComplexVariable<double>(isAvailable, currentValue, availableValues);
                    NotifyPropertyChanged(nameof(Status_ExecutionTime));
                    break;
                case "Camera_InspectionDate":
                    Camera_InspectionDate = new DAComplexVariable<string>(isAvailable, currentValue, availableValues);
                    NotifyPropertyChanged(nameof(Camera_InspectionDate));
                    break;
            }
        }

        /// <summary>
        /// Called by properties to update the UI when they change.
        /// </summary>
        /// <param name="propertyName"></param>
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
