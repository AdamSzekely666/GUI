using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Input;

namespace MatroxLDS
{
    /// <summary>
    /// Holds the data the is exchanged between the OPC-UA server and the UI.
    /// </summary>
    public class OPCDataModel : INotifyPropertyChanged
    {
        private InspectionEndEventResult _displayedInspectionEndResult;

        private int _historySize;
        private bool _isDisplayMode = false;
        private bool _isServerConnected;
        private double _nbFail;
        private double _nbPass;
        private int _reconnectionAttempts;
        private bool _showNavbar;

        #region Binding properties

        public DAOPCBinding<int> ImageWriter_ResizeHeight { get; set; }

        public DAOPCBinding<bool> IntensityChecker_AverageConditionEnabled { get; set; }

        public DAOPCBinding<double> IntensityChecker_AverageConditionHighValue { get; set; }

        public DAOPCBinding<double> IntensityChecker_AverageConditionLowValue { get; set; }

        public DAOPCBinding<string> IntensityChecker_AverageConditionOperator { get; set; }

        public DAOPCBinding<bool> IntensityChecker_CountConditionEnabled { get; set; }

        public DAOPCBinding<double> IntensityChecker_CountConditionHighValue { get; set; }

        public DAOPCBinding<double> IntensityChecker_CountConditionLowValue { get; set; }

        public DAOPCBinding<string> IntensityChecker_CountConditionOperator { get; set; }

        public DAOPCBinding<string> IntensityChecker_Filter { get; set; }

        public DAOPCBinding<double> IntensityChecker_PixelIncludeHighValue { get; set; }

        public DAOPCBinding<double> IntensityChecker_PixelIncludeLowValue { get; set; }

        public DAOPCBinding<string> IntensityChecker_PixelSelection { get; set; }

        public DAOPCBinding<double> IntensityChecker_RegionX { get; set; }

        public DAOPCBinding<double> IntensityChecker_RegionY { get; set; }

        public DAOPCBinding<double> IntensityChecker_RegionHeight { get; set; }

        public DAOPCBinding<double> IntensityChecker_RegionWidth { get; set; }
        #endregion

        #region Event properties
        public DAOPCEvent<CameraEndEventResult> CameraEndResult { get; set; }

        /// <summary>
        /// Holds all the information from the Event "InspectionEnd" in the Design Assistant project. Property for this Event are also
        /// automatically mapped by the OPC-UA Connection Manager, to the properties of DAImageResults.
        /// </summary>
        public DAOPCEvent<InspectionEndEventResult> InspectionEndResult { get; set; }

        #endregion

        #region Flowchart properties

        public DAOPCFlowchart OnSendTrigger { get; set; }

        public DAOPCFlowchart OnRerunTrigger { get; set; }

        #endregion

        #region UI related properties

        public ICommand EnterLostFocusCommand { get => new UpdateTextboxSourceCommand(); }

        /// <summary>
        /// The visibility state for the navigation bar.
        /// </summary>
        public bool ShowNavBar
        {
            get => _showNavbar;

            set
            {
                _showNavbar = value;
                NotifyPropertyChanged(nameof(ShowNavBar));
            }
        }
        /// <summary>
        /// Image results that are actually displayed in the UI. Can be the most recent result or the one selected if updates are paused.
        /// </summary>
        public InspectionEndEventResult DisplayedInspectionEndResult
        {
            get { return _displayedInspectionEndResult; }

            set
            {
                _displayedInspectionEndResult = value;
                NotifyPropertyChanged(nameof(DisplayedInspectionEndResult));
            }
        }

        /// <summary>
        /// Set to true to stop the results section from updating it's results section and show the selected image result instead of the most recent one.
        /// </summary>
        public bool IsDisplayMode
        {
            get { return _isDisplayMode; }

            set
            {
                _isDisplayMode = value;
                NotifyPropertyChanged(nameof(IsDisplayMode));
            }
        }

        /// <summary>
        ///    Counter for the occurences of Pass inspections received.
        /// </summary>
        public double NbPass
        {
            get { return _nbPass; }

            set
            {
                _nbPass = value;
                if (!IsDisplayMode)
                {
                    NotifyPropertyChanged(nameof(NbPass));
                }
            }
        }

