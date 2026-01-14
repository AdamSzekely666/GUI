using Opc.Ua;
using Opc.Ua.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace MatroxLDS
{
    /// <summary>
     /// Represents an event in Design Assistant.
     /// </summary>
    public class DAOPCEvent<T> where T : IEventResult, ICloneable, INotifyPropertyChanged, new()
    {
        private readonly DAOPCSession _daOPCSession;

        /// <summary>
        /// This event fires when the theres is a change to the current result.
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs> OnCurrentResultChange;

        /// <summary>
        /// The corresponding NodeId in the DAOPC server.
        /// </summary>
        public NodeId NodeID { get; }

        /// <summary>
        /// The result of the last received event notification.
        /// </summary>
        public T CurrentResult { get; private set; }

        /// <summary>
        /// The result history of all received event notifications.
        /// </summary>
        public CircularList<T> ResultsHistory { get; }

        /// <summary>
        /// Creates a DAOPCEvent object.
        /// </summary>
        /// <param name="session"> The connected session with the DAOPC server. </param>
        /// <param name="eventName"> The name of the event in the DAOPC server. </param>
        /// <param name="ResultHistoryMaxLength"> The maximum amount of results to be stored in the history. </param>
        public DAOPCEvent(DAOPCSession session, string eventName, int ResultHistoryMaxLength)
        {
            System.Diagnostics.Debug.WriteLine($"🔵 [DAOPCEvent CONSTRUCTOR] Starting for event:  '{eventName}'");

            _daOPCSession = session;

            // Trim event name and remove any spaces
            string cleanEventName = eventName?.Trim() ?? "";

            // Create NodeID without spaces
            string nodeIdString = $"ns=2;s=Events.{cleanEventName}";
            NodeID = new NodeId(nodeIdString);

            System.Diagnostics.Debug.WriteLine($"🔵 [DAOPCEvent CONSTRUCTOR] NodeID: '{NodeID}'");
            System.Diagnostics.Debug.WriteLine($"🔵 [DAOPCEvent CONSTRUCTOR] NodeID string length: {nodeIdString.Length}");

            ResultsHistory = new CircularList<T>(ResultHistoryMaxLength);

            InitializeCurrentResult();
            System.Diagnostics.Debug.WriteLine($"✅ [DAOPCEvent CONSTRUCTOR] CurrentResult initialized");

            System.Diagnostics.Debug.WriteLine($"🔵 [DAOPCEvent CONSTRUCTOR] Attempting to load 'All' property...");
            TryLoadAllProperty();

            System.Diagnostics.Debug.WriteLine($"🔵 [DAOPCEvent CONSTRUCTOR] Calling SubscribeToEvent()...");
            SubscribeToEvent();
            System.Diagnostics.Debug.WriteLine($"✅ [DAOPCEvent CONSTRUCTOR] SubscribeToEvent() returned");

            System.Diagnostics.Debug.WriteLine($"✅ [DAOPCEvent CONSTRUCTOR] Constructor complete!");
        }
        private void TryLoadAllProperty()
        {
            try
            {
                // Build the All property node path - make sure no extra spaces
                string allNodeString = NodeID.ToString().Replace(" ", "") + ".All";
                var allPropertyNode = new NodeId(allNodeString);

               // System.Diagnostics.Debug.WriteLine($"   Reading node: '{allPropertyNode}'");
               // System.Diagnostics.Debug.WriteLine($"   Node string:  '{allNodeString}'");

                var value = _daOPCSession.ReadNode(allPropertyNode, Attributes.Value);

                if (value != null && value.Count > 0 && value[0]?.Value != null)
                {
                    bool isAvailable;
                    Variant currentValue;
                    List<string> availableValues;
                    string variableName;

                    DAOPCUtils.ExtractDAObjectFields(allPropertyNode, value[0].Value,
                        out isAvailable, out currentValue, out availableValues, out variableName);

                    System.Diagnostics.Debug.WriteLine($"   Property: {variableName}, IsAvailable: {isAvailable}");

                    if (isAvailable && currentValue.Value != null)
                    {
                        CurrentResult.UpdateModel(variableName, isAvailable, currentValue.Value, availableValues);

                        if (currentValue.Value is byte[] bytes)
                        {
                            System.Diagnostics.Debug.WriteLine($"   ✅ Loaded 'All' with {bytes.Length} bytes of image data");
                        }
                        else
                        {
                            //System.Diagnostics.Debug.WriteLine($"   ✅ Loaded 'All' but data is not byte[]");
                        }
                    }
                    else
                    {
                       // System.Diagnostics.Debug.WriteLine($"   ⚠️ 'All' is not available or has no data");
                    }
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine($"   ⚠️ ReadNode returned null or empty");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"   ❌ Error loading 'All' property: {ex.Message}");
            }
        }

        /// <summary>
        /// Subscribes to an event on the server.
        /// </summary>
        /// <remark>Subscribing to an event is done the same way as monitoring a binding, we just monitor the EventNotifier instead of a value
        /// directly.</remark>
        private void SubscribeToEvent()
        {
            var monitoredObjectNodeId = NodeID;
            EventFilter filter = GetFilter(new NodeId(DANodeMappings.DA_EVENT_FILTER_TYPE_NODEID));

            var monitoredItem = new MonitoredItem
            {
                StartNodeId = monitoredObjectNodeId,
                AttributeId = Attributes.EventNotifier,
                MonitoringMode = MonitoringMode.Reporting,
                SamplingInterval = 0,
                QueueSize = 100000,
                DiscardOldest = true,
                Filter = filter
            };
            monitoredItem.Notification += new MonitoredItemNotificationEventHandler(Event_Notification);

            _daOPCSession.AddMonitoredItem(monitoredItem);
        }

        /// <summary>
        /// Creates a filter for the event subscription.
        /// It contains a Select clause and a Where clause. These work similarly to their SQL counter parts.
        /// </summary>
        /// <param name="eventTypeId"></param>
        /// <returns>The filter to use for the monitoring</returns>
        private static EventFilter GetFilter(NodeId eventTypeId)
        {
            EventFilter filter = new();

            // We select all the fields of the event object type, defined by the Design Assistant OPC
            // server. See Types > EventTypes > BaseEventType > DARuntimeEvent in a server browser.
            var selectClause = new SimpleAttributeOperandCollection
            {
                GetSelectOperand("EventType", ObjectTypeIds.BaseEventType),
                GetSelectOperand("EventName", eventTypeId),
                GetSelectOperand("Results", eventTypeId)
            };

            ContentFilter whereClause = new();

            filter.SelectClauses = selectClause;
            filter.WhereClause = whereClause;
            return filter;
        }

        /// <summary>
        /// Creates a select operand which is necessary to create the filter.
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        private static SimpleAttributeOperand GetSelectOperand(string variableName, NodeId parentNode)
        {
            SimpleAttributeOperand operand = new()
            {
                TypeDefinitionId = parentNode,
                AttributeId = Attributes.Value
            };

            var browsePath = new QualifiedNameCollection
            {
                new QualifiedName(variableName, 2)
            };

            operand.BrowsePath = browsePath;

            return operand;
        }

        /// <summary>
        /// Reads the intial values of the event. 
        /// </summary>
        private void GetEventInitialValues()
        {
            System.Diagnostics.Debug.WriteLine("🔵 [DAOPCEvent] GetEventInitialValues() started");

            int propertyCount = 0;
            foreach (var eventVariableProperty in typeof(T).GetProperties())
            {
                propertyCount++;
                System.Diagnostics.Debug.WriteLine($"🔵 [DAOPCEvent] Processing property #{propertyCount}: {eventVariableProperty.Name}");

                if (eventVariableProperty.PropertyType.IsGenericType &&
                    eventVariableProperty.PropertyType.GetGenericTypeDefinition() == typeof(DAComplexVariable<>))
                {
                    bool isAvailable;
                    Variant currentValue;
                    List<string> availableValues;
                    string variableName;

                    var eventVariableNodeId = new NodeId(NodeID.ToString() + $".{eventVariableProperty.Name}");

                    System.Diagnostics.Debug.WriteLine($"   Reading node: {eventVariableNodeId}");

                    DataValueCollection value = null;
                    try
                    {
                        value = _daOPCSession.ReadNode(eventVariableNodeId, Attributes.Value);
                        System.Diagnostics.Debug.WriteLine($"   ✅ ReadNode returned successfully");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"   ❌ ReadNode threw exception: {ex.Message}");
                        continue; // Skip this property
                    }

                    if (value?[0].Value != null)
                    {
                        try
                        {
                            DAOPCUtils.ExtractDAObjectFields(eventVariableNodeId, value[0].Value,
                                out isAvailable, out currentValue, out availableValues, out variableName);

                            System.Diagnostics.Debug.WriteLine($"   Property:  {variableName}, IsAvailable: {isAvailable}");

                            if (isAvailable == false)
                            {
                                System.Diagnostics.Debug.WriteLine($"   ⚠️ Skipping '{variableName}' - not available");
                                continue;
                            }

                            if (CurrentResult == null)
                            {
                                System.Diagnostics.Debug.WriteLine("   Initializing CurrentResult.. .");
                                InitializeCurrentResult();
                            }

                            CurrentResult.UpdateModel(variableName, isAvailable, currentValue.Value, availableValues);
                            System.Diagnostics.Debug.WriteLine($"   ✅ Loaded '{variableName}'");
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"   ❌ Error processing property:  {ex.Message}");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"   ⚠️ ReadNode returned null for {eventVariableProperty.Name}");
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine($"🔵 [DAOPCEvent] Finished processing {propertyCount} properties");

            // Add to history if we have a result
            if (CurrentResult != null)
            {
                ResultsHistory.Add((T)CurrentResult.Clone());
                System.Diagnostics.Debug.WriteLine($"✅ [DAOPCEvent] Initial values loaded, added to history");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"⚠️ [DAOPCEvent] No initial values loaded, CurrentResult is null");
            }

            System.Diagnostics.Debug.WriteLine($"✅ [DAOPCEvent] GetEventInitialValues() COMPLETE - returning now");
        }
        /// <summary>
        /// Handles the event notification.
        /// </summary>
        /// <param name="monitoredItem"></param>
        /// <param name="e"></param>
        private void Event_Notification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            EventFieldList notifications = (EventFieldList)e.NotificationValue;

            // Get ALL image properties from InspectionEndEventResult
            var imageProperties = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(DAComplexVariable<byte[]>))
                .Select(p => p.Name)
                .ToArray();

            System.Diagnostics.Debug.WriteLine($"🔔 [EVENT] Notification received - will check {imageProperties.Length} properties");

            int successCount = 0;
            int failCount = 0;

            // Read each image property from OPC-UA
            foreach (string propertyName in imageProperties)
            {
                try
                {
                    // NO SPACES - Fixed! 
                    string nodeString = NodeID.ToString().Replace(" ", "") + $".{propertyName}";
                    var propertyNode = new NodeId(nodeString);

                    System.Diagnostics.Debug.WriteLine($"   🔍 Trying to read:  {nodeString}");

                    var value = _daOPCSession.ReadNode(propertyNode, Opc.Ua.Attributes.Value);

                    if (value != null && value.Count > 0 && value[0]?.Value != null)
                    {
                        bool isAvailable;
                        Variant currentValue;
                        List<string> availableValues;
                        string variableName;

                        DAOPCUtils.ExtractDAObjectFields(propertyNode, value[0].Value,
                            out isAvailable, out currentValue, out availableValues, out variableName);

                        System.Diagnostics.Debug.WriteLine($"      Extracted - IsAvailable: {isAvailable}, ValueType: {currentValue.Value?.GetType().Name}");

                        if (isAvailable && currentValue.Value != null && currentValue.Value is byte[] bytes)
                        {
                            CurrentResult.UpdateModel(propertyName, isAvailable, currentValue.Value, availableValues);
                            System.Diagnostics.Debug.WriteLine($"      ✅ Updated '{propertyName}':  {bytes.Length} bytes");
                            successCount++;
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"      ⚠️ '{propertyName}' not available or wrong type");
                            failCount++;
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"      ⚠️ ReadNode returned null/empty for '{propertyName}'");
                        failCount++;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"      ❌ Exception reading '{propertyName}': {ex.Message}");
                    failCount++;
                }
            }

            System.Diagnostics.Debug.WriteLine($"🔔 [EVENT] Complete - {successCount} succeeded, {failCount} failed");
        }
        /// <summary>
        /// Propagates the OnPropertyChange of the current result to the OnCurrentResultChange event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DAOPCEvent_OnCurrentResultChange(object sender, PropertyChangedEventArgs e)
        {
            OnCurrentResultChange?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
        }

        /// <summary>
        /// We create the result object when we know there are currently results.
        /// </summary>
        private void InitializeCurrentResult()
        {
            CurrentResult = new();
            CurrentResult.PropertyChanged += DAOPCEvent_OnCurrentResultChange;
        }
    }
}
