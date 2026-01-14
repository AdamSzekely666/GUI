using Opc.Ua;
using Opc.Ua.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MatroxLDS
{
    /// <summary>
    /// Represents a binding in Design Assistant.
    /// </summary>
    public class DAOPCBinding<T> : DAOPCBinding
    {
        private readonly DAOPCSession _daOPCSession;

        /// <summary>
        ///  The complex value of the binding, which contains it's available value, current value and is avaialble value.
        /// </summary>
        public DAComplexVariable<T> Value { get; set; }

        /// <summary>
        /// The corresponding NodeId in the DAOPC server.
        /// </summary>
        public NodeId NodeID { get; }

        /// <summary>
        /// DAOPCBindingConstructor
        /// </summary>
        /// <param name="session">The connected session with the DAOPC server.</param>
        /// <param name="bindingName">The name of the binding in Design Assistant.</param>
        public DAOPCBinding(DAOPCSession session, string bindingName)
        {
            _daOPCSession = session;
            NodeID = new NodeId($"ns=2;s={bindingName}"); Value = new DAComplexVariable<T>();

            MonitorBinding();
        }

        /// <summary>
        /// Monitors a binding on the server, which will notify when changed.
        /// </summary>
        private void MonitorBinding()
        {
            var monitoredItem = new MonitoredItem
            {
                StartNodeId = new NodeId(NodeID.Identifier, NodeID.NamespaceIndex),
                AttributeId = Attributes.Value,
                MonitoringMode = MonitoringMode.Reporting,
                SamplingInterval = DAOPCSession.PUBLISHING_INTERVAL,
                QueueSize = 0,
                DiscardOldest = true
            };
            monitoredItem.Notification += new MonitoredItemNotificationEventHandler(Binding_Notification);

            _daOPCSession.AddMonitoredItem(monitoredItem);
        }

        /// <summary>
        /// Write to the Design Assistant currently monitored binding.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="value"></param>
        /// <param name="enumValue"></param>
        /// <param name="rerunCurrentInspection"></param>
        public async void Write(object value, int? enumValue = null)
        {
            if (!_daOPCSession.Connected)
            {
                Trace.WriteLine($"Could not write to property {NodeID.Identifier}, because the connection was lost.");
                return;
            }

            // If the type of the binding we want to write to is an Enum, like an operator binding, we need to write
            // an enum type, which contains the string value and the int value of the enum.
            var type = _daOPCSession.ReadNode(NodeID + ".CurrentValue", Attributes.DataType).SingleOrDefault()?.Value as NodeId;
            if (type == new NodeId(DANodeMappings.ENUM_TYPE_NODE_ID) && enumValue != null && enumValue >= 0)
            {
                value = new EnumValueType()
                {
                    DisplayName = (string)value,
                    Value = (int)enumValue
                };
            }

            StatusCodeCollection status = await Task.Run(() => _daOPCSession.Write(NodeID, Attributes.Value, value));
            if (!StatusCode.IsGood(status[0]))
            {
                Trace.WriteLine($"Could not write value for {NodeID}");
            }
            else
            {
                FireOnWriteSucessful();
            }
        }

        /// <summary>
        /// Handles the binding notification.
        /// </summary>
        /// <param name="monitoredItem"></param>
        /// <param name="e"></param>
        private void Binding_Notification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            try
            {
                Debug.WriteLine($"🔔 [BINDING] Notification received for:  {monitoredItem.StartNodeId}");

                var isFirstNotification = Value.AvailableValues == null;
                var dequeuedValues = monitoredItem.DequeueValues();

                bool isAvailable;
                Variant currentValue;
                List<string> availableValues;

                try
                {
                    DAOPCUtils.ExtractDAObjectFields(monitoredItem.StartNodeId, dequeuedValues[0].Value, out isAvailable, out currentValue, out availableValues, out _);
                    Debug.WriteLine($"📊 [BINDING] Extracted - IsAvailable: {isAvailable}, CurrentValue: {currentValue.Value}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"❌ [BINDING] Error extracting fields: {ex.Message}");
                    Debug.WriteLine($"❌ Exception type: {ex.GetType().Name}");
                    return; // Skip this notification
                }

                Value.AvailableValues = availableValues;

                if (isAvailable)
                {
                    try
                    {
                        // Safe cast - handle type conversions
                        object valueToSet = currentValue.Value;

                        // If T is int but value is double, convert it
                        if (typeof(T) == typeof(int) && valueToSet is double doubleValue)
                        {
                            valueToSet = (int)doubleValue;
                        }
                        // If T is int but value is long, convert it
                        else if (typeof(T) == typeof(int) && valueToSet is long longValue)
                        {
                            valueToSet = (int)longValue;
                        }
                        // If T is int but value is short, convert it
                        else if (typeof(T) == typeof(int) && valueToSet is short shortValue)
                        {
                            valueToSet = (int)shortValue;
                        }
                        // If T is int but value is byte, convert it
                        else if (typeof(T) == typeof(int) && valueToSet is byte byteValue)
                        {
                            valueToSet = (int)byteValue;
                        }

                        // Only update if value changed
                        if (!EqualityComparer<T>.Default.Equals((T)Value.CurrentValue, (T)valueToSet))
                        {
                            Value.CurrentValue = (T)valueToSet;
                            Debug.WriteLine($"✅ [BINDING] Updated CurrentValue to:  {Value.CurrentValue}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"❌ [BINDING] Error setting CurrentValue:  {ex.Message}");
                        Debug.WriteLine($"❌ CurrentValue type: {currentValue.Value?.GetType().Name}, Expected: {typeof(T).Name}");
                    }
                }
                Value.IsAvailable = isAvailable;
                if (isFirstNotification)
                {
                    Value.PropertyChanged += ComplexVariable_PropertyChanged;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ [BINDING] Fatal error in Binding_Notification: {ex.Message}");
                Debug.WriteLine($"❌ Stack:  {ex.StackTrace}");
            }
        }
        /// <summary>
        /// Handles the change of the ComplexVariable, in which case we want to write the update to the DAOPC server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComplexVariable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Value.CurrentValue))
            {
                Write(Value.CurrentValue, Value.AvailableValues.IndexOf(Value.CurrentValue.ToString()));
            }
        }
    }

    /// <summary>
    /// Non-generic base class, which serves the purpose of providing the OnWriteSuccessful statically for all bindings. 
    /// </summary>
    public abstract class DAOPCBinding
    {
        /// <summary>
        /// Event that fires when the value of the binding has been written to the server.
        /// </summary>
        public static event EventHandler OnWriteSuccessful;

        public static void ResetOnWriteSuccessfulEvent(){
            OnWriteSuccessful = null;
        }
        protected void FireOnWriteSucessful()
        {
            OnWriteSuccessful.Invoke(this, EventArgs.Empty);
        }
    }
}