        /// <summary>
        ///    Counter for the occurences of Fail inspections received.
        /// </summary>
        public double NbFail
        {
            get { return _nbFail; }

            set
            {
                _nbFail = value;
                if (!IsDisplayMode)
                {
                    NotifyPropertyChanged(nameof(NbFail));
                }
            }
        }

        /// <summary>
        ///    Stores the server connection status.
        /// </summary>
        public bool IsServerConnected
        {
            get { return _isServerConnected; }

            set
            {
                _isServerConnected = value;
                NotifyPropertyChanged(nameof(IsServerConnected));
            }
        }

        /// <summary>
        ///     Counter for the times we attempted to reconnect to server.
        /// </summary>
        public int ReconnectionAttempts
        {
            get { return _reconnectionAttempts; }

            set
            {
                _reconnectionAttempts = value;
                NotifyPropertyChanged(nameof(ReconnectionAttempts));
            }
        }

        /// <summary>
        /// Used to display the results in the result page.
        /// </summary>
        public List<InspectionEndEventResult> InspectionEndResultHistory
        {
            get 
            { 
                if(InspectionEndResult == null)
                {
                    return new List<InspectionEndEventResult>();
                }
                return (InspectionEndResult?.ResultsHistory.GetList() as IEnumerable<InspectionEndEventResult>).Reverse().ToList(); 
            }
        }
        #endregion

        /// <summary>
        /// Invoked to notify the UI of a change of value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public OPCDataModel(int historySize = 20)
        {
            _historySize = historySize;
            ShowNavBar = false;
            DAOPCBinding.ResetOnWriteSuccessfulEvent();
            DAOPCBinding.OnWriteSuccessful += DAOPCBinding_OnWriteSuccessful;
        }

        /// <summary>
        /// When the session is connected, we instanciated the DAOPC objects of the model.
        /// </summary>
        /// <param name="session"></param>
        public void InitializeOPCProperties(DAOPCSession session)
        {
            CreateBindings(session);
            CreateEvents(session);
            CreateFlowcharts(session);
        }

        public void ReinitializeOnConnect(DAOPCSession session)
        {
            IsServerConnected = true;
            ReconnectionAttempts = 0;
            NbPass = 0;
            NbFail = 0;
        }

        private void CreateBindings(DAOPCSession session)
        {
            ImageWriter_ResizeHeight = new DAOPCBinding<int>(session, "ImageWriter_ResizeHeight");

            IntensityChecker_AverageConditionEnabled = new DAOPCBinding<bool>(session, "IntensityChecker_AverageConditionEnabled");
            IntensityChecker_AverageConditionOperator = new DAOPCBinding<string>(session, "IntensityChecker_AverageConditionOperator");
            IntensityChecker_AverageConditionLowValue = new DAOPCBinding<double>(session, "IntensityChecker_AverageConditionLowValue");
            IntensityChecker_AverageConditionHighValue = new DAOPCBinding<double>(session, "IntensityChecker_AverageConditionHighValue");

            IntensityChecker_CountConditionEnabled = new DAOPCBinding<bool>(session, "IntensityChecker_CountConditionEnabled");
            IntensityChecker_CountConditionOperator = new DAOPCBinding<string>(session, "IntensityChecker_CountConditionOperator");
            IntensityChecker_CountConditionLowValue = new DAOPCBinding<double>(session, "IntensityChecker_CountConditionLowValue");
            IntensityChecker_CountConditionHighValue = new DAOPCBinding<double>(session, "IntensityChecker_CountConditionHighValue");

            IntensityChecker_Filter = new DAOPCBinding<string>(session, "IntensityChecker_Filter");

            IntensityChecker_PixelSelection = new DAOPCBinding<string>(session, "IntensityChecker_PixelSelection");
            IntensityChecker_PixelIncludeHighValue = new DAOPCBinding<double>(session, "IntensityChecker_PixelIncludeHighValue");
            IntensityChecker_PixelIncludeLowValue = new DAOPCBinding<double>(session, "IntensityChecker_PixelIncludeLowValue");

            IntensityChecker_RegionX = new DAOPCBinding<double>(session, "IntensityChecker_RegionX");
            IntensityChecker_RegionY = new DAOPCBinding<double>(session, "IntensityChecker_RegionY");
            IntensityChecker_RegionWidth = new DAOPCBinding<double>(session, "IntensityChecker_RegionWidth");
            IntensityChecker_RegionHeight = new DAOPCBinding<double>(session, "IntensityChecker_RegionHeight");

            NotifyPropertyChanged(null);

        }

