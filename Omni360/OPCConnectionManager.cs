using Opc.Ua;
using Opc.Ua.Client;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;

namespace MatroxLDS
{ 
    /// <summary>
    /// Manages all interactions with the Design Assistant OPC-UA server.
    /// </summary>
    public class OPCConnectionManager : IDisposable
    {
        private string _serverName;
        private string _portNumber;

        private bool _tryReconnect;

        public DAOPCSession Session { get; set; }

        private OPCDataModel _model;
        private bool disposedValue;
        public const int PUBLISHING_INTERVAL = 250;

        public OPCConnectionManager(OPCDataModel model, bool tryReconnect = true)
        {
            _model = model;
            _tryReconnect = tryReconnect;
        }

        /// <summary>
        /// Connects to the specified OPC-UA server and browse nodes if the connection is sucessful.
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="portNumber"></param>
        /// <returns>If connection is successful</returns>
        public bool Connect(string serverName, string portNumber)
        {
            bool connected = false;
            try
            {
                Session = new DAOPCSession(serverName, portNumber);
            } 
            catch (ServiceResultException ex)
            {
                Trace.WriteLine($"Could not establish connection with message: {ex.Message}");
                return false;
            }

            if (Session != null && Session.Connected)
            {
                connected = true;

                _model.IsServerConnected = true;                
                _model.ReinitializeOnConnect(Session);
                _model.InitializeOPCProperties(Session);
                _serverName = serverName;
                _portNumber = portNumber;
                Session.KeepAlive += Client_KeepAlive;
            }
            return connected;
        }

        /// <summary>
        /// Closes the connection with the server.
        /// </summary>
        /// <remarks> We need to manually close connection each time the client terminates. Else, the monitoring will not function correctly until the server is restarted.</remarks>
        public void CloseConnection()
        {
            if(Session != null)
            {
                try
                {
                    Session.KeepAlive -= Client_KeepAlive;
                    Session.Dispose();
                    _model.IsServerConnected = false;
                }
                catch (Exception)
                {
                    //Sometimes the connection is already close by the time we get here.
                }
            }
        }

        /// <summary>
        /// Internally connect with the same hostname and port number.
        /// </summary>
        /// <returns>If connection is successful</returns>
        private bool Connect()
        {
            return Connect(_serverName, _portNumber);
        }

        /// <summary>
        /// Tries to reconnect to the server at a set delay when the connection is lost.
        /// </summary>
        private object _reconnectLock = new object();
        private bool _isReconnecting = false;
        private async void TryReconnect()
        {
            lock(_reconnectLock)
            {
                if(_isReconnecting)
                {
                    return;
                }

                _isReconnecting = true;
            }

            if (!_model.IsServerConnected || !_tryReconnect)
            {
                return;
            }
            Trace.WriteLine("The connection with the server was lost. Attempting to reconnect.");
            // If we lost connection, we need to start a new session, because we can't reconnect to the 
            // previous one.
            CloseConnection();

            while (true)
            {
                await Task.Delay(500);
                var isReconnected = Connect();

                if (isReconnected)
                {
                    lock(_reconnectLock)
                    {
                        _isReconnecting = false;
                    }                    

                    if (disposedValue)
                    {
                        CloseConnection();
                        return;
                    }
                    break;
                }
                else
                {
                    _model.IsServerConnected = false;
                    _model.ReconnectionAttempts += 1;
                }
            }
        }

        /// <summary>
        /// Handles the keep alive ping of the server, sent at a set time interval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_KeepAlive(ISession session, KeepAliveEventArgs e)
        {
            if (e != null && Session != null)
            {
                if (ServiceResult.IsGood(e.Status))
                {
                    _model.IsServerConnected = true;
                }
                else
                {
                    TryReconnect();
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_model.IsServerConnected)
                    {                        
                        CloseConnection();
                    }
                }
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~OPCConnectionManager()
        {
             // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
             Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

