using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Client.ComplexTypes;
using System;
using System.Threading;

namespace MatroxLDS
{
    /// <summary>
    /// Wrapper on the OPC-UA session class.
    /// </summary>
    public class DAOPCSession: IDisposable
    {
        private Session _session;
        private Subscription _subscription;

        /// <summary>
        /// Indicated if the session is currently connected or not.
        /// </summary>
        public bool Connected => _session.Connected;

        /// <summary>
        /// Handles the keep alive event, in which confirmation is received by the server that it's still connected and the session still valid.
        /// </summary>
        public event KeepAliveEventHandler KeepAlive
        {
            add { _session.KeepAlive += value; }
            remove { _session.KeepAlive -= value; }
        }

        /// <summary>
        /// The delay at which the client will receive notifications, in millisecond. The server will send a notification each 
        /// PUBLISHING_INTERVAL ms if there is a notification to be sent.
        /// </summary>
        public const int PUBLISHING_INTERVAL = 250;

        /// <summary>
        /// Creates a DAOPCSession object.
        /// </summary>
        /// <param name="serverName">Name of the DAOPC server.</param>
        /// <param name="portNumber">Port number to communicate with the DAOPC server.</param>
        public DAOPCSession(string serverName, string portNumber)
        {
            _session = CreateDAOPCUASession(serverName, portNumber);
            _session.KeepAliveInterval = 30000;
            _subscription = CreateSubscription();
            try
            {
                //// Load DANodes custom types for bindings and events.
                //var complexTypeSystem = new ComplexTypeSystem(_session);

                //complexTypeSystem.LoadType(NodeId.Parse(DANodeMappings.DA_VARIABLE_TYPE_NODEID)).ConfigureAwait(false).GetAwaiter().GetResult();
                //complexTypeSystem.LoadType(NodeId.Parse(DANodeMappings.DA_RUNTIME_EVENT_NODEID)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while initializing the client:{ex.Message}");
            }
        }

        /// <summary>
        /// Adds a monitored item to the subscription. This can be a binding or an event.
        /// </summary>
        /// <param name="monitoredItem"></param>
        public void AddMonitoredItem(MonitoredItem monitoredItem)
        {
            if (_subscription == null)
            {
                _subscription = CreateSubscription();
            }
            _subscription.AddItem(monitoredItem);
            _subscription.ApplyChanges();
        }

        /// <summary>
        /// Returns the children nodes of the specified NodeId.
        /// </summary>
        /// <param name="nodeToBrowse"></param>
        public ReferenceDescriptionCollection Browse(NodeId nodeToBrowse)
        {
            ReferenceDescriptionCollection childNodes;
            Byte[] cpda;

            _session.Browse(null, null, nodeToBrowse, 0u, BrowseDirection.Forward, ReferenceTypeIds.HierarchicalReferences, true, (uint)NodeClass.Variable | (uint)NodeClass.Object | (uint)NodeClass.Method, out cpda, out childNodes);
            return childNodes;
        }

        /// <summary>
        /// Executes the associated flowchart in Design Assistant.
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="methodId"></param>
        public void Call(NodeId objectId, NodeId methodId)
        {
            _session.Call(objectId, methodId);
        }