        private void CreateEvents(DAOPCSession session)
        {
            InspectionEndResult = new DAOPCEvent<InspectionEndEventResult>(session, "InspectionEnd", _historySize);
            InspectionEndResult.OnCurrentResultChange += InspectionEnd_EventVariablesModel_PropertyChanged;
            UpdateInspectionEndResult(InspectionEndResult.CurrentResult);

            CameraEndResult = new DAOPCEvent<CameraEndEventResult>(session, "CameraEnd", _historySize);
        }

        private void CreateFlowcharts(DAOPCSession session)
        {
            OnSendTrigger = new DAOPCFlowchart(session, DANodeMappings.NEXT_IMAGE_FLOWCHART_NAME);
            OnRerunTrigger = new DAOPCFlowchart(session, DANodeMappings.RERUN_FLOWCHART_NAME);
        }

        /// <summary>
        /// Called by properties to update the UI when they change.
        /// </summary>
        /// <param name="propertyName"></param>
        public void NotifyPropertyChanged(string propertyName, bool isInitialization = false)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventExtendedArgs(propertyName, isInitialization));
        }

        /// <summary>
        /// Updates UI when we resume results updates.
        /// </summary>
        public void RefreshResults()
        {
            NotifyPropertyChanged(nameof(InspectionEndResultHistory));
            NotifyPropertyChanged(nameof(NbPass));
            NotifyPropertyChanged(nameof(NbFail));
        }

        /// <summary>
        /// Notified when a binding successfully writes to Design Assistant, in which case we rerun the current inspection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DAOPCBinding_OnWriteSuccessful(object sender, System.EventArgs e)
        {
            //// We don't currently do not rerun the inspection when the window is resized. The correctly formatted image will be sent at the next inspection.
            //if(sender is DAOPCBinding<int> binding && binding.NodeID.ToString() == DANodeMappings.IMAGEWRITER_RESIZE_NODEID)
            //{
            //    return;
            //}

            if (OnRerunTrigger != null)
            {
                OnRerunTrigger.Call();
            }
        }

        /// <summary>
        /// Handles a change in InspectionEnd EventVariables model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InspectionEnd_EventVariablesModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateInspectionEndResult(InspectionEndResult.CurrentResult, e.PropertyName);
        }

        /// <summary>
        /// Updates the UI when InspectionEndEventVariables changes.
        /// </summary>
        /// <param name="inspectionEndModel"></param>
        /// <param name="propertyName", the property that changed. If all properties changes keep null></param>
        private void UpdateInspectionEndResult(InspectionEndEventResult inspectionEndModel, string propertyName = "")
        {
            if(inspectionEndModel == null)
            {
                return;
            }

            if ((propertyName == nameof(inspectionEndModel.Status_Result) || propertyName == string.Empty) && inspectionEndModel.Status_Result?.CurrentValue == "Pass")
            {
                NbPass++;
            }
            else if ((propertyName == nameof(inspectionEndModel.Status_Result) || propertyName == string.Empty) && inspectionEndModel.Status_Result?.CurrentValue == "Fail")
            {
                NbFail++;
            }

            if ((propertyName == nameof(inspectionEndModel.ImageWriter_Image) || propertyName == string.Empty) && inspectionEndModel.ImageWriter_Image?.CurrentValue?.Count() > 0)
            {
                try
                {
                    InspectionEndResult.CurrentResult.RenderedImage = ClientUtils.GetImageStream((Bitmap)((new ImageConverter()).ConvertFrom(InspectionEndResult.CurrentResult.ImageWriter_Image.CurrentValue)));
                } 
                catch (Exception)
                {
                    Trace.WriteLine("Could not convert server bytes to ImageSource");
                }
            }

            // If the results updated are paused, we don't want to show the new images.
            if (!_isDisplayMode)
            {
                DisplayedInspectionEndResult = inspectionEndModel;
                NotifyPropertyChanged(nameof(InspectionEndResultHistory));
            }
            NotifyPropertyChanged(nameof(InspectionEndResult));
            NotifyPropertyChanged(nameof(DisplayedInspectionEndResult));
        }
    }
}
