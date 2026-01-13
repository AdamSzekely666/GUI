using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MatroxLDS
{
    public class CameraEndEventResult : IEventResult, ICloneable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DAComplexVariable<int> Camera_ImageHeight { get; set; }

        public DAComplexVariable<int> Camera_ImageWidth { get; set; }

        /// <summary>
        /// Redirects the result value from the event notification to the correct property.
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="isAvailable"></param>
        /// <param name="currentValue"></param>
        /// <param name="availableValues"></param>
        public void UpdateModel(string variableName, bool isAvailable, object currentValue, List<string> availableValues)
        {
            switch (variableName)
            {
                case "Camera_ImageHeight":
                    Camera_ImageHeight = new DAComplexVariable<int>(isAvailable, currentValue, availableValues);
                    break;

                case "Camera_ImageWidth":
                    Camera_ImageWidth = new DAComplexVariable<int>(isAvailable, currentValue, availableValues);
                    break;
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Called by properties to update the UI when they change.
        /// </summary>
        /// <param name="propertyName"></param>
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
