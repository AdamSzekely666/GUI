using System;
using System.Timers;

namespace MatroxLDS
{
    /// <summary>
    /// Singleton service owning the serial reader. Non-fatal on missing hardware.
    /// Auto-retries connection every RetryIntervalMs if open fails.
    /// </summary>
    public sealed class CardReaderService : IDisposable
    {
        private static readonly Lazy<CardReaderService> _instance = new Lazy<CardReaderService>(() => new CardReaderService());
        public static CardReaderService Instance => _instance.Value;

        private ICardReader _reader;
        private Timer _reconnectTimer;
        private readonly object _sync = new object();

        // events
        public event Action<string> CardPresented;
        public event Action<string> StatusChanged; // "Connected: COM5", "Disconnected", "Stopped"

        public bool IsConnected { get; private set; } = false;
        public string Status { get; private set; } = "Stopped";

        // Settings
        public int RetryIntervalMs { get; set; } = 5000;

        private string _portName;
        private int _baud;

        private CardReaderService() { }

        /// <summary>
        /// Start the service. Non-blocking; will auto-retry if the port cannot be opened.
        /// </summary>
        public void Start(string portName = "COM5", int baud = 9600)
        {
            lock (_sync)
            {
                _portName = portName;
                _baud = baud;

                StopInternalReader();

                TryStartReaderOnce();

                if (_reconnectTimer == null)
                {
                    _reconnectTimer = new Timer(RetryIntervalMs) { AutoReset = true };
                    _reconnectTimer.Elapsed += (s, e) => TryStartReaderOnce();
                }
                _reconnectTimer.Interval = RetryIntervalMs;
                _reconnectTimer.Start();
            }
        }

        private void TryStartReaderOnce()
        {
            lock (_sync)
            {
                try
                {
                    if (_reader != null)
                    {
                        // Already running
                        return;
                    }

                    var candidate = new SerialCardReader(_portName, _baud);

                    candidate.Start();

                    var st = candidate.GetStatus();
                    if (st != null && st.StartsWith("Open", StringComparison.OrdinalIgnoreCase))
                    {
                        _reader = candidate;
                        _reader.CardPresented += OnCardPresented;
                        IsConnected = true;
                        Status = $"Connected: {_portName}@{_baud}";
                        StatusChanged?.Invoke(Status);
                        _reconnectTimer?.Stop();
                        return;
                    }

                    candidate.Dispose();
                    IsConnected = false;
                    Status = "Disconnected";
                    StatusChanged?.Invoke(Status);
                }
                catch
                {
                    IsConnected = false;
                    Status = "Disconnected";
                    StatusChanged?.Invoke(Status);
                }
            }
        }

        private void OnCardPresented(string cardId)
        {
            try { CardPresented?.Invoke(cardId); } catch { }
        }

        /// <summary>
        /// Stop the service and reader.
        /// </summary>
        public void Stop()
        {
            lock (_sync)
            {
                _reconnectTimer?.Stop();
                _reconnectTimer?.Dispose();
                _reconnectTimer = null;
                StopInternalReader();
                IsConnected = false;
                Status = "Stopped";
                StatusChanged?.Invoke(Status);
            }
        }

        private void StopInternalReader()
        {
            if (_reader != null)
            {
                try
                {
                    _reader.CardPresented -= OnCardPresented;
                    _reader.Stop();
                    _reader.Dispose();
                }
                catch { }
                finally { _reader = null; }
            }
        }

        public string GetStatus() => Status;

        public void Dispose()
        {
            Stop();
        }
    }
}