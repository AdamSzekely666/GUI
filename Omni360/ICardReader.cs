using System;

namespace MatroxLDS
{
    public interface ICardReader : IDisposable
    {
        /// <summary>Raised when a card UID is read (normalized string).</summary>
        event Action<string> CardPresented;

        /// <summary>Start the reader (open port, start listening).</summary>
        void Start();

        /// <summary>Stop the reader (close port, stop listening).</summary>
        void Stop();

        /// <summary>Friendly name / status of the reader.</summary>
        string GetStatus();
    }
}