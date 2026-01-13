using System.Collections.Generic;
using System.ComponentModel;

namespace MatroxLDS
{
    /// <summary>
    /// This represents the value of bindings and event result variable we receive from the DAOPC server.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DAComplexVariable<T> : INotifyPropertyChanged
    {
        private T _currentValue;
        private List<string> _availableValues;
        private bool _isAvaialable;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// A list of available values, representing a finite set of possible choice for the variable, if it exists.
        /// </summary>
        public List<string> AvailableValues
        {
            get
            {
                return _availableValues;
            }
            set
            {
                _availableValues = value;
                NotifyPropertyChanged(nameof(AvailableValues));
            }
        }

        /// <summary>
        /// Availability of the current value. Some configuration of the project might disable some variable, in which case this will be false.
        /// </summary>
        public bool IsAvailable
        {
            get
            {
                return _isAvaialable;
            }
            set
            {
                _isAvaialable = value;
                NotifyPropertyChanged(nameof(IsAvailable));
            }
        }

        /// <summary>
        /// The current value of the variable. If available values is defined, this will be a member of that list. 
        /// </summary>
        public T CurrentValue {
            get
            {
                return _currentValue;
            }
            set
            {
                if(! EqualityComparer<T>.Default.Equals(_currentValue, value))
                {
                    _currentValue = value;
                    NotifyPropertyChanged(nameof(CurrentValue));
                }
            }
        }

        public DAComplexVariable(): this(false, null, null){}

        public DAComplexVariable(bool isAvailable, object currentValue, List<string> availableValues)
        {
            AvailableValues = availableValues;
            if (currentValue != null)
            {
                CurrentValue = (T)currentValue;
            }
            IsAvailable = isAvailable;
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
