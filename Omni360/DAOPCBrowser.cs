using Opc.Ua;
using OPCUADemoClient.DAOPCUA;
using System.Collections.Generic;

namespace MatroxLDS
{
    /// <summary>
    /// Browses the DAOPC server and stores the NodeIds of all bindings, events and flowcharts found.
    /// </summary>
    public class DAOPCBrowser
    {
        DAOPCSession _session;

        public List<NodeId> BindingsNodeIds { get; }
        public List<NodeId> EventsNodeIds { get; }
        public List<NodeId> FlowchartsNodeIds { get; }

        public DAOPCBrowser(DAOPCSession session)
        {
            _session = session;

            BindingsNodeIds = new();
            EventsNodeIds = new();
            FlowchartsNodeIds = new();
        }

        /// <summary>
        /// Explores the complete three of the nodes defined by the Design Assistant OPC-UA server.
        /// </summary>
        public void Browse_DAnodes()
        {
            // We start browsing "ObjectIds.ObjectsFolder", because we know it contains the folders Bindings, Events and Flowchart.
            ReferenceDescriptionCollection daObjectsNodes = _session.Browse(ObjectIds.ObjectsFolder);

            foreach (var node in daObjectsNodes)
            {
                ReferenceDescriptionCollection daValueObjectNodes = _session.Browse((NodeId)node.NodeId);

                switch (node.DisplayName.Text)
                {
                    case "Bindings":
                        // We need to load the type of the Design Assistant complex objects. These contains 3 properties : Current_Value, Is_Available
                        // and Available_Values. All Design Assistant object have the same type, so we only need to load it once.
                        // This ensures that the data in received notification is properly match to this format and not just a unusable byte[].
                        foreach (var bindingNode in daValueObjectNodes)
                        {
                            BindingsNodeIds.Add((NodeId)bindingNode.NodeId);
                        }
                        break;
                    case "Events":
                        // The first time we load events, we read the inspection end results so we can display them before the next inspection end happens.
                        foreach (var eventNode in daValueObjectNodes)
                        {
                            EventsNodeIds.Add((NodeId)eventNode.NodeId);
                        }
                        break;
                    case "Flowcharts":
                        foreach (var flowchartNode in daValueObjectNodes)
                        {
                            FlowchartsNodeIds.Add((NodeId)flowchartNode.NodeId);
                        }
                        break;
                }
            }
        }
    }
}
