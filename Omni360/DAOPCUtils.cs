using Opc.Ua;
using Opc.Ua.Client.ComplexTypes;
using System.Collections.Generic;
using System.Linq;

namespace MatroxLDS
{
    /// <summary>
    /// Utility methods for OPC classes.
    /// </summary>
    public static class DAOPCUtils
    {
        /// <summary>
        /// Parses the notification data and maps it to the corresponding variable.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="serverObject"></param>
        /// <param name="isAvailable"></param>
        /// <param name="currentValue"></param>
        /// <param name="availableValues"></param>
        /// <param name="variableName"></param>
        public static void ExtractDAObjectFields(NodeId nodeId, object serverObject, out bool isAvailable, out Variant currentValue, out List<string> availableValues, out string variableName)
        {
            BaseComplexType complexValue = ((ExtensionObject)serverObject).Body as BaseComplexType;

            isAvailable = (bool)complexValue["IsAvailable"];
            currentValue = (Variant)complexValue["CurrentValue"];

            if (currentValue.Value is ExtensionObject extensionObject)
            {
                var innerObject = extensionObject.Body;
                if (innerObject is EnumValueType enumValueType)
                {
                    currentValue = enumValueType.DisplayName.Text;
                }
            }

            availableValues = new List<string>();
            Variant[] availableValuesVariant = (Variant[])complexValue["AvailableValues"];

            foreach (var item in availableValuesVariant)
            {
                var stringToAdd = item;
                if (item.Value != null && item.TypeInfo.ToString() != "String")
                {
                    stringToAdd = ((EnumValueType)((ExtensionObject)item.Value).Body).DisplayName.Text;
                }
                availableValues.Add(stringToAdd.Value?.ToString());
            }

            if (complexValue.GetPropertyNames().Any(x => x == "Name"))
            {
                variableName = complexValue["Name"].ToString();
            }
            else
            {
                variableName = GetNodeServerName(nodeId);
            }
        }

        /// <summary>
        /// Extract the name of the nodeId appearing in the Design Assistant OPC-UA tab.
        /// </summary>
        /// <param name="variableNode"></param>
        public static string GetNodeServerName(NodeId variableNode)
        {
            return variableNode.ToString().Split('.').Last();
        }
    }
}