        /// <summary>
        /// Reads the specified attribute of the specified node.
        /// </summary>
        /// <param name="nodeToRead"></param>
        /// <param name="attribute"></param>
        /// <returns>The value of the attribute read</returns>
        /// <remarks> On BadNodeIdUnkown, we suspect the server node might not be instanciated yet. We will then retry the read 3 times.</remarks>
        public DataValueCollection ReadNode(NodeId nodeToRead, uint attribute)
        {
            ReadValueIdCollection nodesToRead = new();
            DataValueCollection results;
            DiagnosticInfoCollection diagnosticInfos;
            ReadValueId nodeToReadValue = new()
            {
                NodeId = nodeToRead,
                AttributeId = attribute                
            };
            nodesToRead.Add(nodeToReadValue);            

            try
            {
                _session.Read(null, 0, TimestampsToReturn.Neither, nodesToRead, out results, out diagnosticInfos);

                if (results[0].StatusCode == StatusCodes.BadNodeIdUnknown)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Thread.Sleep(1000 + (1000 * i ^ 2));
                        _session.Read(null, 0, TimestampsToReturn.Neither, nodesToRead, out results, out diagnosticInfos);
                        if (StatusCode.IsGood(results[0].StatusCode))
                        {
                            return results;
                        }
                    }
                }
                return results;
            }
            catch (Exception)
            {                
                return null;
            }            
        }        

        /// <summary>
        /// Writes to the specified nodeId in Design Assistant.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="attributeId"></param>
        /// <param name="value"></param>
        /// <returns>The status of the write request.</returns>
        public StatusCodeCollection Write(NodeId nodeId, uint attributeId, object value)
        {
            StatusCodeCollection status;
            DiagnosticInfoCollection diagnosticInfo;

            WriteValue nodeTowrite = new() { NodeId = nodeId, AttributeId = attributeId, Value = new DataValue { WrappedValue = new Variant(value) } };
            WriteValueCollection nodesToWrite = new()
            {
                nodeTowrite
            };

            _session.Write(null, nodesToWrite, out status, out diagnosticInfo);

            return status;
        }

        /// <summary>
        ///  Creates the session configuration and uses it to create
        ///  a session with the Design Assistant OPC-UA server.
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="portNumber"></param>
        /// <returns>The created session with the OPC-UA server</returns>
        private Session CreateDAOPCUASession(string serverName, string portNumber)
        {
            // Configures the application
            var config = new ApplicationConfiguration()
            {

                ApplicationName = "daOPCUAClient",
                ApplicationUri = $"urn:{System.Net.Dns.GetHostName()}:daOPCUAClient",
                ApplicationType = ApplicationType.Client,

                // The client is configured to accept any certificates. If security is an issue, these configuration should be modified.
                SecurityConfiguration = new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { StoreType = @"Directory", StorePath = @".\Certificates", SubjectName = "OPCUADemoClient" },
                    TrustedIssuerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = @".\Certificates" },
                    TrustedPeerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = @".\Certificates" },
                    RejectedCertificateStore = new CertificateTrustList { StoreType = @"Directory", StorePath = @".\Certificates" },
                    AutoAcceptUntrustedCertificates = true
                },
                TransportConfigurations = new TransportConfigurationCollection(),                
                TransportQuotas = new TransportQuotas 
                { 
                    OperationTimeout = 20000,
                    MaxByteStringLength = 200000000,
                    MaxMessageSize = 200000000,
                    MaxStringLength = 200000000,                    
                },
                ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 },
                TraceConfiguration = new TraceConfiguration(),
            };

            config.Validate(ApplicationType.Client).GetAwaiter().GetResult();

            // Makes the certificate validation accept bad certificates, bypass them.
            // The client is configured to accept any certificates. If security is an issue, these configuration should be modified.
            if (config.SecurityConfiguration.AutoAcceptUntrustedCertificates)
            {
                config.CertificateValidator.CertificateValidation += (s, e) => { e.Accept = (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted); };
            }

            var endPoint = CoreClientUtils.SelectEndpoint(config, $"opc.tcp://{serverName}:{portNumber}", useSecurity: false, discoverTimeout: 15000);            

            return Session.Create(config, new ConfiguredEndpoint(null, endPoint, EndpointConfiguration.Create(config)), false, "", 60000, null, null).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Creates a subscription on the server, which will be use to monitor bindings and events.
        /// </summary>
        public Subscription CreateSubscription()
        {
            var subscription = new Subscription(_session.DefaultSubscription) { PublishingInterval = PUBLISHING_INTERVAL, PublishingEnabled= true };

            _session.AddSubscription(subscription);
            subscription.Create();

            return subscription;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_session.Connected)
                    {
                        _session.Close();
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
