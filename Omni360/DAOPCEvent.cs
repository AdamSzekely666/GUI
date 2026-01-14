using Opc.Ua;
using Opc.Ua.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            _daOPCSession = session;
            NodeID = new NodeId($"ns=2;s=Events.{eventName}");
            ResultsHistory = new CircularList<T>(ResultHistoryMaxLength);

            GetEventInitialValues();
            SubscribeToEvent();
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
            foreach (var eventVariableProperty in typeof(T).GetProperties())
            {
                if (eventVariableProperty.PropertyType.IsGenericType && eventVariableProperty.PropertyType.GetGenericTypeDefinition() == typeof(DAComplexVariable<>)){
                    bool isAvailable;
                    Variant currentValue;
                    List<string> availableValues;
                    string variableName;

                    var eventVariableNodeId = new NodeId(NodeID.ToString() + $".{eventVariableProperty.Name}");
                    var value = _daOPCSession.ReadNode(eventVariableNodeId, Attributes.Value);

                    if(value?[0].Value != null)
                    {
                        DAOPCUtils.ExtractDAObjectFields(eventVariableNodeId, value[0].Value, out isAvailable, out currentValue, out availableValues, out variableName);

                        if (isAvailable == false)
                        {
                            return;
                        }

                        if (CurrentResult == null)
                        {
                            InitializeCurrentResult();
                        }

                        CurrentResult.UpdateModel(variableName, isAvailable, currentValue.Value, availableValues);
                    }
                }
            }
            ResultsHistory.Add((T)CurrentResult.Clone());
        }

        /// <summary>
        /// Handles the event notification.
        /// </summary>
        /// <param name="monitoredItem"></param>
        /// <param name="e"></param>
        private void Event_Notification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            if (CurrentResult == null)
            {
                InitializeCurrentResult();
            }

            var notifications = e.NotificationValue as EventFieldList;
            // EventFields[2] corresponds to the "Results" part of the notification, where all the event results are contained.
            // The type definition used for events is DARuntimeEvent and can be found at nodeId ns=2;i=1003 on the server.  
            var eventResults = (Variant[])notifications.EventFields[2].Value;

            foreach (var result in eventResults)
            {
                bool isAvailable;
                Variant currentValue;
                List<string> availableValues;
                string variableName;

                DAOPCUtils.ExtractDAObjectFields(monitoredItem.StartNodeId, result.Value, out isAvailable, out currentValue, out availableValues, out variableName);

                CurrentResult.UpdateModel(variableName, isAvailable, currentValue.Value, availableValues);
            }
            ResultsHistory.Add((T)CurrentResult.Clone());
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
