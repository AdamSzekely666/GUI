using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace MatroxLDS
{
    /// <summary>
    /// Serial port card reader for virtual COM devices (e.g. rfIDEAS reader in COM mode).
    /// Raises CardPresented with the trimmed token; suppresses duplicate reads within duplicateSuppressMs.
    /// </summary>
    public class SerialCardReader : ICardReader
    {
        public event Action<string> CardPresented;

        private readonly SerialPort _port;
        private readonly StringBuilder _buffer = new StringBuilder();
        private readonly int _duplicateSuppressMs;
        private string _lastCard;
        private DateTime _lastCardTime = DateTime.MinValue;
        private readonly object _sync = new object();
        private bool _disposed = false;

        public SerialCardReader(string portName, int baudRate = 9600, int duplicateSuppressMs = 1500)
        {
            _duplicateSuppressMs = duplicateSuppressMs;
            _port = new SerialPort(portName, baudRate)
            {
                NewLine = "\n",
                ReadTimeout = 500,
                Encoding = Encoding.ASCII,
                DtrEnable = false,
                RtsEnable = false
            };
            _port.DataReceived += Port_DataReceived;
        }

        public void Start()
        {
            if (_disposed) return;
            if (!_port.IsOpen)
            {
                try
                {
                    _port.Open();
                }
                catch (Exception)
                {
                    // Startup may fail; caller should handle or poll GetStatus
                }
            }
        }

        public void Stop()
        {
            if (_port.IsOpen)
            {
                try
                {
                    _port.DataReceived -= Port_DataReceived;
                    _port.Close();
                }
                catch { }
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string s = _port.ReadExisting();
                if (string.IsNullOrEmpty(s)) return;

                lock (_sync)
                {
                    _buffer.Append(s);
                    // split on newline or carriage-return
                    while (true)
                    {
                        string full = _buffer.ToString();
                        int idx = full.IndexOfAny(new[] { '\r', '\n' });
                        if (idx < 0) break;
                        string token = full.Substring(0, idx).Trim();
                        string remaining = (idx + 1 < full.Length) ? full.Substring(idx + 1) : "";
                        _buffer.Clear();
                        _buffer.Append(remaining);

                        if (!string.IsNullOrEmpty(token))
                            RaiseCardIfNotDuplicate(token);
                    }

                    // fallback: if buffer too long, attempt whitespace split
                    if (_buffer.Length > 128)
                    {
                        var parts = _buffer.ToString().Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length > 0)
                        {
                            string token = parts[0].Trim();
                            _buffer.Clear();
                            if (parts.Length > 1)
                                _buffer.Append(string.Join(" ", parts, 1, parts.Length - 1));
                            if (!string.IsNullOrEmpty(token))
                                RaiseCardIfNotDuplicate(token);
                        }
                    }
                }
            }
            catch
            {
                // ignore read exceptions
            }
        }

        private void RaiseCardIfNotDuplicate(string token)
        {
            try
            {
                var now = DateTime.UtcNow;
                if (_lastCard == token && (now - _lastCardTime).TotalMilliseconds < _duplicateSuppressMs)
                {
                    return;
                }
                _lastCard = token;
                _lastCardTime = now;
                ThreadPool.QueueUserWorkItem(_ => CardPresented?.Invoke(token));
            }
            catch { }
        }

        public string GetStatus()
        {
            if (_port == null) return "No port";
            return _port.IsOpen ? $"Open: {_port.PortName} @{_port.BaudRate}" : $"Closed: {_port.PortName}";
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            try { Stop(); } catch { }
            try { _port?.Dispose(); } catch { }
        }
    }
}