using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MatroxLDS
{
    public class OpcUaConnectionManager
    {
        private Dictionary<string, Session> _sessions = new Dictionary<string, Session>();
        private ApplicationConfiguration _configuration;

        [Obsolete]
        public async Task InitializeAsync()
        {
            // Create application configuration (shared for all connections)
            _configuration = new ApplicationConfiguration()
            {
                ApplicationName = "OmniCheck360_OpcUaClient",
                ApplicationType = ApplicationType.Client,
                SecurityConfiguration = new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier(),
                    AutoAcceptUntrustedCertificates = true, // For testing - change in production! 
                },
                TransportConfigurations = new TransportConfigurationCollection(),
                TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
                ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 }
            };

            await _configuration.Validate(ApplicationType.Client);
        }

        [Obsolete]
        public async Task<bool> ConnectAsync(string projectName, string endpointUrl)
        {
            try
            {
                if (_sessions.ContainsKey(projectName))
                {
                    Console.WriteLine($"Already connected to {projectName}");
                    return true;
                }

                var endpointConfiguration = EndpointConfiguration.Create(_configuration);
                var endpoint = new ConfiguredEndpoint(null, new EndpointDescription(endpointUrl), endpointConfiguration);

                var session = await Session.Create(
                    _configuration,
                    endpoint,
                    false,
                    $"OmniCheck360_{projectName}",
                    60000,
                    new UserIdentity(new AnonymousIdentityToken()),
                    null
                );

                _sessions[projectName] = session;
                Console.WriteLine($"Connected to {projectName} at {endpointUrl}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection to {projectName} failed: {ex.Message}");
                return false;
            }
        }

        [Obsolete]
        public T ReadValue<T>(string projectName, string nodeId)
        {
            if (!_sessions.ContainsKey(projectName))
                throw new InvalidOperationException($"Not connected to {projectName}");

            var session = _sessions[projectName];
            var node = new NodeId(nodeId);
            var value = session.ReadValue(node);
            return (T)value.Value;
        }

        public async Task WriteValueAsync(string projectName, string nodeId, object value)
        {
            if (!_sessions.ContainsKey(projectName))
                throw new InvalidOperationException($"Not connected to {projectName}");

            var session = _sessions[projectName];
            var node = new NodeId(nodeId);
            var writeValue = new WriteValue
            {
                NodeId = node,
                AttributeId = Attributes.Value,
                Value = new DataValue(new Variant(value))
            };

            var response = await session.WriteAsync(
                null,
                new WriteValueCollection { writeValue },
                CancellationToken.None);

            if (StatusCode.IsBad(response.Results[0]))
                throw new Exception($"Write failed: {response.Results[0]}");
        }

        // Subscribe to value changes (replaces ValueChanged events)
        [Obsolete]
        public Subscription CreateSubscription(string projectName, int publishingInterval = 1000)
        {
            if (!_sessions.ContainsKey(projectName))
                throw new InvalidOperationException($"Not connected to {projectName}");

            var session = _sessions[projectName];
            var subscription = new Subscription(session.DefaultSubscription)
            {
                PublishingInterval = publishingInterval,
                PublishingEnabled = true
            };

            session.AddSubscription(subscription);
            subscription.Create();

            return subscription;
        }

        public void DisconnectAll()
        {
            foreach (var session in _sessions.Values)
            {
                session?.Close();
                session?.Dispose();
            }
            _sessions.Clear();
        }

        [Obsolete]
        public void Disconnect(string projectName)
        {
            if (_sessions.ContainsKey(projectName))
            {
                _sessions[projectName]?.Close();
                _sessions[projectName]?.Dispose();
                _sessions.Remove(projectName);
            }
        }
    }
}