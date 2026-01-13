using Opc.Ua;

namespace MatroxLDS
{
    /// <summary>
    /// Represents a flowchart in Design Assistant.
    /// </summary>
    public class DAOPCFlowchart
    {
        private readonly NodeId _methodId;
        private readonly NodeId _nodeId;
        private readonly DAOPCSession _session;

        /// <summary>
        /// Creates a DAOPCFlowchart object.
        /// </summary>
        /// <param name="session">The connected session with the DAOPC server.</param>
        /// <param name="flowchartName"> The name of the Flowchart in Design Assistant. </param>
        public DAOPCFlowchart(DAOPCSession session, string flowchartName)
        {
            _nodeId = DANodeMappings.DA_NAMESPACE + $"Flowcharts.{flowchartName}";
            _methodId = _nodeId + ".ExecuteFlowchart";
            _session = session;
        }

        /// <summary>
        /// Executes the associated flowchart in Design Assistant.
        /// </summary>
        public void Call()
        {
            _session.Call(_nodeId, _methodId);
        }
    }
}
